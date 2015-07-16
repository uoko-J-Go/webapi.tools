using System.Linq;
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
            // if (!needRpcStyleResponse)
            // 暂时放开为了不影响开发, 最后直接替换
            if (needRpcStyleResponse)
            {
                return response;
            }

            var originData = await response.Content.ReadAsAsync<object>(cancellationToken);
            var rpcResult = new RPCStyleResult { data = originData };
            // 转化为 RPC 统一的风格
            if (!response.IsSuccessStatusCode)
            {
                response.StatusCode = HttpStatusCode.OK;
                rpcResult.code = "550";
                rpcResult.message = "请求失败,详情见 data 信息";
            }

            response.Content = request.CreateResponse(HttpStatusCode.OK, rpcResult).Content;
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