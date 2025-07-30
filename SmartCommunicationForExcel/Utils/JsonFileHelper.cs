using Newtonsoft.Json;
using SmartCommunicationForExcel.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Utils
{
    public class JsonFileHelper<T> : FormatJsonToString
    {
        public string FileToJson(string strPath)
        {
            if (File.Exists(strPath))
            {
                try
                {
                    return File.ReadAllText(strPath);
                }
                catch (Exception ex)
                {
                    MyLog.WriteException("JsonFileHelper", ex);
                    return string.Empty;
                }
            }
            else
            {
                MyLog.WriteError("File Not Exists,File:" + strPath);
                return string.Empty;
            }
        }

        public bool JsonToFile(string strJson, string strPath)
        {
            try
            {
                File.WriteAllText(strPath, strJson);
                return true;
            }
            catch (Exception ex)
            {
                MyLog.WriteException("JsonFileHelper", ex);
                return false;
            }
        }

        public string ObjectToJson(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T JsonToObj(string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        public void ObjectToFile(T obj, string strFile)
        {
            JsonToFile(ObjectToJson(obj), strFile);
        }

        public T FileToObject(string strFile)
        {
            return JsonToObj(FileToJson(strFile));
        }
    }
}
