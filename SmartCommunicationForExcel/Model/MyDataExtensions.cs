using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Model
{
    public static class MyDataExtensions
    {
        //根据键值获取对象
        public static MyData GetMyData(this List<MyData> myDatas, string key)
        {
            return myDatas.Where(it => it.Key == key).SingleOrDefault();
        }


        public static void SetValue_Result(this List<MyData> myDatas, short rlt)
        {
            MyData myData = myDatas.GetMyData("ResultCode");
            if (myData != null)
            {
                myData.ValueData = rlt;
            }
            else
            {
                //不存在 则添加
                myDatas.Add(new MyData("ResultCode", MyData.MyType.Int16, rlt));
            }
        }

        public static void SetValue_Result(this List<MyData> myDatas, int rlt)
        {
            MyData myData = myDatas.GetMyData("ResultCode");
            if (myData != null)
            {
                myData.ValueData = rlt;
            }
            else
            {
                //不存在 则添加
                myDatas.Add(new MyData("ResultCode", MyData.MyType.Int16, rlt));
            }
        }

        public static void SetValue_Message(this List<MyData> myDatas, string message)
        {
            MyData myData = myDatas.GetMyData("Message");
            if (myData != null)
            {
                myData.ValueData = message;
            }
            else
            {
                //不存在 则添加
                myDatas.Add(new MyData("Message", MyData.MyType.String, message));
            }
        }

        public static void SetValue_MessageW(this List<MyData> myDatas, string message)
        {
            MyData myData = myDatas.GetMyData("Message");
            if (myData != null)
            {
                myData.ValueData = message;
            }
            else
            {
                //不存在 则添加
                myDatas.Add(new MyData("Message", MyData.MyType.WString, message));
            }
        }

        public static void SetValue_ResultAndMessage(this List<MyData> myDatas, short rlt, string message)
        {
            myDatas.SetValue_Result(rlt);
            myDatas.SetValue_Message(message);
        }

        public static void SetValue_ResultAndMessageW(this List<MyData> myDatas, short rlt, string message)
        {
            myDatas.SetValue_Result(rlt);
            myDatas.SetValue_MessageW(message);
        }

        public static void SetValue_ResultAndMessage(this List<MyData> myDatas, int rlt, string message)
        {
            myDatas.SetValue_Result(rlt);
            myDatas.SetValue_Message(message);
        }

        public static void SetValue_ResultAndMessageW(this List<MyData> myDatas, int rlt, string message)
        {
            myDatas.SetValue_Result(rlt);
            myDatas.SetValue_MessageW(message);
        }

        public static void SetValue<T>(this List<MyData> myDatas, string key, MyData.MyType myType, T t)
        {
            MyData myData = myDatas.GetMyData(key);
            if (myData != null)
            {
                myData.ValueData = t;
            }
            else
            {
                //不存在 则添加
                myDatas.Add(new MyData(key, myType, t));
            }
        }
    }
}
