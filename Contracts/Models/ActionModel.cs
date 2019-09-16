using System.Runtime.Serialization;

namespace Contracts.Models
{
    [DataContract(Namespace = "WCFBasis")]
    public class ActionModel
    {
        [DataMember]
        public string Name { get; set; }
    
        [DataMember]
        public int Delay { get; set; }
    }
}