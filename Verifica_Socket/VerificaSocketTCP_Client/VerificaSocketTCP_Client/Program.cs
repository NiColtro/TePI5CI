using System;
using System.Net.Sockets;

namespace VerificaSocketTCP_Client {
    static class Client {
        static public void Azione() {
            // Socket setup & Server connect
            TcpClient c = new TcpClient("127.0.0.1", 2003);
            NetworkStream ns = c.GetStream();

            // Ask for user input
            string n1, n2, op;
            Console.WriteLine("Inserisci il primo numero: ");
            n1 = Console.ReadLine();
            Console.WriteLine("Inserisci il secondo numero: ");
            n2 = Console.ReadLine();
            Console.WriteLine("Inserisci l'operatore: ");
            op = Console.ReadLine();

            // Send data
            ns.Write(System.Text.Encoding.ASCII.GetBytes(n1 + ";" + op + ";" + n2));

            // Get result from Server
            Byte[] buf = new byte[64];
            ns.Read(buf, 0, buf.Length);
            string payload = System.Text.Encoding.ASCII.GetString(buf);
            Console.WriteLine(payload);
            
            // Close Socket things
            ns.Close();
            c.Close();
        }
    }
    
    class Program {
        static void Main(string[] args) {
            Client.Azione();
        }
    }
}