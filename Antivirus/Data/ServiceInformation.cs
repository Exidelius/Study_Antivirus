using System;

namespace Antivirus.Data
{
    /// <summary>
    /// Содержит пути для работы с сервисом.
    /// Все необходимые данные класса должны содержаться в AppData\Roaming, т.к. это банально проще для работы, ведь получить доступ к этой директории можно с помощью кода, не прибегая к точным путям. При этом аппдата привязана к конкретному пользователю.
    /// </summary>
    static class ServiceInformation
    {
        private static string _folderName = "AntivirusFolder";
        private static string _folderPath = /*GetLocalPath()*/$@"C:\{_folderName}";

        public static string ServiceName { get { return "AntivirusService"; } }

        public static string FolderPath { get { return _folderPath; } }
        public static string DBFolderPath { get { return FolderPath + @"\DB"; } }
        public static string CarantineFolderPath { get { return FolderPath + @"\Carantine"; } }
        public static string LogsFolderPath { get { return FolderPath + @"\Logs"; } }

        public static string LogsPath { get { return LogsFolderPath + @"\logs.txt"; } }
        public static string DBPath { get { return DBFolderPath + @"\db.txt"; } }
        public static string CarantinePath { get { return CarantineFolderPath + @"\carantine.txt"; } }

        //private static string GetLocalPath() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + _folderName;
    }
}
