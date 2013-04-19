using System.Runtime.Serialization;

namespace CoinbaseSharp.Resources.Wrappers
{
    //
    // Wrapper class
    //
    [DataContract]
    public class UserInfo
    {
        [DataMember(Name = "user", EmitDefaultValue = false)]
        public User User { get; set; }
    }
}
