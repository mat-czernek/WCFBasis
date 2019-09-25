using System;
using System.Collections.Generic;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;

namespace Service.Actions
{
    public interface IServiceAction
    {
        void Execute();
    }
}