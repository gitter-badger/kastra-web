using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.Areas.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
    public class ModuleController : Controller
    {
        private readonly ISecurityManager _securityManager;
        private readonly IViewManager _viewManager;

        public ModuleController(ISecurityManager securityManager, IViewManager viewManager)
        {
            _securityManager = securityManager;
            _viewManager = viewManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            IList<ModuleInfo> modules = _viewManager.GetModulesList();
            List<ModuleModel> moduleModels = new List<ModuleModel>(modules.Count);

            foreach (ModuleInfo module in modules)
            {
                moduleModels.Add(ToModuleModel(module));
            }

            return Json(moduleModels);
        }

        [HttpGet]
		public IActionResult Get(int id)
		{
			ModuleInfo module = _viewManager.GetModule(id, false, true);

			if(module == null)
			{
				return NotFound();
			}

			return Json(ToModuleModel(module));
		}

        [HttpPost]
        public IActionResult Update([FromBody]ModuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Save module
            ModuleInfo module = new ModuleInfo();
            module.ModuleId = model.Id;
            module.ModuleDefId = model.DefinitionId;
            module.PageId = model.PageId;
            module.PlaceId = model.PlaceId;
            module.Name = model.Name;

            _viewManager.SaveModule(module);

            #region Permissions

            ModulePermissionInfo modulePermission = null;
            IList<ModulePermissionInfo> modulePermissions = _securityManager.GetModulePermissionsByModuleId(module.ModuleId);
            IList<PermissionInfo> permissions = _securityManager.GetPermissionsList();

            foreach (var permission in permissions)
            {
                if (model.Permissions.Contains(permission.PermissionId)
                    && !modulePermissions.Any(mp => mp.PermissionId == permission.PermissionId))
                {
                    modulePermission = new ModulePermissionInfo();
                    modulePermission.ModuleId = module.ModuleId;
                    modulePermission.PermissionId = permission.PermissionId;

                    _securityManager.SaveModulePermission(modulePermission);
                }
                else if (!model.Permissions.Contains(permission.PermissionId)
                         && modulePermissions.Any(mp => mp.PermissionId == permission.PermissionId))
                {
                    _securityManager.DeleteModulePermission(module.ModuleId, permission.PermissionId);
                }
            }

            #endregion

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]int id)
        {
            if (_viewManager.GetModule(id) == null)
            {
                return BadRequest("Module not found");
            }

            if (_viewManager.DeleteModule(id))
            {
                return Ok();
            }

            return BadRequest();
        }

        #region Private methods

        public static ModuleModel ToModuleModel(ModuleInfo moduleInfo)
        {
            ModuleModel model = new ModuleModel();
            model.Id = moduleInfo.ModuleId;
            model.Name = moduleInfo.Name;
            model.PageId = moduleInfo.PageId;
            model.PlaceId = moduleInfo.PlaceId;
            model.DefinitionId = moduleInfo.ModuleDefId;
			model.Permissions = moduleInfo.ModulePermissions.Select(p => p.PermissionId).ToArray();

            return model;
        }

        #endregion
    }
}
