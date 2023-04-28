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
        private static string _folderPath;

        public static string ServiceName { get { return "AntivirusService"; } }

        public static string FolderPath { get { return _folderPath; } }
        public static string DBFolderPath { get { return FolderPath + "AntivirusService"; } }
        public static string CarantineFolderPath { get { return FolderPath + "Carantine"; } }
        public static string SocketFolderPath { get { return FolderPath + "Socket"; } }

        public static string SocketPath { get { return SocketFolderPath + @"\antivirus_socket.txt"; } }
        public static string DBPath { get { return SocketFolderPath + @"\db.txt"; } }
        public static string CarantinePath { get { return SocketFolderPath + @"\antivirus_carantine.txt"; } }

        static ServiceInformation()
        {
            _folderPath = GetLocalPath();
        }
        private static string GetLocalPath() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + _folderName;
    }
}
