
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Currency : CoinbaseResource
    {
        public Currency() : base() { }
        public Currency(List<string> errors) : base(errors) { }
        public Currency(WebException ex) : base(ex) { }

        [DataMember(Name = "iso_code")]
        public string ISO { get; set;  }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return ISO != null && Name != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Currency: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Currency";
            }
            else
            {
                return Name;
            }
        }
    }
}
