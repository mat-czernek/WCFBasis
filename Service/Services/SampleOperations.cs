using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    public class SampleOperations
    {
        public List<SampleOperationModel> OperationsList { get; }
        
        public SampleOperations()
        {
            var random = new Random();
            
            OperationsList = new List<SampleOperationModel>
            {
                new SampleOperationModel {Delay = random.Next(20000, 30000), Name = "Operation no.1"},
                new SampleOperationModel {Delay = random.Next(20000, 30000), Name = "Operation no.2"},
                new SampleOperationModel {Delay = random.Next(20000, 30000), Name = "Operation no.3"},
                new SampleOperationModel {Delay = random.Next(20000, 30000), Name = "Operation no.4"},
                new SampleOperationModel {Delay = random.Next(20000, 30000), Name = "Operation no.5"}
            };
        }
    }
}