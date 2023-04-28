using Antivirus.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace Antivirus.Service
{
    public static class MessageHandler
    {
        public static List<string> viruses = new List<string>();

        public static String Processing(String operation)
        {
            String ResultOperation = "";
            try
            {
                String[] messArr = operation.ToString().Split('#');
                File.AppendAllText(ServiceInformation.SocketPath, operation + "\r\n");

                switch (messArr[0])
                {
                    case HandlerInformation.scanFile:
                        File.AppendAllText(ServiceInformation.SocketPath, HandlerInformation.scanFile + " Нашел\r\n");
                        ResultOperation = FileScan(messArr[1]);
                        Scan.SetStop(false);
                        break;
                    case HandlerInformation.scanPath:
                        File.AppendAllText(ServiceInformation.SocketPath, HandlerInformation.scanPath + " Нашел\r\n");
                        ResultOperation = PathScan(messArr[1]);
                        Scan.SetStop(false);
                        break;
                    case HandlerInformation.scanFull:
                        File.AppendAllText(ServiceInformation.SocketPath, HandlerInformation.scanFull + " Нашел\r\n");
                        ResultOperation = FullScan();
                        Scan.SetStop(false);
                        break;

                    case HandlerInformation.delete:
                        File.AppendAllText(ServiceInformation.SocketPath, "Удаление" + " Нашел\r\n");
                        ResultOperation = DeleteViruses();
                        break;
                    case HandlerInformation.carantine:
                        File.AppendAllText(ServiceInformation.SocketPath, "Карантин" + " Нашел\r\n");
                        ResultOperation = CarantineViruses();
                        break;

                    case HandlerInformation.carantineShow:
                        String Result = Carantine.ShowCarantineForSent();
                        if (Result != "") ResultOperation = Result;
                        else ResultOperation = "None";
                        break;
                    case HandlerInformation.carantineRecoverAll:
                        ResultOperation = Carantine.DropCarantine();
                        break;
                    case HandlerInformation.carantineDeleteAll:
                        ResultOperation = Carantine.DeleteAll();
                        break;
                    case HandlerInformation.carantineDeleteOne:
                        ResultOperation = Carantine.DeleteOne(messArr[1]);
                        break;
                    case HandlerInformation.carantineRecoverOne:
                        ResultOperation = Carantine.GoOutCatantine(messArr[1]);
                        break;

                    case HandlerInformation.scanPause:
                        Scan.SetPause(true);
                        break;
                    case HandlerInformation.scanContinue:
                        Scan.SetPause(false);
                        break;
                    case HandlerInformation.scanStop:
                        Scan.SetStop(true);
                        break;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ServiceInformation.SocketPath, ex.Message + "\r\n");
                File.AppendAllText(ServiceInformation.SocketPath, ex + "\r\n");
            }

            return ResultOperation;
        }

        #region Scans
        private static String FileScan(String path)
        {
            viruses.Clear();
            String ScanResult = Scan.ScanFile(path);

            if (ScanResult != "None")
            {
                if (ScanResult == HandlerInformation.resultStop) return HandlerInformation.resultStop;
                viruses.Add(ScanResult);
                return ScanResult;
            }
            else return HandlerInformation.resultNone;
        }


        private static String PathScan(String path)
        {
            viruses.Clear();
            List<string> ListResult = Scan.ScanFolder(path);

            if (ListResult.Count > 0)
            {
                if (ListResult.Contains(HandlerInformation.resultStop))
                {
                    File.AppendAllText(@"C:\Users\xiaomi\Desktop\antivirus_test.txt", "Понял остановку\r\n");
                    return HandlerInformation.resultStop;
                }
                viruses.AddRange(ListResult);
                String result = "Вирусы найдены в следующих файлах:#";
                foreach (String item in ListResult) { result += item + "#"; }
                return result;
            }
            else return HandlerInformation.resultNone;
        }

        private static String FullScan()
        {
            viruses.Clear();
            List<string> ListResult = Scan.ScanMachine();
            if (ListResult.Count > 0)
            {
                if (ListResult.Contains(HandlerInformation.resultStop)) return HandlerInformation.resultStop;
                viruses.AddRange(ListResult);
                String result = "Вирусы найдены в следующих файлах:#";
                foreach (String item in ListResult) { result += item + "#"; }
                return result;
            }
            else return HandlerInformation.resultNone;
        }

        #endregion

        #region Delete

        public static String DeleteVirus(String fileName)
        {
            Bin recycle = new Bin();
            if (File.Exists(fileName)) recycle.Recycle(fileName);
            return HandlerInformation.resultDelete;
        }

        private static String DeleteViruses()
        {
            File.AppendAllText(ServiceInformation.SocketPath, "Я в удалении" + " Нашел\r\n");
            String result = "";
            foreach (String item in viruses)
            {
                File.AppendAllText(ServiceInformation.SocketPath, item + " Нашел\r\n");
                result = DeleteVirus(item);
            }
            viruses.Clear();

            return result;
        }

        #endregion

        #region Carantine

        private static String CarantineViruses()
        {
            File.AppendAllText(ServiceInformation.SocketPath, "Я в карантине" + " Нашел\r\n");
            foreach (String item in viruses)
            {
                File.AppendAllText(ServiceInformation.SocketPath, item + " Нашел\r\n");
                String newSafePath = Carantine.GoInCarantine(item);
            }
            viruses.Clear();
            return HandlerInformation.resultCarantine;
        }
        #endregion
    }
}
