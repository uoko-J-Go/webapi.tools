using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 处理 发生在具体 action 或 controller 中的未处理的异常,这个和默认的全局异常处理还不一样.
    /// 这一类的异常返回 BadRequest(400) 等错误状态.
    /// </summary>
    public class UnifyExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            var request = actionExecutedContext.Request;
            var httpError = new HttpError(exception, request.ShouldIncludeErrorDetail())
                            {
                                Message = "请求出错,请查看异常信息"
                            };


            var response = request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            actionExecutedContext.Response = response;
        }
    }
}