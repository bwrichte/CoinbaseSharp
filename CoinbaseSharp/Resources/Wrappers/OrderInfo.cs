using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources.Wrappers
{
    //
    // Wrapper class
    //
    [DataContract]
    public class OrderInfo
    {
        [DataMember(Name = "order")]
        public Order Order { get; set; }
    }
}
