using System;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace Service.Actions
{
    public class ServiceActionsFactory : IServiceActionsFactory
    {
        private static readonly object SyncObject = new object();
        
        private bool _isRequestFromRegisteredClient(Guid clientId)
        {
            lock (SyncObject)
            {
                return ServiceOperationsApi.RegisteredClients.FindAll(client => client.Id == clientId).Count != 0;
            }
        }
        
        public IServiceAction Create(ActionModel model)
        {
            switch (model.Type)
            {
                case ActionType.SampleOperation:
                {
                    if(_isRequestFromRegisteredClient(model.ClientId))
                        return new SampleOperationAction(model.ClientId);
                    
                    return new InvalidAction();
                }

                case ActionType.RegisterClient:
                    return new RegisterClientAction(model.ClientId, OperationContext.Current.GetCallbackChannel<ICallbacksApi>());

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