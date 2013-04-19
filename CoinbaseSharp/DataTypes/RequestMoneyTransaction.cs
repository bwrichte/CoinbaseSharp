using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class RequestMoneyTransaction : MoneyTransaction
    {
        [DataMember(Name = "from")]
        public string FromAddr { get; set; }

        public override string ToString()
        {
            return string.Format("Request {0} from {1}", Amount, FromAddr);
        }
    }
}
