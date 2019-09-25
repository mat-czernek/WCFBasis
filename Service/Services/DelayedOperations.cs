using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    /// <summary>
    /// Sample class that defines time consuming like operations
    /// </summary>
    public class DelayedOperations
    {
        public List<DelayedOperationModel> OperationsList { get; }
        
        public DelayedOperations()
        {
            var random = new Random();
            
            OperationsList = new List<DelayedOperationModel>
            {
                new DelayedOperationModel {Delay = random.Next(2000, 3000), Name = "Operation no.1"},
                new DelayedOperationModel {Delay = random.Next(2000, 3000), Name = "Operation no.2"},
                new DelayedOperationModel {Delay = random.Next(2000, 3000), Name = "Operation no.3"},
                new DelayedOperationModel {Delay = random.Next(2000, 3000), Name = "Operation no.4"},
                new DelayedOperationModel {Delay = random.Next(2000, 3000), Name = "Operation no.5"}
            };
        }
    }
}