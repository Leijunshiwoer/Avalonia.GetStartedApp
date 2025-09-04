using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    public static class S7Connection
    {
        public static List<PlcConfigModel> Plcs { get; set; } = new List<PlcConfigModel>()
        {
            new PlcConfigModel()
            {
                 Plc=new Plc(CpuType.S71500,"127.0.0.1",0,1),
                 PlcName="OP10",
                 PlcKeepData=new PlcKeepData(),
                 IsConnected=false,
                 Message="等待连接",
            },
            new PlcConfigModel()
            {
                 Plc=new Plc(CpuType.S71500,"127.0.0.1",0,1),
                 PlcName="OP20",
                 PlcKeepData=new PlcKeepData(),
                 IsConnected=false,
                 Message="等待连接",
            },
        };
        public static async Task<bool> ConnectPlcAsync(PlcConfigModel _plcConfig)
        {
            var startTime = DateTime.Now;
            try
            {
                // 已连接则直接返回成功
                if (_plcConfig.Plc.IsConnected)
                {
                    _plcConfig.Message = $"重连成功（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return true;
                }

                _plcConfig.Message = "正在连接PLC...";
                // 执行异步连接（确保OpenAsync是真正的异步方法）
                await _plcConfig.Plc.OpenAsync();

                // 再次检查连接状态（防止连接过程中意外断开）
                if (_plcConfig.Plc.IsConnected)
                {
                    _plcConfig.Message = $"连接成功（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return true;
                }
                else
                {
                    _plcConfig.Message = $"连接失败：未知原因（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
                    return false;
                }
            }
            catch (Exception ex)
            {
                _plcConfig.Message = $"连接失败（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）: {ex.Message}";
                return false;
            }
            finally
            {
                // 最终同步连接状态
                _plcConfig.IsConnected = _plcConfig.Plc.IsConnected;
            }
        }

        /// <summary>
        /// 断开PLC
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        public static async Task ClosePlcAsync(PlcConfigModel _plcConfig)
        {
            // 记录开始时间用于性能分析
            var startTime = DateTime.Now;
            try
            {
                if (_plcConfig.Plc.IsConnected)
                {
                    // 断开连接
                    await Task.Run(() => _plcConfig.Plc.Close());
                }
                _plcConfig.Message = $"断开连接（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）";
            }
            catch (Exception ex)
            {
                _plcConfig.Message = $"断开连接（耗时: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms）: {ex.Message}";
            }
            finally
            {
                _plcConfig.IsConnected = _plcConfig.Plc.IsConnected;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        public static async Task ReadPlcAsync(PlcConfigModel _plcConfig)
        {
            int reconnectDelay = 1000; // 重连延迟时间（毫秒），连接失败后逐渐延长重试间隔
            const int maxReconnectDelay = 10000; // 最大重连延迟10秒
            //await Task.Run(async () =>
            //{
            while (true)
            {
                #region 自动重连
                if (!_plcConfig.Plc.IsConnected)
                {
                    bool connectSuccess = await ConnectPlcAsync(_plcConfig);
                    if (!connectSuccess)
                    {
                        // 连接失败，等待后重试（延迟逐渐增加，避免频繁尝试）
                        await Task.Delay(reconnectDelay);
                        reconnectDelay = Math.Min(reconnectDelay * 2, maxReconnectDelay);
                        continue;
                    }
                    // 连接成功，重置重连延迟
                    reconnectDelay = 1000;
                }
                #endregion

                #region 数据读取
                try
                {
                    // 读取PLC数据
                    PlcDbController.ReadFromPlc(_plcConfig.Plc, _plcConfig.PlcKeepData, 5000, 0);
                    _plcConfig.Message = "Read plc data success.";
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Unable to write data to the transport connection"))
                    {
                        _plcConfig.Plc.Close();
                    }
                    if (ex.Message.Contains("Unable to read data to the transport connection"))
                    {
                        _plcConfig.Plc.Close();
                    }
                    _plcConfig.PlcKeepData = new PlcKeepData();
                    _plcConfig.Message = ex.Message;

                    await Task.Delay(reconnectDelay);
                }
                finally
                {
                    _plcConfig.IsConnected = _plcConfig.Plc.IsConnected;
                }
                #endregion
            }
            //});
        }
    }




    public static class PlcDbController
    {
        /// <summary>
        /// 获取一个plc db块数据
        /// </summary>
        /// <param name="plc">plc对象</param>
        /// <param name="t">泛型对象,传进来什么，返回什么</param>
        /// <param name="db">数据块地址</param>
        /// <param name="startByteAdr">起始地址</param>
        public static void ReadFromPlc<T>(Plc plc, T t, int db, int startByteAdr = 0) where T : class
        {
            plc.ReadClass(t, db, startByteAdr);
        }

        /// <summary>
        /// 写一个plc db块数据
        /// </summary>
        /// <param name="plc">plc对象</param>
        /// <param name="t">泛型对象,传进来什么，写什么</param>
        /// <param name="db">数据块地址</param>
        /// <param name="startByteAdr">起始地址</param>
        public static void WriteToPlc<T>(Plc plc, T t, int db, int startByteAdr = 0) where T : class
        {
            plc.WriteClass(t, db, startByteAdr);
        }
    }
}
