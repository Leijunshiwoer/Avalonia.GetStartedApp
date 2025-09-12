using System;
using System.Threading.Tasks;
using TouchSocket.Mqtt;

/// <summary>
/// MQTT客户端服务接口
/// </summary>
public interface IMqttClientService : IDisposable
{
    /// <summary>
    /// 是否已连接
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 连接状态
    /// </summary>
    string ConnectStatus { get; }

    /// <summary>
    /// 连接状态变更事件
    /// </summary>
    event Action<string> ConnectionStatusChanged;

    /// <summary>
    /// 消息接收事件
    /// </summary>
    event Action<MqttArrivedMessage> MessageReceived;

    /// <summary>
    /// 重连尝试事件
    /// </summary>
    event Action<int> ReconnectAttempt;

    /// <summary>
    /// 初始化客户端
    /// </summary>
    /// <param name="ipHost">MQTT服务器地址和端口</param>
    /// <param name="clientId">客户端ID</param>
    /// <param name="reconnectInterval">重连间隔(毫秒)</param>
    /// <returns></returns>
    Task InitClientAsync(string ipHost, string clientId, int reconnectInterval = 5000);

    /// <summary>
    /// 连接到MQTT服务器
    /// </summary>
    /// <returns></returns>
    Task ConnectAsync();

    /// <summary>
    /// 断开与MQTT服务器的连接
    /// </summary>
    /// <returns></returns>
    Task DisconnectAsync();

    /// <summary>
    /// 订阅多个主题
    /// </summary>
    /// <param name="topics">要订阅的主题数组</param>
    /// <returns></returns>
    Task SubscribeAsync(string[] topics);

    /// <summary>
    /// 发布消息到指定主题
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="payload">消息内容</param>
    /// <returns></returns>
    Task PublishAsync(string topic, string payload);
}
