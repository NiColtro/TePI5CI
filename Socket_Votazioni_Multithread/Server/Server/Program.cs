using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings;
using Microsoft.VisualBasic.CompilerServices;

namespace Votazione_Server {

    internal class Candidate {
        public string Name { get; set; }
        public int Votes { get; set; }

        public Candidate(string name, int votes) {
            Name = name;
            Votes = votes;
        }
    }
    
    internal static class Tools {
        public static string ReadFile() {
            return File.ReadAllText("Z:/voti.txt");
        }
        
        public static void WriteFile(string text) {
            File.WriteAllText(@"Z:\voti.txt", text);
        }
        
        public static string ReadStream(NetworkStream ns) {
            byte[] buffer = new byte[2046];
            
            int len = ns.Read(buffer, 0, buffer.Length);
  
            return Encoding.ASCII.GetString(buffer, 0, len);
        }
        
        public static void WriteStream(NetworkStream ns, string s) {
            byte[] buf = Encoding.ASCII.GetBytes(s);
            ns.Write(buf);
        }
    }

    internal class ClientHandler {
        private TcpClient c;

        public ClientHandler(TcpClient c) {
            this.c = c;
        }

        public void Action() {

            try {
                string command = "";
                NetworkStream ns = c.GetStream();
                
                do {

                    string request = Tools.ReadStream(ns);
                    command = request.Split(" ")[0];

                    switch (command) {
                        
                        case "VOTA":
                            string[] voteLines = Tools.ReadFile().Split(";");
                            string voteNewFile = "";
                            
                            bool newCandidate = true;
                            string voteCandidate = request.Split(" ")[1];

                            foreach (string line in voteLines)
                                if (line.Split(":")[0] == voteCandidate) {
                                    int newVotes = int.Parse(line.Split(":")[1]) + 1;
                                    
                                    voteNewFile += (voteCandidate + ":" + newVotes + ";");
                                    newCandidate = false;
                                }
                                else if (line != "")
                                    voteNewFile += (line + ";");

                            if (newCandidate)
                                voteNewFile += (voteCandidate + ":1;");
                            
                            Tools.WriteFile(voteNewFile);
                            Tools.WriteStream(ns, "Hai votato per " + voteCandidate);
                            break;
                        
                        case "RISULTATO":
                            string[] singleResultLines = Tools.ReadFile().Split(";");
                            
                            string singleResultcandidate = request.Split(" ")[1];

                            foreach (string line in singleResultLines)
                                if (line.Split(":")[0] == singleResultcandidate) {
                                    int votes = int.Parse(line.Split(":")[1]);
                                    
                                    Tools.WriteStream(ns, "Il candidato " + singleResultcandidate + " ha " + votes + " voti");
                                }
                            break;
                        
                        case "RISULTATI":
                            string[] totalResultsLines = Tools.ReadFile().Split(";");
                            
                            List<Candidate> candidates = new List<Candidate>();

                            foreach (string line in totalResultsLines)
                                if (line != "")
                                    candidates.Add(new Candidate(line.Split(":")[0], int.Parse(line.Split(":")[1])));
                            
                            candidates.Sort((a, b) => b.Votes.CompareTo(a.Votes));
                            string totalResultsResponse = "";
                            
                            candidates.ForEach(c => totalResultsResponse += c.Name + " => " + c.Votes + "\n");
                            Tools.WriteStream(ns, totalResultsResponse);
                            break;

                        case "EXIT":
                            Console.WriteLine("Client disconnected");
                            ns.Close();
                            c.Close();
                            break;
                        
                        default:
                            Tools.WriteStream(ns, "Comando sconosciuto.");
                            break;                        
                    }
                } while (command != "EXIT");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
    
    internal class Program {
        static void Main(string[] args) {

            TcpListener lis = new TcpListener(IPAddress.Loopback, 2003);
            lis.Start();

            while (true) {
                ClientHandler ch = new ClientHandler(lis.AcceptTcpClient());
                new Thread(ch.Action).Start();
                Console.WriteLine("Handling new client");
            }
            
            lis.Stop();
        }
    }
}
