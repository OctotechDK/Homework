using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Homework.Business.Models;
using Homework.Business.Repositories;
using PagedList;

namespace Homework.Controllers
{
    [Authorize]
    public class FormSubmissionController : Controller
    {
        private const int PageSize = 10;
        private readonly ISubmissionsRepository submissionsRepository;

        public FormSubmissionController(ISubmissionsRepository submissionsRepository)
        {
            this.submissionsRepository = submissionsRepository;
        }

        public async Task<ViewResult> Index(int? page)
        {
            ViewBag.Title = "Form Submissions";

            var submissions = (await submissionsRepository.GetAllSubmissionsAsync())
                .Select(submission => new FormSubmissionViewModel
                {
                    FirstName = submission.FirstName,
                    SurName = submission.SurName,
                    Email = submission.Email,
                    PhoneNumber = submission.PhoneNumber,
                    DateOfBirth = submission.DateOfBirth,
                    ProductSerialNumber = submission.ProductSerialNumber
                });

            var pageNumber = page ?? 1;
            return View("FormSubmissions", submissions.OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToPagedList(pageNumber, PageSize));
        }
    }
}
