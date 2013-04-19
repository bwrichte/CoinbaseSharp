using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Transaction : CoinbaseResource
    {
        public Transaction() : base() { }
        public Transaction(List<string> errors) : base(errors) { }
        public Transaction(WebException ex) : base(ex) { }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string ID { get; set; }

        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public string CreatedAt
        {
            get
            {
                return TimeCreated == null ? null : TimeCreated.ToString();
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

        [DataMember(Name = "amount", EmitDefaultValue = false)]
        public Amount Amount { get; set; }

        [DataMember(Name = "request", EmitDefaultValue = false)]
        public bool IsRequest { get; set; }

        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        [DataMember(Name = "sender", EmitDefaultValue = false)]
        public User Sender { get; set; }

        [DataMember(Name = "recipient", EmitDefaultValue = false)]
        public User Recipient { get; set; }

        [DataMember(Name = "recipient_address", EmitDefaultValue = false)]
        public string RecipientAddress { get; set; }

        [DataMember(Name = "hsh", EmitDefaultValue = false)]
        public string Hash { get; set; }

        [DataMember(Name = "notes", EmitDefaultValue = false)]
        public string Notes { get; set; }

        [DataMember(Name = "confirmations", EmitDefaultValue = false)]
        public int Confirmations { get; set; }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return ID != null && Sender != null 
                    && (Recipient != null || RecipientAddress != null)
                    && Status != null && Amount != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Transaction: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Transaction";
            }
            else
            {
                return string.Format(
                    "ID: {0} Sender: {1} Recipient: {2} DeltaBTC: {3} [{4}]",
                    ID,
                    Sender.Name,
                    (Recipient != null) ? Recipient.Name : RecipientAddress,
                    Amount,
                    Status
                    );
            }
        }
    }
}
