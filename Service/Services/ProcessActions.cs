using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    public class ProcessActions
    {
        public List<ActionModel> Actions { get; }

        public ProcessActions()
        {
            var random = new Random();
            
            Actions = new List<ActionModel>
            {
                new ActionModel {Delay = random.Next(3000, 12000), Name = "Action no.1"},
                new ActionModel {Delay = random.Next(3000, 12000), Name = "Action no.2"},
                new ActionModel {Delay = random.Next(3000, 12000), Name = "Action no.3"},
                new ActionModel {Delay = random.Next(3000, 12000), Name = "Action no.4"},
                new ActionModel {Delay = random.Next(3000, 12000), Name = "Action no.5"}
            };
        }
    }
}