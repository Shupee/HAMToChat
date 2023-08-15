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

        private MediaManager mediaManager;


        public MediaUtilities()
        {
                musixmatchClient = MusixmatchLogin();
                mediaManager = new MediaManager();
                if (mediaManager.IsStarted)
                    mediaManager.Dispose();
                mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
                mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
                mediaManager.OnAnyPlaybackStateChanged += MediaManager_OnAnyPlaybackStateChanged;
                mediaManager.OnAnyMediaPropertyChanged += MediaManager_OnAnyMediaPropertyChanged;
                mediaManager.Start();


                new Waiter(delegate
                {
                    if (currentSession == null || currentSession.ControlSession == null)
                        return;
                    if (OriginalTimeSpan != currentSession.ControlSession.GetTimelineProperties().Position)
                    {
                        OriginalTimeSpan = currentSession.ControlSession.GetTimelineProperties().Position;
                        timeSpan = currentSession.ControlSession.GetTimelineProperties().Position;
                    }
                    else
                        timeSpan = TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds + 100);
                        var w = CurrentLine();
                    if(lastLine != w)
                    {
                        w = w == null ? "" : w;
                    if (OnNewLir != null)
                        OnNewLir.Invoke(w);
                        lastLine = w;
                    }
                }, 100).Start();
            new Waiter(delegate
                {
                    foreach (var item in currentSessions)
                    {
                        if (item.Key.ControlSession.GetPlaybackInfo().PlaybackStatus != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                        {
                            currentSession = item.Key;
                            currentproperties = item.Value;
                            break;
                        }
                    }
                    if (name == SongName() || string.Empty == SongName())
                        return;
                    if (currentproperties != null)
                    {
                        name = SongName();
                        artist = Artist();
                        var mbname = currentproperties.Title.Contains("-") ? currentproperties.Title.Split("-")[1] : currentproperties.Title;
                        MuxixMatchSubtitles = GetSubtitlesMX(mbname, artist);
                        if (MuxixMatchSubtitles == null)
                        {
                            mbname = currentproperties.Title.Contains("-") ? currentproperties.Title.Split("-")[0] : currentproperties.Title;
                            MuxixMatchSubtitles = GetSubtitlesMX(mbname, artist);
                        }
                        //LyricsLine lyricsLine = new LyricsLine();
                        if (MuxixMatchSubtitles != null && MuxixMatchSubtitles.Lines != null)
                            for (int i = 0; i < MuxixMatchSubtitles.Lines.ToArray().Length; i++)
                            {
                                if (i != 0)
                                    if ((MuxixMatchSubtitles.Lines[i].LyricsTime.TotalSeconds - MuxixMatchSubtitles.Lines[i - 1].LyricsTime.TotalSeconds) > 2.5f)
                                    {
                                        MuxixMatchSubtitles.Lines[i].Text = MuxixMatchSubtitles.Lines[i - 1].Text + ". " + MuxixMatchSubtitles.Lines[i].Text;
                                        MuxixMatchSubtitles.Lines[i].LyricsTime = MuxixMatchSubtitles.Lines[i - 1].LyricsTime;
                                        MuxixMatchSubtitles.Lines.Remove(MuxixMatchSubtitles.Lines[i - 1]);
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
                var musixmatchClient = new MusixmatchClient(new MusixmatchToken());
                Logger.Info($"[{nameof(MusixmatchClient)}] Successful login!", ConsoleColor.Green);
                return musixmatchClient;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
                Logger.Warn($"[{nameof(MusixmatchClient)}] login retry...");
                return MusixmatchLogin();
            }
        }
        string lastLine = "";
        string GetSubtitleStr(Subtitles sub, int currentlyMs)
        {
            var currentTimeSpanTs = (int)TimeSpan.FromMilliseconds(currentlyMs).TotalSeconds;
            var strline = "";
            if(sub == null || sub.Lines.Count == 0)
                return strline;
            foreach (var item in sub.Lines)
            {
                if ((int)item.LyricsTime.TotalSeconds <= currentTimeSpanTs)
                    strline = item.Text;
            }
            return strline;
        }
        Subtitles GetSubtitlesMX(string name, string artist)
        {
            try
            {
                var tracks = musixmatchClient.SongSearch(artist, name);
                if (tracks.Count < 1)
                    return null;
                var MuxixMatchTrack = tracks.First();
                int trackId = MuxixMatchTrack.TrackId;
                if (MuxixMatchTrack != null)
                    return musixmatchClient.GetTrackSubtitles(trackId);
            }
            catch{}
            return null;

        }

        public GlobalSystemMediaTransportControlsSessionPlaybackStatus PlayTipe()
        {
            if (currentSession != null && currentSession.ControlSession != null)
                return currentSession.ControlSession.GetPlaybackInfo().PlaybackStatus;
            else
                return GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused;
        }

        public string SongName()
            => currentproperties == null ? null : currentproperties.Title;
        public string Artist()
               => currentproperties == null ? null : currentproperties.Artist;
        public string CurrentLine()
        {
            var sub = GetSubtitleStr(MuxixMatchSubtitles, (int)timeSpan.TotalMilliseconds);
            if (sub == null)
                return null;
            return sub == string.Empty ? "🎶" : sub;
        }
        public string TimeAndEndTime()
        {
            if (currentSession == null || currentSession.ControlSession == null)
                return "";
            TimeSpan currentt = timeSpan;
            TimeSpan Durationt = currentSession.ControlSession.GetTimelineProperties().EndTime;
            string currenttime = string.Format("{0:D2}:{1:D2}", currentt.Minutes, currentt.Seconds);
            string Durationtime = string.Format("{0:D2}:{1:D2}", Durationt.Minutes, Durationt.Seconds);
            return $"[{currenttime}/{Durationtime}]";
        }
        string name = "";
        string artist = "";
        public event Action<string> OnNewLir;
        GlobalSystemMediaTransportControlsSessionMediaProperties currentproperties = null;
        MediaSession currentSession = null;
        MusixmatchClient musixmatchClient;
        Subtitles MuxixMatchSubtitles;
        TimeSpan OriginalTimeSpan;
        TimeSpan timeSpan;
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
