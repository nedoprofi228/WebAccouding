using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace accoudingWeb.Controllers;

public class IndexController: Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return Content(User.Identity.Name);
    }
}