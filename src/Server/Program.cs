using System.Runtime.InteropServices;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

using RockScissorsPaperLib;


namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.WaitNewConnections();
        }
    }

    public class Server
    {
        private Dictionary<TcpClient, string> clients;
        private Dictionary<TcpClient, rsp> players;

        public Server()
        {
            clients = new Dictionary<TcpClient, string>();
            players = new Dictionary<TcpClient, rsp>();
        }

        public void WaitNewConnections()
        {
            (new Thread(new ThreadStart(Game))).Start(); // старт игрового обработчика

            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress addr = IPAddress.Parse("26.129.184.182");
                server = new TcpListener(addr, port);
                server.Start();
                Console.WriteLine("Server is online");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    (new Thread(new ParameterizedThreadStart(HandleConnection))).Start(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.Stop();
            }
        }

        public void HandleConnection(Object c)
        {
            TcpClient client = c as TcpClient;
            Console.WriteLine("New handler is online");

            Byte[] bytes = new byte[256];
            String data = null;
            NetworkStream stream = client.GetStream();
            int i;

            SendMessage(client, "name?"); // Запрос на имя
            
            while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("New request came in");
                RequestProcessing(client, data);
            }
        }

        public void Game()
        {
            Console.WriteLine("Game cycle is online");
            while (true)
            {
                if (clients.Count < 2) // если не достаточно игроков для игры
                    foreach (var x in clients.Keys)
                        SendMessage(x, "waiting");
                else if (players.Keys.Count < 2) // смотрим, все ли выбрали свои предметы
                {
                    foreach (var x in clients.Keys)
                        if (!players.ContainsKey(x))
                            SendMessage(x, "choice?");
                }
                else
                {
                    AdditionalWinnerChecker w = new AdditionalWinnerChecker();
                    var answers = players.Values.ToArray(); // предметы
                    TcpClient[] playerses = players.Keys.ToArray(); // клиенты
                    int winner = w.Play(answers[0], answers[1])-1;
                    foreach (var x in clients.Keys)
                    {
                        SendMessage(x, $"winner:{clients[playerses[winner]]}:{answers[winner]}");
                    }
                    break;
                }

                Thread.Sleep(1000); // обработчик засыпает на секунду
            }
        }

        public void RequestProcessing(TcpClient client, string mess)
        {
            String[] req = mess.Split(":");
            switch(req[0])
            {
                case "name": // запись в клиенты
                    clients.Add(client, req[1]);
                    foreach (var x in clients.Keys)
                        if (x != client)
                            SendMessage(x, $"player:{req[1]}");
                        Console.WriteLine($"New player is {req[1]}");
                    break;
                case "choice":
                    rsp choice = rsp.Lizard;
                    switch(req[1])
                    {
                        case "paper":
                            choice = rsp.Paper;
                            break;
                        case "lizard":
                            choice = rsp.Lizard;
                            break;
                        case "rock":
                            choice = rsp.Rock;
                            break;    
                        case "scissors":
                            choice = rsp.Scissors;
                            break;
                        case "spoke":
                            choice = rsp.Spoke;
                            break;
                    }
                    players.Add(client, choice);
                    foreach (var x in clients.Keys)
                        if (true/*x != client*/)
                            SendMessage(x, $"choised:{clients[client]}");
                    break;
                case "continue":
                    break;
                default:
                    Console.WriteLine($"Unknow request: {mess}");
                    break;
            }
        }

        public void SendMessage(TcpClient client, string mess)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(mess);
            NetworkStream stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
