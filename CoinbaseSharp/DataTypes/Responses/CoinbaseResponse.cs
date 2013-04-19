
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.DataTypes.Responses
{
    [DataContract]
    public class CoinbaseResponse : CoinbaseResource
    {
        //
        // TODO: Figure out annotation
        //
        private CoinbaseResource _resource;

        public CoinbaseResponse() : base() { }
        public CoinbaseResponse(List<string> errors) : base(errors) { }
        public CoinbaseResponse(WebException ex) : base(ex) { }

        [IgnoreDataMember]
        protected virtual Type ResourceType
        {
            get
            {
                return typeof(CoinbaseResponse);
            }
        }

        [IgnoreDataMember]
        protected CoinbaseResource Resource
        {
            get
            {
                if (ResourceType == null)
                {
                    return null;
                }

                if (_resource == null && HasErrors)
                {
                    var constructor = ResourceType.GetConstructor(new Type[] { typeof(List<string>) });
                    if (constructor == null)
                    {
                        throw new ArgumentException("ResourceType does not have required constructor");
                    }
                    _resource = (CoinbaseResource) constructor.Invoke(new object[] { Errors });
                }
                else if (!_resource.HasErrors && HasErrors)
                {
                    _resource.Errors = Errors;
                }

                return _resource == null ? null : _resource;
            }
            set
            {
                _resource = value;
            }
        }
    }
}
