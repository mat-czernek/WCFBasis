using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    /// <summary>
    /// Sample class that defines time consuming like operations
    /// </summary>
    public class ProcessOperations
    {
        /// <summary>
        /// List of time consuming operations
        /// </summary>
        public List<OperationModel> Operations { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
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