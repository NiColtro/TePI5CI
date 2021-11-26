using System;
using System.Net;
using System.Net.Sockets;

namespace Socket1_Server {

    static class Server {
        static public void Action() {
            
            // Fase di setup
            IPAddress s_ip = Dns.GetHostAddresses("127.0.0.1")[0]; // Prendo il primo tra gli IP del PC (ricorda: 127.0.0.1 = lookback = questoPc!)
            TcpListener s_listener = new TcpListener(s_ip, 2003); // Nuovo "Portinaio" => questo Pc, porta a scelta (2003 mi piace.)
            s_listener.Start(); // Starto il "Portinaio"

            // Fase di attesa e "Accoglienza Cliente"
            TcpClient s_client = s_listener.AcceptTcpClient(); // Appena arriva un "Cliente", il "Portinaio" passa la richiesta ad un nuovo "Delegato" del server
            NetworkStream s_stream = s_client.GetStream(); // Il "Delegato" inizializza un canale di comunicazione bidirezionale con il Cliente

            Byte[] s_buffer = new Byte[1024]; // "Scatola" per l'informazione che viene ricevuta (quindi inviata+ricevuta) sullo stream preposto
            
            // Fase di scambio informazioni
            while (s_stream.Read(s_buffer, 0, s_buffer.Length) > 0) { // Finché ci sono "Scatole" sullo stream
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(s_buffer)); // Leggi contenuto scatola, traducilo e stampalo a video
                s_buffer = new Byte[1024];
            }

            // Saluti finali
            s_stream.Close(); // Interrompi "canale di comunicazione"
            s_client.Close(); // Saluta ed abbandona il "Cliente"
        }
    }
    
    class Program {
        static void Main(string[] args) {
            Server.Action();
        }
    }
}