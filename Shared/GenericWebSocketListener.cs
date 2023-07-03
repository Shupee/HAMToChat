using System.Net.WebSockets;
using System.Text;

namespace HR.Shared {
    public class GenericWebSocketListener : IDisposable {
        public static bool logIO = false;

        protected ClientWebSocket? socket;
        protected Thread? thread;

        public event Action<string>? OnMessageReceived;
        protected virtual void HandleMessageReceived (string message) {}

        public event Action? OnClose;
        public event Action? OnOpen;

        public void SendMessage (string message) {
            if (socket == null) return;

            if (logIO)
                Logger.DebugInfo($"-> {message}");

            var type = WebSocketMessageType.Text; var token = CancellationToken.None;
            socket.SendAsync(message.ToArraySegmentBuffer(), type, true, token).Wait();
        }

        protected virtual void BeforeConnect (ClientWebSocket socket) { }
        protected virtual void AfterConnect (ClientWebSocket socket) { }

        public void Connect(string wssURL) {
            var uri = new Uri(wssURL);

            OnOpen.SafeInvoke();
            Logger.Info($"Connecting {GetType().Name} to \"{uri.Host}\"...");

            socket = new ClientWebSocket();
            BeforeConnect(socket);

            socket.ConnectAsync(uri, CancellationToken.None).Wait();
            AfterConnect(socket);

            thread = new Thread(() => {
                byte[] buffer = new byte[1024];
                ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer);

                while (socket.State == WebSocketState.Open) {
                    var packet = socket.ReceiveAsync(bufferSegment, CancellationToken.None).WaitForResult();

                    if (packet.MessageType == WebSocketMessageType.Close) {
                        Close(); return;

                    } else {
                        string text = Encoding.ASCII.GetString(bufferSegment.ToArray(), 0, packet.Count);

                        if (logIO)
                            Logger.DebugInfo($"<- {text}");

                        HandleMessageReceived(text);
                        OnMessageReceived.SafeInvoke(text);
                    }
                }
            });

            thread.Start();
        }

        public Thread GetThread() {
            return thread;
        }
        public ClientWebSocket GetSocket() {
            return socket;
        }
        public WebSocketState GetState () {
            if (socket == null) return WebSocketState.None;
            return socket.State;
        }

        public void WaitForConnection(int msTimeout = 100) {
            this.WaitWhile(msTimeout, () => socket == null);
        }

        public void WaitForClose(int msTimeout = 100) {
            this.WaitWhile(msTimeout, () => {
                if (socket == null) return false;
                return socket.State == WebSocketState.Open;
            });
        }

        public void Close() {
            OnClose?.SafeInvoke();
            Logger.Info($"Closing {GetType().Name}...");

            if (socket != null) {
                var status = WebSocketCloseStatus.NormalClosure;
                socket.CloseAsync(status, $"{GetType().Name} was closed.", CancellationToken.None).Wait();
            }
        }

        public void Dispose() {
            Logger.Info($"Disposing {GetType().Name}...");

            try {
                if (thread != null) {
                    Logger.Warn($"Interrupting {GetType().Name} thread...");
                    thread.Interrupt();
                }
            } finally {
                Close();
            }
        }
    }
}
