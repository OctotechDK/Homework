using System.Web.Mvc;
using Homework.Models;

namespace Homework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new FormSubmissionViewModel());
        }
    }
}