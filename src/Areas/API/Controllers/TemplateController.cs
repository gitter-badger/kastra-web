using System.Collections.Generic;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.Areas.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
	public class TemplateController : Controller
    {
        private readonly IViewManager _viewManager;

		public TemplateController(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        [HttpGet]
        public IActionResult List()
        {
			IList<TemplateInfo> templates = _viewManager.GetPageTemplatesList();
            List<TemplateModel> templateModels = new List<TemplateModel>(templates.Count);
            
			foreach (TemplateInfo template in templates)
            {
				templateModels.Add(ToTemplateModel(template));
            }

            return Json(templateModels);
        }
        
        #region Private methods
        
		public static TemplateModel ToTemplateModel(TemplateInfo templateInfo)
        {
            TemplateModel model = new TemplateModel();
			model.Id = templateInfo.TemplateId;
			model.Name = templateInfo.Name;

            return model;
        }

        #endregion
    }
}
