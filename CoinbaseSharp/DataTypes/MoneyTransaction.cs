using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class MoneyTransaction
    {
        [IgnoreDataMember]
        public Amount Amount
        {
            get
            {
                if (AmountString == null || AmountCurrencyISO == null)
                {
                    return null;
                }
                return new Amount() { AmountValue = decimal.Parse(AmountString, NumberStyles.Any), Currency = AmountCurrencyISO };
            }
            set
            {
                AmountString = value.AmountValue.ToString();
                AmountCurrencyISO = value.Currency;
            }
        }

        [DataMember(Name = "amount_string")]
        public string AmountString { get; set; }

        [DataMember(Name = "amount_currency_iso")]
        public string AmountCurrencyISO { get; set; }

        [DataMember(Name = "notes", EmitDefaultValue = false)]
        public string Notes { get; set; }
    }
}
