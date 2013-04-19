
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class User : CoinbaseResource
    {
        public User() : base() { }
        public User(List<string> errors) : base(errors) { }
        public User(WebException ex) : base(ex) { }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string ID { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "receive_address", EmitDefaultValue = false)]
        public string ReceiveAddress { get; set; }

        [DataMember(Name = "time_zone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        [DataMember(Name = "native_currency", EmitDefaultValue = false)]
        public string NativeCurrency { get; set; }

        [DataMember(Name = "balance", EmitDefaultValue = false)]
        public Amount Balance { get; set; }

        [DataMember(Name = "buy_level", EmitDefaultValue = false)]
        public int BuyLevel { get; set; }

        [DataMember(Name = "sell_level", EmitDefaultValue = false)]
        public int SellLevel { get; set; }

        [DataMember(Name = "buy_limit", EmitDefaultValue = false)]
        public Amount BuyLimit { get; set; }
        
        [DataMember(Name = "sell_limit", EmitDefaultValue = false)]
        public Amount SellLimit { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return Email != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid User: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete User";
            }
            else if (Name != null)
            {
                return Name;
            }
            else
            {
                return Email;
            }
        }
    }
}
