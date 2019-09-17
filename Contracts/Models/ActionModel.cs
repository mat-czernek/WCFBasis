using System.Runtime.Serialization;
using Contracts.Enums;

namespace Contracts.Models
{
    [DataContract(Namespace = "WCFBasis")]
    public class ActionModel
    {
        [DataMember]
        public string Name { get; set; }
    
        [DataMember]
        public int Delay { get; set; }

        [DataMember] 
        public ActionStatus Status { get; set; } = ActionStatus.Idle;
    }
}