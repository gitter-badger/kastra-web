using Kastra.Core.Business;
using Kastra.Core.Controllers;
using Kastra.Core.Services;
using Kastra.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Kastra.Web.Controllers
{
    [SiteConfiguration]
    public class PageController : TemplateController
    {
        public PageController(IViewManager viewManager,
                              CacheEngine cacheEngine, 
                              IViewComponentDescriptorCollectionProvider viewcomponents, 
                              IParameterManager parameterManager) 
                            : base(viewManager, cacheEngine, viewcomponents, parameterManager){}

        public IActionResult Error()
        {
            return View();
        }
    }
}
