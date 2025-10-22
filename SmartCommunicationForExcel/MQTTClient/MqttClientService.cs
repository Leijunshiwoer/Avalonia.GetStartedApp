using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Mqtt;
using TouchSocket.Sockets;

public class MqttClientService: IMqttClientService
{
    private MqttTcpClient _client;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    private bool _isConnected = false;
    private string _connectStatus = "未连接";
    private int _reconnectInterval = 5000; // 重连间隔(毫秒)
    private int _maxReconnectInterval = 30000; // 最大重连间隔(毫秒)
    private bool _isReconnecting = false; // 重连状态标记

    // 连接状态变更事件
    public event Action<string> ConnectionStatusChanged;
    // 消息接收事件
    public event Action<MqttArrivedMessage> MessageReceived;
    // 重连尝试事件
    public event Action<int> ReconnectAttempt;

    public bool IsConnected => _isConnected;
    public string ConnectStatus => _connectStatus;

    public async Task InitClientAsync(string ipHost, string clientId, int reconnectInterval = 5000)
    {
        if (_client != null)
        {
            await DisconnectAsync();
        }

        _reconnectInterval = reconnectInterval;
        _client = new MqttTcpClient();

        await _client.SetupAsync(new TouchSocketConfig()
           .SetRemoteIPHost(ipHost)
           .SetMqttConnectOptions(options =>
           {
               options.ClientId = clientId;
               options.ProtocolName = "MQTT";
               options.Version = MqttProtocolVersion.V311;
               options.KeepAlive = 60;
               options.CleanSession = false;
           })
           .ConfigurePlugins(a =>
           {
               a.AddMqttConnectingPlugin(async (mqttSession, e) =>
               {
                   Console.WriteLine($"Client Connecting: {e.ConnectMessage.ClientId}");
                   UpdateConnectionStatus("MQTT客户端连接中...");
                   await e.InvokeNext();
               });

               a.AddMqttConnectedPlugin(async (mqttSession, e) =>
               {
                   Console.WriteLine($"Client Connected: {e.ConnectMessage.ClientId}");
                   _isConnected = true;
                   _isReconnecting = false;
                   // 重置重连间隔
                   _reconnectInterval = Math.Min(_reconnectInterval, 5000);
                   UpdateConnectionStatus("MQTT客户端已连接");
                   await e.InvokeNext();
               });

               a.AddMqttReceivedPlugin(async (mqttSession, e) =>
               {
                   var message = e.MqttMessage;
                   MessageReceived?.Invoke(message);
                   await e.InvokeNext().ConfigureAwait(EasyTask.ContinueOnCapturedContext);
               });

               a.AddMqttClosedPlugin(async (mqttSession, e) =>
               {
                   Console.WriteLine($"Client Closed: {e.Message}");
                   _isConnected = false;
                   UpdateConnectionStatus("MQTT客户端已断开连接");

                   // 如果不是主动断开连接，则启动重连
                   if (!_cts.IsCancellationRequested)
                   {
                       _ = StartReconnectLoopAsync();
                   }
                   await e.InvokeNext();
               });
           }));
    }

    public async Task ConnectAsync()
    {
        if (_client == null)
        {
            throw new InvalidOperationException("客户端未初始化，请先调用InitClientAsync方法");
        }

        try
        {
            await _client.ConnectAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"连接失败: {ex.Message}");
            UpdateConnectionStatus($"连接失败: {ex.Message}");

            // 启动重连
            _ = StartReconnectLoopAsync();
        }
    }

    public async Task DisconnectAsync()
    {
        // 取消重连
        _cts.Cancel();

        if (_client != null && _isConnected)
        {
            await _client.CloseAsync();
        }

        _isConnected = false;
        _isReconnecting = false;
        UpdateConnectionStatus("MQTT客户端已断开连接");

        // 重置取消令牌以便下次使用
        _cts.Dispose();
        _cts = new CancellationTokenSource();
    }

    // 订阅主题
    public async Task SubscribeAsync(string[] topics)
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("客户端未连接，无法订阅主题");
        }

        var subscribeRequests = new List<SubscribeRequest>();
        for(int i=0;i<topics.Length;i++)
        {
            subscribeRequests.Add(new SubscribeRequest(topics[i], QosLevel.ExactlyOnce));
        }
        var message = new MqttSubscribeMessage(subscribeRequests.ToArray());
        await _client.SubscribeAsync(message);
    }

    // 发布消息
    public async Task PublishAsync(string topic,string payload)
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("客户端未连接，无法发布消息");
        }

        var message = new MqttPublishMessage(topic, false, QosLevel.ExactlyOnce, Encoding.UTF8.GetBytes(payload));
     
        await _client.PublishAsync(message);
    }

    // 启动重连循环
    private async Task StartReconnectLoopAsync()
    {
        // 防止重复启动重连
        if (_isReconnecting || _cts.IsCancellationRequested)
        {
            return;
        }

        _isReconnecting = true;
        int attempt = 0;

        while (!_isConnected && !_cts.IsCancellationRequested)
        {
            attempt++;
            ReconnectAttempt?.Invoke(attempt);
            Console.WriteLine($"尝试重连 ({attempt})...");
            UpdateConnectionStatus($"尝试重连 ({attempt})...");

            try
            {
                // 等待重连间隔
                await Task.Delay(_reconnectInterval, _cts.Token);

                // 尝试连接
                await _client.ConnectAsync(_cts.Token);

                // 如果连接成功，循环会因为_isConnected变为true而退出
            }
            catch (OperationCanceledException)
            {
                // 取消操作，退出循环
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重连失败: {ex.Message}");

                // 实现指数退避策略，重连间隔逐渐增加，但不超过最大值
                _reconnectInterval = Math.Min(_reconnectInterval * 2, _maxReconnectInterval);
            }
        }
    }

    private void UpdateConnectionStatus(string status)
    {
        _connectStatus = status;
        ConnectionStatusChanged?.Invoke(status);
    }

    // 释放资源
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        _client?.Dispose();
    }
}
