using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Data.ViewModels;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userMgr)
        {
            _userManager = userMgr;
        }
        #region RESTful Conventions
        /// <summary>
        /// POST: api/user/register
        /// </summary>
        /// <returns>Creates a new User and return it accordingly.
        ///</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Add([FromBody]UserViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // check if the Username/Email already exists
            IdentityUser user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null) return BadRequest("Username already exists");
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) return BadRequest("Email already exists.");
            var now = DateTime.Now;
            // create a new Item with the client-sent json data
            user = new IdentityUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email
            };
            // Add the user to the Db with the choosen password
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //todo
            }
            else
            {
                //todo
            }
            // persist the changes into the Database.
            await _userManager.UpdateAsync(user);
            UserViewModel userVm = new UserViewModel()
            {
                UserName = user.UserName,
                DisplayName = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash
            };

            // return the newly-created User to the client.
            return Json(userVm);
        }
        #endregion
    }
}
