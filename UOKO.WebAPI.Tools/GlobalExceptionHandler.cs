using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 这个其实做的和 DefaultExceptionHandler 一样
    /// 
    /// 为 unhandled exceptions 定义全局的异常处理
    /// 如果异常走到这里,通常是未处理的异常. 比如 Controller 构造,路由, MessageHandler 这些过程的.
    /// 所以都应该以 500 错误响应.
    /// 
    /// 业务代码中的异常,应该由 ExceptionFilter 进行过滤处理, 那一类的异常返回 BadRequest(400) 等错误状态.
    /// </summary>
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {

            var exception = context.Exception;
            var request = context.Request;

            // 通过异常信息,构建基础的错误信息
            var httpError = new HttpError(exception, request.ShouldIncludeErrorDetail());
            var result = request.CreateErrorResponse(HttpStatusCode.InternalServerError, httpError);

            context.Result = new ResponseMessageResult(result);
        }
    }
}
