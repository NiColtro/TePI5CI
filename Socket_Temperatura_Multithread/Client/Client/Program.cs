using System;
using System.Net.Sockets;

namespace Client {
    static class myClient {
        static public void Azione() {

            TcpClient c = new TcpClient("127.0.0.1", 2003);
            NetworkStream ns = c.GetStream();

            Byte[] buf;
            bool exit = false;

            do {
                Console.WriteLine("Cosa vuoi fare?");
                string command = Console.ReadLine();

                switch (command) {

                    case "CONVERT":
                        Console.WriteLine("Inserisci temperatura: ");
                        string input = Console.ReadLine();

                        buf = System.Text.Encoding.ASCII.GetBytes("CONVERT " + input);
                        ns.Write(buf);
                        
                        buf = new Byte[32];
                        ns.Read(buf, 0, buf.Length);
                        Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length));
                        break;
                    
                    case "GIVE":
                        buf = System.Text.Encoding.ASCII.GetBytes("GIVE");
                        ns.Write(buf);
                        
                        buf = new Byte[32];
                        ns.Read(buf, 0, buf.Length);
                        Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length));
                        break;
                    
                    case "ALL":
                        buf = System.Text.Encoding.ASCII.GetBytes("ALL");
                        ns.Write(buf);
                        
                        buf = new Byte[5000]; // Uso un buffer grande per non usare il ciclo, che da problemi
                        ns.Read(buf, 0, buf.Length);
                        Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length));
                        break;

                    case "EXIT":
                        exit = true;
                        buf = System.Text.Encoding.ASCII.GetBytes("EXIT");
                        ns.Write(buf, 0, buf.Length);
                        Console.WriteLine("Esco.");
                        break;

                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }
            } while (!exit);

            ns.Close();
            c.Close();
        }
    }

    class Program {
        static void Main(string[] args) {
            myClient.Azione();
        }
    }
}