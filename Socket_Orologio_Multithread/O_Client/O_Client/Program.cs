using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace O_Client
{

    class Program
    {
        static void Main(string[] args)
        {
            Cliente c = new Cliente();
            c.Attività();
        }
    }

    class Cliente
    {
        public void Attività()
        {

            TcpClient c;
            NetworkStream st;

            try
            {

                c = new TcpClient(Dns.GetHostName(), 10101);
                st = c.GetStream();

                while (c.Connected)
                {
                    byte[] bufIn = new Byte[20];
                    String msgIn = String.Empty;
                    Int32 bytes = st.Read(bufIn, 0, bufIn.Length);
                    msgIn = System.Text.Encoding.ASCII.GetString(bufIn, 0, bytes);
                    Console.WriteLine("[Client]" + " Ricevuto << " + msgIn);
                }

                st.Close();
                c.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Client] Errore");
            }

        }
    }
}
