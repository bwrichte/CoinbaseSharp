using System.Runtime.Serialization;

namespace CoinbaseSharp.DataTypes.Requests
{
    [DataContract]
    public class SendMoneyTransactionRequest
    {
        [DataMember(Name = "transaction")]
        public SendMoneyTransaction Transaction { get; set; }
    }
}
