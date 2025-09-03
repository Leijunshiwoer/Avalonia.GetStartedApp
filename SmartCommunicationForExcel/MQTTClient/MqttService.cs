using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.MQTT;

namespace SmartCommunicationForExcel.MQTTClient
{
    public class MqttService : IMqttService, IDisposable
    {
        // 线程安全单例
        private static readonly Lazy<MqttService> _instance = new Lazy<MqttService>(() => new MqttService());

        // MQTT 客户端实例
        private MqttClient _mqttClient;

        // 连接状态和参数
        private volatile bool _isConnected;
        private string _lastIpAddress = "127.0.0.1";
        private int _lastPort = 1888;
        private string _lastClientId = "kstopa";

        // 重连相关字段
        private readonly object _reconnectLock = new object();
        private bool _isReconnecting;
        private int _reconnectDelay = 5000; // 初始重连延迟5秒
        private const int _maxReconnectDelay = 60000; // 最大重连延迟60秒
        private CancellationTokenSource _reconnectCts;

        // 事件定义
        public event Action<OperateResult> ConnectionStatusChanged;
        public event Action<string, string> MessageReceived;
        public event Action<Exception> NetworkErrorOccurred;

        // 连接状态对外暴露
        public bool IsConnected => _isConnected;

        // 构造函数
        public MqttService() { }

        // 单例访问点
        public static MqttService Instance => _instance.Value;

