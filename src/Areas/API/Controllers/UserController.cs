using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kastra.Web.API.Models.User;
using Kastra.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
