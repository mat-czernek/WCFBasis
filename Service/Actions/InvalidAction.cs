using System;
using Contracts.Enums;
using Service.Services;

namespace Service.Actions
{
    public class InvalidAction : IServiceAction
    {
        public void Execute()
        {
            return;
        }
    }
}