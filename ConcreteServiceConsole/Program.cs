using System;
using Autofac;
using Contracts;
using Service;
using Service.Actions;
using Service.Clients;
using Service.Notifications;
using Service.Services;

namespace ConcreteServiceConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<ClientsRepository>().As<IClientsRepository>().SingleInstance();
            containerBuilder.RegisterType<ClientsNotificationFactory>().As<IClientsNotificationFactory>().SingleInstance();
            containerBuilder.RegisterType<ClientsManagement>().As<IClientsManagement>().SingleInstance();
            containerBuilder.RegisterType<ServiceActionsHandler>().As<IServiceActionsHandler>().SingleInstance();
            containerBuilder.RegisterType<ServiceContract>().As<IServiceContract>().SingleInstance();
            
            var container = containerBuilder.Build();
            
            using(var scope = container.BeginLifetimeScope())
            {
                var host = new WcfServiceHost(scope.Resolve<IServiceContract>());

                host.Open();

                Console.ReadKey();

                host.Close();
            }
        }
    }
}