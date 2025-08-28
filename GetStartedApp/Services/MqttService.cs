using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.MQTT;

namespace GetStartedApp.Services
{
    public class MqttService : IMqttService, IDisposable
    {
        // 1. 线程安全单例（Lazy<T> 自带线程安全，默认 LazyThreadSafetyMode.ExecutionAndPublication）
        private static readonly Lazy<MqttService> _instance = new Lazy<MqttService>(() => new MqttService());

        // MQTT 客户端实例（异步操作核心对象）
        private MqttClient _mqttClient;

        // 连接状态（线程安全访问，使用 volatile 防止指令重排序）
        private volatile bool _isConnected;

        // 事件定义（供外部订阅，如 ViewModel）
        public event Action<OperateResult> ConnectionStatusChanged;
        /// <summary>
        /// 
        /// </summary>
        public event Action<string, string> MessageReceived;
        public event Action<Exception> NetworkErrorOccurred;

        // 2. 连接状态对外暴露（线程安全）
        public bool IsConnected => _isConnected;

        // 3. 构造函数：支持单例（无参）和依赖注入（带参，如后续扩展 DbHelper）
       
        // 供依赖注入容器使用的构造函数（若需注入其他服务，如日志、数据库）
        public MqttService(/* 可添加依赖参数，如 ILogger<MqttService> logger */) { }

        // 4. 单例访问点（对外提供唯一实例）
        public static MqttService Instance => _instance.Value;

