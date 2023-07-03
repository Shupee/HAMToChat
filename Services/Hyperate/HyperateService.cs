using HR.Shared;

namespace HR.Services.Hyperate {
    public class HyperateService : IHeartbeatServiceFactory {
        public string GetIdentifier() {
            return "hyperate";
        }

        public string GetName() {
            return "HypeRate";
        }

        public IHeartbeatService CreateService(KVArgs args) {

            /*string device = null;
            if (args.Count > 0) {
                device = args.StealFirst();
            }*/

            if (!args.Require("device", out string device)) return null;

            HyperateSocket socket = new HyperateSocket();
            socket.JoinDeviceChannel(device);

            return socket;
        }
    }
}
