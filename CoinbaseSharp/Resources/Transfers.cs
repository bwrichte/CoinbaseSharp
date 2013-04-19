
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources.Wrappers;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Transfers : CoinbaseResource
    {
        public Transfers() : base() { }
        public Transfers(List<string> errors) : base(errors) { }
        public Transfers(WebException ex) : base(ex) { }

        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        [DataMember(Name = "num_pages")]
        public int NumberOfPages { get; set; }

        [DataMember(Name = "current_page")]
        public int CurrentPage { get; set; }

        [IgnoreDataMember]
        public List<Transfer> TransferList {  get;  private set; }

        [DataMember(Name = "transfers")]
        public List<TransferInfo> TransferInfoList
        {
            get
            {
                if (TransferList == null)
                {
                    return null;
                }
                else
                {
                    return TransferList.Select(x => new TransferInfo() { Transfer = x }).ToList();
                }
            }
            set
            {
                TransferList = value.Select(x => x.Transfer).ToList();
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return TransferList != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Set of Transfers: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Set of Transfers";
            }
            else if (TotalCount == 0)
            {
                return "[]";
            }
            else
            {
                return string.Format("[{0}]", string.Join(";", TransferList.Select(x => x.ToString())));
            }
        }
    }
}