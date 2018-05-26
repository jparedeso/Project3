using Microsoft.AspNetCore.Mvc;

namespace Project3.Web.Controllers
{
    public class ForumController : Controller
    {
        public ActionResult Forum()
        {
            return View();
        }
    }
}