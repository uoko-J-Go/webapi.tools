using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 这个是做整个 ModelState 强制校验的.
    /// 如果 ModelState 有错误, 那么就直接返回统一的错误信息.
    /// 可以通过指定 OverrideModelStateEnsureFilter 来达到排除,
    /// 然后自己在方法体中做相应的处理.
    /// </summary>
    public class ModelStateEnsureValidFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                return;
            }
            var request = actionContext.Request;
            if (request == null)
            {
                return;
            }

            var modelState = actionContext.ModelState;
            if (modelState == null
                || modelState.IsValid)
            {
                return;
            }

            // 返回统一错误信息, 用 ErrorInfo 代替 ModelStateKey(ModelState) ,因为除了 ModelState 以外
            // 还可能是用户自定义的一些 ErrorInfo ,只不过我们都统一使用 ModelState 的格式进行返回.
            var httpError = new HttpError(modelState, request.ShouldIncludeErrorDetail());
            object modelStateInfo;
            var hasModelStateInfo = httpError.TryGetValue(HttpErrorKeys.ModelStateKey, out modelStateInfo);
            if (hasModelStateInfo)
            {
                httpError.Remove(HttpErrorKeys.ModelStateKey);
                httpError["ErrorInfo"] = modelStateInfo;
            }
            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
        }
    }
}