﻿using System;
using System.IO;
using System.Linq;
using System.Net;
<<<<<<< server
=======
using System.Net.Sockets;
>>>>>>> master
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace pr5_Kochetov_Lipina
{
<<<<<<< server
    public class Program
    {
        static IPAddress ServerIPAddress;
        static int ServerPort;
        static int MaxClient;
        static int Duration;
        static List<Client> AllClients = new List<Client>();
        public void Main(string[] args)
        {
            OnSettings();
            Thread tListner = new Thread(ConnectServer);
            tListner.Start();
            Thread tDisconnect = new Thread(DisconnectClient);
            tDisconnect.Start();
            while (true) SetCommand();
        }
        static void DisconnectClient()
        {
            while (true)
            {
                for (int i = 0; i < AllClients.Count; i++)
                {
                    int ClientDuration = (int)DateTime.Now.Subtract(AllClients[i].DateConnect).TotalSeconds;
                    if (ClientDuration > Duration)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Client: {AllClients[i].Token} disconnect from server due to timeout");
                        AllClients.RemoveAt(i);
                    }
                }
                Thread.Sleep(1000);
            }
=======

    class Program
    {
        static IPAddress ServerIPAddress;
        static int ServerPort;
        static string ClientToken;
        static DateTime ClientDateConnection;
        static void Main(string[] args)
        {
            OnSettings();
            Thread tCheckToken = new Thread(CheckToken);
            tCheckToken.Start();
            while (true) SetCommand();
        }
        static void OnSettings()
        {
            string Path = Directory.GetCurrentDirectory() + "/.config";
            if (File.Exists(Path))
            {
                StreamReader sr = new StreamReader(Path);
                ServerIPAddress = IPAddress.Parse(sr.ReadLine());
                ServerPort = int.Parse(sr.ReadLine());
                sr.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Server IP-address: {ServerIPAddress.ToString()}; \nServer port: {ServerPort};");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Please, provide the IP-address: ");
                ServerIPAddress = IPAddress.Parse(Console.ReadLine());
                Console.Write($"Please, specify the port: ");
                ServerPort = int.Parse(Console.ReadLine());
                StreamWriter sw = new StreamWriter(Path);
                sw.WriteLine(ServerIPAddress.ToString());
                sw.WriteLine(ServerPort);
                sw.Close();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("To change, write the command: /config");
>>>>>>> master
        }
        static void SetCommand()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string Command = Console.ReadLine();
            switch (Command)
            {
                case "/config": File.Delete(Directory.GetCurrentDirectory() + "/.config"); OnSettings(); break;
<<<<<<< server
                case "/status": GetStatus(); break;
                case "/help": Help(); break;
                default: if (Command.Contains("/disconnect")) DisconnectServer(Command); break;
            }
        }
        static string SetCommandClient(string Command)
        {
            if (Command == "/token")
                if (AllClients.Count < MaxClient)
                {
                    var newClient = new Client();
                    AllClients.Add(newClient);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"New client connection: {newClient.Token}");
                    return newClient.Token;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There isn't enough space on the license server");
                    return "/limit";
                }
            else
            {
                var Client = AllClients.Find(x => x.Token == Command);
                return Client != null ? "/connect" : "/disconnect";
            }
        }
        static void DisconnectServer(string Command)
        {
            try
            {
                string Token = Command.Replace("/disconnect ", "");
                var DisconnectClient = AllClients.Find(x => x.Token == Token);
                AllClients.Remove(DisconnectClient);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Client: {Token} disconnect from server");
=======
                case "/connect": ConnectServer(); break;
                case "/status": GetStatus(); break;
                case "/help": Help(); break;
            }
        }
        static void ConnectServer()
        {
            IPEndPoint EndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
            Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.Connect(EndPoint);
>>>>>>> master
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
            }
<<<<<<< server
        } 


        static void ConnectServer()
        {
            IPEndPoint EndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
            Socket SocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketListener.Bind(EndPoint);
            SocketListener.Listen(MaxClient);
            while (true)
            {
                Socket Handler = SocketListener.Accept();
                byte[] bytes = new byte[10485760];
                int byteRec = Handler.Receive(bytes);
                string Message = Encoding.UTF8.GetString(bytes, 0, byteRec);
                string Response = SetCommandClient(Message);
                Handler.Send(Encoding.UTF8.GetBytes(Response));
            }
        }
        static void GetStatus()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Count clients: {AllClients.Count}");
            foreach (var client in AllClients)
            {
                int Duration = (int)DateTime.Now.Subtract(client.DateConnect).TotalSeconds;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Client: {client.Token}, time connection: {client.DateConnect.ToString("HH:mm:ss dd.MM")}, duration: {Duration}");
            }
        }
        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Command to the clients: ");
