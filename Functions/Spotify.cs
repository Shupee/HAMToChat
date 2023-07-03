using MusixmatchClientLib;
using MusixmatchClientLib.API.Model.Types;
using MusixmatchClientLib.Auth;
using MusixmatchClientLib.Types;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Diagnostics;
using static SpotifyAPI.Web.Scopes;
using static SpotifyAPI.Web.PlayerCurrentlyPlayingRequest;

namespace HR.Functions
{
    public class APIsLogin
    {
        private static readonly EmbedIOAuthServer _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
        public static async Task<SpotifyClient> SpotifyLogin(string CredentialsPath, string clientId)
        {
            var json = await File.ReadAllTextAsync(CredentialsPath);
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);
            var authenticator = new PKCEAuthenticator(clientId!, token!);
            authenticator.TokenRefreshed += (sender, token)
              => File.WriteAllText(CredentialsPath, JsonConvert.SerializeObject(token));
            var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(authenticator);
            return new SpotifyClient(config);
        }
        public static async Task StartAuthentication(string clientId, string CredentialsPath)
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            string End = null;
            await _server.Start();
            _server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await _server.Stop();
                PKCETokenResponse token = await new OAuthClient().RequestToken(
                  new PKCETokenRequest(clientId!, response.Code, _server.BaseUri, verifier)
                );

