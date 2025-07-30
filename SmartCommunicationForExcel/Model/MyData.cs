using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel
{
    public class MyData
    {
        public MyData()
        {

        }
        public MyData(string key, MyType type, object data)
        {
            Key = key;
            ValueType = type;
            ValueData = data;
        }
        public string Key { get; set; }
        public MyType ValueType { get; set; }
        public object ValueData { get; set; }

        public enum MyType
        {
            Int32,
            Int16,
            String,
            Float,
            WString
        }
    }
}
