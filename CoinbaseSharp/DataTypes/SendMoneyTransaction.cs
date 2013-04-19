using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes
{
    [DataContract]
    public class SendMoneyTransaction : MoneyTransaction
    {
        [DataMember(Name = "to")]
        public string ToAddr { get; set; }

        public override string ToString()
        {
            return string.Format("Send {0} to {1}", Amount, ToAddr);
        }
    }
}
