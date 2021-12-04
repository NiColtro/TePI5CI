using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server {
    static class myServer {
        static private void w2f(string s) {
            BinaryWriter bw = new BinaryWriter(File.Open(@"Z:\temperature.txt", FileMode.Append));
            bw.Write(System.Text.Encoding.ASCII.GetBytes(s));
            bw.Close();
        }
        
        static private string allFile() {
            return File.ReadAllText(@"Z:\temperature.txt");
        }
        
        static public void Azione() {
            IPAddress ip = Dns.GetHostAddresses("127.0.0.1")[0];
            TcpListener lis = new TcpListener(ip, 2003);
            
            lis.Start();

            TcpClient c = lis.AcceptTcpClient();
            NetworkStream ns = c.GetStream();

            string command = "";
            
            do {
                Byte[] buf = new Byte[32];
                ns.Read(buf, 0, buf.Length);
                string request = System.Text.Encoding.ASCII.GetString(buf);

                if (request.Contains("EXIT"))
                    command = "EXIT";
                else if (request.Contains("GIVE"))
                    command = "GIVE";
                else if (request.Contains("ALL"))
                    command = "ALL";
                else command = request.Split(" ")[0];
                
                buf = new Byte[32];
                switch (command) {
                    case "CONVERT":
                        int t = int.Parse(request.Split(" ")[1]); //(0 °C × 9/5) + 32 = 32 °F
                        t = (t * 9 / 5) + 32;
                    
                        buf = System.Text.Encoding.ASCII.GetBytes(t.ToString());
                        ns.Write(buf, 0, buf.Length);
                        w2f("CONVERT " + t + Environment.NewLine);
                        break;
                    
                    case "GIVE":
                        Console.WriteLine("Inserisci dato da inviare: ");
                        string input = Console.ReadLine();
                        buf = System.Text.Encoding.ASCII.GetBytes(input);
                        ns.Write(buf, 0, buf.Length);
                        w2f("GIVE " + input);
                        break;
                    
                    case "ALL":
                        Console.WriteLine(allFile());
                        buf = System.Text.Encoding.ASCII.GetBytes(allFile());
                        ns.Write(buf, 0, buf.Length);
                        break;
                    
                    case "EXIT":
                        Console.WriteLine("Esco.");
                        break;
                }
            } while (command != "EXIT");
            
            ns.Close();
            c.Close();
            lis.Stop();
        }
    }
    
    class Program {
        static void Main(string[] args) {
            myServer.Azione();
        }
    }
}