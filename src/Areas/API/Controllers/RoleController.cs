using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Web.API.Controllers
{
	[Area("Api")]
    [Authorize("Administration")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        
        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            IList<ApplicationRole> roles = _roleManager.Roles.ToList();
            List<RoleModel> roleModels = new List<RoleModel>(roles.Count);

            foreach (ApplicationRole role in roles)
            {
                roleModels.Add(ToRoleModel(role));
            }

            return Json(roleModels);
        }

		[HttpGet]
        public IActionResult Get(string id)
        {
			ApplicationRole role = _roleManager.Roles
			                                   .Include(r => r.Claims)
			                                   .SingleOrDefault(r => r.Id == id);

            if(role == null)
			{
				return NotFound();
			}
         
			return Json(ToRoleModel(role));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]string id)
        {
            ApplicationRole role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);

            if (role == null)
            {
                return BadRequest("Role not found");
            }

            await _roleManager.DeleteAsync(role);

            return Ok();
        }

        #region Private methods

		public static RoleModel ToRoleModel(ApplicationRole roleInfo)
        {         
			RoleModel model = new RoleModel();
            model.Id = roleInfo.Id;
            model.Name = roleInfo.Name;
			model.Permissions = new List<int>(roleInfo.Claims.Count);

			int permissionId = 0;

			foreach(var claim in roleInfo.Claims)
			{
				if(claim.ClaimType == Constants.ModuleConfig.ModulePermissionType && Int32.TryParse(claim.ClaimValue, out permissionId))
				{
					model.Permissions.Add(permissionId);
				}
			}
            
            return model;
        }

        #endregion
    }
}
