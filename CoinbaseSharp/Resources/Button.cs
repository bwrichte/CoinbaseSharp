
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.DataTypes;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public enum ButtonType
    {
        [EnumMember(Value = "buy_now")]
        BuyNow,
        [EnumMember(Value = "donation")]
        Donation,
        [EnumMember(Value = "subscription")]
        Subscription
    }

    [DataContract]
    public enum ButtonStyle
    {
        [EnumMember(Value = "buy_now_large")]
        BuyNowLarge,
        [EnumMember(Value = "buy_now_small")]
        BuyNowSmall,
        [EnumMember(Value = "donation_large")]
        DonationLarge,
        [EnumMember(Value = "donation_small")]
        DonationSmall,
        [EnumMember(Value = "custom_large")]
        CustomLarge,
        [EnumMember(Value = "custom_small")]
        CustomSmall,
        [EnumMember(Value = "none")]
        None
    }

    [DataContract]
    public class Button : CoinbaseResource
    {
        public Button() : base() { }
        public Button(List<string> errors) : base(errors) { }
        public Button(WebException ex) : base(ex) { }

        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string TypeValue { get; set; }

        [IgnoreDataMember]
        public ButtonType Type
        {
            get
            {
                foreach (ButtonType value in Enum.GetValues(typeof(ButtonType)))
                {
                    EnumMemberAttribute attribute = value.GetType()
                        .GetField(value.ToString())
                        .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .SingleOrDefault() as EnumMemberAttribute;
                    string text = attribute == null ? value.ToString() : attribute.Value;
                    if (text.Equals(TypeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return value;
                    }
                }
                return ButtonType.BuyNow;
            }
            set
            {
                EnumMemberAttribute attribute = value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                    .SingleOrDefault() as EnumMemberAttribute;
                TypeValue = attribute == null ? value.ToString() : attribute.Value;
            }
        }

        [DataMember(Name = "style", EmitDefaultValue = false)]
        public string StyleValue { get; set; }

        [IgnoreDataMember]
        public ButtonStyle Style
        {
            get
            {
                foreach (ButtonStyle value in Enum.GetValues(typeof(ButtonStyle)))
                {
                    EnumMemberAttribute attribute = value.GetType()
                        .GetField(value.ToString())
                        .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .SingleOrDefault() as EnumMemberAttribute;
                    string text = attribute == null ? value.ToString() : attribute.Value;
                    if (text.Equals(TypeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return value;
                    }
                }
                return ButtonStyle.BuyNowLarge;
            }
            set
            {
                EnumMemberAttribute attribute = value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                    .SingleOrDefault() as EnumMemberAttribute;
                StyleValue = attribute == null ? value.ToString() : attribute.Value;
            }
        }

        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "custom", EmitDefaultValue = false)]
        public string Custom { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "price_currency_iso")]
        public string PriceCurrencyISO { get; set; }

        [DataMember(Name = "price_string")]
        public string PriceString { get; set; }

        [IgnoreDataMember]
        public decimal Cost
        {
            get
            {
                return PriceString == null
                  ? 0m 
                  : decimal.Parse(PriceString);
            }
            set
            {
                PriceString = value.ToString();
            }
        }

        [DataMember(Name = "price", EmitDefaultValue = false)]
        public Price Price { get; set; }

        [IgnoreDataMember]
        public bool IsValidCoinbaseButton
        {
            get
            {
                return Code != null;
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return Name != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Button: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Button";
            }
            else if (Code != null)
            {
                return string.Format(
                    "{0} ({1}) for {2}",
                    Name,
                    Code,
                    Price
                    );
            }
            else
            {
                return string.Format(
                    "{0} for {1} {2}",
                    Name,
                    PriceString,
                    PriceCurrencyISO
                    );
            }
        }
    }
}
