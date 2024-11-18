using Microsoft.AspNetCore.Mvc;

namespace KoiFishManager.Api.Controllers
{
    public class FishsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
