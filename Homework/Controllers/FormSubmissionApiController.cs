using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Homework.Business.Models;
using Homework.Business.Repositories;

namespace Homework.Controllers
{
    public class FormSubmissionApiController : ApiController
    {
        private readonly ISubmissionsRepository submissionsRepository;
        private readonly IDrawRepository drawRepository;

        public FormSubmissionApiController(ISubmissionsRepository submissionsRepository, IDrawRepository drawRepository)
        {
            this.submissionsRepository = submissionsRepository;
            this.drawRepository = drawRepository;
        }

        [HttpPost]
        public async Task<JsonResult<SuccessResponse>> Submit(FormSubmissionViewModel model)
        {
            if (!ModelState.IsValid || !model.DateOfBirth.HasValue || !model.ProductSerialNumber.HasValue)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Model is not valid"),
                    ReasonPhrase = "Model is not valid"
                });

            var validSerial = drawRepository.IsValidDrawProductSerialNumber(model.ProductSerialNumber.Value);

            if(!await validSerial)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Supplied product serial number not valid"),
                    ReasonPhrase = "Supplied product serial number not valid"
                });

            var validSubmissionAmount = submissionsRepository.GetTotalSubmissionCountForProductSerial(model.ProductSerialNumber.Value);

            if(await validSubmissionAmount == 2)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Maximum allowed submissions surpassed"),
                    ReasonPhrase = "Maximum allowed submissions surpassed"
                });

            await submissionsRepository.AddSubmissionAsync(model);

            return Json(new SuccessResponse(model.FirstName));
        }

        public class SuccessResponse
        {
            public string Message { get; set; }

            public SuccessResponse(string name)
            {
                Message = $"Thank you for your submission, best of luck {name}";
            }
        }
    }
}
