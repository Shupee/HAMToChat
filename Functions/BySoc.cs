using System;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using System.Net.WebSockets;
using WebSocketSharp;

namespace HAMToChat.Functions
{
    //
    internal class BySoc
    {
        public static int HR = 0;
        public BySoc(bool IsMiband7)
        {
            new Thread(new ParameterizedThreadStart(async delegate
            {
                var server = new WebSocketServer(3228);
                server.AddWebSocketService<MyWebSocketService>("/");
                server.Start();
                await Task.Delay(1000);
                if (!IsMiband7)
                {
                    OpenBrowser("https://vard88508.github.io/vrc-osc-miband-hrm/html/");
                }
                else
                {
                    OpenBrowser("https://1sup4ik1.github.io/HAMToChat/mi7/");
                }

                Console.WriteLine("Waiting for connection from browser...");
                await Task.Delay(-1); // Wait indefinitely

            })).Start();

        }
        class MyWebSocketService : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                int.TryParse(e.Data, out HR);
            }
        }


        static void OpenBrowser(string url)
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}
