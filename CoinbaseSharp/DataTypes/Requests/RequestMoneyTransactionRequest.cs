using System.Runtime.Serialization;

namespace CoinbaseSharp.DataTypes.Requests
{
    [DataContract]
    public class RequestMoneyTransactionRequest
    {
        [DataMember(Name = "transaction")]
        public RequestMoneyTransaction Transaction { get; set; }
    }
}
