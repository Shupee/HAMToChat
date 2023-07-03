using HR.Shared;
using Newtonsoft.Json;

namespace HR.Services.PulsoidAPI {
    public class PulsoidListener : GenericWebSocketListener, IHeartbeatService {
        protected override void HandleMessageReceived(string text) {
            var message = JsonConvert.DeserializeObject<WSSMessage>(text);
            if (message.data == null) return;

            OnHeartbeat.SafeInvoke(message.data.heart_rate);
        }

        public event Action<int> OnHeartbeat;

        public class WSSMessage {
            public long measured_at;
            public WSSHeartbeatData data;
        }
        public class WSSHeartbeatData {
            public int heart_rate;
        }
    }
}
