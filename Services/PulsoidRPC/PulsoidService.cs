using HR.Shared;

namespace HR.Services.PulsoidRPC {
    public class PulsoidService : IHeartbeatServiceFactory {
        public string GetIdentifier() {
            return "pulsoid";
        }

        public string GetName() {
            return "Pulsoid RPC";
        }

        public IHeartbeatService CreateService(KVArgs args) {

            /*string input = null;
            if (args.Count > 0) {
                input = args.StealFirst();
            } else {
                Logger.Error("Cannot create service: No input provided.");
                return null;
            }*/

            /*Guid guid;
            if (input.Contains("pulsoid.net")) {
                guid = PulsoidRPC.ParseSuffixGuid(input);
            } else {
                guid = Guid.Parse(input);
            }*/

            if (!args.Require("input", out string input)) return null;

            Guid guid;
            if (input.Contains("pulsoid.net")) {
                guid = PulsoidRPC.ParseSuffixGuid(input);
            } else {
                guid = Guid.Parse(input);
            }

            var ramiel = PulsoidRPC.RetrieveRamielID(guid);
            if (!ramiel.HasValue) {
                Logger.Error("Cannot create service: RPC returned no ramiel stream.");
                Logger.Warn("It seems the Pulsoid service does not know this widget. Make sure what you entered is correct and try again.");
                return null;
            }

            var wssURL = PulsoidRPC.CreateRamielURL(ramiel.Value);
            PulsoidListener listener = new PulsoidListener();
            listener.Connect(wssURL);

            return listener;
        }
    }
}
