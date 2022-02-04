using System;
using System.Net.Sockets;
using System.Text;

namespace Chat_Client
{
    internal class MessageHandler
    {
        TcpClient _client;
        public NetworkStream stream;
        bool _ready; // Ready to read from stream

        public MessageHandler(TcpClient client)
        {
            _client = client;
            _ready = false;
            stream = _client.GetStream(); // Get stream from client

            new Thread(MsgRecive).Start(); // Start thread
        }

        public void MsgRecive()
        {
            while (true) // While connected
            {
                if (_ready) // If ready to use stream
                {
                    byte[] buf = new byte[1024];
                    int len = stream.Read(buf, 0, buf.Length); // Read from stream

                    string payload = Encoding.ASCII.GetString(buf, 0, len);
                    Console.WriteLine("\n" + payload); // Write on screen
                }                
            }
        }

        public void MsgSend(string payload)
        {
            stream.Write(Encoding.ASCII.GetBytes(payload)); // Send
            _ready = true;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient c = new TcpClient(); // Setup
            c.Connect("127.0.0.1", 2022);

            MessageHandler mh = new MessageHandler(c); // Init new message handler
            
            Console.Write("[?] Choose your username: ");
            new Thread(mh.MsgRecive).Start(); // Start th: listen to incoming messages

            while (true)
            {
                string payload = Console.ReadLine();
                mh.MsgSend(payload);
            }
        }
    }
}
