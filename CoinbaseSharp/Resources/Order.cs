
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.DataTypes;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Order : CoinbaseResource
    {
        public Order() : base() { }
        public Order(List<string> errors) : base(errors) { }
        public Order(WebException ex) : base(ex) { }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public string CreatedAt
        {
            get
            {
                return TimeCreated.ToString();
            }
            set
            {
                TimeCreated = DateTime.Parse(value);
            }
        }

        [IgnoreDataMember]
        public DateTime TimeCreated { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "total_btc")]
        public Price TotalBTC { get; set; }

        [DataMember(Name = "total_native")]
        public Price TotalNative { get; set; }

        [DataMember(Name = "custom")]
        public string Custom { get; set; }

        [DataMember(Name = "button")]
        public Button Button { get; set; }

        [DataMember(Name = "transaction")]
        public OrderTransaction Transaction { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return ID != null && TotalBTC != null 
                    && TotalNative != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Order: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Order";
            }
            else
            {
                return string.Format("{0} for {1} ({2})", ID, TotalBTC, TotalNative);
            }
        }
    }
}
