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
                    sendLine += "❤️" + hRToChat.HR + " BPM";
                if (_config.Stress)
                    sendLine += (_config.BPMToChat ? "" : "") + $"Tense: {_stress.GetStress(hRToChat.HR)}%";
                if (spotify.IsPlaying)
                {
                    if (_config.SPInf || _config.SPLyr || _config.SPArt)
                    {
                        if (_config.Stress || _config.BPMToChat)
                            sendLine += "";
                        if (_config.SPInf)
                        {
                            sendLine += $"{spotify.TimeAndEndTime()}" + (spotify.IsRepeat() ? "🔁" : "");
                            sendLine += "" + $"🎵{spotify.trackName}🎵";
                            if (_config.SPArt || _config.SPLyr)
                                sendLine += "";
                        }
                        if (_config.SPArt)
                        {
                            sendLine += $"by {spotify.Artists}";
                            if (_config.SPLyr)
                                sendLine += "";
                        }
                        if (_config.SPLyr)
                        {
                            sendLine += $">{spotify.GetCurrentLine()}" + (blink ? "_" : "");
                        }
                    }
                }
                OscChatbox.SendMessage(sendLine, direct: true);
            }, 1800);
            waiter.Start();
            if (!Program.IsConsole)
                this.FormClosed += delegate { Process.GetCurrentProcess().Kill(); };
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
    }

}