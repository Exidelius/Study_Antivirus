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
            //Directory.CreateDirectory(@"C:\smth");

            CreateFolderIfNotExists(ServiceInformation.FolderPath);

            CreateFolderIfNotExists(ServiceInformation.CarantineFolderPath);
            CreateFolderIfNotExists(ServiceInformation.LogsFolderPath);
            CreateFolderIfNotExists(ServiceInformation.DBFolderPath);

            CreateObjectIfNotExists(ServiceInformation.CarantinePath);
            CreateObjectIfNotExists(ServiceInformation.LogsPath);
            //UploadDumpIfNotExists(ServiceInformation.DBPath);
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
