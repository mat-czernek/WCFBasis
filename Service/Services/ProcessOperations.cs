using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    public class ProcessOperations
    {
        public List<OperationModel> Operations { get; }

        public ProcessOperations()
        {
            var random = new Random();
            
            Operations = new List<OperationModel>
            {
                new OperationModel {Delay = random.Next(25000, 45000), Name = "Operation no.1"},
                new OperationModel {Delay = random.Next(25000, 45000), Name = "Operation no.2"},
                new OperationModel {Delay = random.Next(25000, 45000), Name = "Operation no.3"},
                new OperationModel {Delay = random.Next(25000, 45000), Name = "Operation no.4"},
                new OperationModel {Delay = random.Next(25000, 45000), Name = "Operation no.5"}
            };
        }
    }
}