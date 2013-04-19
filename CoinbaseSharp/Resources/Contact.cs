using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Contact : CoinbaseResource
    {
        public Contact() : base() { }
        public Contact(List<string> errors) : base(errors) { }
        public Contact(WebException ex) : base(ex) { }

        [DataMember(Name = "email")]
        public string Email { get; set; }

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
                return string.Format("Invalid Contact: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Contact";
            }
            else
            {
                return Email;
            }
        }
    }
}
