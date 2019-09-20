using System;
using Service;

namespace ConcreteServiceConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var host = new Host();
            
            host.Open();

            Console.ReadKey();
            
            host.Close();
        }
    }
}