using System;
using Contracts.Enums;

namespace Service.Actions
{
    public interface IAction
    {
        ActionType Type { get; set; }
        
        ActionStatus Status { get; set; }
        
        Guid ClientId { get; }

        void Take();
    }
}