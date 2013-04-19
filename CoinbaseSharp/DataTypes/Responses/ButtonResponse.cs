using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class ButtonResponse : CoinbaseResponse
    {
        public ButtonResponse() : base() { }
        public ButtonResponse(List<string> errors) : base(errors) { }
        public ButtonResponse(WebException ex) : base(ex) {  }

        [DataMember(Name = "button")]
        public Button Button
        {
            get
            {
                return (Button)Resource;
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
                return typeof(Button);
            }
        }
    }
}
