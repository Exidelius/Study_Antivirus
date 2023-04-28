using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Antivirus.Data;

namespace Antivirus.Service
{
    public static class SheduleHandler
    {
        private static DateTime TimeForScan = default(DateTime);
        private static string path = "";

        public static void MessageHandlerForShedule(string message)
        {
            string[] newArray = message.Split('#');
            DateTime newTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(newArray[1]), Convert.ToInt32(newArray[2]), 0);
            TimeForScan = newTime;
            path = newArray[0];
        }

        public static string StartTask()
        {
            while (true)
            {
                if (TimeForScan == default(DateTime))
                {
                    File.AppendAllText(ServiceInformation.SocketPath, $"Надо {TimeForScan.Hour} {TimeForScan.Minute}" + "\r\n");
                    File.AppendAllText(ServiceInformation.SocketPath, $"Надо {DateTime.Now.Hour} {DateTime.Now.Minute}" + "\r\n");
                    while (TimeForScan.Hour != DateTime.Now.Hour || TimeForScan.Minute != DateTime.Now.Minute)
                    {
                        Task.Delay(10000);
                    }

                    File.AppendAllText(ServiceInformation.SocketPath, "Время пришло " + "\r\n");

                    List<String> viruses = Scan.ScanFolder(path);

                    String result = "";
                    if (viruses.Count > 0)
                    {
                        MessageHandler.viruses.Clear();
                        MessageHandler.viruses.AddRange(viruses);

                        foreach (var virus in viruses)
                        {
                            result += virus + "#";
                        }
                    }

                    File.AppendAllText(ServiceInformation.SocketPath, "Получилось " + result + "\r\n");

                    return result;
                }
        }
        }
    }
}
