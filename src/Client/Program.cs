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
                Int32 port = 13000;
                IPAddress addr = IPAddress.Parse("26.129.184.182");
                client = new TcpClient();
                client.Connect(addr, port);     

                HandleConnection();          
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
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
                    Console.WriteLine("Какое вы хотите себе имя?");
                    string name = Console.ReadLine();
                    SendMessage($"name:{name}");
                    break;
                case "choice?":
                    Console.WriteLine("Какой ваш выбор? Варианты выбора предмета: paper, lizard, rock, scissors, spoke");
                    string choice = Console.ReadLine();
                    SendMessage($"choice:{choice}");
                    break;
                case "waiting":
                    Console.WriteLine("Ожидание новых игроков...");
                    break;
                case "player":
                    Console.WriteLine($"Подключился новый игрок под именем {text[1]}");
                    break;
                case "choised":
                    Console.WriteLine($"{text[1]} сделал выбор");
                    break;
                case "winner":
                    Console.WriteLine($"{text[1]} won, he chose {text[2]}");
                    break;
                case "continue?":
                    Console.WriteLine("Хотите продолжить?(yes/no)");
                    string answer = Console.ReadLine();
                    Console.WriteLine($"continue{answer}");
                    break;
            }
        }
    }
}
