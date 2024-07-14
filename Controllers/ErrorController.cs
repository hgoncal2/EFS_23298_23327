using Microsoft.AspNetCore.Mvc;

namespace EFS_23298_23327.Controllers
{
    public class ErrorController : Controller
    {

        [HttpGet]
        public IActionResult Error(int? codigo) {
            if (codigo!=null) {
                if (codigo == 404) {
                    return View("NotFound");
                } else {
                    return View("GenError");
                }

            }
            return View();
        }
    }
}
