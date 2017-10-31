using System.Linq;
using System.Web.Mvc;
using Homework.Business.Models;
using PagedList;

namespace Homework.Controllers
{
    [Authorize]
    public class FormSubmissionController : Controller
    {
        private readonly ApplicationDbContext context;

        public FormSubmissionController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ViewResult Index(int? page)
        {
            ViewBag.Title = "Form Submissions";

            var submissions = context.Submissions.Select(submission => new FormSubmissionViewModel
            {
                FirstName = submission.FirstName,
                SurName = submission.SurName,
                Email = submission.Email,
                PhoneNumber = submission.PhoneNumber,
                DateOfBirth = submission.DateOfBirth,
                ProductSerialNumber = submission.ProductSerialNumber
            });

            int pageSize = 10;
            int pageNumber = page ?? 1;
            return View("FormSubmissions", submissions.OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToPagedList(pageNumber, pageSize));
        }
    }
}
