using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kastra.Core;
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
        private readonly IModuleManager _moduleManager;
        private readonly IViewManager _viewManager;

        public ModuleDefinitionController(IModuleManager moduleManager, IViewManager viewManager)
        {
            _moduleManager = moduleManager;
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

        [HttpGet]
        public IActionResult ModuleFound()
        {
            ModuleDefinitionInfo moduleDefinition = null;
            ModuleInstallModel moduleModel = null;
            List<ModuleInstallModel> model = new List<ModuleInstallModel>();
            IList<ModuleDefinitionInfo> modulesDefinitions = _viewManager.GetModuleDefsList();

            foreach (Assembly assembly in KastraAssembliesContext.Instance.GetModuleAssemblies())
            {
                moduleDefinition = modulesDefinitions.SingleOrDefault(m => assembly.Location.Contains(m.Path));

                moduleModel = new ModuleInstallModel();
                moduleModel.AssemblyName = assembly.GetName().Name;
                moduleModel.Version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                moduleModel.ModuleDefinitionId = moduleDefinition?.ModuleDefId ?? 0;
                moduleModel.Name = moduleDefinition?.Name ?? String.Empty;

                model.Add(moduleModel);
            }

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Install([FromBody] string assemblyName)
        {
            Assembly assembly = null;

            if(String.IsNullOrEmpty(assemblyName))
            {
                return BadRequest();
            }

            // Get assembly with assembly name
            assembly = KastraAssembliesContext.Instance.GetModuleAssemblies()
                                              .SingleOrDefault(a => a.GetName().Name == assemblyName);

            if(assembly == null)
            {
                return BadRequest("Module assembly not found");
            }

            // Install module
            _moduleManager.InstallModule(assembly);

            return Ok();
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
