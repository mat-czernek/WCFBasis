using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace Service.Actions
{
    /// <summary>
    /// Class handles operations required to register new WCF client
    /// </summary>
    public class RegisterClientAction : IServiceAction
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
        /// Operation context, WCF communication channel with client
        /// </summary>
        private readonly ICallbacksApi _operationContext; 
        
        /// <summary>
        /// Gets the action type
        /// </summary>
        public ActionType Type { get; } = ActionType.RegisterClient;
        
        /// <summary>
        /// Gets the action status
        /// </summary>
        public ActionStatus Status { get; private set; } = ActionStatus.Idle;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="clientId">New client Id</param>
        /// <param name="operationContext">WCF communication channel with client</param>
        public RegisterClientAction(Guid clientId, ICallbacksApi operationContext)
        { 
            ClientId = clientId;
            _operationContext = operationContext;
        }
        
        /// <summary>
        /// Method registers new client in WCF service
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
                // client already exists on the list of registered clients
                if (ServiceOperationsApi.RegisteredClients.FindIndex(client => client.Id == ClientId) >= 0)
                {
                    Status = ActionStatus.Completed;
                    return;
                }

                var clientModel = new ClientModel
                {
                    Id = ClientId,
                    CallbacksApiChannel = _operationContext,
                    RegistrationTime = DateTime.Now
                };
                
                ServiceOperationsApi.RegisteredClients.Add(clientModel);
                
                Status = ActionStatus.Completed;
                
                // send message to client
                clientModel.CallbacksApiChannel.Message("Client registered successfully!");
            }
        }
    }
}