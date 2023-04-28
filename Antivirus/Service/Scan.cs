using Antivirus.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus.Service
{
    public static class Scan
    {
        private static bool _isPause = false;
        private static bool _isStop = false;

        private static List<string> txtSign = File.ReadAllLines(ServiceInformation.DBPath).ToList();


        public static void SetStop(bool value)
        {
            _isStop = value;
        }

        public static void SetPause(bool value)
        {
            _isStop = value;
        }

        public static List<string> ScanFolder(string path)
        {
            List<string> result = new List<string>();

            var files = GetFiles(path);

            foreach (var file in files)
            {
                string 
            }

            return result;
        }
    }
}
