
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CoinbaseSharp.Resources.Wrappers;

namespace CoinbaseSharp.Resources
{
    [DataContract]
    public class Users : CoinbaseResource
    {
        public Users() : base() { }
        public Users(List<string> errors) : base(errors) { }
        public Users(WebException ex) : base(ex) { }

        [IgnoreDataMember]
        public List<User> UserList { get; private set; }

        [DataMember(Name = "users")]
        public List<UserInfo> UserInfoList
        {
            get
            {
                if (UserList == null)
                {
                    return null;
                }
                else
                {
                    return UserList.Select(x => new UserInfo() { User = x }).ToList();
                }
            }
            set
            {
                UserList = value.Select(x => x.User).ToList();
            }
        }

        [IgnoreDataMember]
        public override bool IsComplete
        {
            get
            {
                return UserList != null;
            }
        }

        [IgnoreDataMember]
        public User CurrentUser
        {
            get
            {
                if (HasErrors)
                {
                    return new User(Errors);
                }
                else if (!IsComplete)
                {
                    return null;
                }
                else
                {
                    return UserList[0];
                }
            }
        }

        public override string ToString()
        {
            if (HasErrors)
            {
                return string.Format("Invalid Set of Users: {0}", ErrorMessage);
            }
            else if (!IsComplete)
            {
                return "Incomplete Set of Users";
            }
            else if (UserList.Count == 0)
            {
                return "[]";
            }
            else
            {
                return string.Format("[{0}]", string.Join(";", UserList.Select(x => x.ToString())));
            }
        }
    }
}