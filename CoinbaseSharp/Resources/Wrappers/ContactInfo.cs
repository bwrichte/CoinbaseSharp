using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources.Wrappers
{
    //
    // Wrapper class
    //
    [DataContract]
    public class ContactInfo
    {
        [DataMember(Name = "contact")]
        public Contact Contact { get; set; }
    }
}
