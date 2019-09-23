using System;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Delegates;
using Service.Services;

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

        public event OnRegistrationSuccessDelegate OnRegistrationSuccess;
        
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


        private bool _isAlreadyRegistered(Guid clientId, out int clientIndex)
        {
            clientIndex =
                ClientsRepository.RegisteredClients.FindIndex(client => client.Id == ClientId);

            return clientIndex >= 0;
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
                if(_isAlreadyRegistered(ClientId, out var clientIndex))
                {
                    ClientsRepository.RegisteredClients[clientIndex].CallbacksApiChannel = _operationContext;
                    ClientsRepository.RegisteredClients[clientIndex].LastActivityTime = DateTime.Now;
                    Status = ActionStatus.Completed;
                    return;
                }

                var clientModel = new ClientModel
                {
                    Id = ClientId,
                    CallbacksApiChannel = _operationContext,
                    RegistrationTime = DateTime.Now,
                    LastActivityTime = DateTime.Now
                };
                
                ClientsRepository.RegisteredClients.Add(clientModel);
                
                Status = ActionStatus.Completed;
                
                // send message to client
                //clientModel.CallbacksApiChannel.UpdateGeneralStatus("Client registered successfully!");
                OnRegistrationSuccess?.Invoke(clientModel.Id);
            }
        }
    }
}