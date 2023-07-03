using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HR.Services.Hyperate {
    public class HyperateInstanceScraper {

        public static string ParseAttribute (string line, string name) {
            string pattern = $"{name}=\"";
            int begin = (line.IndexOf(pattern) + pattern.Length);
            int end = line.IndexOf('\"', begin);
            return line.Substring(begin, end - begin);
        }

        public static bool HasAttribute (string line, string name) {
            return line.Contains($"{name}=\"");
        }

        public static void TryParseAttribute (string line, string name, ref string field) {
            if (HasAttribute(line, name)) {
                field = ParseAttribute(line, name);
            } else {
                Logger.DebugWarn($"Missing attribute \"{name}\"");
            }
        }

        public class HyperateInstance {

            public readonly string html;
            public readonly string url;

            public HyperateInstance (string html, string url) {
                this.html = html;
                this.url = url;
            }

            public HyperateInstanceInfo ExtractInfo () {

                HyperateInstanceInfo info = new HyperateInstanceInfo(this);
                string[] lines = html.Split('\n');

                for (int i = 0; i < lines.Length; i++) {
                    string line = lines[i];

                    if (line.Contains("<meta ")) { // It's a meta tag
                        if (line.Contains("name=\"csrf-token\"")) { // It's the CSRF token

                            info.csrf = ParseAttribute(line, "content");
                        }
                    } else {
                        if (line.Contains("<div ")) {

                            // Make sure the div is our data container
                            if (!HasAttribute(line, "data-phx-main")) continue;

                            // Read the data
                            TryParseAttribute(line, "data-phx-session", ref info.phxSession);
                            TryParseAttribute(line, "data-phx-static", ref info.phxStatic);
                            TryParseAttribute(line, "data-phx-view", ref info.phxView);

                            // If 'data-phx-root-id' does not have any meaningful data, use 'id' instead.
                            TryParseAttribute(line, "data-phx-root-id", ref info.phxId);
                            if (info.phxId == null) TryParseAttribute(line, "id", ref info.phxId);
                        }
                    }
                }

                return info;
            }
        }

        public class HyperateInstanceInfo {
            public readonly HyperateInstance instance;
            public HyperateInstanceInfo (HyperateInstance instance) {
                this.instance = instance;
            }

            public string csrf;
            public string phxSession;
            public string phxStatic;
            public string phxView;
            public string phxId;
        }

        public static HyperateInstance RetrieveInstance (string url) {
            Logger.Info($"Retrieving Hyperate instance from \"{url}\"...");

            // Create the request
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "GET";

            // Read response data
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {

                string result = streamReader.ReadToEnd();
                return new HyperateInstance(result, url);
            }
        }
    }
}
