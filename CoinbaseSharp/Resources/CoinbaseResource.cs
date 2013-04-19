
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class CoinbaseResource
    {
        public CoinbaseResource()
        {
        }

        public CoinbaseResource(List<string> errors)
        {
            Errors = errors;
        }

        public CoinbaseResource(WebException ex)
        {
            if (Errors == null)
            {
                Errors = new List<string>();
            }
            Errors.Add(ex.Message);
        }

        [DataMember(Name = "success", EmitDefaultValue = false)]
        public bool Success { get; set; }

        [DataMember(Name = "error", EmitDefaultValue = false)]
        public string Error
        {
            get
            {
                if (Errors == null)
                {
                    return null;
                }
                else
                {
                    return Errors[0];
                }
            }
            set
            {
                if (Errors == null)
                {
                    Errors = new List<string>();
                }
                Errors.Add(value);
            }
        }

        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public List<string> Errors { get; set; }

        [IgnoreDataMember]
        public bool HasErrors
        {
            get
            {
                return Errors != null;
            }
        }

        [IgnoreDataMember]
        public string ErrorMessage
        {
            get
            {
                if (Errors == null)
                {
                    return null;
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Errors.Count; ++i)
                {
                    sb.Append(Errors[i]);
                    if (i < Errors.Count - 1)
                    {
                        sb.Append("; and ");
                    }
                }
                return sb.ToString();
            }
        }

        [IgnoreDataMember]
        public virtual bool IsComplete
        {
            get
            {
                return true;
            }
        }

        [IgnoreDataMember]
        public bool IsValid
        {
            get
            {
                return !HasErrors && IsComplete;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return ErrorMessage;
            }

            return null;
        }
    }
}
