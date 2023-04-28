using Antivirus.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Service
{
    public static class Carantine
    {
        private static Dictionary<string, string> virusesInCarantine = new Dictionary<string, string>();

        public static string GoInCarantine(string path)
        {
            try
            {
                string newPath = GetCarantinePath(path);

                File.Move(path, newPath);
                virusesInCarantine.Add(newPath, path);

                File.AppendAllText(ServiceInformation.SocketPath, $"В карантин добавлен: {newPath}\r\n");

                return newPath;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex.Message}\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex}\r\n");

                return "";
            }
        }

        private static string GetCarantinePath(string path)
        {
            var safeFile = Path.ChangeExtension(path, ".virusdetected");
            var fileInfo = new FileInfo(safeFile);

            return $@"{ServiceInformation.CarantinePath}\{fileInfo.Name}";
        }

        public static string GoOutCatantine(string path)
        {
            try
            {
                string carantinePath = GetCarantinePath(path);
                File.Move(carantinePath, path);
                virusesInCarantine.Remove(carantinePath);

                var fileInfo = new FileInfo(path);
                return $"Восстановлен файл {fileInfo.Name}";
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex.Message}\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex}\r\n");
                return "";
            }
        }

        private static void DropFromCarantine(string path)
        {
            try
            {
                string oldFile = virusesInCarantine[path];
                var oldInfo = new FileInfo(oldFile);

                string resultFile = Path.ChangeExtension(path, ".exe");
                var resultInfo = new FileInfo(resultFile);

                string newPath = $@"{oldInfo.DirectoryName}\{resultInfo.Name}";
                File.Move(path, newPath);
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex.Message}\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, $"{ex}\r\n");
            }
        }

        private static string DropCarantine()
        {
            if (virusesInCarantine.Keys.Count > 0) {
                foreach (var item in virusesInCarantine.Keys)
                {
                    DropFromCarantine(item);
                }
            }
            virusesInCarantine.Clear();
            return "Все угрозы освобождены";
        }

        private static string DeleteAll(string path)
        {
            RecycleBin recycle = new RecycleBin();

            if (virusesInCarantine.Keys.Count > 0)
            {
                foreach (var item in virusesInCarantine.Keys)
                {
                    recycle.Recycle(item);
                }
            }

            virusesInCarantine.Clear();
            return "Все угрозы уничтожены";
        }

        private static string DeleteOne(string path)
        {
            RecycleBin recycle = new RecycleBin();
            try
            {
                string carantinePath = GetCarantinePathPath(path);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
