using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Antivirus.Data;
using Antivirus.Service;

namespace Antivirus
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();

            //eventLog1 = new EventLog();
            //if (!EventLog.SourceExists("MySource"))
            //{
            //    EventLog.CreateEventSource(
            //        "MySource", "MyNewLog");
            //}
            //eventLog1.Source = "MySource";
            //eventLog1.Log = "MyNewLog";


            //Запрет остановки службы после запуска
            this.CanStop = false;
            this.CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            DataChecker.CheckAndCreateIfNotExists();

            Thread SocketThread = new Thread(SocketHandler.CreateSocket);
            SocketThread.Start();

            Thread ShaduleThread = new Thread(SocketWorker.CreateSocket);
            ShaduleThread.Start();

            Thread WatchThread = new Thread(Worker.WatchForAntivirusService);
            WatchThread.Start();

        }

        protected override void OnStop()
        {
            SocketHandler.FinishSocket();
        }
    }
}
