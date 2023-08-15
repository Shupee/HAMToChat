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
        readonly Tense _tense = new(60, 120);
        readonly HRToChat hRToChat;
        readonly MediaUtilities mediaUtilities = new();
        readonly Waiter waiter;
        bool blink;
        public ToChat()
        {
            try
            {
                if (ConfigManager.Instance.Config.ApiHB == "pulsoid")
                    hRToChat = new HRToChat();
                else if (ConfigManager.Instance.Config.ApiHB == "vardAPI")
                    new BySoc(false);
                else if(ConfigManager.Instance.Config.ApiHB == "Ismb7")
                    new BySoc(true);
                InitializeComponent();
                if (ConfigManager.Instance.Config.ApiHB == "NULL")
                    checkBox1.Enabled = false;
                if (ConfigManager.Instance.Config.ApiHB == "NULL")
                    checkBox2.Enabled = false;

                checkBox1.Checked = ConfigManager.Instance.Config.BPMToChat;
                checkBox2.Checked = ConfigManager.Instance.Config.Stress;
                checkBox5.Checked = ConfigManager.Instance.Config.SPArt;
                checkBox4.Checked = ConfigManager.Instance.Config.SPLyr;
                checkBox3.Checked = ConfigManager.Instance.Config.SPName;
                checkBox6.Checked = ConfigManager.Instance.Config.activity;
                checkBox7.Checked = ConfigManager.Instance.Config.SPTime;
                checkBox8.Checked = ConfigManager.Instance.Config.UWS;
                double ms = 3600;
                int st = 2;
                mediaUtilities.OnNewLir += new Action<string>((string str) =>
                {
                    if (ConfigManager.Instance.Config.UWS && ConfigManager.Instance.Config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        Update(str);
                });
                waiter = new Waiter(delegate
                {
                    st++;
                    if (ConfigManager.Instance.Config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing && !string.IsNullOrEmpty(mediaUtilities.CurrentLine()))
                    {
                        if (st == 3)
                        {
                            ms = 3400;
                            st = 0;
                        }
                        else
                            ms = 2480;
                        blink = !blink;
                    }
                    else
                        ms = 3600;
                    waiter.ChangeTime(ms);
                    if (ConfigManager.Instance.Config.UWS && ConfigManager.Instance.Config.SPLyr && mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
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

            if (ConfigManager.Instance.Config.BPMToChat && ConfigManager.Instance.Config.ApiHB != "null")
            {
                int hr;
                if (ConfigManager.Instance.Config.ApiHB == "vardAPI" || ConfigManager.Instance.Config.ApiHB == "Ismb7")
                    hr = BySoc.HR;
                else
                    hr = hRToChat.HR;
                AddInfo(ref sendLine, "❤️" + hr + " BPM" + (ConfigManager.Instance.Config.Stress ? $"|Tense: {_tense.GetStress(hr)}%" : ""));
            }

            if (ConfigManager.Instance.Config.activity)
                AddInfo(ref sendLine, $"now in \"{GetCurrentWindow.GetActiveWindowTitle()}\"");

            if (mediaUtilities.PlayTipe() == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
            {
                if (ConfigManager.Instance.Config.SPName || ConfigManager.Instance.Config.SPLyr || ConfigManager.Instance.Config.SPArt || ConfigManager.Instance.Config.SPTime)
                {
                    AddInfo(ref sendLine, "");
                    if (ConfigManager.Instance.Config.SPTime)
                        AddInfo(ref sendLine, $"{mediaUtilities.TimeAndEndTime()}");
                    if (ConfigManager.Instance.Config.SPName)
                        AddInfo(ref sendLine, $"🎵{mediaUtilities.SongName()}🎵");
                    if (ConfigManager.Instance.Config.SPArt)
                        AddInfo(ref sendLine, $"by {mediaUtilities.Artist()}");
                    if (ConfigManager.Instance.Config.SPLyr && mediaUtilities.CurrentLine() != null)
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
            ConfigManager.Instance.Config.BPMToChat = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.Stress = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.SPArt = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.SPLyr = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.SPName = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.activity = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.SPTime = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Instance.Config.UWS = ((CheckBox)sender).Checked;
            ConfigManager.Instance.SaveConfig();
        }
    }

}