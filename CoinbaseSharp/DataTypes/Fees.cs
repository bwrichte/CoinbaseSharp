using System;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class Fees
    {
        [DataMember(Name = "coinbase")]
        public Price CoinbaseFee { get; set; }

        [DataMember(Name = "bank")]
        public Price BankFee { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Coinbase Fee = {0} and Bank Fee = {1}",
                CoinbaseFee,
                BankFee != null ? BankFee : Price.ZERO_PRICE
                );
        }
    }
}
