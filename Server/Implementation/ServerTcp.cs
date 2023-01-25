using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server.Implementation
{
    /// <summary>
    /// Сервер реализованный с помощью протокола TCP
    /// </summary>
    public class ServerTcp : IServer
    {
        private const string MESSAGE_FORMAT = "{0}: {1}";
        private const string UNDEFINED_USERNAME = "Undefiend";
        private const string UNDEFINED_NAME_FORMAT = "{0}_{1}";

        private readonly Dictionary<TcpClient, string> _clients = new Dictionary<TcpClient, string>();

        private int _undefinedCount;

        public void Run(int port, IPAddress ipAddress)
        {
            TcpListener server = null;

            try
            {
                server = new TcpListener(ipAddress, port);

                server.Start();

                while (true)
                {
                    var newClient = server.AcceptTcpClient();
                    Console.WriteLine("New client trying connect...");
                    InitializeNewClient(newClient);
                    new Thread(() => HandleClient(newClient)).Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server?.Stop();
            }
        }

        private void HandleClient(TcpClient client)
        {
            while (client.Connected)
            {
                if (TryReadInformation(client, out var bytes, out var i) == false)
                {
                    RemoveClient(client);
                    break;
                }

                if (i == 0)
                {
                    RemoveClient(client);
                    break;
                }

                var message = Encoding.ASCII.GetString(bytes, 0, i);

                // server log
                lock (_clients)
                {
                    Console.WriteLine("{0}: {1}", _clients[client], message);
                }

                SendMessageToAllUsers(client, message);
            }

            RemoveClient(client);
        }

        private void SendMessageToAllUsers(TcpClient sender, string message)
        {
            lock (_clients)
            {
                var senderName = _clients[sender];
                foreach (var client in _clients.Keys.Where(client => client != sender))
                    SendMessage(senderName, message, client);
            }
        }

        private void SendMessage(string from, string message, TcpClient client)
        {
            var sendMessage = string.Format(MESSAGE_FORMAT, from, message);
            var clientStream = client.GetStream();
            var bufferedMessage = Encoding.ASCII.GetBytes(sendMessage);
            clientStream.Write(bufferedMessage, 0, bufferedMessage.Length);
        }

        private bool TryReadInformation(TcpClient client, out byte[] information, out int bytesCount)
        {
            var stream = client.GetStream();
            information = new byte[256];
            bytesCount = 0;

            try
            {
                bytesCount = stream.Read(information, 0, information.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Read information went wrong: {e.Message}");
                return false;
            }

            return true;
        }

        private void InitializeNewClient(TcpClient client)
        {
            if (TryReadInformation(client, out var bytes, out var i) && i > 0)
            {
                var message = Encoding.ASCII.GetString(bytes, 0, i);
                AddNewUser(GetUserNameFromMessage(message), client);
            }
            else
            {
                AddNewUser(GetUndefinedUsername(), client);
            }
        }

        private void AddNewUser(string userName, TcpClient client)
        {
            lock (_clients) 
                _clients.Add(client, userName);

            Console.WriteLine($"{userName} connected!");
        }

        private void RemoveClient(TcpClient client)
        {
            lock (_clients)
            {
                if (_clients.ContainsKey(client))
                    _clients.Remove(client);

                client.Close();
            }
        }

        private string GetUserNameFromMessage(string message) => 
            string.IsNullOrWhiteSpace(message) ? GetUndefinedUsername() : message;

        private string GetUndefinedUsername()
        {
            return _undefinedCount > 0
                ? string.Format(UNDEFINED_NAME_FORMAT, UNDEFINED_USERNAME, ++_undefinedCount)
                : UNDEFINED_USERNAME;
        }
    }
}