using HR.Services.Hyperate.PhoenixMockup;
using System.Timers;
using Timer = System.Timers.Timer;

namespace HR.Services.Hyperate {
    public class HyperateSocket : PhoenixSocket, IHeartbeatService {
        protected static string accessKey = "API_KEY_HERE";

        protected override void HandlePhoenixMessage(PhoenixMessage message, string json) {
            if (message.GetEvent() == "hr_update") {
                if (message.HasPayloadValue("hr")) {

                    int hr = (int)message.GetPayloadValue<long>("hr");
                    OnHeartbeat.SafeInvoke(hr);

                } else {
                    Logger.DebugWarn($"Received hr_update message without hr value in payload: {json}");
                }

            } else if (message.GetEvent() == "phx_reply") {
                if (message.HasPayloadValue("status")) {
                    string status = message.GetPayloadValue<string>("status");
                    
                    if (message.GetRef() == 64) {
                        // Reply is for Join message

                        if (status == "error") {
                            Logger.Error($"Could not join Phoenix channel: {json}");
                        } else {
                            if (status != "ok") {
                                Logger.Warn($"Received unknown request reply: {json}");
                            } else {
                                Logger.Info("Successfully joined Phoenix channel!", ConsoleColor.Green);
                            }
                        }
                    } else {

                        if (status == "error") {
                            Logger.Error($"Received negative status reply: {json}");
                        } else {
                            if (status != "ok") {
                                Logger.Warn($"Received unknown status reply: {json}");
                            }
                        }
                    }

                } else {
                    Logger.DebugWarn($"Received phx_reply message without status in payload: {json}");
                }
            }
        }

        public void SendConnectionHeartbeat (System.Timers.Timer owner) {

            if (GetState() != System.Net.WebSockets.WebSocketState.Open) {
                owner.Stop();
                return;
            }

            PhoenixMessage heartbeatMessage = new PhoenixMessage() {
                @event = "heartbeat", @ref = 0,
                topic = $"phoenix",
                payload = new Dictionary<string, object>()
            };

            SendPhoenixMessage(heartbeatMessage);
        }

        public void JoinDeviceChannel (string device = null) {
            if (device == null) {
                Logger.Warn("Device not specified! Using internal testing channel...");
                device = "internal-testing";

            } else {
                Logger.Info($"Joining Phoenix Channel \"{device}\"...");
            }

            PhoenixMessage joinMessage = new PhoenixMessage() {
                @event = "phx_join", @ref = 64,
                topic = $"hr:{device}",
                payload = new Dictionary<string, object>()
            };

            SendPhoenixMessage(joinMessage);

            Timer heartbeatTimer = new Timer();
            heartbeatTimer.Interval = 10000;
            heartbeatTimer.Elapsed += new ElapsedEventHandler((a, b) => SendConnectionHeartbeat(heartbeatTimer));
            heartbeatTimer.Start();
        }

        public HyperateSocket () {
            Connect($"wss://app.hyperate.io/socket/websocket?token={accessKey}");
        }

        public event Action<int> OnHeartbeat;
    }
}
