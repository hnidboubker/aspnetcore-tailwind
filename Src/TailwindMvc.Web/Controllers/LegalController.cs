using Microsoft.AspNetCore.Mvc;

namespace TailwindMvc.Web.Controllers
{
    public class LegalController : Controller
    {
        public IActionResult CGU()
        {
            return View();
        }
        public IActionResult CGV()
        {
            return View();
        } 
        public IActionResult Confidentiality()
        {
            return View();
        }
        public IActionResult RGPD()
        {
            return View();
        }
    }
}
