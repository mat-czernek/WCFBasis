using System.ServiceProcess;

namespace ConcreteService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WCFBasisService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
