using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HR.Services.PulsoidRPC {
    public static class PulsoidRPC {

        public static string CreateRamielURL (Guid id) {
            return $"wss://ramiel.pulsoid.net/listen/{id}";
        }

        public static Guid ParseSuffixGuid (string url) {
            // Split the URL using '/' and parse the last element as a GUID
            return Guid.Parse(url.Split('/').Last());
        }

        public class RPCRequest {
            public string jsonrpc = "2.0";
            public string method;
            public Dictionary<string, string> @params = new Dictionary<string, string>();
            public string id;
        }
        public class RPCResponse {
            public string id;
            public RPCResult result;

            public static RPCResponse Parse(string json) {
                return JsonConvert.DeserializeObject<RPCResponse>(json);
            }

            public bool HasResult() {
                return (result != null);
            }
            public RPCResult GetResult() {
                return result;
            }

            public bool HasRamielURL () {
                return HasRamielURL(out bool _);
            }
            public bool HasRamielURL(out bool hasResult) {
                if (!(hasResult = HasResult())) return false;
                return GetResult().HasRamielURL();
            }
            public string GetRamielURL() {
                if (!HasResult()) return null;
                return GetResult().GetRamielURL();
            }
        }
        public class RPCResult {
            public string ramielUrl;

            public bool HasRamielURL() {
                return (ramielUrl != null);
            }
            public string GetRamielURL() {
                return ramielUrl;
            }
        }

        public static RPCResponse SendRPC (RPCRequest request) {

            // Create the request
            var rpcURL = "https://pulsoid.net/v1/api/public/rpc";
            var httpRequest = (HttpWebRequest)WebRequest.Create(rpcURL);
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";

            // Write request data
            using (var writer = new StreamWriter(httpRequest.GetRequestStream())) {
                string json = JsonConvert.SerializeObject(request);
                writer.Write(json);
            }

            // Read response data
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<RPCResponse>(result);
            }
        }

        public static Guid? RetrieveRamielID (Guid widget) {
            Logger.Info("Retrieving Ramiel ID from Pulsoid RPC...");

            // Create request
            RPCRequest request = new RPCRequest();
            request.@params.Add("widgetId", widget.ToString());
            request.id = Guid.NewGuid().ToString();
            request.method = "getWidget";

            // Send request
            RPCResponse response = SendRPC(request);

            // Check if everything went alright
            if (!response.HasRamielURL())
                return null;

            // Parse and return the ID
            var url = response.GetRamielURL();
            return ParseSuffixGuid(url);
        }
    }
}
