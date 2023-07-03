using HR.Shared;
using Newtonsoft.Json;

namespace HR.Services.Hyperate.PhoenixMockup {
    public class PhoenixSocket : GenericWebSocketListener {
        protected virtual void HandlePhoenixMessage (PhoenixMessage message, string json) { }

        public void SendPhoenixMessage (PhoenixMessage message) {
            string json = JsonConvert.SerializeObject (message);
            SendMessage(json);
        }

        protected override void HandleMessageReceived(string text) {
            PhoenixMessage message = JsonConvert.DeserializeObject<PhoenixMessage>(text);
            HandlePhoenixMessage(message, text);
        }
    }
}
