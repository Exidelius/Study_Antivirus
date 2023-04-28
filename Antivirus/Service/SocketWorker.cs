using Antivirus.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Service
{
    public static class SocketWorker
    {
        private static Socket socket;

        private const string ip = "127.0.0.2";
        private const int port = 8082;

        public static void CreateSocket()
        {
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(tcpEndPoint);
            socket.Listen(100);

            while (true)
            {
                bool cont = true;
                Socket listener = socket.Accept();

                while (cont)
                {
                    if (listener.Available > 0)
                    {
                        string message = ReceiveMessage(listener);
                        if (message == "EndOfSocket")
                        {
                            cont = false;
                        }
                        else
                        {
                            File.AppendAllText(ServiceInformation.SocketFolderPath, "Получил сообщение " + message + "\r\b");
                            SentAsync(message, listener);
                        }
                    }
                }
                socket.Close();
                CreateSocket();
            }
        }

        private static string ReceiveMessage(Socket listener)
        {
            var buffer = new byte[256];
            var size = 0;
            var data = new StringBuilder();

            if (listener.Connected)
            {
                size = listener.Receive(buffer);
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

        private static async Task SentAsync(string message, Socket listener)
        {
            await Task.Run(() => SheduleHandler.MessageHandlerForShadule(message));
            String answer = await Task.Run(() => SheduleHandler.StartTask());
            File.AppendAllText(ServiceInformation.SocketPath, "Отпраляю ответ " + answer + "\r\n");
            try
            {
                SentMessage(answer, listener);
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, ex + "\r\n");
            }
            File.AppendAllText(ServiceInformation.SocketPath, "Отправил ответ " + answer + "\r\n");
        }

        public static void FinishSocket()
        {
            socket.Close();
        }
    }
}

