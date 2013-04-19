
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class OrderResponse : CoinbaseResponse
    {
        public OrderResponse() : base() { }
        public OrderResponse(List<string> errors) : base(errors) { }
        public OrderResponse(WebException ex) : base(ex) {  }

        [DataMember(Name = "order")]
        public Order Order
        {
            get
            {
                return (Order)Resource;
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
                return typeof(Order);
            }
        }
    }
}
