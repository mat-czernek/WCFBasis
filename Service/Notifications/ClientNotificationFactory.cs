using Service.Services;

namespace Service.Notifications
{
    public class ClientNotificationFactory
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientNotificationFactory(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }
        
        public INotification CreateGeneralNotification(string message)
        {
            return new GeneralNotification(_clientsRepository, message);
        }
    }
}