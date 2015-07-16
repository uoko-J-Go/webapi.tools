using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 处理 发生在具体 action 或 controller 中的未处理的异常,这个和默认的全局异常处理还不一样.
    /// 如果这一类异常中包含 TipForUI, 说明是我们主动抛出的,需要提示用户的.
    /// 这一类的异常返回 BadRequest(400) 等错误状态,以及友好提示信息还有错误信息.
    /// </summary>
    public class UnifyExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var request = actionExecutedContext.Request;

            if (exception.Data.Contains("TipForUI"))
            {
                var errorInfo = exception.Data["TipForUI"];
                var httpError = new HttpError(exception.Message);
                httpError["ErrorInfo"] = errorInfo;
                var response = request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                actionExecutedContext.Response = response;
            }
        }
    }
}