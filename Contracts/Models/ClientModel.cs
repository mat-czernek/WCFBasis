using System;
using System.Runtime.Serialization;

namespace Contracts.Models
{
    /// <summary>
    /// Class represents WCF client data. It's used to "register" client in WCF service.
    /// Registration is required to execute methods on client side by service - callbacks
    /// </summary>
    [DataContract(Namespace = "WCFBasis")]
    public class ClientModel
    {
        /// <summary>
        /// Client unique identifier
        /// </summary>
        [DataMember]
        public Guid Id;

        /// <summary>
        /// Callback communication channel used to executes method on client side by service
        /// </summary>
        [DataMember]
        public ICallbacks CallbacksChannel;

        /// <summary>
        /// Client registration timestamp
        /// </summary>
        [DataMember]
        public DateTime RegistrationTime;
    }
}