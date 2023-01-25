using System.Net;
using Server.Implementation;

namespace Server
{
    /// <summary>
    /// Инициализация сервера.
    /// </summary>
    public static class ServerStartup
    {
        public static void Main()
        {
            IServer server = new ServerTcp();
            server.Run(666, IPAddress.Parse("127.0.0.1"));
        }
    }
}