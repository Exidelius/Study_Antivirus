using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Data
{
    public static class HandlerInformation
    {
        public const string scanFile = "fileScan";
        public const string scanPath = "pathScan";
        public const string scanFull = "fullScan";

        public const string scanPause = "ScanPause";
        public const string scanContinue = "ScanContinue";
        public const string scanStop = "ScanStop";

        public const string delete = "deleteViruses";
        public const string carantine = "carantineViruses";

        public const string carantineShow = "showCarantine";
        public const string carantineRecoverAll = "returnAllFromCarantine";
        public const string carantineDeleteAll = "deleteAllFromCarantine";
        public const string carantineRecoverOne = "returnOneFromCarantine";
        public const string carantineDeleteOne = "deleteOneFromCarantine";


        public const string resultNone = "Вирусы не найдены";
        public const string resultStop = "Сканирование остановлено";
        public const string resultDelete = "Угроза уничтожена";
        public const string resultCarantine = "Угрозы перемещены в карантин";
    }
}
