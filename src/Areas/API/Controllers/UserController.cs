using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Kastra.Web.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
    public class UserController : Controller
    {
        #region Private members

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        #endregion

        public UserController(UserManager<ApplicationUser> userManager, 
                              RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<UserModel> userModels = new List<UserModel>(users.Count);

            foreach (ApplicationUser user in users)
            {
                userModels.Add(ToUserModel(user));
            }

            return Json(userModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody]UserModel model)
        {
            // Save user
            ApplicationUser user = _userManager.Users.Include(u => u.Roles).SingleOrDefault(u => u.Id == model.Id);

            if(user == null)
            {
                user = new ApplicationUser();
                user.UserName = model.Email;
                user.Email = model.Email;
                user.EmailConfirmed = true;

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if(!result.Succeeded)
                {
                    return BadRequest(result);
                }
            }
            else {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.DateModified = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);
            }

            // Save roles
            IdentityUserRole<String> currentRole = null;
            IList<ApplicationRole> roles = _roleManager.Roles.ToList();
            IList<IdentityUserRole<String>> currentRoles = user.Roles.ToList();

            if(roles != null)
            {
                foreach (var role in roles)
                {
                    currentRole = currentRoles.SingleOrDefault(cr => cr.RoleId == role.Id);

                    if (currentRole != null)
                    {
                        if (!model.Roles.Any(sr => sr == role.Id))
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        if (model.Roles.Any(sr => sr == role.Id))
                            await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }

            return Ok(new { Succeeded = true, UserId = user.Id });
        }

		[HttpGet]
		public IActionResult Get(string id)
		{
			if (String.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			ApplicationUser user = _userManager.Users.Include(u => u.Roles).SingleOrDefault(u => u.Id == id);

            if(user == null)
			{
				return NotFound();
			}
            
			return Json(ToUserModel(user));
		}

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody]string id)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        #region Private methods
        
        public static UserModel ToUserModel(ApplicationUser user)
        {
            UserModel model = new UserModel();
            model.Id = user.Id;
            model.UserName = user.UserName;
            model.Email = user.Email;
			model.Roles = user.Roles.Select(r => r.RoleId).ToArray();

            return model;
        }

        #endregion
    }
}
