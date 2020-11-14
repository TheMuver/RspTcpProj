using System.Threading;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static NetworkStream stream;
        static TcpClient client;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter ip:port");
                string[] n = Console.ReadLine().Split(":");
                Int32 port = Convert.ToInt32(n[1]);
                IPAddress addr = IPAddress.Parse(n[0]);
                client = new TcpClient();
                client.Connect(addr, port);     

                HandleConnection();          
            }
            catch(SocketException)
            {
                Console.WriteLine("Network err. Try again.");
            }
            catch(Exception)
            {
                Console.WriteLine("Something went wrong.");
            }

            Console.ReadLine();
        }


        public static void SendMessage(string data)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            stream = client.GetStream();
            stream.Write(msg, 0, msg.Length);
        }

        public static void HandleConnection()
        {
            Byte[] bytes = new Byte[256];
            string data = null;
            stream = client.GetStream();
            int i;

            while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i); 

                RequestProccesing(data);         
            }

            client.Close();
        }

        public static void RequestProccesing(string data)
        {
            stream = client.GetStream();   
            string[] text = data.Split(':');

            switch(text[0])
            {
                case "name?":
                    Console.WriteLine("What's your name?");
                    string name = Console.ReadLine();
                    SendMessage($"name:{name}");
                    break;
                case "choice?":
                    Console.WriteLine("Chosen subject? Options to choose: paper, lizard, rock, scissors, spoke");
                    string choice = Console.ReadLine();
                    SendMessage($"choice:{choice}");
                    break;
                case "waiting":
                    Console.WriteLine("Waiting for new players...");
                    break;
                case "player":
                    Console.WriteLine($"A new player named {text[1]} has joined");
                    break;
                case "choised":
                    Console.WriteLine($"{text[1]} make a choice");
                    break;
                case "winner":
                    Console.WriteLine($"{text[1]} won, he chose {text[2]}");
                    break;
                case "continue?":
                    Console.WriteLine("Want to continue??(yes/no)");
                    string answer = Console.ReadLine();
                    Console.WriteLine($"continue:{answer}");
                    break;
            }
        }
    }
}
