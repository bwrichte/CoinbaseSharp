using System.Runtime.Serialization;

namespace CoinbaseSharp.Authentication
{
    [DataContract]
    public class APIKey
    {
        public APIKey()
        {
        }

        public APIKey(string api_key)
        {
            KeyValue = api_key;
        }

        [DataMember(Name = "api_key")]
        public string KeyValue { get; set; }

        public override string ToString()
        {
            return KeyValue;
        }
    }
}
