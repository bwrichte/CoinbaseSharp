
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class ReceiveAddress : CoinbaseResource
    {
        public ReceiveAddress() : base() { }
        public ReceiveAddress(List<string> errors) : base(errors) { }
        public ReceiveAddress(WebException ex) : base(ex) { }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "callback_url", EmitDefaultValue = false)]
        public string CallbackURL { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return Address != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid ReceiveAddress: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete ReceiveAddress";
            }
            else
            {
                return Address;
            }
        }
    }
}
