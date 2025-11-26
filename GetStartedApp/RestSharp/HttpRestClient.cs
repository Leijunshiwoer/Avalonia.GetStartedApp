using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Sockets;

namespace GetStartedApp.RestSharp
{
    public class HttpRestClient
    {
        private readonly string webUrl;

        //请求客户端
        private readonly RestClient client;

        //通过构造函数注入一个基地址
        public HttpRestClient(string webUrl)
        {
            this.webUrl = webUrl;
            client = new RestClient(webUrl);
        }

        public async Task<ApiResponse> ExcuteAsync(BaseRequest baseRequest)
        {
            try
            {
                var request = new RestRequest(baseRequest.Route, baseRequest.Method);

                // 对于POST/PUT等需要请求体的方法，使用AddJsonBody
                if (baseRequest.Parameter != null &&
                    baseRequest.Method != Method.Get &&
                    baseRequest.Method != Method.Head)
                {
                    request.AddJsonBody(baseRequest.Parameter);
                }

                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                else
                {
                    return new ApiResponse()
                    {
                        Status = false,
                        Message = response.ErrorMessage + "  ****  " + response.Content?.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<ApiResponse<T>> ExcuteAsync<T>(BaseRequest baseRequest)
        {
            try
            {
                var request = new RestRequest(baseRequest.Route, baseRequest.Method);
                request.AddHeader("Content-Type", baseRequest.ContentType);

                // 最简单的修改：只在非 GET/HEAD 方法时添加请求体
                if (baseRequest.Parameter != null &&
                    baseRequest.Method != Method.Get &&
                    baseRequest.Method != Method.Head)
                {
                    //不要name
                    request.AddParameter(null, JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                }

                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
                else
                    return new ApiResponse<T>()
                    {
                        Status = false,
                        Message = response.ErrorMessage + "***" + response.Content.ToString()
                    };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = ex.Message.ToString()
                };
            }
        }

    }
}
