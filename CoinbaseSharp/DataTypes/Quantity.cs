
using System.Runtime.Serialization;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class Quantity
    {
        [DataMember(Name = "qty")]
        public decimal Value { get; set; }

        [DataMember(Name = "agree_btc_amount_varies", EmitDefaultValue = false)]
        public bool AgreeBTCAmountVaries { get; set; }

        public override string ToString()
        {
            return string.Format("{0} BTC", Value);
        }
    }
}
