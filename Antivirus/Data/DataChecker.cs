using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Data
{
    public static class DataChecker
    {
        public static void CheckAndCreateIfNotExists()
        {
            CreateFolderIfNotExists(ServiceInformation.FolderPath);

            CreateFolderIfNotExists(ServiceInformation.CarantineFolderPath);
            CreateFolderIfNotExists(ServiceInformation.SocketFolderPath);
            CreateFolderIfNotExists(ServiceInformation.DBFolderPath);

            CreateObjectIfNotExists(ServiceInformation.CarantinePath);
            CreateObjectIfNotExists(ServiceInformation.SocketPath);
            CreateObjectIfNotExists(ServiceInformation.DBPath);
        }

        private static void CreateFolderIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void UploadDumpIfNotExists(string path)
        {
            string fileName = @"\db.txt";

            if (!File.Exists(path))
            {
                File.Copy(Directory.GetParent(Directory.GetCurrentDirectory()) + @"\DBBackup" + fileName, path);
            }
        }

        private static void CreateObjectIfNotExists(string path)
        {
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
        }
    }
}
