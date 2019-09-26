using System.Runtime.Serialization;
using Contracts.Enums;

namespace Contracts.Models
{
    [DataContract(Namespace = "WCFBasis")]
    public class SampleOperationModel
    {
        [DataMember]
        public string Name { get; set; }
    
        [DataMember]
        public int Delay { get; set; }

        [DataMember]
        public OperationStatus Status { get; set; } = OperationStatus.Idle;
    }
}