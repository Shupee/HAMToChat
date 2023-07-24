using BuildSoft.VRChat.Osc.Chatbox;
using HAMToChat.Functions;
using HASToChat.Functions;
using HR.Functions;
using System.Diagnostics;
using Windows.Media.Control;

namespace HR
{
    public partial class ToChat : Form
    {
        readonly Config _config;
        readonly Tense _tense = new(60, 120);
        readonly HRToChat hRToChat;
        readonly MediaUtilities mediaUtilities = new();
        readonly Waiter waiter;
        bool blink;
        public ToChat()
        {
            var cfg = "Config.json";
            if (File.Exists(cfg))
                _config = new(cfg);
            else
            {
                _config = new(cfg);
                _config.SerializeCfg();
            }

            try
            {
                if (_config.ApiHB == "vardAPI")
                    hRToChat = new HRToChat(_config);
                else if (_config.ApiHB == "pulsoid")
                    new ByVard();
                InitializeComponent();
                if (_config.ApiHB == "NULL")
                    checkBox1.Enabled = false;
                if (_config.ApiHB == "NULL")
                    checkBox2.Enabled = false;

                checkBox1.Checked = _config.BPMToChat;
                checkBox2.Checked = _config.Stress;
                checkBox5.Checked = _config.SPArt;
                checkBox4.Checked = _config.SPLyr;
                checkBox3.Checked = _config.SPName;
                checkBox6.Checked = _config.activity;
                checkBox7.Checked = _config.SPTime;
                checkBox8.Checked = _config.UWS;
                double ms = 1000;
                int st = 2;
                mediaUtilities.OnNewLir += new Action<string>((string str) =>
                {
                    if (_config.UWS && _config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        Update(str);
                });
                waiter = new Waiter(delegate
                {
                    st++;
                    if (_config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing && !string.IsNullOrEmpty(mediaUtilities.CurrentLine()))
                    {
                        if (st == 3)
                        {
                            ms = 1480;
                            st = 0;
                        }
                        else
                            ms = 1380;
                        blink = !blink;
                    }
                    else
                        ms = 1800;
                    waiter.ChangeTime(ms);
                    if (_config.UWS && _config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        return;
                    Update();
                }, 1800);
                waiter.Start();
                if (!Program.IsConsole)
                    this.FormClosed += delegate { Process.GetCurrentProcess().Kill(); };
            }
            catch (Exception ex)
            {

                File.WriteAllText("log.txt", ex.ToString());
            }

        }
        void Update(string str = "")
        {

            string sendLine = "";
            if (File.Exists("text.txt"))
            {
                var txt = File.ReadAllLines("text.txt");
                if (txt.Length > 0 && txt[0] != string.Empty)
                    foreach (var item in txt)
                        AddInfo(ref sendLine, item);
            }

            if (_config.BPMToChat && _config.ApiHB != "null")
            {
                int hr = _config.ApiHB == "vardAPI" ? ByVard.HR : hRToChat.HR;
                AddInfo(ref sendLine, "❤️" + hr + " BPM" + (_config.Stress ? $"|Tense: {_tense.GetStress(hr)}%" : ""));
            }

            if (_config.activity)
                AddInfo(ref sendLine, $"now in \"{GetCurrentWindow.GetActiveWindowTitle()}\"");

            if (mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
            {
                if (_config.SPName || _config.SPLyr || _config.SPArt || _config.SPTime)
                {
                    AddInfo(ref sendLine, "");
                    if (_config.SPTime)
                        AddInfo(ref sendLine, $"{mediaUtilities.TimeAndEndTime()}");
                    if (_config.SPName)
                        AddInfo(ref sendLine, $"🎵{mediaUtilities.SongName()}🎵");
                    if (_config.SPArt)
                        AddInfo(ref sendLine, $"by {mediaUtilities.Artist()}");
                    if (_config.SPLyr && mediaUtilities.CurrentLine() != null)
                        if (str != string.Empty)
                            AddInfo(ref sendLine, $">{str}" + (blink ? "_" : ""));
                        else
                            AddInfo(ref sendLine, $">{mediaUtilities.CurrentLine()}" + (blink ? "_" : ""));
                }
            }
            if (sendLine != string.Empty)
                OscChatbox.SendMessage(sendLine, direct: true);
        }
        void AddInfo(ref string mainline, string lineAdd)
        {
            if (lineAdd == null)
                return;
            if (mainline != string.Empty)
                mainline += "";
            mainline += lineAdd;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _config.BPMToChat = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _config.Stress = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            _config.SPArt = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _config.SPLyr = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            _config.SPName = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            _config.activity = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            _config.SPTime = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            _config.UWS = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }
    }

}