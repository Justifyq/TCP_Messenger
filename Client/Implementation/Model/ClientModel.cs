using System.Net.Sockets;
using System.Text;

namespace Client.Implementation.Model
{
    /// <summary>
    /// Модель для клиента.
    /// </summary>
    public class ClientModel
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        public ClientModel(string host, int port)
        {
            _client = new TcpClient(host, port);
            _stream = _client.GetStream();
        }

        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="message">сообщение</param>
        public void SendMessage(string message)
        {
            var information = Encoding.ASCII.GetBytes(message);
            _stream.Write(information, 0, information.Length);
        }

        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <returns>сообщеие</returns>
        public string ReceiveMessage()
        {
            var information = new byte[256];
            var informationCount = _stream.Read(information, 0, information.Length);
            return Encoding.ASCII.GetString(information, 0, informationCount);
        }
    }
}