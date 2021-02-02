using IceCoffee.LogManager;
using System;
using TianYiSdtdServerTools.Server.Sockets;

namespace TianYiSdtdServerTools.Server.Terminal
{
    class Startup
    {
        private static readonly TcpServer _server;

        static Startup()
        {
            _server = new TcpServer();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Try start server on {0}. Enter 'exit' stop server", Shared.SocketConfig.Port);

            _server.Start(Shared.SocketConfig.Port);

            while (true)
            {
                string text = Console.ReadLine();
                if (text == "exit")
                {
                    break;
                }
            }

            _server.Stop();

            Console.WriteLine("TCP server has stopped.");
            Console.ReadKey();
        }
    }
}
