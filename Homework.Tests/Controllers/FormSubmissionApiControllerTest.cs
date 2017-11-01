using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Homework.Business.Models;
using Homework.Business.Repositories;
using Homework.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace Homework.Tests.Controllers
{
    [TestFixture]
    public class FormSubmissionApiControllerTest
    {
        [Test(Description = "Submit invalid model for draw with invalid Product serial and Date of birth")]
        public void SubmitModelStateInvalidAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();
            var drawRepository = Substitute.For<IDrawRepository>();
            var controller = new FormSubmissionApiController(submissionRepository, drawRepository);

            var model = new FormSubmissionViewModel
            {
                FirstName = "Nicolai",
                SurName = "Oksen",
                Email = "nicolai@oksen.com",
                PhoneNumber = "12345678",
                DateOfBirth = null,
                ProductSerialNumber = null
            };

            controller.Awaiting(async y => await y.Submit(model))
                .ShouldThrow<HttpResponseException>()
                .Where(x => x.Response.ReasonPhrase.Equals("Model is not valid")
                    && x.Response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test(Description = "Submit model with invalid product serial number")]
        public void SubmitInvalidProductSerialAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();
            var drawRepository = Substitute.For<IDrawRepository>();

            var serial = Guid.NewGuid();
            drawRepository.IsValidDrawProductSerialNumber(serial).ReturnsForAnyArgs(false);

            var controller = new FormSubmissionApiController(submissionRepository, drawRepository);

            var model = new FormSubmissionViewModel
            {
                FirstName = "Nicolai",
                SurName = "Oksen",
                Email = "nicolai@oksen.com",
                PhoneNumber = "12345678",
                DateOfBirth = DateTimeOffset.Now,
                ProductSerialNumber = serial
            };

            controller.Awaiting(async y => await y.Submit(model))
                .ShouldThrow<HttpResponseException>()
                .Where(x => x.Response.ReasonPhrase.Equals("Supplied product serial number not valid")
                    && x.Response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test(Description = "Submit model with too many submission for given product serial")]
        public void SubmitTooManySubmissionsAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();
            var drawRepository = Substitute.For<IDrawRepository>();

            var serial = Guid.NewGuid();
            drawRepository.IsValidDrawProductSerialNumber(serial).ReturnsForAnyArgs(true);
            submissionRepository.GetTotalSubmissionCountForProductSerial(serial).Returns(2);

            var controller = new FormSubmissionApiController(submissionRepository, drawRepository);

            var model = new FormSubmissionViewModel
            {
                FirstName = "Nicolai",
                SurName = "Oksen",
                Email = "nicolai@oksen.com",
                PhoneNumber = "12345678",
                DateOfBirth = DateTimeOffset.Now,
                ProductSerialNumber = serial
            };

            controller.Awaiting(async y => await y.Submit(model))
                .ShouldThrow<HttpResponseException>()
                .Where(x => x.Response.ReasonPhrase.Equals("Maximum allowed submissions surpassed")
                            && x.Response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test(Description = "Submit model with too many submission for given product serial")]
        public async Task SubmitSuccessfullAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();
            var drawRepository = Substitute.For<IDrawRepository>();

            var serial = Guid.NewGuid();
            drawRepository.IsValidDrawProductSerialNumber(serial).ReturnsForAnyArgs(true);
            submissionRepository.GetTotalSubmissionCountForProductSerial(serial).Returns(0);

            var controller = new FormSubmissionApiController(submissionRepository, drawRepository);

            var model = new FormSubmissionViewModel
            {
                FirstName = "Nicolai",
                SurName = "Oksen",
                Email = "nicolai@oksen.com",
                PhoneNumber = "12345678",
                DateOfBirth = DateTimeOffset.Now,
                ProductSerialNumber = serial
            };

            var result = await controller.Submit(model);

            result.Should().NotBeNull();
            result.Content.Message.ShouldBeEquivalentTo("Thank you for your submission, best of luck Nicolai");
        }
    }
}
