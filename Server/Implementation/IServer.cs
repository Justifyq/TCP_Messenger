using System.Net;

namespace Server.Implementation
{
    /// <summary>
    /// Интерфейс для сервера
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Запустить сервер
        /// </summary>
        /// <param name="port">Порт</param>
        /// <param name="ipAddress">IP адрес</param>
        void Run(int port, IPAddress ipAddress);
    }
}