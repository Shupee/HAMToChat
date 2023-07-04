using BuildSoft.VRChat.Osc.Chatbox;
using HR.Functions;
using System.Diagnostics;

namespace HR
{
    public partial class ToChat : Form
    {
        readonly Config _config = new("Config.json");
        readonly Stress _stress = new(60, 120);
        readonly HRToChat hRToChat;
        readonly Spotify spotify = new Spotify();
        readonly Waiter waiter;
        int st = 2;
        bool blink;
        public ToChat()
        {
            hRToChat = new HRToChat(_config);
            InitializeComponent();
            checkBox1.Checked = _config.BPMToChat;
            checkBox2.Checked = _config.Stress;
            checkBox5.Checked = _config.SPArt;
            checkBox4.Checked = _config.SPLyr;
            checkBox3.Checked = _config.SPInf;
            checkBox6.Checked = _config.activity;
            waiter = new Waiter(delegate
            {

                st++;
                if (_config.SPLyr && spotify.IsPlaying)
                {
                    if (st == 3)
                    {
                        waiter.ChangeTime(1280);
                        st = 0;
                    }
                    else
                        waiter.ChangeTime(1080);
                    blink = !blink;
                }
                else
                    waiter.ChangeTime(1800);

                string sendLine = "";
                if (_config.BPMToChat)
                    AddInfo(ref sendLine, "❤️" + hRToChat.HR + " BPM");
                if (_config.Stress)
                    AddInfo(ref sendLine, $"Tense: {_stress.GetStress(hRToChat.HR)}%");
                if (spotify.IsPlaying)
                {
                    if (_config.SPInf || _config.SPLyr || _config.SPArt)
                    {
                        AddInfo(ref sendLine, "");
                        if (_config.SPInf)
                        {
                            AddInfo(ref sendLine, $"{spotify.TimeAndEndTime()}" + (spotify.IsRepeat() ? "🔁" : ""));
                            AddInfo(ref sendLine, $"🎵{spotify.trackName}🎵");
                        }
                        if (_config.SPArt)
                            AddInfo(ref sendLine, $"by {spotify.Artists}");
                        if (_config.SPLyr)
                            AddInfo(ref sendLine, $">{spotify.GetCurrentLine()}" + (blink ? "_" : ""));
                    }
                }
                if (_config.activity)
                    AddInfo(ref sendLine, $"now in \"{GetCurrentWindow.GetActiveWindowTitle()}\"");
                OscChatbox.SendMessage(sendLine, direct: true);
            }, 1800);
            waiter.Start();
            if (!Program.IsConsole)
                this.FormClosed += delegate { Process.GetCurrentProcess().Kill(); };
        }
        void AddInfo(ref string mainline,string lineAdd)
        {
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
            _config.SPInf = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            _config.activity = ((CheckBox)sender).Checked;
            _config.SerializeCfg();
        }
    }

}