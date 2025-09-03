using HslCommunication;
using HslCommunication.MQTT;
using System;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.MQTTClient
{
    // 配套接口（依赖注入核心，面向接口编程）
    public interface IMqttService
    {
        bool IsConnected { get; }
        event Action<OperateResult> ConnectionStatusChanged;
        event Action<string, string> MessageReceived;
        event Action<Exception> NetworkErrorOccurred;

        Task<OperateResult> ConnectAsync(string ipAddress = "127.0.0.1", int port = 1888, string clientId = "kstopa");
        Task<OperateResult> SubscribeAsync(params string[] topics);
        Task<OperateResult> PublishAsync(string topic, string payload, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);
        Task DisconnectAsync();
    }
}