using System.ServiceProcess;
using System.Threading;
using Antivirus.Data;

namespace Antivirus.Service
{
    static class Worker
    {
        private static ServiceController sc;
        public static void WatchForAntivirusService()
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scCurrent in scServices)
            {

                if (scCurrent.ServiceName == ServiceInformation.ServiceName)
                {
                    while (true)
                    {
                        sc = new ServiceController(ServiceInformation.ServiceName);
                        if (sc.Status == ServiceControllerStatus.Stopped) sc.Start();
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
