using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    /// <summary>
    /// PLC配置模型（包含专属锁）
    /// </summary>
    public class PlcConfigModel
    {
        /// <summary>
        /// PLC核心通信对象（S7.Net库）
        /// </summary>
        public Plc Plc { get; set; }

        /// <summary>
        /// PLC名称（唯一标识）
        /// </summary>
        public string PlcName { get; set; }

        /// <summary>
        /// PLC数据存储对象（读取结果存放在这里）
        /// </summary>
        public PlcKeepData PlcKeepData { get; set; } = new PlcKeepData();

        /// <summary>
        /// 连接状态（对外暴露的状态）
        /// </summary>
        public bool IsConnected { get; set; } = false;

        /// <summary>
        /// 状态信息（如“连接成功”“读取失败”）
        /// </summary>
        public string Message { get; set; } = "等待连接";

        /// <summary>
        /// PLC专属锁（每个实例独立，解决并发冲突）
        /// 用readonly确保锁对象初始化后不可修改，避免锁失效
        /// </summary>
        public readonly object PlcExclusiveLock = new object();
    }
}
