namespace HR.Services.Hyperate.PhoenixMockup {
    public class PhoenixMessage {

        public int? @ref;
        public string topic;
        public string @event;
        public Dictionary<string, object> payload;

        public void SetRef (int? _ref) {
            @ref = _ref;
        }
        public int? GetRef () {
            return @ref;
        }

        public void SetTopic (string topic) {
            this.topic = topic;
        }
        public string GetTopic () {
            return topic;
        }

        public void SetEvent (string _event) {
            @event = _event;
        }
        public string GetEvent () {
            return @event;
        }

        public void SetPayload (Dictionary<string, object> _payload) {
            payload = _payload;
        }
        public Dictionary<string, object> GetPayload () {
            return payload;
        }

        public void SetPayloadValue (string key, object value) {
            if (payload == null) payload = new Dictionary<string, object>();
            payload.Add(key, value);
        }
        public bool HasPayloadValue (string key) {
            if (payload == null) return false;
            return payload.ContainsKey(key);
        }
        public object GetPayloadValue (string key) {
            if (payload == null) return null;
            payload.TryGetValue(key, out object value);
            return value;
        }
        public T GetPayloadValue <T> (string key) {
            if (payload == null) return default;
            payload.TryGetValue(key, out object value);
            
            try {
                return (T)payload[key];
            } catch {
                Logger.Warn($"Conversion failure: Key=\"{key}\", typeof(T)={typeof(T).FullName}, typeof(value)={value.GetType().FullName}");
                return default;
            }
        }
    }
}
