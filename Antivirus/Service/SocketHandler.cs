using Antivirus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Service
{
    public static class SocketHandler
    {
        private static Socket socket;

        private const string ip = "127.0.0.1";
        private const int port = 8080;

        public static void CreateSocket()
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(tcpEndPoint);
            socket.Listen(1);

            bool cont = true;
            Socket listener = socket.Accept();

            while (cont)
            {
                if (listener.Available > 0)
                {
                    string message = GetMessage(listener);

                    if (message == "EndSocket")
                    {
                        cont = false;
                    }
                    else
                    {
                        SentAsync(message, listener);
                    }
                }
            }

            socket.Close();
            CreateSocket();
        }

        public static void FinishSocket()
        {
            socket.Close();
        }

        public static async Task SentAsync(string message, Socket listener)
        {
            string answer = await Task.Run(() => MessageHandler.Processing(message));
            if (message != HandlerInformation.scanPause && message != HandlerInformation.scanContinue && message != HandlerInformation.scanStop) SentMessage(message, listener);
        }

        private static string GetMessage(Socket listener)
        {
            byte[] buffer = new byte[256];
            StringBuilder data = new StringBuilder();

            if (listener.Connected)
            {
                int size = listener.Receive(buffer);
                data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                return data.ToString();
            }
            return "";
        }

        private static void SentMessage(string message, Socket listener)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

            if (listener.Connected)
            {
                listener.Send(messageBuffer);
            }
        }
    }
}
