using HR.Services.PulsoidAPI;
using HR.Shared;
using System.Diagnostics;

namespace HR.Functions
{
    internal class HRToChat
    {
        public int HR = 0;
        public event Action<int>? OnHB;
        public HRToChat(Config config)
        {
            if (config.Token == null)
            {
            IL_0:
                Logger.Warn("Enter Token:");
                if (!Program.IsConsole)
                {
                    MessageBox.Show(
                        "Open Config.json and enter token",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    new Config("Config.json").SerializeCfg();
                    Process.GetCurrentProcess().Kill();
                }

                string tokenTry = Console.ReadLine();
                if (tokenTry == null || string.Empty == tokenTry)
                {
                    Logger.Error("Not valid token!! Try again");
                    goto IL_0;
                }
                config.Token = tokenTry;
                config.SerializeCfg();
            }
            var ArgParss = new KVArgs(new string[] { $"token={config.Token}" });
            var HBService = new PulsoidTokenService().CreateService(ArgParss);
            HBService.OnHeartbeat += new Action<int>((HBRes) =>
            {
                HR = HBRes;
                OnHB.Invoke(HBRes);
            });
        }
    }
}