=======
            if (Socket.Connected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connection to server successful");
                Socket.Send(Encoding.UTF8.GetBytes("/token"));
                byte[] bytes = new byte[10485760];
                int byteRec = Socket.Receive(bytes);
                string Response = Encoding.UTF8.GetString(bytes, 0, byteRec);
                if (Response == "/limit")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There isn't enough space on the license server");
                }
                else
                {
                    ClientToken = Response;
                    ClientDateConnection = DateTime.Now;
                    Console.WriteLine($"Received connection token: {ClientToken}");
                }
            }
        }
        static void CheckToken()
        {
            while (true)
            {
                if (!String.IsNullOrEmpty(ClientToken))
                {
                    IPEndPoint EndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
                    Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        Socket.Connect(EndPoint);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    if (Socket.Connected)
                    {
                        Socket.Send(Encoding.UTF8.GetBytes(ClientToken));
                        byte[] bytes = new byte[10485760];
                        int byteRec = Socket.Receive(bytes);
                        string Response = Encoding.UTF8.GetString(bytes, 0, byteRec);
                        if (Response == "/disconnect")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The client is disconnected from server");
                            ClientToken = String.Empty;
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
        static void GetStatus()
        {
            int Duration = (int)DateTime.Now.Subtract(ClientDateConnection).TotalSeconds;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Client: {ClientToken}, time connection: {ClientDateConnection.ToString("HH:mm:ss dd.MM")}, duration: {Duration}");
        }
        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Command to the server: ");
>>>>>>> master
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/config");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  - set initial settings");
            Console.ForegroundColor = ConsoleColor.Green;
<<<<<<< server
            Console.Write("/disconnect");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - disconnect users from server");
=======
            Console.Write("/connect");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - connection to the server");
>>>>>>> master
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/status");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  - show list users");
<<<<<<< server
        }



        static void OnSettings()
        {
            string Path = Directory.GetCurrentDirectory() + "/.config";
            if (File.Exists(Path))
            {
                StreamReader sr = new StreamReader(Path);
                ServerIPAddress = IPAddress.Parse(sr.ReadLine());
                ServerPort = int.Parse(sr.ReadLine());
                MaxClient = int.Parse(sr.ReadLine());
                Duration = int.Parse(sr.ReadLine());
                sr.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Server IP-address: {ServerIPAddress.ToString()};\nServer port: {ServerPort};\nMax client: {MaxClient};\nDuration: {Duration};");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Please, provide the IP-address: ");
                ServerIPAddress = IPAddress.Parse(Console.ReadLine());
                Console.Write($"Please, specify the port: ");
                ServerPort = int.Parse(Console.ReadLine());
                Console.Write($"Please, specify the maximum number of clients: ");
                MaxClient = int.Parse(Console.ReadLine());
                Console.Write($"Please, specify the duration of the license: ");
                Duration = int.Parse(Console.ReadLine());
                StreamWriter sw = new StreamWriter(Path);
                sw.WriteLine(ServerIPAddress.ToString());
                sw.WriteLine(ServerPort);
                sw.WriteLine(MaxClient);
                sw.WriteLine(Duration);
                sw.Close();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("To change, write the command: /config");





=======
>>>>>>> master
        }
    }
}


 
