using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
    }
}
