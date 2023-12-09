using static WindowsMediaController.MediaManager;
using WindowsMediaController;
using Windows.Media.Control;
using HR;
using HR.Functions;
using MusixmatchClientLib;
using MusixmatchClientLib.Types;
using System.Xml.Linq;
using MusixmatchClientLib.Auth;

namespace HASToChat.Functions
{
    internal class MediaUtilities
    {

        private readonly MediaManager _mediaManager;


        public MediaUtilities(float WaitTime)
        {
                _musixmatchClient = MusixmatchLogin();
                _mediaManager = new MediaManager();
                if (_mediaManager.IsStarted)
                    _mediaManager.Dispose();
                _mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
                _mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
                _mediaManager.OnAnyPlaybackStateChanged += MediaManager_OnAnyPlaybackStateChanged;
                _mediaManager.OnAnyMediaPropertyChanged += MediaManager_OnAnyMediaPropertyChanged;
                _mediaManager.Start();


                new Waiter(delegate
                {
                    if (_currentSession == null || _currentSession.ControlSession == null)
                    {
                        return;
                    }
                    if (_originalTimeSpan != _currentSession.ControlSession.GetTimelineProperties().Position)
                    {
                        _originalTimeSpan = _currentSession.ControlSession.GetTimelineProperties().Position;
                        _timeSpan = _currentSession.ControlSession.GetTimelineProperties().Position;
                    }
                    else
                        _timeSpan = TimeSpan.FromMilliseconds(_timeSpan.TotalMilliseconds + 100);
                        var w = CurrentLine();
                    if(lastLine != w)
                    {
                        w = w == null ? "" : w;
                        OnNewLir?.Invoke(w);
                        lastLine = w;
                    }
                }, 100).Start();
            new Waiter(delegate
                {
                    foreach (var item in currentSessions)
                    {
                        if (item.Key.ControlSession.GetPlaybackInfo().PlaybackStatus != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                        {
                            _currentSession = item.Key;
                            _currentProperties = item.Value;
                            break;
                        }
                    }
                    if (_name == SongName() || string.Empty == SongName())
                    {
                        return;
                    }
                    if (_currentProperties != null)
                    {
                        _name = SongName();
                        _artist = Artist();
                        var mbname = _currentProperties.Title.Contains("-") ? _currentProperties.Title.Split("-")[1] : _currentProperties.Title;
                        _MuxixmatchSubtitles = GetSubtitlesMX(mbname, _artist);
                        if (_MuxixmatchSubtitles == null)
                        {
                            mbname = _currentProperties.Title.Contains("-") ? _currentProperties.Title.Split("-")[0] : _currentProperties.Title;
                            _MuxixmatchSubtitles = GetSubtitlesMX(mbname, _artist);
                        }
                        //LyricsLine lyricsLine = new LyricsLine();
                        if (_MuxixmatchSubtitles != null && _MuxixmatchSubtitles.Lines != null)
                            for (int i = 0; i < _MuxixmatchSubtitles.Lines.ToArray().Length; i++)
                            {
                                if (i != 0)
                                    if ((_MuxixmatchSubtitles.Lines[i].LyricsTime.TotalSeconds - _MuxixmatchSubtitles.Lines[i - 1].LyricsTime.TotalSeconds) < WaitTime)
                                    {
                                        _MuxixmatchSubtitles.Lines[i].Text = _MuxixmatchSubtitles.Lines[i - 1].Text + ". " + _MuxixmatchSubtitles.Lines[i].Text;
                                        _MuxixmatchSubtitles.Lines[i].LyricsTime = _MuxixmatchSubtitles.Lines[i - 1].LyricsTime;
                                        _MuxixmatchSubtitles.Lines.Remove(_MuxixmatchSubtitles.Lines[i - 1]);
                                    }
                            }
                        //foreach (var item in MuxixMatchSubtitles.Lines)
                        //{
                        //    if ((item.LyricsTime.TotalSeconds - lyricsLine.LyricsTime.TotalSeconds) > 2)
                        //    {
                        //        item.Text = item.Text + " " + lyricsLine.Text;
                        //        MuxixMatchSubtitles.Lines.Remove(lyricsLine);
                        //    }
                        //    lyricsLine = item;
                        //}
          
                        Thread.Sleep(3000);
                    }


                }, 100).Start();


        }
        public static MusixmatchClient MusixmatchLogin()
        {
            try
            {
                MusixmatchToken token;
                if (ConfigManager.Instance.Config.MusixmatchToken != "NULL")
                {
                    token = new MusixmatchToken(ConfigManager.Instance.Config.MusixmatchToken);
                    Logger.Info($"[{nameof(MusixmatchClient)}] Reusing saved token...");
                }
                else
                {
                    token = new MusixmatchToken();
                    Logger.Info($"[{nameof(MusixmatchClient)}] Generated a new token: {token}");
                }
                var musixmatchClient = new MusixmatchClient(token);
                musixmatchClient.GetUserWeeklyTop("BY"); // Test request to throw an exception when the token is invalid
                Logger.Info($"[{nameof(MusixmatchClient)}] Successful login!", ConsoleColor.Green);
                ConfigManager.Instance.Config.MusixmatchToken = token.ToString();
                ConfigManager.Instance.SaveConfig();
                return musixmatchClient;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
                Logger.Warn($"[{nameof(MusixmatchClient)}] Generating a new token and starting over...");
                ConfigManager.Instance.Config.MusixmatchToken = "NULL";
                Thread.Sleep(5000); // Wait 5 seconds before generating a new token
                return MusixmatchLogin();
            }
        }
        private string lastLine = "";
        string GetSubtitleStr(Subtitles sub, int currentlyMs)
        {
            var currentTimeSpanTs = (int)TimeSpan.FromMilliseconds(currentlyMs).TotalSeconds;
            var strLine = "";
            if (sub == null || sub.Lines.Count == 0)
            {
                return strLine;
            }
            foreach (var item in sub.Lines)
            {
                if ((int)item.LyricsTime.TotalSeconds <= currentTimeSpanTs)
                {
                    strLine = item.Text;
                }
            }
            return strLine;
        }
        private Subtitles GetSubtitlesMX(string name, string artist)
        {
            try
            {
                var tracks = _musixmatchClient.SongSearch(artist, name);
                if (tracks.Count < 1)
                {
                    return null;
                }
                var MuxixMatchTrack = tracks.First();
                int trackId = MuxixMatchTrack.TrackId;
                if (MuxixMatchTrack != null)
                {
                    return _musixmatchClient.GetTrackSubtitles(trackId);
                }
            }
            catch {}
            return null;

        }

        public bool HasLyric()
         => _MuxixmatchSubtitles != null;

        public GlobalSystemMediaTransportControlsSessionPlaybackStatus PlayType()
        {
            if (_currentSession != null && _currentSession.ControlSession != null)
            {
                return _currentSession.ControlSession.GetPlaybackInfo().PlaybackStatus;
            }
            else
            {
                return GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused;
            }
        }

        public string SongName()
            => _currentProperties?.Title;
        public string Artist()
               => _currentProperties?.Artist;
        public string CurrentLine()
        {
            var sub = GetSubtitleStr(_MuxixmatchSubtitles, (int)_timeSpan.TotalMilliseconds);
            if (sub == null)
            {
                return null;
            }
            return sub == string.Empty ? "🎶" : sub;
        }
        public string TimeAndEndTime()
        {
            if (_currentSession == null || _currentSession.ControlSession == null)
            {
                return "";
            }
            TimeSpan CurrentTime = _timeSpan;
            TimeSpan DurationTime = _currentSession.ControlSession.GetTimelineProperties().EndTime;
            string currentTimeStr = string.Format("{0:D2}:{1:D2}", CurrentTime.Minutes, CurrentTime.Seconds);
            string DurationTimeStr = string.Format("{0:D2}:{1:D2}", DurationTime.Minutes, DurationTime.Seconds);
            return $"[{currentTimeStr}/{DurationTimeStr}]";
        }
        private string _name = "";
        private string _artist = "";
        public event Action<string> OnNewLir;
        private GlobalSystemMediaTransportControlsSessionMediaProperties _currentProperties = null;
        private MediaSession _currentSession = null;
        private MusixmatchClient _musixmatchClient;
        private Subtitles _MuxixmatchSubtitles;
        private TimeSpan _originalTimeSpan;
        private TimeSpan _timeSpan;
        #region Credits to dubya dude for this
        private void MediaManager_OnAnySessionOpened(MediaManager.MediaSession session)
          => currentSessions.Add(session, null);
        private void MediaManager_OnAnySessionClosed(MediaManager.MediaSession session)
          =>  currentSessions.Remove(session);
        private void MediaManager_OnAnyPlaybackStateChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionPlaybackInfo args)
        {}
        private void MediaManager_OnAnyMediaPropertyChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionMediaProperties args)
        {
            if (currentSessions.Keys.Contains(sender))
                currentSessions[sender] = args;
        }
        public static Dictionary<MediaManager.MediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties> currentSessions = new Dictionary<MediaManager.MediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties>();

        #endregion
    }
}
