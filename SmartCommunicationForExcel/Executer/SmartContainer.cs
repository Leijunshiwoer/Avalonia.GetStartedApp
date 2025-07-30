using Amib.Threading;
using SmartCommunicationForExcel.EventHandle.Siemens;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using SmartCommunicationForExcel.Utils;
using System;
using System.Collections.Generic;
using Unity;
using SmartCommunicationForExcel.Interface;
using System.Linq;
using SmartCommunicationForExcel.EventHandle.Omron;
using SmartCommunicationForExcel.Implementation.Omron;
using SmartCommunicationForExcel.EventHandle.Mitsubishi;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.EventHandle.Beckhoff;
using SmartCommunicationForExcel.Implementation.Beckhoff;

namespace SmartCommunicationForExcel.Executer
{
    public class SmartContainer
    {
        private int m_cycleTime = 0;
        public IUnityContainer Container { get; set; }
        // private SmartConfig.SmartConfig _smartConfig;
        private SmartConfigForExcel.SmartSiemensConfigForExcelForm _smartSiemensConfigForExcelFrom;
        private SmartConfigForExcel.SmartOmronConfigForExcelForm _smartOmronConfigForExcelFrom;
        private SmartConfigForExcel.SmartMitsubishiConfigForExcelForm _smartMitsubishiConfigForExcelFrom;
        private SmartConfigForExcel.SmartBeckhoffConfigForExcelForm _smartBeckhoffConfigForExcelFrom;
        public SmartContainer()
        {
            Container = new UnityContainer();
           //SmartThreadPool Inject
           Container.RegisterType<ISiemensEventExecuter, DefaultSiemensEventExecuter>();
           //add
           Container.RegisterType<IOmronEventExecuter, DefaultOmronEventExecuter>();

           //注册了线程回调
           STPStartInfo ssi = new STPStartInfo() { CallToPostExecute = CallToPostExecute.Always, FillStateWithArgs = true};

           SmartThreadPool stp = new SmartThreadPool(ssi) { Name = "SmartThread From Container" };

           Container.RegisterInstance(stp);
        }

        public void Register<TFrom, TTo>() where TTo : TFrom
        {
            Container.RegisterType<TFrom, TTo>();
        }

        public T Resolve<T>(string Name = "")
        {
            return Name == "" ? Container.Resolve<T>() : Container.Resolve<T>(Name, null);
        }

        public void RegisterSingle<T>()
        {
            Container.RegisterSingleton<T>();
        }

        public void RegisterInstance<T>(string name, T t)
        {
            Container.RegisterInstance<T>(name, t);
        }

        public bool IsRegister<T>(string Name)
        {
            return Container.IsRegistered<T>(Name);
        }

        /// <summary>
        /// 获取容器内启动的实例名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetContainerClientNames()
        {
            List<string> names = new List<string>();
            foreach (var item in dictSiemensInstance)
            {
                names.Add(item.Key);
            }
            return names;
        }

        //打开配置文档
        /// <summary>
        /// 打开配置文档客户端
        /// </summary>
        /// <param name="strInstanceName">实例名称 如为空则不绑定配置文件显示</param>
        /// <returns></returns>
        public ResultState ShowSimensConfig(string strInstanceName = "")
        {
            ResultState rs = new ResultState();
            if (string.Empty != strInstanceName)
            {
                ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> gc;
                if (dictSmartSiemensConfigInstance.TryGetValue(strInstanceName, out gc))
                {
                    _smartSiemensConfigForExcelFrom = new SmartConfigForExcel.SmartSiemensConfigForExcelForm();
                    BindSiemensConfig(gc);
                    //_smartConfig.Show();
                    _smartSiemensConfigForExcelFrom.Show();

                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "未找到已启动的实例参数";
                }
            }
            else
            {
               // BindConfig(null);
                // _smartConfig.Show();
               // _smartConfigForExcelFrom.Show();
            }
            return rs;
        }
        //打开配置文档
        /// <summary>
        /// 打开配置文档客户端
        /// </summary>
        /// <param name="strInstanceName">实例名称 如为空则不绑定配置文件显示</param>
        /// <returns></returns>
        public ResultState ShowOmronConfig(string strInstanceName = "")
        {
            ResultState rs = new ResultState();
            if (string.Empty != strInstanceName)
            {
                IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance> gc;
                if (dictSmartOmronConfigInstance.TryGetValue(strInstanceName, out gc))
                {
                    _smartOmronConfigForExcelFrom = new SmartConfigForExcel.SmartOmronConfigForExcelForm();
                    BindOmronConfig(gc);
                    //_smartConfig.Show();
                    _smartOmronConfigForExcelFrom.Show();

                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "未找到已启动的实例参数";
                }
            }
            else
            {
                // BindConfig(null);
                // _smartConfig.Show();
                // _smartConfigForExcelFrom.Show();
            }
            return rs;
        }

