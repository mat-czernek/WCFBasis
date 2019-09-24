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

        public IServiceAction Create(ActionModel model)
        {
            switch (model.Type)
            {
                case ActionType.SampleOperation:
                {
                    if (!ClientsRepository.IsRegisteredClient(model.ClientId)) return new InvalidAction();
                    
                    var sampleOperationAction = new SampleOperationAction(model.ClientId);
                    
                    return sampleOperationAction;

                }

                case ActionType.RegisterClient:
                {
                    var newClient = new RegisterClientAction(model.ClientId,
                        OperationContext.Current.GetCallbackChannel<ICallbacksApi>());

                    return newClient;
                }

                case ActionType.UnregisterClient:
                {
                    if(ClientsRepository.IsRegisteredClient(model.ClientId))
                        return new UnregisterClientAction(model.ClientId);
                    
                    return new InvalidAction();
                }

                case ActionType.UpdateChannel:
                {
                    if(ClientsRepository.IsRegisteredClient(model.ClientId))
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