using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    internal class ConfigManager
    {
        private static readonly object lockObject = new object();
        private static ConfigManager instance;
        public const string ConfigPath = "Config.json";

        private string configPath = ConfigPath;
        public Config Config { get; set; } = new Config();
        //
        public ConfigManager(string configPath)
        {
            this.configPath = configPath;
            if (File.Exists(configPath))
                LoadConfig();
            else
                SaveConfig();
        }

        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigManager(ConfigPath);
                        }
                    }
                }
                return instance;
            }
        }

        public void LoadConfig() => Config = Config.DeserializeCfg(File.ReadAllText(configPath));
        public void SaveConfig() => File.WriteAllText(configPath, Config.SerializeCfg());
    }
}
