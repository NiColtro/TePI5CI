using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace VerificaSocketTCP_Server {
    static class Server {
        public static void Azione() {
            // TCP Listener Setup
            IPAddress ip = Dns.GetHostAddresses("127.0.0.1")[0];
            TcpListener lis = new TcpListener(ip, 2003);
            lis.Start();

            // Socket setup
            TcpClient c = lis.AcceptTcpClient();
            NetworkStream ns = c.GetStream();

            // Read from Socket
            Byte[] buf = new byte[64];
            ns.Read(buf, 0, buf.Length);
            string payload = System.Text.Encoding.ASCII.GetString(buf);

            // Operations
            int n1 = int.Parse(payload.Split(';')[0]);
            int n2 = int.Parse(payload.Split(';')[2]);
            string op = payload.Split(';')[1];
            int result;
            
            switch (op) {
                case "+":
                    result = n1 + n2;
                    break;
                case "-":
                    result = n1 - n2;
                    break;
                case "*":
                    result = n1 * n2;
                    break;
                case "/":
                    result = n1 / n2;
                    break;
                default:
                    result = -1;
                    break;
            }

            // Send op
            buf = System.Text.Encoding.ASCII.GetBytes(Environment.NewLine + n1 + " " + op + " " + n2 + " = " + result);
            ns.Write(buf);

            // Save 2 file
            BinaryWriter bw = new BinaryWriter(File.Open(@"Z:\op.txt", FileMode.Append));
            bw.Write(buf);
            bw.Close();
            
            // Close Socket things
            ns.Close();
            c.Close();
            lis.Stop();
        }
    }
    
    class Program {
        static void Main(string[] args) {
            Server.Azione();
        }
    }
}