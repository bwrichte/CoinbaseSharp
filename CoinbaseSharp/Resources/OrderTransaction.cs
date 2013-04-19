
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class OrderTransaction : CoinbaseResource
    {
        public OrderTransaction() : base() { }
        public OrderTransaction(List<string> errors) : base(errors) { }
        public OrderTransaction(WebException ex) : base(ex) { }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "confirmations")]
        public int Confirmations { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return ID != null && Hash != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid OrderTransaction: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete OrderTransaction";
            }
            else
            {
                return string.Format("{0} (Hash = {1}", ID, Hash);
            }
        }
    }
}
