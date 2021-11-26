using System;
using System.Net;
using System.Net.Sockets;

namespace Socket1_Client {
    
    static class Client {
        static public void Action() {
            
            // Connessione al Server
            TcpClient c_client = new TcpClient("127.0.0.1", 2003); // Mi connetto al server con Ip = QuestoPc & porta = 2003 (scelta)
            NetworkStream c_stream = c_client.GetStream(); // Il "Cliente" inizializza un canale di comunicazione bidirezionale con il "Delegato" del server

            Byte[] c_buffer; // "Scatola" per l'informazione che viene inviata (quindi inviata+ricevuta) sullo stream preposto

            // Fase di scambio informazioni
            c_buffer = System.Text.Encoding.ASCII.GetBytes("Messaggio di test 123!"); // Creo una "Scatola" da inviare poi sullo stream
            c_stream.Write(c_buffer, 0, c_buffer.Length); // Invio la "Scatola", che contiene il mio messaggio, sul canale di comunicazione
            
            // Saluti finali
            c_stream.Close(); // Interrompi "canale di comunicazione"
            c_client.Close(); // Saluta ed abbandona il "Delegato del Server"
        }
    }
    
    class Program {
        static void Main(string[] args) {
            Client.Action();
        }
    }
}