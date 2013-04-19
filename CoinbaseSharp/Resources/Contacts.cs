
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources.Wrappers;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Contacts : CoinbaseResource
    {
        public Contacts() : base() { }
        public Contacts(List<string> errors) : base(errors) { }
        public Contacts(WebException ex) : base(ex) { }

        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        [DataMember(Name = "num_pages")]
        public int NumberOfPages { get; set; }

        [DataMember(Name = "current_page")]
        public int CurrentPage { get; set; }

        [IgnoreDataMember]
        public List<Contact> ContactList { get; private set; }

        [DataMember(Name = "contacts")]
        public List<ContactInfo> ContactInfoList
        {
            get
            {
                if (ContactList == null)
                {
                    return null;
                }
                else
                {
                    return ContactList.Select(x => new ContactInfo() { Contact = x }).ToList();
                }
            }
            set
            {
                ContactList = value.Select(x => x.Contact).ToList();
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return ContactList != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Set of Contacts: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Set of Contacts";
            }
            else if (TotalCount == 0)
            {
                return "[]";
            }
            else
            {
                return string.Format("[{0}]", string.Join(";", ContactList.Select(x => x.ToString())));
            }
        }
    }
}