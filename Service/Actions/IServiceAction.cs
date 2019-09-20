using System;
using Contracts.Enums;

namespace Service.Actions
{
    /// <summary>
    /// Interface defines properties and methods for actions processed by the WCF service
    /// </summary>
    public interface IServiceAction
    {
        /// <summary>
        /// Gets the action type
        /// </summary>
        ActionType Type { get; }
        
        /// <summary>
        /// Gets the action processing status
        /// </summary>
        ActionStatus Status { get; }
        
        /// <summary>
        /// Gets the client Id on behalf which action has been executed
        /// </summary>
        Guid ClientId { get; }

        /// <summary>
        /// Method executes action
        /// </summary>
        void Take();
    }
}