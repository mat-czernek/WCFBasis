using System;
using System.Runtime.Serialization;

namespace Contracts.Models
{
    [DataContract(Namespace = "WCFBasis")]
    public class ClientModel : IClientModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public IClientCallbackContract CallbackChannel { get; set; }

        [DataMember]
        public DateTime RegistrationTime { get; set; }

        public DateTime LastActivityTime { get; set; } = DateTime.MinValue;
    }
}