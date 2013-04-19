
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class TransferResponse : CoinbaseResponse
    {
        public TransferResponse() : base() { }
        public TransferResponse(List<string> errors) : base(errors) { }
        public TransferResponse(WebException ex) : base(ex) { }

        [DataMember(Name = "transfer")]
        public Transfer Transfer
        {
            get
            {
                return (Transfer)Resource;
            }
            set
            {
                Resource = value;
            }
        }

        [IgnoreDataMember]
        protected override Type ResourceType
        {
            get
            {
                return typeof(Transfer);
            }
        }
    }
}
