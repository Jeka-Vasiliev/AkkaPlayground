using System;
using Akka.Actor;

namespace AkkaPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("iot-system"))
            {
                var supervisor = system.ActorOf(IotSypervisor.Props(), "iot-supervisor");
            }

            Console.ReadLine();
        }
    }
}
