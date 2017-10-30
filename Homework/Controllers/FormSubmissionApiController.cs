using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Homework.Models;

namespace Homework.Controllers
{
    public class FormSubmissionApiController : ApiController
    {
        private readonly ApplicationDbContext context; 

        public FormSubmissionApiController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult<SuccessResponse>> Submit(FormSubmissionViewModel model)
        {
            if (!ModelState.IsValid || !model.DateOfBirth.HasValue || !model.ProductSerialNumber.HasValue)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Model is not valid"),
                    ReasonPhrase = "Model is not valid"
                });

            var validSerial = context.DrawSerialNumbers.AnyAsync(x => x.Id == model.ProductSerialNumber);

            if(!await validSerial)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Supplied product serial number not valid"),
                    ReasonPhrase = "Supplied product serial number not valid"
                });

            var validSubmissionAmount = context.Submissions.CountAsync(x => x.ProductSerialNumber == model.ProductSerialNumber);

            if(await validSubmissionAmount == 2)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Maximum allowed submissions surpassed"),
                    ReasonPhrase = "Maximum allowed submissions surpassed"
                });

            context.Submissions.Add(new FormSubmissionModel
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                SurName = model.SurName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth.Value,
                ProductSerialNumber = model.ProductSerialNumber.Value
            });

            try
            {
                var save = context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

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
