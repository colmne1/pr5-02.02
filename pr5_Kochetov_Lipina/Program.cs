﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pr5_Kochetov_Lipina
{
    public class Program
    {
        static IPAddress ServerIPAddress;
        static int ServerPort;
        static int MaxClient;
        static int Duration;
        static List<Client> AllClients = new List<Client>();
        public void Main(string[] args)
        {
        }
        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Command to the clients: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/config");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  - set initial settings");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/disconnect");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - disconnect users from server");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/status");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  - show list users");
        }
    }
}
