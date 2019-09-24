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
            var host = new WcfServiceHost(new ServiceApi(
                    new ClientsRepository(), 
                    new ServiceActionsFactory()
                ));
            
            host.Open();

            Console.ReadKey();
            
            host.Close();
        }
    }
}