                await File.WriteAllTextAsync(CredentialsPath, JsonConvert.SerializeObject(token));
                End = token.ToString();
            };
            var request = new LoginRequest(_server.BaseUri, clientId!, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challenge,
                CodeChallengeMethod = "S256",
                Scope = new List<string> { UserReadEmail, UserReadPrivate, PlaylistReadPrivate, PlaylistReadCollaborative, UserReadCurrentlyPlaying, UserReadPlaybackState }
            };

            Uri uri = request.ToUri();
            try
            {
                BrowserUtil.Open(uri);
            }
            catch (Exception)
            {
                Logger.Error($"Unable to open URL, manually open: {uri}");
            }
            while (End == null) { }
        }
        public static MusixmatchClient MusixmatchLogin()
        {

            try
            {
                var musixmatchClient = new MusixmatchClient(new MusixmatchToken());
                Logger.Info($"[{nameof(MusixmatchClient)}] Successful login!",ConsoleColor.Green);
                return musixmatchClient;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
                Logger.Warn($"[{nameof(MusixmatchClient)}] login retry...");
                return MusixmatchLogin();
            }
        }
    }
    internal class Spotify
    {
        private class Settings
        {
            public bool discordstatus = true;
            public bool lyricsOnly = false;
            public string spotifyClientId = "client_Id";
        }
        private const string CredentialsPath = "credentials.json";
        private const string SettingsPath = "Settings.json";
        public Spotify()
        {
            var settings = new Settings();
            if (File.Exists(SettingsPath))
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
            else
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings));
            }
            var clientId = settings.spotifyClientId;

            if (clientId == "client_Id")
            {
                Logger.Error("Please set client_Id in Settings.json");
                Console.ReadKey();
                Process.GetCurrentProcess().Kill();
            }
            if (!File.Exists(CredentialsPath))
            {
                APIsLogin.StartAuthentication(clientId, CredentialsPath).GetAwaiter();
                new Spotify();
                return;
            }
            Start(clientId);
        }
        public string trackName = "";
        public string Artists = "";
        public bool IsPlaying;
        Subtitles MuxixMatchSubtitles = null;
        Track MuxixMatchTrack;
        MusixmatchClient musixmatchClient;
        SpotifyClient spotifyClient;
        CurrentlyPlaying CurrentSong;
        CurrentlyPlayingContext currentContext;
        public void Start(string clientId)
        {
            spotifyClient = APIsLogin.SpotifyLogin(CredentialsPath, clientId).GetAwaiter().GetResult();
            musixmatchClient = APIsLogin.MusixmatchLogin();

            var me = spotifyClient.UserProfile.Current().GetAwaiter().GetResult();
            Console.ForegroundColor = ConsoleColor.Green;
            Logger.Info($"[{nameof(SpotifyClient)}] Welcome {me.DisplayName} ({me.Id}), you're authenticated!",ConsoleColor.Green);
            Console.ResetColor();
            var playlists = spotifyClient.PaginateAll(spotifyClient.Playlists.CurrentUsers().ConfigureAwait(false).GetAwaiter().GetResult()).GetAwaiter().GetResult();
            Logger.Info($"[{nameof(SpotifyClient)}] Total Playlists in your Account: {playlists.Count}");
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        currentContext = ((PlayerClient)spotifyClient.Player).GetCurrentPlayback().Result;
                        CurrentSong = spotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest(AdditionalTypes.Episode)).GetAwaiter().GetResult();
                        IsPlaying = CurrentSong == null ? false : CurrentSong.IsPlaying;
                        if (CurrentSong == null || !CurrentSong.IsPlaying) { Thread.Sleep(3000); continue; }
                        if (CurrentSong.Item == null)
                            continue;
                        var NewTrackName = ((FullTrack)CurrentSong.Item).Name;
                        if (trackName != NewTrackName)
                        {
                            trackName = NewTrackName;
                            Artists = "";
                            foreach (var item in ((FullTrack)CurrentSong.Item).Artists)
                            {
                                if (((FullTrack)CurrentSong.Item).Artists.Last().Name != item.Name)
                                    Artists += item.Name + ",";
                                else
                                    Artists += item.Name;
                            }
                            foreach (var artist in ((FullTrack)CurrentSong.Item).Artists)
                            {
                                var tracks = musixmatchClient.SongSearch(artist.Name, trackName);
                                if (tracks.Count < 1)
                                    break;
                                MuxixMatchTrack = tracks.First();
                                int trackId = MuxixMatchTrack.TrackId;
                                if (MuxixMatchTrack != null)
                                {
                                    try { MuxixMatchSubtitles = musixmatchClient.GetTrackSubtitles(trackId); } catch { MuxixMatchSubtitles = null; break; }
                                    break;
                                }
                            }
                        }
                    }
                    catch {}
                    Thread.Sleep(3000);
                }
            }).Start();
        }
        public bool IsRepeat()
        {
            if (CurrentSong == null)
                return false;
            if (currentContext?.RepeatState == "track")
                return true;
            return false;
        }

        public int TimeInMs()
        {
            if (CurrentSong == null || CurrentSong.ProgressMs == null)
                return 0;
            return (int)CurrentSong.ProgressMs;
        }

        public string TimeAndEndTime()
        {
            if (CurrentSong == null)
                return "";
            var durationMs = ((FullTrack)CurrentSong.Item).DurationMs;
            var currentlyMs = TimeInMs();
            TimeSpan currentt = TimeSpan.FromMilliseconds(currentlyMs);
            TimeSpan Durationt = TimeSpan.FromMilliseconds(durationMs);
            string currenttime = string.Format("{0:D2}:{1:D2}", currentt.Minutes, currentt.Seconds);
            string Durationtime = string.Format("{0:D2}:{1:D2}", Durationt.Minutes, Durationt.Seconds);
           return $"[{currenttime}/{Durationtime}]";
        }
        public string GetCurrentLine()
        {
            try
            {
                CurrentSong = spotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest(AdditionalTypes.Episode)).GetAwaiter().GetResult();
                if (CurrentSong == null || !CurrentSong.IsPlaying) { Thread.Sleep(3000); return ""; }

                if (CurrentSong.Item == null)
                    return "";
                if (MuxixMatchSubtitles != null)
                {
                    var currentlyMs = TimeInMs();
                    var curentLineLir = "";
                    var currentTimeSpanTs = (int)TimeSpan.FromMilliseconds(currentlyMs).TotalSeconds;
                    foreach (var item in MuxixMatchSubtitles.Lines)
                        if ((int)item.LyricsTime.TotalSeconds <= currentTimeSpanTs)
                            curentLineLir = item.Text;
                    return curentLineLir == "" ? "🎶" : $"{curentLineLir}";
                }
            }
            catch  { }
            return "";
        }
    }
}
