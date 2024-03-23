using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using WebSocketSharp;

namespace HR.Functions
{
    internal class HRToChat
    {
        public int HR = 0;
        public event Action<int>? OnHB;
        public HRToChat()
        {
            if (ConfigManager.Instance.Config.Token == null)
            {
            IL_0:
                Logger.Warn("Enter Token:");
                if (!Program.IsConsole)
                {
                    MessageBox.Show(
                        "Open Config.json and enter token",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    ConfigManager.Instance.Config = new() { Token = "" };
                    ConfigManager.Instance.SaveConfig();
                    Process.GetCurrentProcess().Kill();
                }
                string tokenTry = Console.ReadLine();
                if (tokenTry == null || string.Empty == tokenTry)
                {
                    Logger.Error("Not valid token!! Try again");
                    goto IL_0;
                }
                ConfigManager.Instance.Config.Token = tokenTry;
                ConfigManager.Instance.SaveConfig();

            }
            new Thread(new ParameterizedThreadStart(delegate
            {
                WebSocket webSocket = new WebSocket(GetMyBpmPlsToken(ConfigManager.Instance.Config.Token).Result);
                webSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                webSocket.Connect();
                webSocket.OnMessage += (sender, e) =>
                {
                    HR = JsonConvert.DeserializeObject<Data>(e.Data).data["heartRate"];
                };
            })).Start();

            //new Waiter(delegate {
            //    HR = GetMyBpmPlsAsync(ConfigManager.Instance.Config.Token).GetAwaiter().GetResult();
            //}, 2000).Start();
            //var ArgParss = new KVArgs(new string[] { $"token={config.Token}" });
            //var TokenService = new PulsoidTokenService();
            //Open(ArgParss, TokenService);
            //TokenService.listener.OnClose += delegate
            //{
            //    Open(ArgParss, TokenService);
            //};

        }
        //"https://pulsoid.net/v1/api/feed/ce72ae82-ca71-4545-b3a8-a2ec35700000"
        private static async Task<int> GetMyBpmPlsAsync(string feed)
        {
            var httpClient = new HttpClient();
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

            var uri = new Uri(feed);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(request);

            // Handle the response here
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MyBPM>(content).bpm;
            }
            else
            {
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
                return 0;
            }
        }
        static async Task<string> GetMyBpmPlsToken(string widgetId)
        {

            var url = "https://pulsoid.net/v1/api/public/rpc";
            var jsonBody = "{\"method\":\"getWidget\",\"jsonrpc\":\"2.0\",\"params\":{\"widgetId\":\"" + $"{widgetId}" + "\"},\"id\":\"" +
                $"{Guid.NewGuid().ToString()}" + "\"}";

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = content;

                // Установка параметра credentials в "include"
                httpClient.DefaultRequestHeaders.Add("credentials", "include");

                var response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject ojObject = (JObject)JObject.Parse(responseContent)["result"];
                    var ojObject1 = JObject.Parse(ojObject.ToString())["ramielUrl"];
                    return ojObject1.ToString();
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                }
                return "NOPE";
            }
        }
        //{"timestamp":1711186327935,"data":{"heartRate":82}}
        class MyBPM
        {
            public int bpm;
            public string measured_at;
        }    
        class Data
        {
            public long timestamp;
            public Dictionary<string,int> data;
        }
    }
}
