using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Antivirus.Data;
using Antivirus.Service;

namespace Antivirus
{
    public partial class Service1 : ServiceBase
    {
        Thread SocketThread;
        Thread SheduleThread;
        Thread WatchThread;

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
            this.CanStop = true;
            this.CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            //Thread dataCheckerThread = new Thread(DataChecker.CheckAndCreateIfNotExists);
            //dataCheckerThread.Start();
            DataChecker.CheckAndCreateIfNotExists();

            SocketThread = new Thread(SocketHandler.CreateSocket);
            SocketThread.Start();

            SheduleThread = new Thread(SocketWorker.CreateSocket);
            SheduleThread.Start();

            WatchThread = new Thread(Worker.WatchForAntivirusService);
            WatchThread.Start();

        }

        protected override void OnStop()
        {
            SocketHandler.FinishSocket();
            SocketWorker.FinishSocket();
            WatchThread.Abort();
            try
            {
                SocketThread.Abort();
                SheduleThread.Abort();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
