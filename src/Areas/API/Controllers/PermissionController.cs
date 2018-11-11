using System.Collections.Generic;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
    public class PermissionController : Controller
    {
        private readonly ISecurityManager _securityManager;

        public PermissionController(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            IList<PermissionInfo> permissions = _securityManager.GetPermissionsList();
            List<PermissionModel> permissionModels = new List<PermissionModel>(permissions.Count);

            foreach (PermissionInfo permission in permissions)
            {
                permissionModels.Add(ToPermissionModel(permission));
            }

            return Json(permissionModels);
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Add([FromBody]string name)
		{
			// Get permission
            if (_securityManager.GetPermissionByName(name) != null)
                return BadRequest();

            // Save permission
            PermissionInfo permission = new PermissionInfo();
            permission.Name = name;

            if (!_securityManager.SavePermission(permission))
                return BadRequest(name);

            return Ok(permission);
		}

		[HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromBody]int id)
        {
            if (_securityManager.GetPermissionById(id) == null)
            {
                return BadRequest("Permission not found");
            }

            if (_securityManager.DeletePermission(id))
            {
                return Ok();
            }

            return BadRequest();
        }

        #region Private methods

        public static PermissionModel ToPermissionModel(PermissionInfo permissionInfo)
        {
            PermissionModel model = new PermissionModel();
            model.Id = permissionInfo.PermissionId;
            model.Name = permissionInfo.Name;

            return model;
        }

        #endregion
    }
}
