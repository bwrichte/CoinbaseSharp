using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources.Wrappers
{
    //
    // Wrapper class
    //
    [DataContract]
    public class TransactionInfo
    {
        [DataMember(Name = "transaction", EmitDefaultValue = false)]
        public Transaction Transaction { get; set; }
    }
}
