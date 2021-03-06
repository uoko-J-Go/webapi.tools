﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 为了兼容 UOKO RPC 请求
    /// </summary>
    public class RPCResponseMsgHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            var needRpcStyleResponse = request.Headers.Any(item => item.Key == "uoko-rpc-response");
            if (!needRpcStyleResponse)
            {
                return response;
            }

            object originData = null;
            if (response.Content != null)
            {
                originData = await response.Content.ReadAsAsync<object>(cancellationToken);
            }

            var rpcResult = new RPCStyleResult {data = originData};
            // 转化为 RPC 统一的风格
            if (!response.IsSuccessStatusCode)
            {
                rpcResult.code = ((int)response.StatusCode).ToString();
                rpcResult.message = "请求失败,详情见 data 信息";
            }

            response.Content = request.CreateResponse(HttpStatusCode.OK, rpcResult).Content;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }


        public class RPCStyleResult
        {
            public string code { get; set; }
            public string message { get; set; }
            public object data { get; set; }

            public RPCStyleResult()
            {
                code = "200";
            }
        }
    }
}