
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.DataTypes;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Transfer : CoinbaseResource
    {
        public Transfer() : base() { }
        public Transfer(List<string> errors) : base(errors) { }
        public Transfer(WebException ex) : base(ex) { }

        [DataMember(Name = "_type")]
        public string Type { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "transaction_id")]
        public string TransactionID { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt
        {
            get
            {
                return (TimeCreated != null) ? TimeCreated.ToString() : null;
            }
            set
            {
                if (value != null)
                {
                    TimeCreated = DateTime.Parse(value);
                }
            }
        }

        [IgnoreDataMember]
        public DateTime TimeCreated { get; set; }

        [DataMember(Name = "fees")]
        public Fees Fees { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "payout_date")]
        public string PayoutAt
        {
            get
            {
                return (PayoutTime != null) ? PayoutTime.ToString() : null;
            }
            set
            {
                if (value != null)
                {
                    PayoutTime = DateTime.Parse(value);
                }
            }
        }

        [IgnoreDataMember]
        public DateTime PayoutTime { get; set; }

        [DataMember(Name = "btc")]
        public Amount Bitcoins { get; set; }

        [DataMember(Name = "subtotal")]
        public Amount Subtotal { get; set; }

        [DataMember(Name = "total")]
        public Amount Total { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return Type != null && Status != null 
                    && Bitcoins != null && Subtotal != null 
                    && Total != null && Fees != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Transfer: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Transfer";
            }
            else
            {
                return string.Format("Transfer {0} for {1} by {2}", Bitcoins, Total, Type);
            }
        }
    }
}
