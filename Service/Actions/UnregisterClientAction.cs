using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace Service.Actions
{
    /// <summary>
    /// Class handles operations required to unregister WCF client
    /// </summary>
    public class UnregisterClientAction : IServiceAction
    {
        /// <summary>
        /// Thread synchronization context
        /// </summary>
        private static readonly object SyncObject = new object();

        /// <summary>
        /// WCF client Id
        /// </summary>
        public Guid ClientId { get; }

        /// <summary>
        /// Gets the action type
        /// </summary>
        public ActionType Type { get; set; } = ActionType.RegisterClient;

        /// <summary>
        /// Gets the action status
        /// </summary>
        public ActionStatus Status { get; set; } = ActionStatus.Idle;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="clientId">Client id that needs to be unregistered from service</param>
        public UnregisterClientAction(Guid clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// Method unregisters the WCF client from the service
        /// </summary>
        public void Take()
        {
            if (ClientId == Guid.Empty)
            {
                Status = ActionStatus.Completed;
                return;
            }

            lock (SyncObject)
            {
                var clientToUnregister = ServiceOperationsApi.RegisteredClients.SingleOrDefault(client => client.Id == ClientId);

                // client does not exist on the list of registered clients
                if (clientToUnregister == null)
                {
                    Status = ActionStatus.Completed;
                    return;
                }
            
                ServiceOperationsApi.RegisteredClients.Remove(clientToUnregister);
            }
            
            Status = ActionStatus.Completed;
        }
    }
}