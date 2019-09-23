namespace Service.Services
{
    public interface IClientsRepository
    {
        void StartMaintenance();
        
        void StopMaintenance();
    }
}