        // 5. 异步连接 MQTT 服务器（核心异步方法，返回操作结果）
        public async Task<OperateResult> ConnectAsync(
            string ipAddress = "127.0.0.1",
            int port = 1888,
            string clientId = "kstopa")
        {
            // 先判断连接状态，避免重复连接
            if (_isConnected)
            {
                var result = new OperateResult("Already connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
                // 初始化 MQTT 连接参数（异步操作前先初始化对象，避免线程安全问题）
                var connectionOptions = new MqttConnectionOptions
                {
                    IpAddress = ipAddress,
                    Port = port,
                    ClientId = clientId,
                    // 可扩展：若服务器需要认证，添加用户名密码（异步操作不影响）
                    // UserName = "your-username",
                    // Password = "your-password"
                };

                // 实例化 MQTT 客户端（HslCommunication 的 MqttClient 支持异步操作）
                _mqttClient = new MqttClient(connectionOptions);

                // 注册异步事件处理（关键：事件处理方法为 async void）
                _mqttClient.OnNetworkError += Mqtt_OnNetworkError;
                _mqttClient.OnClientConnected += Mqtt_OnClientConnected;
                _mqttClient.OnMqttMessageReceived += Mqtt_OnMqttMessageReceived;

                // 核心异步操作：连接服务器（Hsl 的 ConnectServerAsync 是真正的异步 IO 操作）
                OperateResult connectResult = await _mqttClient.ConnectServerAsync();

                // 更新连接状态（volatile 保证线程可见性）
                _isConnected = connectResult.IsSuccess;

                // 触发连接状态事件（通知外部，如 ViewModel）
                ConnectionStatusChanged?.Invoke(connectResult);

                return connectResult;
            }
            catch (Exception ex)
            {
                // 捕获所有异常，封装为操作结果返回
                var errorResult = new OperateResult($"Connect failed: {ex.Message}");
                ConnectionStatusChanged?.Invoke(errorResult);
                NetworkErrorOccurred?.Invoke(ex); // 触发错误事件，供外部记录日志
                return errorResult;
            }
        }

        // 6. 异步订阅主题（支持多主题批量订阅）
        public async Task<OperateResult> SubscribeAsync(params string[] topics)
        {
            // 校验前置条件：连接状态 + 客户端实例
            if (!_isConnected || _mqttClient == null)
            {
                var result = new OperateResult("Subscribe failed: Not connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
                // 异步订阅（Hsl 的 SubscribeMessageAsync 为异步方法）
                OperateResult subscribeResult = await _mqttClient.SubscribeMessageAsync(topics);
                return subscribeResult;
            }
            catch (Exception ex)
            {
                var errorResult = new OperateResult($"Subscribe failed: {ex.Message}");
                NetworkErrorOccurred?.Invoke(ex);
                return errorResult;
            }
        }

        // 7. 异步发布消息（支持自定义 QoS 等级，默认 QoS0）
        public async Task<OperateResult> PublishAsync(
            string topic,
            string payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            // 校验前置条件
            if (!_isConnected || _mqttClient == null)
            {
                var result = new OperateResult("Publish failed: Not connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
                // 构建 MQTT 消息（Payload 转为字节数组）
                var mqttMessage = new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = Encoding.UTF8.GetBytes(payload),
                    QualityOfServiceLevel = qos,
                    Retain = false // 是否保留消息，根据业务需求设置
                };

                // 异步发布（Hsl 的 PublishMessageAsync 为异步方法）
                OperateResult publishResult = await _mqttClient.PublishMessageAsync(mqttMessage);
                return publishResult;
            }
            catch (Exception ex)
            {
                var errorResult = new OperateResult($"Publish failed: {ex.Message}");
                NetworkErrorOccurred?.Invoke(ex);
                return errorResult;
            }
        }

        // 8. 异步断开连接（释放资源，支持 IDisposable）
        public async Task DisconnectAsync()
        {
            if (_isConnected && _mqttClient != null)
            {
                try
                {
                    // 异步关闭连接（Hsl 的 ConnectCloseAsync 为异步方法，避免同步阻塞）
                    await _mqttClient.ConnectCloseAsync();
                }
                catch (Exception ex)
                {
                    NetworkErrorOccurred?.Invoke(ex);
                }
                finally
                {
                    // 无论是否成功，都更新状态并取消事件订阅
                    _isConnected = false;
                    UnsubscribeEvents(); // 取消事件订阅，避免内存泄漏
                    ConnectionStatusChanged?.Invoke(new OperateResult("Disconnected from MQTT server"));
                }
            }
        }

        // 9. 网络错误事件处理（async void 符合事件规范，内部无 await 可简化为 void）
        private void Mqtt_OnNetworkError(object? sender, EventArgs e)
        {
            _isConnected = false;
            var error = new Exception("MQTT network error occurred (e.g., disconnected, timeout)");
            NetworkErrorOccurred?.Invoke(error);
            ConnectionStatusChanged?.Invoke(new OperateResult("Network error: Disconnected from server"));
        }

        // 10. 客户端连接成功事件处理（async void：事件处理允许异步，内部 await 订阅操作）
        private async void Mqtt_OnClientConnected(MqttClient client)
        {
            _isConnected = true;
            ConnectionStatusChanged?.Invoke(new OperateResult("Connected to MQTT server successfully"));

            // 异步订阅主题（连接成功后自动订阅，避免同步阻塞）
            try
            {
                await SubscribeAsync("devices/+/#"); // 批量订阅可传多个主题
            }
            catch (Exception ex)
            {
                NetworkErrorOccurred?.Invoke(new Exception($"Auto-subscribe failed: {ex.Message}"));
            }
        }

        // 11. 消息接收事件处理（同步处理，若后续需数据库操作，需转为异步后台执行）
        private void Mqtt_OnMqttMessageReceived(MqttClient client, MqttApplicationMessage message)
        {
            try
            {
                // 同步解析消息（轻量操作，若解析复杂需改为 async）
                string topic = message.Topic;
                string payload = Encoding.UTF8.GetString(message.Payload);

                // 触发消息接收事件（通知外部，如 ViewModel 处理或异步存库）
                MessageReceived?.Invoke(topic, payload);

                // 【扩展】若需异步存数据库，此处需用 Task.Run 或注入异步服务
                // _ = SaveMessageToDbAsync(topic, payload); // 用 _ 忽略 Task，避免阻塞
            }
            catch (Exception ex)
            {
                NetworkErrorOccurred?.Invoke(new Exception($"Message parse failed: {ex.Message}"));
            }
        }

        // 12. 取消事件订阅（避免内存泄漏，在断开连接或释放时调用）
        private void UnsubscribeEvents()
        {
            if (_mqttClient == null) return;

            _mqttClient.OnNetworkError -= Mqtt_OnNetworkError;
            _mqttClient.OnClientConnected -= Mqtt_OnClientConnected;
            _mqttClient.OnMqttMessageReceived -= Mqtt_OnMqttMessageReceived;
        }

        // 13. IDisposable 实现（释放 MQTT 客户端资源，避免内存泄漏）
        public void Dispose()
        {
            // 异步断开连接（此处用 Wait 是因为 Dispose 是同步方法，需确保资源释放）
            _ = DisconnectAsync().ConfigureAwait(false); // 不捕获上下文，避免死锁

            // 释放客户端实例
            _mqttClient?.Dispose();
            _mqttClient = null;
        }
    }


    
}