
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class UserResponse : CoinbaseResponse
    {
        public UserResponse() : base() { }
        public UserResponse(List<string> errors) : base(errors) { }
        public UserResponse(WebException ex) : base(ex) { }

        [DataMember(Name = "user")]
        public User User
        {
            get
            {
                return (User)Resource;
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
                return typeof(User);
            }
        }
    }
}
