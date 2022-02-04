using System;
using System.Net.Sockets;
using System.Text;

namespace Chat_Server
{
    static class Storage
    {
        public static List<ClientHandler> clients; // List of connected users
    }

    internal class ClientHandler
    {
        TcpClient _client;
        NetworkStream _stream;
        string? _username;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream(); // Get stream from client

            new Thread(Action).Start(); // Start thread
        }

        public void Action()
        {
            Console.WriteLine("[!] Handling new client...");

            try
            {
                while (_client.Connected)
                {
                    byte[] buf = new byte[1024];
                    int len = _stream.Read(buf, 0, buf.Length); // Read from stream

                    string payload = Encoding.ASCII.GetString(buf, 0, len);

                    if (_username == null)
                    {
                        _username = payload;
                        Broadcast("[+] " + _username + " joined the chat.");
                    }
                    else
                        Broadcast("[M] " + _username + " => " + payload);
                }
            }
            catch (Exception ex)
            {
                Storage.clients.Remove(this);
                Broadcast("[-] " + _username + " left the chat.");
            }
            finally
            {
                _stream.Close();
                _client.Close();
            }
        }

        public void SendMessage(string s) // Send message to user object
        {
            Console.WriteLine("[!] Sending '" + s + "' to " + _username);
            _stream.Write(Encoding.ASCII.GetBytes(s));
        }

        public void Broadcast(string s) // Iterate and send message to every user
        {
            Storage.clients.ForEach(c =>
            {
                if (c != this && c._username != null) // If target is not the sender itself
                    c.SendMessage(s);
            });
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Storage.clients = new List<ClientHandler>();

            TcpListener lis = new TcpListener(System.Net.IPAddress.Loopback, 2022); // Setup
            lis.Start();

            while (true)
            {
                Storage.clients.Add(new ClientHandler(lis.AcceptTcpClient()));
            }
        }
    }
}