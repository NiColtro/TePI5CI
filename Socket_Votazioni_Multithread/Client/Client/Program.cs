using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings;

namespace Votazione_Client {
    internal class Program {
        static void Main(string[] args) {
            
            try {
                string command;
                Byte[] buf = new byte[2046];
                
                TcpClient c = new TcpClient();
                c.Connect("127.0.0.1", 2003);

                NetworkStream ns = c.GetStream();

                do {
                    Console.Write("[!] Cosa vuoi fare? ");
                    command = Console.ReadLine();
                    ns.Write(Encoding.ASCII.GetBytes(command));

                    int len = ns.Read(buf, 0, buf.Length);
                    
                    Console.WriteLine("\n***");
                    Console.WriteLine(Encoding.ASCII.GetString(buf, 0, len));
                    Console.WriteLine("***\n");
                } while (command != "EXIT");
                
                ns.Close();
                c.Close();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
