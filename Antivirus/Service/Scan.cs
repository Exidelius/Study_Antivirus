using Antivirus.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;

namespace Antivirus.Service
{
    public static class Scan
    {
        private static bool _isPause = false;
        private static bool _isStop = false;

        private static List<string> txtSign = File.ReadAllLines(ServiceInformation.DBPath).ToList();


        public static void SetStop(bool value)
        {
            _isStop = value;
        }

        public static void SetPause(bool value)
        {
            _isStop = value;
        }

        public static List<string> ScanFolder(string path)
        {
            List<string> result = new List<string>();

            var files = GetFiles(path);

            foreach (var file in files)
            {
                //string crutchThing = file.Replace(@"\", @"\\\").Replace(@"\\\\\\\\\", @"\\\");
                //string scanres = ScanFile(crutchThing);
                string scanres = ScanFile(file);

                switch (scanres)
                {
                    case "None":
                        break;
                    case HandlerInformation.resultStop:
                        File.AppendAllText(ServiceInformation.SocketFolderPath, "Остановка сканирования папки\r\n");
                        return new List<string> { HandlerInformation.resultStop };
                    default:
                        result.Add(scanres);
                        break;
                }
            }

            return result;
        }

        public static string ScanFile(string path)
        {
            byte[] byteArrayFile = File.ReadAllBytes(path);
            File.AppendAllText(ServiceInformation.SocketPath, $"Сканирую файл {path}\r\n");

            foreach (var signature in txtSign)
            {
                if (_isStop)
                {
                    File.AppendAllText(ServiceInformation.SocketPath, "Остановка сканирования файла\r\n");
                    return HandlerInformation.resultStop;
                }

                if (CheckSignatureAsync(byteArrayFile, signature).Result)
                {
                    if (_isStop)
                    {
                        File.AppendAllText(ServiceInformation.SocketPath, "Остановка сканирования файла\r\n");
                        return HandlerInformation.resultStop;
                    }

                    return path;
                }
            }
            File.AppendAllText(ServiceInformation.SocketPath, "Файл чист\r\n");
            return "None";
        }

        private static async Task<bool> CheckSignatureAsync(byte[] file, string signature)
        {
            try
            {
                string[] bytesSignature = signature.Split(' ');

                int signatureLength = bytesSignature.Length;
                int similarityCount = 0;

                for (int i = 0; i < file.Length; i++)
                {
                    if (_isStop)
                    {
                        File.AppendAllText(ServiceInformation.SocketPath, "Обнаружена остановка сканирования");
                        return false;
                    }

                    while (_isPause)
                    {
                        await Task.Delay(1000);
                    }

                    if (bytesSignature[similarityCount] == "??" || byte.Parse(bytesSignature[similarityCount], System.Globalization.NumberStyles.HexNumber) == file[i])
                    {
                        similarityCount++;
                    }
                    else
                    {
                        similarityCount = 0;
                        continue;
                    }

                    if (similarityCount == signatureLength) return true;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex.Message}\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex}\r\n");
            }
            return false;
        }

        public static List<string> ScanMachine()
        {
            List<string> result = new List<string>();

            try
            {
                DriveInfo[] discs = DriveInfo.GetDrives();

                File.AppendAllText(ServiceInformation.SocketPath, "Диски найдены:\r\n");

                foreach (var disc in discs)
                {
                    File.AppendAllText(ServiceInformation.SocketPath, "Сканирование диска:\r\n");

                    string[] files = GetFiles(disc.Name);
                    if (files[0] == HandlerInformation.resultStop) return new List<string> { HandlerInformation.resultStop };

                    File.AppendAllText(ServiceInformation.SocketPath, $"Получил файлы: {files.Length}\r\n");

                    foreach (var file in files)
                    {
                        string scanres = ScanFile(file);

                        if (scanres == HandlerInformation.resultStop)
                        {
                            File.AppendAllText(ServiceInformation.SocketPath, "Остановка сканирования\r\n");
                            return new List<string> { HandlerInformation.resultStop };
                        } 
                        result.Add(scanres);
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex.Message}\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex}\r\n");
            }

            return result;
        }

        private static string[] GetFiles(string path)
        {
            var files = new List<string>();

            try
            {
                if (_isStop) return new string[] { HandlerInformation.resultStop };

                files.AddRange(Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly));

                foreach (var directory in Directory.GetDirectories(path))
                {
                    if (_isStop) return new string[] { HandlerInformation.resultStop };

                    files.AddRange(GetFiles(directory));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is UnauthorizedAccessException))
                {
                    File.AppendAllText(ServiceInformation.SocketPath, $"Сообщение: {ex.Message}\r\n");
                    File.AppendAllText(ServiceInformation.SocketPath, $"Ошибка: {ex}\r\n");
                }
            }

            return files.ToArray();
        }
    }
}
