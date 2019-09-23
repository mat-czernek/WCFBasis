using System.Collections.Generic;
using Contracts.Models;

namespace Contracts.Delegates
{
    public delegate void ServiceActionsQueueDelegate(List<OperationModel> actions);
}