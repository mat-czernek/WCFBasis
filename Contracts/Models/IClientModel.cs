using System;
using System.Runtime.Serialization;

namespace Contracts.Models
{
    public interface IClientModel
    {
        Guid Id { get; set; }
        
        IClientCallbackContract CallbackChannel { get; set; }
        
        DateTime RegistrationTime { get; set; }

        DateTime LastActivityTime { get; set; } 
    }
}