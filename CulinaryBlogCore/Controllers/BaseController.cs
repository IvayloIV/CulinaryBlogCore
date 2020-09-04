using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Enums;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<ApplicationUser> _userManager { get; }

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        protected async Task<bool> IsAdminOrOwner(string userId)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();

            return roles.Any(r => r == Role.Admin.ToString()) || user.Id == userId;
        }
    }
}
