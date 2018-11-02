
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kastra.Web.Filters
{
    public class SiteConfigurationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            IParameterManager parameterManager = context.HttpContext.RequestServices.GetService(typeof(IParameterManager)) as IParameterManager;
            SiteConfigurationInfo siteConfiguration = parameterManager.GetSiteConfiguration();

            Controller controller = context.Controller as Controller;
            if (controller != null) 
            {
                controller.ViewBag.Title = siteConfiguration.Title;
                controller.ViewBag.Theme = siteConfiguration.Theme;
            }
        }
    }
}
