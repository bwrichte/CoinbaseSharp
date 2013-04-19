
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources.Wrappers;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Transactions : CoinbaseResource
    {
        public Transactions() : base() { }
        public Transactions(List<string> errors) : base(errors) { }
        public Transactions(WebException ex) : base(ex) { }

        [DataMember(Name = "current_user")]
        public User CurrentUser { get; set; }

        [DataMember(Name = "balance")]
        public Amount Balance { get; set; }

        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        [DataMember(Name = "num_pages")]
        public int NumberOfPages { get; set; }

        [DataMember(Name = "current_page")]
        public int CurrentPage { get; set; }

        [IgnoreDataMember]
        public List<Transaction> TransactionList {  get;  private set; }

        [DataMember(Name = "transactions")]
        public List<TransactionInfo> TransactionInfoList
        {
            get
            {
                if (TransactionList == null)
                {
                    return null;
                }
                else
                {
                    return TransactionList.Select(x => new TransactionInfo() { Transaction = x }).ToList();
                }
            }
            set
            {
                TransactionList = value.Select(x => x.Transaction).ToList();
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return TransactionList != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Set of Transactions: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Set of Transactions";
            }
            else if (TotalCount == 0)
            {
                return "[]";
            }
            else
            {
                return string.Format("[{0}]", string.Join(";", TransactionList.Select(x => x.ToString())));
            }
        }
    }
}