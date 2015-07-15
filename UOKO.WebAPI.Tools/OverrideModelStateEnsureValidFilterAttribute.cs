using System;
using System.Web.Http.Filters;

namespace UOKO.WebAPI.Tools
{
    /// <summary>
    /// 排除 ModelStateEnsureValidFilter
    /// </summary>
    public sealed class OverrideModelStateEnsureValidFilterAttribute : Attribute, IOverrideFilter
    {
        public Type FiltersToOverride
        {
            get { return typeof (ModelStateEnsureValidFilterAttribute); }
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}