using Newtonsoft.Json;
using System.Diagnostics;

namespace HR
{
    public class Config
    {
        public static Config DeserializeCfg(string json)
        {
            var configDeserialized = JsonConvert.DeserializeObject<Config>(json);
            var configDefault = new Config(); // there's a better approach I'm sure
            return new Config
            {
                activity = configDeserialized.activity,
                ApiHB = configDeserialized.ApiHB ?? configDefault.ApiHB,
                BPMToChat = configDeserialized.BPMToChat,
                MusixmatchToken = configDeserialized.MusixmatchToken ?? configDefault.MusixmatchToken,
                SPArt = configDeserialized.SPArt,
                SPLyr = configDeserialized.SPLyr,
                SPName = configDeserialized.SPName,
                SPTime = configDeserialized.SPTime,
                Stress = configDeserialized.Stress,
                Token = configDeserialized.Token ?? configDefault.Token,
                UWS = configDeserialized.UWS
            };
        }

        public string SerializeCfg() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public bool BPMToChat, Stress, activity, UWS;
        public string ApiHB = "NULL";
        public bool SPTime, SPName, SPLyr, SPArt;
        public string? Token = "NULL";
        public string? MusixmatchToken = "NULL";
    }
}
