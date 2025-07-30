using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.LogNet;
using System.IO;

namespace SmartCommunicationForExcel.Utils
{
    public class MyLog
    {
        private static LogNetDateTime _log = new LogNetDateTime(Environment.CurrentDirectory + "/Log", GenerateMode.ByEveryDay);

        public static void CheckLogDirectory(string strDir)
        {
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }
        }

        public static void WriteInfo(string strMsg)
        {
            CheckLogDirectory(Environment.CurrentDirectory + "/Log");
            _log.WriteInfo(strMsg);
        }

        public static void WriteError(string strMsg)
        {
            CheckLogDirectory(Environment.CurrentDirectory + "/Log");
            _log.WriteError(strMsg);
        }

        public static void WriteException(string strKey, Exception ex)
        {
            CheckLogDirectory(Environment.CurrentDirectory + "/Log");
            _log.WriteException(strKey, ex);
        }
    }
}
