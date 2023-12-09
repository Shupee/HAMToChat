using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    internal class ConfigManager
    {
        private static readonly object _lockObject = new();
        private static ConfigManager _instance;
        public const string ConfigPath = "Config.json";
        private readonly string _configPath = ConfigPath;
        public Config Config { get; set; } = new Config();
        //
        public ConfigManager(string configPath)
        {
            this._configPath = configPath;
            if (File.Exists(configPath))
            {
                LoadConfig();
            }
            else
            {
                SaveConfig();
            }
        }

        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        _instance ??= new ConfigManager(ConfigPath);
                    }
                }
                return _instance;
            }
        }

        public void LoadConfig() => Config = Config.DeserializeCfg(File.ReadAllText(_configPath));
        public void SaveConfig() => File.WriteAllText(_configPath, Config.SerializeCfg());
    }
}
