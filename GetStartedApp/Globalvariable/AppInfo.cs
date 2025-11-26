
using GetStartedApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Globalvariable
{
    public static class MachineInfo
    {
        public static bool ConnPLC { get; set; }
        public static int MachineStatus { get; set; }
        public static double OP10CT { get; set; }
        public static double OP20CT { get; set; }
        public static double OP30CT { get; set; }
    }

    public static class UserInfo
    {
        //用户ID
        public static int UserId { get; set; }

        //用户名称
        public static string UserName { get; set; }

        public static string Password { get; set; }
        public static UserDto User { get; set; }
    }

    public static class TaskInfo
    {
        public static short ClearFinishedR { get; set; } = 0;
        public static short ClearFinishedW { get; set; } = 0;
        //public static Product_Task_Config TaskConfig { get; set; }

        public static short LaserMod { get; set; }
    }

    public static class RecipeInfo_1
    {
        //需要同步的配方编号
        public static short RecipeNo { get; set; }//同步配方号

        //1下载配方 根据下发工单 当收到PLC同步配方事件后 复位0
        public static short LoadRecipe { get; set; }//切换配方使用

        public static short ChangedRecipeNo { get; set; }//下载型号的配方编号
        public static short ChangedRecipe { get; set; }//需要改变时候1
        public static short LoadRecipeStatus { get; set; }//配方切换成功，失败 1ok， 2ng
    }

    public static class RecipeInfo_2
    {
        //需要同步的配方编号
        public static short RecipeNo { get; set; }//同步配方号

        //1下载配方 根据下发工单 当收到PLC同步配方事件后 复位0
        public static short LoadRecipe { get; set; }//切换配方使用

        public static short ChangedRecipeNo { get; set; }//下载型号的配方编号
        public static short ChangedRecipe { get; set; }//需要改变时候1
        public static short LoadRecipeStatus { get; set; }//配方切换成功，失败 1ok， 2ng
    }

    public static class RecipeInfo_3
    {
        //需要同步的配方编号
        public static short RecipeNo { get; set; }//同步配方号

        //1下载配方 根据下发工单 当收到PLC同步配方事件后 复位0
        public static short LoadRecipe { get; set; }//切换配方使用

        public static short ChangedRecipeNo { get; set; }//下载型号的配方编号
        public static short ChangedRecipe { get; set; }//需要改变时候1
        public static short LoadRecipeStatus { get; set; }//配方切换成功，失败 1ok， 2ng
    }

    public static class WarningInfo
    {
        public static bool IsSave { get; set; }
    }

    public static class QuickWearPartInfo
    {
       // public static List<Machine_QuickWearPart_Config> QuickWearParts { get; set; }//易损易耗件
    }

    public static class ProductCount
    {
        //底座投料数
        public static int DZ_WorkingCount { get; set; }

        public static int DZ_Ok { get; set; }
        public static int DZ_Ng { get; set; }

        //管壳投料数
        public static int GK_WorkingCount { get; set; }

        //成品OK
        public static int Ok { get; set; }

        //成品NG
        public static int Ng { get; set; }
    }

    public static class Storage_10
    {
        public static short Storage01 { get; set; } //底座
        public static short Storage02 { get; set; } //内O
        public static short Storage03 { get; set; } //点火管
        public static short Storage04 { get; set; } //外O
        public static short BarcodeStatus { get; set; }//1物料编码匹配 2物料编码不匹配
    }

    public static class Storage_20
    {
        public static short Storage01 { get; set; } //管壳
        public static short BarcodeStatus { get; set; }//1物料编码匹配 2物料编码不匹配
    }

    public static class Storage_30
    {
        public static short Storage01 { get; set; } //短路组件
        public static short BarcodeStatus { get; set; }//1物料编码匹配 2物料编码不匹配
    }

    /// <summary>
    /// 实时数据
    /// </summary>
    public static class RTDatas
    {
        public static REData OP10_ST1 { get; set; }
        public static REData OP10_ST2 { get; set; }
        public static REData OP10_ST3 { get; set; }
        public static REData OP10_ST4 { get; set; }
        public static REData OP10_ST5 { get; set; }
        public static REData OP10_ST6 { get; set; }
        public static REData OP10_ST7 { get; set; }
        public static REData OP10_ST8 { get; set; }
        public static REData OP10_ST9 { get; set; }
        public static REData OP10_ST10 { get; set; }
        public static REData OP10_ST11 { get; set; }
        public static REData OP10_ST12 { get; set; }

        public static REData OP20_ST1 { get; set; }
        public static REData OP20_ST2 { get; set; }
        public static REData OP20_ST3 { get; set; }
        public static REData OP20_ST4 { get; set; }
        public static REData OP20_ST5 { get; set; }
        public static REData OP20_ST6 { get; set; }
        public static REData OP20_ST7 { get; set; }
        public static REData OP20_ST8 { get; set; }
        public static REData OP20_ST9 { get; set; }
        public static REData OP20_ST10 { get; set; }
        public static REData OP20_ST11 { get; set; }
        public static REData OP20_ST12 { get; set; }
        public static REData OP20_ST13 { get; set; }
        public static REData OP20_ST14 { get; set; }

        public static REData OP30_ST1 { get; set; }
        public static REData OP30_ST2 { get; set; }
        public static REData OP30_ST3 { get; set; }
        public static REData OP30_ST4 { get; set; }
        public static REData OP30_ST5 { get; set; }
        public static REData OP30_ST6 { get; set; }
        public static REData OP30_ST7 { get; set; }
        public static REData OP30_ST8 { get; set; }
        public static REData OP30_ST9 { get; set; }
        public static REData OP30_ST10 { get; set; }
        public static REData OP30_ST11 { get; set; }
        public static REData OP30_ST12 { get; set; }
        public static REData OP30_ST13 { get; set; }
        public static REData OP30_ST14 { get; set; }
    }

    public class REData
    {
        public string Name { get; set; }
        public bool Result { get; set; }
        public string Value { get; set; }
    }
}