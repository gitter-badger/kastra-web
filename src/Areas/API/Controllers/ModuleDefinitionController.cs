using System.Collections.Generic;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.ModuleDefinition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
	[Area("Api")]
    [Authorize("Administration")]
    public class ModuleDefinitionController : Controller
    {
        private readonly IViewManager _viewManager;

        public ModuleDefinitionController(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        [HttpGet]
        public IActionResult List()
        {
			IList<ModuleDefinitionInfo> moduleDefinitions = _viewManager.GetModuleDefsList();
            List<ModuleDefinitionModel> moduleDefinitionModels = new List<ModuleDefinitionModel>(moduleDefinitions.Count);
            
            foreach (ModuleDefinitionInfo moduleDefinition in moduleDefinitions)
            {
				moduleDefinitionModels.Add(ToModuleDefinitionModel(moduleDefinition));
            }

			return Json(moduleDefinitionModels);
        }

        #region Private methods

		public static ModuleDefinitionModel ToModuleDefinitionModel(ModuleDefinitionInfo moduleDefinitionInfo)
        {
            ModuleDefinitionModel model = new ModuleDefinitionModel();
			model.Id = moduleDefinitionInfo.ModuleDefId;
			model.Name = moduleDefinitionInfo.Name;
				
            return model;
        }

        #endregion
    }
}
