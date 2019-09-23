using System;
using Contracts;
using Contracts.Enums;
using Service.Services;

namespace Service.Actions
{
    /// <summary>
    /// Class handles operations required to update (preserve) the communication channel with clients
    /// </summary>
    public class UpdateChannelAction : IServiceAction
    {
        /// <summary>
        /// Thread synchronization context
        /// </summary>
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// Gets the action type
        /// </summary>
        public ActionType Type { get; set; } = ActionType.UpdateChannel;

        /// <summary>
        /// Gets the action status
        /// </summary>
        public ActionStatus Status { get; private set; } = ActionStatus.Idle;
        
        /// <summary>
        /// WCF client Id
        /// </summary>
        public Guid ClientId { get; }

        /// <summary>
        /// Operation context, WCF communication channel with client
        /// </summary>
        private readonly ICallbacksApi _operationContext; 

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="clientId">Client Id for which communication channel needs to updated</param>
        /// <param name="operationContext">WCF communication channel with client</param>
        public UpdateChannelAction(Guid clientId, ICallbacksApi operationContext)
        {
            ClientId = clientId;
            _operationContext = operationContext;
        }
        
        /// <summary>
        /// Method unregisters WCF client from the service
        /// </summary>
        public void Take()
        {
            lock (SyncObject)
            {
                var clientIndex = ClientsRepository.RegisteredClients.FindIndex(client => client.Id == ClientId);

                if (clientIndex >= 0)
                {
                    ClientsRepository.RegisteredClients[clientIndex].CallbacksApiChannel = _operationContext;
                    ClientsRepository.RegisteredClients[clientIndex].LastActivityTime = DateTime.Now;
                }
            }
            
            Status = ActionStatus.Completed;
        }
    }
}