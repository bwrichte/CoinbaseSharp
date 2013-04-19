using System;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class Price
    {
        public static readonly Price ZERO_PRICE = new Price() { Cents = 0, CurrencyISO = "USD" };

        [DataMember(Name = "cents")]
        public int Cents { get; set; }

        [DataMember(Name = "currency_iso")]
        public string CurrencyISO { get; set; }

        public override string ToString()
        {
            return string.Format("{0} cents in {1}", Cents, CurrencyISO);
        }
    }
}
