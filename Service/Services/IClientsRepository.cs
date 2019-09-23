namespace Service.Services
{
    public interface IClientsRepository
    {
        void StartMonitoring();
        
        void StopMonitoring();
    }
}