        // 异步连接 MQTT 服务器
        public async Task<OperateResult> ConnectAsync(
            string ipAddress = "127.0.0.1",
            int port = 1888,
            string clientId = "kstopa")
        {
            // 保存连接参数，用于重连
            _lastIpAddress = ipAddress;
            _lastPort = port;
            _lastClientId = clientId;

            // 先判断连接状态，避免重复连接
            if (_isConnected)
            {
                var result = new OperateResult("Already connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
                // 初始化重连令牌源
                _reconnectCts = new CancellationTokenSource();

                // 初始化 MQTT 连接参数
                var connectionOptions = new MqttConnectionOptions
                {
                    IpAddress = ipAddress,
                    Port = port,
                    ClientId = clientId,
                    // 可扩展：添加用户名密码
                    // UserName = "your-username",
                    // Password = "your-password"
                };

                // 实例化 MQTT 客户端
                _mqttClient = new MqttClient(connectionOptions);

                // 注册事件处理
                _mqttClient.OnNetworkError += Mqtt_OnNetworkError;
                _mqttClient.OnClientConnected += Mqtt_OnClientConnected;
                _mqttClient.OnMqttMessageReceived += Mqtt_OnMqttMessageReceived;

                // 连接服务器
                OperateResult connectResult = await _mqttClient.ConnectServerAsync();

                // 更新连接状态
                _isConnected = connectResult.IsSuccess;

                // 触发连接状态事件
                ConnectionStatusChanged?.Invoke(connectResult);

                return connectResult;
            }
            catch (Exception ex)
            {
                var errorResult = new OperateResult($"Connect failed: {ex.Message}");
                ConnectionStatusChanged?.Invoke(errorResult);
                NetworkErrorOccurred?.Invoke(ex);

                // 启动重连机制
                StartReconnect();

                return errorResult;
            }
        }

        // 异步订阅主题
        public async Task<OperateResult> SubscribeAsync(params string[] topics)
        {
            if (!_isConnected || _mqttClient == null)
            {
                var result = new OperateResult("Subscribe failed: Not connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
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

        // 异步发布消息
        public async Task<OperateResult> PublishAsync(
            string topic,
            string payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            if (!_isConnected || _mqttClient == null)
            {
                var result = new OperateResult("Publish failed: Not connected to MQTT server");
                ConnectionStatusChanged?.Invoke(result);
                return result;
            }

            try
            {
                var mqttMessage = new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = Encoding.UTF8.GetBytes(payload),
                    QualityOfServiceLevel = qos,
                    Retain = false
                };

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

        // 异步断开连接
        public async Task DisconnectAsync()
        {
            // 取消重连
            CancelReconnect();

            if (_isConnected && _mqttClient != null)
            {
                try
                {
                    await _mqttClient.ConnectCloseAsync();
                }
                catch (Exception ex)
                {
                    NetworkErrorOccurred?.Invoke(ex);
                }
                finally
                {
                    _isConnected = false;
                    UnsubscribeEvents();
                    ConnectionStatusChanged?.Invoke(new OperateResult("Disconnected from MQTT server"));
                }
            }
        }

        // 启动重连机制
        private void StartReconnect()
        {
            // 确保同时只有一个重连操作在进行
            lock (_reconnectLock)
            {
                if (_isReconnecting || _isConnected || _reconnectCts?.IsCancellationRequested == true)
                    return;

                _isReconnecting = true;
                _ = PerformReconnectAsync(_reconnectCts.Token);
            }
        }

        // 执行重连操作
        private async Task PerformReconnectAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && !_isConnected)
                {
                    ConnectionStatusChanged?.Invoke(new OperateResult($"Attempting to reconnect in {_reconnectDelay / 1000} seconds..."));

                    // 等待重连延迟或取消信号
                    await Task.Delay(_reconnectDelay, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;

                    try
                    {
                        // 尝试重新连接
                        ConnectionStatusChanged?.Invoke(new OperateResult("Reconnecting to MQTT server..."));

                        // 先清理之前的客户端
                        if (_mqttClient != null)
                        {
                            UnsubscribeEvents();
                            _mqttClient.Dispose();
                        }

                        // 创建新的连接参数和客户端
                        var connectionOptions = new MqttConnectionOptions
                        {
                            IpAddress = _lastIpAddress,
                            Port = _lastPort,
                            ClientId = _lastClientId,
                            // 用户名密码和之前保持一致
                        };

                        _mqttClient = new MqttClient(connectionOptions);

                        // 重新注册事件
                        _mqttClient.OnNetworkError += Mqtt_OnNetworkError;
                        _mqttClient.OnClientConnected += Mqtt_OnClientConnected;
                        _mqttClient.OnMqttMessageReceived += Mqtt_OnMqttMessageReceived;

                        // 尝试连接
                        var result = await _mqttClient.ConnectServerAsync();

                        if (result.IsSuccess)
                        {
                            // 连接成功，重置重连延迟
                            _reconnectDelay = 5000;
                            _isConnected = true;
                            ConnectionStatusChanged?.Invoke(new OperateResult("Reconnected to MQTT server successfully"));

                            // 重新订阅之前的主题
                            await SubscribeAsync("devices/+/#");
                            break;
                        }
                        else
                        {
                            ConnectionStatusChanged?.Invoke(new OperateResult($"Reconnection failed: {result.Message}"));
                        }
                    }
                    catch (Exception ex)
                    {
                        NetworkErrorOccurred?.Invoke(new Exception($"Reconnection error: {ex.Message}"));
                    }

                    // 指数退避算法：重连失败后延迟加倍，但不超过最大值
                    _reconnectDelay = Math.Min(_reconnectDelay * 2, _maxReconnectDelay);
                }
            }
            finally
            {
                _isReconnecting = false;
            }
        }

        // 取消重连
        private void CancelReconnect()
        {
            lock (_reconnectLock)
            {
                if (_reconnectCts != null && !_reconnectCts.IsCancellationRequested)
                {
                    _reconnectCts.Cancel();
                    _reconnectCts.Dispose();
                    _reconnectCts = null;
                }
                _isReconnecting = false;
            }
        }

        // 网络错误事件处理
        private void Mqtt_OnNetworkError(object? sender, EventArgs e)
        {
            if (_isConnected) // 只在连接状态下处理网络错误
            {
                _isConnected = false;
                var error = new Exception("MQTT network error occurred (e.g., disconnected, timeout)");
                NetworkErrorOccurred?.Invoke(error);
                ConnectionStatusChanged?.Invoke(new OperateResult("Network error: Disconnected from server"));

                // 启动重连
                StartReconnect();
            }
        }

        // 客户端连接成功事件处理
        private async void Mqtt_OnClientConnected(MqttClient client)
        {
            _isConnected = true;
            ConnectionStatusChanged?.Invoke(new OperateResult("Connected to MQTT server successfully"));

            try
            {
                await SubscribeAsync("devices/+/#");
            }
            catch (Exception ex)
            {
                NetworkErrorOccurred?.Invoke(new Exception($"Auto-subscribe failed: {ex.Message}"));
            }
        }

        // 消息接收事件处理
        private void Mqtt_OnMqttMessageReceived(MqttClient client, MqttApplicationMessage message)
        {
            try
            {
                string topic = message.Topic;
                string payload = Encoding.UTF8.GetString(message.Payload);
                MessageReceived?.Invoke(topic, payload);
            }
            catch (Exception ex)
            {
                NetworkErrorOccurred?.Invoke(new Exception($"Message parse failed: {ex.Message}"));
            }
        }

        // 取消事件订阅
        private void UnsubscribeEvents()
        {
            if (_mqttClient == null) return;

            _mqttClient.OnNetworkError -= Mqtt_OnNetworkError;
            _mqttClient.OnClientConnected -= Mqtt_OnClientConnected;
            _mqttClient.OnMqttMessageReceived -= Mqtt_OnMqttMessageReceived;
        }

        // IDisposable 实现
        public void Dispose()
        {
            // 取消重连
            CancelReconnect();

            // 断开连接
            _ = DisconnectAsync().ConfigureAwait(false);

            // 释放客户端实例
            _mqttClient?.Dispose();
            _mqttClient = null;
        }
    }
}
