using System;
using Service;
using Service.Actions;
using Service.Services;

namespace ConcreteServiceConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var clientsRepository = new ClientsRepository();
            var actionsHandler = new ServiceActionsHandler(clientsRepository);
            
            var host = new WcfServiceHost(new ServiceContract(actionsHandler));
            
            host.Open();

            Console.ReadKey();
            
            host.Close();
        }
    }
}