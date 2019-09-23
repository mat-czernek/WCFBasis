using System;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;

namespace Service.Actions
{
    public class ServiceActionsFactory : IServiceActionsFactory
    {
        private static readonly object SyncObject = new object();

        private bool _isRequestFromRegisteredClient(Guid clientId)
        {
            lock (SyncObject)
            {
                return ClientsRepository.RegisteredClients.FindAll(client => client.Id == clientId).Count != 0;
            }
        }
        
        public IServiceAction Create(ActionModel model)
        {
            switch (model.Type)
            {
                case ActionType.SampleOperation:
                {
                    if (!_isRequestFromRegisteredClient(model.ClientId)) return new InvalidAction();
                    
                    var sampleOperationAction = new SampleOperationAction(model.ClientId);

                    sampleOperationAction.OnOperationChange += ActionEventObserver.OnOperationChange;
                    sampleOperationAction.OnOperationsListChange += ActionEventObserver.OnOperationsListChange;
                    sampleOperationAction.OnOperationsCompleted += ActionEventObserver.OnOperationsCompleted;

                    return sampleOperationAction;

                }

                case ActionType.RegisterClient:
                {
                    var newClient = new RegisterClientAction(model.ClientId,
                        OperationContext.Current.GetCallbackChannel<ICallbacksApi>());

                    newClient.OnRegistrationSuccess += ActionEventObserver.OnRegistrationSuccess;

                    return newClient;
                }

                case ActionType.UnregisterClient:
                {
                    if(_isRequestFromRegisteredClient(model.ClientId))
                        return new UnregisterClientAction(model.ClientId);
                    
                    return new InvalidAction();
                }

                case ActionType.UpdateChannel:
                {
                    if(_isRequestFromRegisteredClient(model.ClientId))
                    {
                        return new UpdateChannelAction(model.ClientId,
                            OperationContext.Current.GetCallbackChannel<ICallbacksApi>());
                    }
                    
                    return new InvalidAction();
                }
                default:
                    return new InvalidAction();
            }
        }
    }
}