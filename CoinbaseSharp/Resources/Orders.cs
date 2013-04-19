
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources.Wrappers;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Orders : CoinbaseResource
    {
        public Orders() : base() { }
        public Orders(List<string> errors) : base(errors) { }
        public Orders(WebException ex) : base(ex) { }

        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        [DataMember(Name = "num_pages")]
        public int NumberOfPages { get; set; }

        [DataMember(Name = "current_page")]
        public int CurrentPage { get; set; }

        [IgnoreDataMember]
        public List<Order> OrderList { get; private set; }

        [DataMember(Name = "orders")]
        public List<OrderInfo> OrderInfoList
        {
            get
            {
                if (OrderList == null)
                {
                    return null;
                }
                else
                {
                    return OrderList.Select(x => new OrderInfo() { Order = x }).ToList();
                }
            }
            set
            {
                OrderList = value.Select(x => x.Order).ToList();
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return OrderList != null;
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Set of Orders: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Set of Orders";
            }
            else if (TotalCount == 0)
            {
                return "[]";
            }
            else
            {
                return string.Format("[{0}]", string.Join(";", OrderList.Select(x => x.ToString())));
            }
        }
    }
}