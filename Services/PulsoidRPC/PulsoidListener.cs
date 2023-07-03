using HR.Shared;
using Newtonsoft.Json;

namespace HR.Services.PulsoidRPC {
    public class PulsoidListener : GenericWebSocketListener, IHeartbeatService {
        protected override void HandleMessageReceived(string text) {
            var message = JsonConvert.DeserializeObject<WSSMessage>(text);
            if (message.data == null) return;

            OnHeartbeat.SafeInvoke(message.data.heartRate);
        }

        public event Action<int> OnHeartbeat;

        public class WSSMessage {
            public long timestamp;
            public WSSHeartbeatData data;
        }
        public class WSSHeartbeatData {
            public int heartRate;
        }
    }
}