        //打开配置文档
        /// <summary>
        /// 打开配置文档客户端
        /// </summary>
        /// <param name="strInstanceName">实例名称 如为空则不绑定配置文件显示</param>
        /// <returns></returns>
        public ResultState ShowMitsubishiConfig(string strInstanceName = "")
        {
            ResultState rs = new ResultState();
            if (string.Empty != strInstanceName)
            {
                IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> gc;
                if (dictSmartMitsubishiConfigInstance.TryGetValue(strInstanceName, out gc))
                {
                    _smartMitsubishiConfigForExcelFrom = new SmartConfigForExcel.SmartMitsubishiConfigForExcelForm();
                    BindMitsubishiConfig(gc);
                    //_smartConfig.Show();
                    _smartMitsubishiConfigForExcelFrom.Show();

                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "未找到已启动的实例参数";
                }
            }
            else
            {
            }
            return rs;
        }

        //打开配置文档
        /// <summary>
        /// 打开配置文档客户端
        /// </summary>
        /// <param name="strInstanceName">实例名称 如为空则不绑定配置文件显示</param>
        /// <returns></returns>
        public ResultState ShowBeckhoffConfig(string strInstanceName = "")
        {
            ResultState rs = new ResultState();
            if (string.Empty != strInstanceName)
            {
                IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> gc;
                if (dictSmartBeckhoffConfigInstance.TryGetValue(strInstanceName, out gc))
                {
                    _smartBeckhoffConfigForExcelFrom = new SmartConfigForExcel.SmartBeckhoffConfigForExcelForm();
                    BindBeckhoffConfig(gc);
                    //_smartConfig.Show();
                    _smartBeckhoffConfigForExcelFrom.Show();
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "未找到已启动的实例参数";
                }
            }
            else
            {
            }
            return rs;
        }

        //绑定配置文件到配置文件对话框
        private void BindSiemensConfig(ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> globalConfig)
        {
            _smartSiemensConfigForExcelFrom.SetModel(globalConfig);
        }
        private void BindOmronConfig(IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance> globalConfig)
        {
            _smartOmronConfigForExcelFrom.SetModel(globalConfig);

        }
        private void BindMitsubishiConfig(IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> globalConfig)
        {
            _smartMitsubishiConfigForExcelFrom.SetModel(globalConfig);
        }

        private void BindBeckhoffConfig(IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> globalConfig)
        {
            _smartBeckhoffConfigForExcelFrom.SetModel(globalConfig);
        }

