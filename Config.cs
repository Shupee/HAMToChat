using Newtonsoft.Json;
using System.Diagnostics;

namespace HR
{
    public class Config
    {
        [NonSerialized]
        readonly string _cfgPath;

        public Config(string CfgPath)
        {
            Console.WriteLine(CfgPath);
            _cfgPath = CfgPath;
            if (File.Exists(CfgPath))
            {
                var Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CfgPath));
                if (Instance == null) return;
                BPMToChat = Instance.BPMToChat;
                Stress = Instance.Stress;
                Token = Instance.Token;
                SPTime = Instance.SPTime;
                SPName = Instance.SPName;
                SPLyr = Instance.SPLyr;
                SPArt = Instance.SPArt;
                activity = Instance.activity;
                UWS = Instance.UWS;
                ApiHB = Instance.ApiHB;
            }
     
        }
        public void SerializeCfg()
            => File.WriteAllText(_cfgPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        public bool BPMToChat, Stress, activity, UWS;
        public string ApiHB = "NULL";
        public bool SPTime, SPName, SPLyr, SPArt;
        public string? Token = "NULL";
    }
}
