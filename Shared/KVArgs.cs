using System.Collections;
using System.Web;

namespace HR.Shared {
    public class KVArgs : IReadOnlyDictionary<string, string> {
        protected List<string> arguments = new List<string>();
        protected Dictionary<string, string> pairs = new Dictionary<string, string>();

        public IEnumerable<string> Keys => pairs.Keys;

        public IEnumerable<string> Values => pairs.Values;

        public int Count => pairs.Count;

        public string this[string key] => pairs[key];

        public KVArgs() { }
        public KVArgs(string[] args) {
            arguments = new List<string>(args);
            Parse();
        }

        public void WriteArguments(string[] args) {
            arguments = new List<string>(args);
            Parse();
        }

        public void Parse() {
            pairs = new Dictionary<string, string>();

            int index = 0;
            arguments.ForEach((argument) => {
                index++;

                if (argument.Contains("=")) {

                    var args = argument.Split('=');
                    if (args.Length == 2) {
                        string key = HttpUtility.UrlDecode(args[0]);
                        string value = HttpUtility.UrlDecode(args[1]);

                        if (!pairs.ContainsKey(key)) {
                            pairs.Add(key, value);
                        } else {
                            Logger.Warn($"Ignoring argument #{index}: Key \"{key}\" is already used!");
                        }

                    } else {
                        Logger.Warn($"Ignoring argument #{index}: Invalid number of elements in delimited string.");
                        return;
                    }

                } else {
                    Logger.DebugInfo($"#{index}: Found valueless key \"{argument}\"");
                    pairs.Add(argument, null);
                }
            });
        }

        public bool ContainsKey(string key) {
            return pairs.ContainsKey(key);
        }

        public bool TryGetValue(string key, out string? value) {
            return pairs.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return pairs.Keys.GetEnumerator();
        }

        public bool Has(string key) => ContainsKey(key);

        public bool HasFlag (string key) {
            if (!Has(key)) return false;

            // Not a flag if it has a value :)
            else {
                bool isFlag = Get(key) == null;
                if (!isFlag) Logger.DebugWarn($"Not classifying \"{key}\" as Flag because it has a value.");
                return isFlag;
            }
        }

        public string Get(string key) {
            if (Has(key)) return pairs[key];
            else return null;
        }

        public bool Require (string key, out string result) {
            if (Has(key)) {
                result = Get(key);
                return true;
            }
            else {
                Logger.Error($"Missing required Argument with key \"{key}\"!");

                result = null;
                return false;
            }
        }
    }
}