        /// <summary>
        /// 根据TagName获取SiemensEventIO对象
        /// </summary>
        /// <param name="siemensEventIOs"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public SiemensEventIO GetSiemensEventIOByTagName(List<SiemensEventIO> siemensEventIOs, string tagName)
        {
            try
            {
                return siemensEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OmronEventIO GetOmronEventIOByTagName(List<OmronEventIO> omronEventIOs, string tagName)
        {
            try
            {
                return omronEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MitsubishiEventIO GetMitsubishiEventIOByTagName(List<MitsubishiEventIO> mitsubushiEventIOs, string tagName)
        {
            try
            {
                return mitsubushiEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BeckhoffEventIO GetBeckhoffEventIOByTagName(List<BeckhoffEventIO> beckhoffEventIOs, string tagName)
        {
            try
            {
                return beckhoffEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCycleTime(int ct)
        {
            m_cycleTime = ct;
        }


        private Dictionary<string, SiemensEventHandle> dictSiemensInstance = new Dictionary<string, SiemensEventHandle>();
        private Dictionary<string, ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance>> dictSmartSiemensConfigInstance = new Dictionary<string, ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance>>();

        private Dictionary<string, OmronEventHandle> dictOmronInstance = new Dictionary<string, OmronEventHandle>();
        private Dictionary<string, IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance>> dictSmartOmronConfigInstance = new Dictionary<string, IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance>>();

        private Dictionary<string, MitsubishiEventHandle> dictMitsubishiInstance = new Dictionary<string, MitsubishiEventHandle>();
        private Dictionary<string, IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance>> dictSmartMitsubishiConfigInstance = new Dictionary<string, IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance>>();

        private Dictionary<string, BeckhoffEventHandle> dictBeckhoffInstance = new Dictionary<string, BeckhoffEventHandle>();
        private Dictionary<string, IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance>> dictSmartBeckhoffConfigInstance = new Dictionary<string, IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance>>();

        public ResultState StartSiemensWorkInstance(string InstanceName, string ConfigFilePath)
        {
            ResultState rs = new ResultState() { IsSuccess = true };

            if (InstanceName == "")
            {
                rs.IsSuccess = false;
                rs.Message = "InstanceName is not allow null";
                return rs;
            }

            try
            {
                if (!dictSiemensInstance.ContainsKey(InstanceName))
                {
                    // SiemensGlobalConfig sgc = new JsonFileHelper<SiemensGlobalConfig>().FileToObject(ConfigFilePath);
                    SiemensGlobalConfig sgc = new MyExcelFileHelper<SiemensGlobalConfig>().ExcelToSiemensObject(ConfigFilePath);
                    if (null == sgc)
                    {
                        rs.IsSuccess = false;
                        rs.Message = "The GlobalConfig Is Null.";
                        return rs;
                    }

                    //改变扫码周期
                    sgc.CpuInfo.CycleTime = m_cycleTime;

                    SiemensEventHandle tmp = Container.Resolve<SiemensEventHandle>();
                    if (!tmp.StartWork(InstanceName, sgc))
                    {
                        rs.IsSuccess = false;
                        rs.Message = InstanceName + " StartWork Fail.";

                        return rs;
                    }
                    else
                    {
                        dictSiemensInstance.Add(InstanceName, tmp);
                        dictSmartSiemensConfigInstance.Add(InstanceName, sgc);
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "The current instance is running";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }
            return rs;
        }

        public ResultState StartOmronWorkInstance(string InstanceName, string ConfigFilePath)
        {
            ResultState rs = new ResultState() { IsSuccess = true };

          
            if (InstanceName == "")
            {
                rs.IsSuccess = false;
                rs.Message = "InstanceName is not allow null";
                return rs;
            }

            try
            {
                if (!dictOmronInstance.ContainsKey(InstanceName))
                {
                    OmronGlobalConfig sgc = new MyExcelFileHelper<OmronGlobalConfig>().ExcelToOmronObject(ConfigFilePath);
                    if (null == sgc)
                    {
                        rs.IsSuccess = false;
                        rs.Message = "The GlobalConfig Is Null.";
                        return rs;
                    }
                    //改变扫码周期
                    sgc.CpuInfo.CycleTime = m_cycleTime;

                    OmronEventHandle tmp = Container.Resolve<OmronEventHandle>();
                    if (!tmp.StartWork(InstanceName, sgc))
                    {
                        rs.IsSuccess = false;
                        rs.Message = InstanceName + " StartWork Fail.";


                        return rs;
                    }
                    else
                    {
                        dictOmronInstance.Add(InstanceName, tmp);
                        dictSmartOmronConfigInstance.Add(InstanceName, sgc);
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "The current instance is running";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }

            return rs;
        }

        public ResultState StartMitsubishiWorkInstance(string InstanceName, string ConfigFilePath)
        {
            ResultState rs = new ResultState() { IsSuccess = true };

            if (InstanceName == "")
            {
                rs.IsSuccess = false;
                rs.Message = "InstanceName is not allow null";
                return rs;
            }

            try
            {
                if (!dictMitsubishiInstance.ContainsKey(InstanceName))
                {
                    // SiemensGlobalConfig sgc = new JsonFileHelper<SiemensGlobalConfig>().FileToObject(ConfigFilePath);
                    MitsubishiGlobalConfig sgc = new MyExcelFileHelper<MitsubishiGlobalConfig>().ExcelToMitsubishiObject(ConfigFilePath);
                    if (null == sgc)
                    {
                        rs.IsSuccess = false;
                        rs.Message = "The GlobalConfig Is Null.";
                        return rs;
                    }

                    //改变扫码周期
                    sgc.CpuInfo.CycleTime = m_cycleTime;

                    MitsubishiEventHandle tmp = Container.Resolve<MitsubishiEventHandle>();
                    if (!tmp.StartWork(InstanceName, sgc))
                    {
                        rs.IsSuccess = false;
                        rs.Message = InstanceName + " StartWork Fail.";
                        return rs;
                    }
                    else
                    {
                        dictMitsubishiInstance.Add(InstanceName, tmp);
                        dictSmartMitsubishiConfigInstance.Add(InstanceName, sgc);
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "The current instance is running";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }
            return rs;
        }

        public ResultState StartBeckhoffWorkInstance(string InstanceName, string ConfigFilePath)
        {
            ResultState rs = new ResultState() { IsSuccess = true };

            if (InstanceName == "")
            {
                rs.IsSuccess = false;
                rs.Message = "InstanceName is not allow null";
                return rs;
            }

            try
            {
                if (!dictBeckhoffInstance.ContainsKey(InstanceName))
                {
                    BeckhoffGlobalConfig sgc = new MyExcelFileHelper<BeckhoffGlobalConfig>().ExcelToBeckhoffObject(ConfigFilePath);
                    if (null == sgc)
                    {
                        rs.IsSuccess = false;
                        rs.Message = "The GlobalConfig Is Null.";
                        return rs;
                    }

                    //改变扫码周期
                    sgc.CpuInfo.CycleTime = m_cycleTime;

                    BeckhoffEventHandle tmp = Container.Resolve<BeckhoffEventHandle>();
                    if (!tmp.StartWork(InstanceName, sgc))
                    {
                        rs.IsSuccess = false;
                        rs.Message = InstanceName + " StartWork Fail.";

                        return rs;
                    }
                    else
                    {
                        dictBeckhoffInstance.Add(InstanceName, tmp);
                        dictSmartBeckhoffConfigInstance.Add(InstanceName, sgc);
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "The current instance is running";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }
            return rs;
        }

        public ResultState StopSiemensWorkInstance(string strInstanceName)
        {
            ResultState rs = new ResultState();
           
            SiemensEventHandle seh;
            if (dictSiemensInstance.TryGetValue(strInstanceName, out seh))
            {
                dictSiemensInstance.Remove(strInstanceName);
                dictSmartSiemensConfigInstance.Remove(strInstanceName);
                seh.WorkStop();
            }
            else
            {
                rs.IsSuccess = false;
                rs.Message = "No instance running in container.";
            }
              
            return rs;
        }

        public ResultState StopOmronWorkInstance(string strInstanceName)
        {
            ResultState rs = new ResultState();

            OmronEventHandle seh;
            if (dictOmronInstance.TryGetValue(strInstanceName, out seh))
            {
                dictOmronInstance.Remove(strInstanceName);
                dictSmartOmronConfigInstance.Remove(strInstanceName);
                seh.WorkStop();
            }
            else
            {
                rs.IsSuccess = false;
                rs.Message = "No instance running in container.";
            }

            return rs;
        }

        public ResultState StopMitsubishiWorkInstance(string strInstanceName)
        {
            ResultState rs = new ResultState();

            MitsubishiEventHandle seh;
            if (dictMitsubishiInstance.TryGetValue(strInstanceName, out seh))
            {
                dictMitsubishiInstance.Remove(strInstanceName);
                dictSmartMitsubishiConfigInstance.Remove(strInstanceName);
                seh.WorkStop();
            }
            else
            {
                rs.IsSuccess = false;
                rs.Message = "No instance running in container.";
            }

            return rs;
        }

        public ResultState StopBeckhoffWorkInstance(string strInstanceName)
        {
            ResultState rs = new ResultState();

            BeckhoffEventHandle seh;
            if (dictBeckhoffInstance.TryGetValue(strInstanceName, out seh))
            {
                dictBeckhoffInstance.Remove(strInstanceName);
                dictSmartBeckhoffConfigInstance.Remove(strInstanceName);
                seh.WorkStop();
            }
            else
            {
                rs.IsSuccess = false;
                rs.Message = "No instance running in container.";
            }

            return rs;
        }
    }
}
