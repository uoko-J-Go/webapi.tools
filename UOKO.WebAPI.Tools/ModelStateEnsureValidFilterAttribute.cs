using System.Linq;
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

            if (actionContext.ActionDescriptor!=null)
            {
                var hasExcludeModelStateEnsureValideAttr =
                    actionContext.ActionDescriptor.GetCustomAttributes<ExcludeModelStateEnsureValidFilterAttribute>()
                                 .Any();
                if (hasExcludeModelStateEnsureValideAttr)
                {
                    return;
                }
            }

            if (actionContext.ControllerContext != null
                && actionContext.ControllerContext.ControllerDescriptor != null)
            {
                var hasExcludeModelStateEnsureValideAttr =
                    actionContext.ControllerContext.ControllerDescriptor
                                 .GetCustomAttributes<ExcludeModelStateEnsureValidFilterAttribute>()
                                 .Any();
                if (hasExcludeModelStateEnsureValideAttr)
                {
                    return;
                }
            }

            var modelState = actionContext.ModelState;
            if (modelState == null
                || modelState.IsValid)
            {
                return;
            }

            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
        }
    }
}