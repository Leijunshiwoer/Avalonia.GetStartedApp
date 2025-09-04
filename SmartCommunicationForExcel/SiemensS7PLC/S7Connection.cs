using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    // 实现IDisposable接口以支持资源释放
    public static class S7Connection 
    {
        private static readonly object _lock = new object();
        private static List<PlcConfigModel> _plcs = new List<PlcConfigModel>();
     

        public static IReadOnlyList<PlcConfigModel> Plcs
        {
            get
            {
                lock (_lock)
                {
                    return _plcs.AsReadOnly();
                }
            }
        }

        static S7Connection()
        {
            // 初始化示例PLC配置
            InitializeDefaultPlcs();
        }

        private static void InitializeDefaultPlcs()
        {
            AddPlc(new PlcConfigModel()
            {
                Plc = new Plc(CpuType.S71500, "192.168.115.132", 0, 1),
                PlcName = "OP10",
                PlcKeepData = new PlcKeepData(),
                IsConnected = false,
                Message = "等待连接",
            });

            AddPlc(new PlcConfigModel()
            {
                Plc = new Plc(CpuType.S71500, "127.0.0.1", 0, 1),
                PlcName = "OP20",
                PlcKeepData = new PlcKeepData(),
                IsConnected = false,
                Message = "等待连接",
            });
        }

        public static void AddPlc(PlcConfigModel plcConfig)
        {
            if (plcConfig == null) throw new ArgumentNullException(nameof(plcConfig));
            if (string.IsNullOrEmpty(plcConfig.PlcName)) throw new ArgumentException("PLC名称不能为空");

            lock (_lock)
            {
                if (_plcs.Any(p => p.PlcName == plcConfig.PlcName))
                {
                    throw new InvalidOperationException($"已存在名为{plcConfig.PlcName}的PLC配置");
                }

                _plcs.Add(plcConfig);
              
            }
        }

  
        public static async Task<bool> ConnectPlcAsync(PlcConfigModel plcConfig)
        {
            if (plcConfig == null) throw new ArgumentNullException(nameof(plcConfig));

            var startTime = DateTime.Now;
            try
            {
                // 已连接则直接返回成功
                if (plcConfig.Plc.IsConnected)
                {
                    plcConfig.Message = $"重连成功（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return true;
                }
                plcConfig.Message = "正在连接PLC...";
                // 执行异步连接
                await plcConfig.Plc.OpenAsync();

                // 再次检查连接状态
                if (plcConfig.Plc.IsConnected)
                {
                    plcConfig.Message = $"连接成功（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return true;
                }
                else
                {
                    plcConfig.Message = $"连接失败：未知原因（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return false;
                }
            }
            catch (Exception ex)
            {
                plcConfig.Message = $"连接失败（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）: {ex.Message}";
                return false;
            }
            finally
            {
                plcConfig.IsConnected = plcConfig.Plc.IsConnected;
            }
        }

        public static async Task ClosePlcAsync(PlcConfigModel plcConfig)
        {
            if (plcConfig == null) throw new ArgumentNullException(nameof(plcConfig));

            var startTime = DateTime.Now;
            try
            {
                if (plcConfig.Plc.IsConnected)
                {
                    await Task.Run(() => plcConfig.Plc.Close());
                }
                plcConfig.Message = $"断开连接（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
            }
            catch (Exception ex)
            {
                plcConfig.Message = $"断开连接错误（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）: {ex.Message}";
            }
            finally
            {
                plcConfig.IsConnected = plcConfig.Plc.IsConnected;
            }
        }

        public static async Task ReadPlcAsync(PlcConfigModel plcConfig)
        {
            if (plcConfig == null) throw new ArgumentNullException(nameof(plcConfig));

            int reconnectDelay = 1000; // 初始重连延迟
            const int maxReconnectDelay = 10000; // 最大重连延迟
            const int readInterval = 100; // 正常读取间隔

            try
            {
                while (true)
                {
                    #region 自动重连逻辑
                    if (!plcConfig.Plc.IsConnected)
                    {
                        bool connectSuccess = await ConnectPlcAsync(plcConfig);
                        if (!connectSuccess)
                        {
                            // 等待后重试，使用取消令牌支持可中断的等待
                            await Task.Delay(reconnectDelay);
                            reconnectDelay = Math.Min(reconnectDelay * 2, maxReconnectDelay);
                            continue;
                        }
                        // 连接成功，重置重连延迟
                        reconnectDelay = 1000;
                    }
                    #endregion

                    #region 数据读取逻辑
                    try
                    {
                        // 读取PLC数据
                        PlcDbController.ReadFromPlc(plcConfig.Plc, plcConfig.PlcKeepData, 5000, 0);
                        plcConfig.Message = "Read plc data success.";

                        // 正常读取后等待，可被取消
                        await Task.Delay(readInterval);
                    }
                    catch (Exception ex)
                    {
                        // 处理连接相关异常
                        if (ex.Message.Contains("Unable to write data to the transport connection") ||
                            ex.Message.Contains("Unable to read data from the transport connection"))
                        {
                            plcConfig.Plc.Close();
                        }

                        plcConfig.PlcKeepData = new PlcKeepData();
                        plcConfig.Message = ex.Message;

                        // 异常后等待并重试
                        await Task.Delay(reconnectDelay);
                    }
                    finally
                    {
                        plcConfig.IsConnected = plcConfig.Plc.IsConnected;
                    }
                    #endregion
                }
            }
            catch (OperationCanceledException)
            {
                // 预期的取消操作，不视为错误
                plcConfig.Message = "监控已停止";
            }
            catch (Exception ex)
            {
                plcConfig.Message = $"监控异常终止: {ex.Message}";
            }
            finally
            {
                // 确保最终关闭连接
                if (plcConfig.Plc.IsConnected)
                {
                    await ClosePlcAsync(plcConfig);
                }
                plcConfig.IsConnected = false;
            }
        }

       

      
    }

    public static class PlcDbController
    {
        /// <summary>
        /// 从PLC读取数据块
        /// </summary>
        public static void  ReadFromPlc<T>(Plc plc, T data, int dbNumber, int startByteAdr = 0) where T : class
        {
            if (plc == null) throw new ArgumentNullException(nameof(plc));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (dbNumber <= 0) throw new ArgumentOutOfRangeException(nameof(dbNumber), "数据块号必须大于0");

            plc.ReadClass(data, dbNumber, startByteAdr);
        }

        /// <summary>
        /// 向PLC写入数据块
        /// </summary>
        public static void WriteToPlc<T>(Plc plc, T data, int dbNumber, int startByteAdr = 0) where T : class
        {
            if (plc == null) throw new ArgumentNullException(nameof(plc));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (dbNumber <= 0) throw new ArgumentOutOfRangeException(nameof(dbNumber), "数据块号必须大于0");

            plc.WriteClass(data, dbNumber, startByteAdr);
        }
    }
}
