using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources.Wrappers
{
    //
    // Wrapper class
    //
    [DataContract]
    public class TransferInfo
    {
        [DataMember(Name = "transfer", EmitDefaultValue = false)]
        public Transfer Transfer { get; set; }
    }
}
