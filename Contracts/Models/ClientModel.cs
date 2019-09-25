using System;
using System.Runtime.Serialization;

namespace Contracts.Models
{
    /// <summary>
    /// Class represents WCF client data. It's used to "register" client in WCF service.
    /// Registration is required to execute methods on client side by service - callbacks
    /// </summary>
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