using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace O_Server
{
    public static class Program
    {
        public static void Main()
        {

            IPAddress[] sIp = Dns.GetHostAddresses(Dns.GetHostName()); //tutti gli ip dell'host server (localhost)
            TcpListener s = null;

            try
            {
                s = new TcpListener(sIp[0], 10101);
                s.Start(); //in attesa di client
                Byte[] buf = new Byte[20];
                String msgIn = null;

                int c = 0;
                while (true)
                {
                    Th Thn = new Th(s.AcceptTcpClient(), c);
                    new Thread(Thn.Action).Start();
                    c++;
                }

                Console.Write("[Server]" + " In attesa di connessione... \n");

            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] Errore");
            }
            finally
            {
                s.Stop();
            }

        }
    }

    class Th
    {
        TcpClient dati;
        int c;

        public Th(TcpClient dati, int c)
        {
            this.dati = dati;
            this.c = c;
        }

        public void Action()
        {
            string msgIn;
            NetworkStream st = dati.GetStream();
            Console.WriteLine("[Server]" + " Connesso a Th" + c);

            try
            {
                while (dati.Connected)
                {
                    msgIn = DateTime.Now.ToString("dd/M/yyyy H:mm:ss");
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(msgIn);
                    st.Write(msg, 0, msg.Length);
                    Console.WriteLine("[Server]" + " Spedito >> " + msgIn + " a Th" + c);
                    Thread.Sleep(1000);
                }

                st.Close();
                dati.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] Il client " + c + " si è disconnesso");
            }
        }
    }
}
