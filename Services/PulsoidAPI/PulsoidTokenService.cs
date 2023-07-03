using HR.Shared;

namespace HR.Services.PulsoidAPI {
    public class PulsoidTokenService : IHeartbeatServiceFactory {
        public string GetIdentifier() {
            return "pulsoid-token";
        }

        public string GetName() {
            return "Pulsoid Token";
        }

        public IHeartbeatService CreateService(KVArgs args) {

            /*Guid token = Guid.Empty;
            if (args.Count > 0) {
                token = Guid.Parse(args.StealFirst());
            } else {
                Logger.Error("Cannot create service: No token provided.");
                return null;
            }*/

            if (!args.Require("token", out string token)) return null;
            bool tokenOK = PulsoidTokenAPI.CheckTokenValidity(Guid.Parse(token));

            if (tokenOK) {
                PulsoidListener listener = new PulsoidListener();
                listener.Connect($"wss://dev.pulsoid.net/api/v1/data/real_time?access_token={token}");
                return listener;

            } else {
                Logger.Error("Service initialization failed.");
                return null;
            }
        }
    }
}
