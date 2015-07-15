using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 方便用户对 ModelState 操作以后
    /// </summary>
    public static class ActionContextExtension
    {

        public static IHttpActionResult GenerateUnifyErrorResult(this HttpActionContext actionContext)
        {

            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            var request = actionContext.Request;
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            var modelState = actionContext.ModelState;
            if (modelState == null
                || modelState.IsValid)
            {
                throw new InvalidOperationException("modelState isValid, no need to generate error");
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
            var errorResponse = request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            var result = new ResponseMessageResult(errorResponse);
            return result;
        }
    }
}