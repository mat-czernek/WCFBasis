using System;
using Contracts.Enums;

namespace Service.Actions
{
    public class InvalidAction : IServiceAction
    {
        public ActionType Type { get; } = ActionType.Invalid;

        public ActionStatus Status { get; } = ActionStatus.Idle;
        
        public Guid ClientId { get; } = Guid.Empty;
        
        public void Take()
        {
            return;
        }
    }
}