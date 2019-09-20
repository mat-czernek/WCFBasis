using System;
using System.Runtime.Serialization;
using Contracts.Enums;

namespace Contracts.Models
{
    [DataContract(Namespace = "WCFBasis")]
    public class ActionModel
    {
        [DataMember]
        public ActionType Type { get; set; }
        
        [DataMember]
        public Guid ClientId { get; set; }
        
        [DataMember]
        public bool ExecuteImmediately { get; set; }
    }
}