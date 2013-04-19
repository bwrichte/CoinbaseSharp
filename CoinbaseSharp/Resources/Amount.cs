
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Amount : CoinbaseResource
    {
        public Amount() : base() { }
        public Amount(List<string> errors) : base(errors) { }
        public Amount(WebException ex) : base(ex) { }

        [DataMember(Name = "amount")]
        public decimal AmountValue { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return AmountValue != null && Currency != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Amount: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Amount";
            }
            else
            {
                return string.Format("{0} {1}", AmountValue, Currency);
            }
        }
    }
}
