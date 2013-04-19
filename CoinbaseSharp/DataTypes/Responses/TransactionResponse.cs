
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class TransactionResponse : CoinbaseResponse
    {
        public TransactionResponse() : base() { }
        public TransactionResponse(List<string> errors) : base(errors) { }
        public TransactionResponse(WebException ex) : base(ex) { }

        [DataMember(Name = "transaction")]
        public Transaction Transaction
        {
            get
            {
                return (Transaction)Resource;
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
                return typeof(Transaction);
            }
        }
    }
}
