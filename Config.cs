using Newtonsoft.Json;

namespace HR
{
    public class Config
    {
        [NonSerialized]
        readonly string _cfgPath;

        public Config(string CfgPath)
        {
            _cfgPath = CfgPath;
            if (File.Exists(CfgPath))
            {
                var Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CfgPath));
                if (Instance == null) return;
                BPMToChat = Instance.BPMToChat;
                Stress = Instance.Stress;
                Token = Instance.Token;
                SPInf = Instance.SPInf;
                SPLyr = Instance.SPLyr;
                SPArt = Instance.SPArt;
            }
        }
        public void SerializeCfg()
            => File.WriteAllText(_cfgPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        public bool BPMToChat, Stress;
        public bool SPInf, SPLyr, SPArt;
        public string? Token;
    }
}
