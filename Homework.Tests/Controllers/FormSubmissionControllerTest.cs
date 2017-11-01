using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using Homework.Business.Models;
using Homework.Business.Repositories;
using Homework.Controllers;
using NSubstitute;
using NUnit.Framework;
using PagedList;

namespace Homework.Tests.Controllers
{
    [TestFixture]
    public class FormSubmissionControllerTest
    {
        [Test(Description = "Get submissions page 1 with 0 submissions")]
        public async Task IndexZeroSubmissionsAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();

            submissionRepository.GetAllSubmissionsAsync().Returns(Enumerable.Empty<FormSubmissionModel>());

            var controller = new FormSubmissionController(submissionRepository);

            var result = await controller.Index(null);

            result.Should().NotBeNull();
            result.Model.As<IPagedList<FormSubmissionViewModel>>().Count.Should().Be(0);
        }

        [Test(Description = "Get submissions page 1 with 2 submissions")]
        public async Task IndexTwoSubmissionsAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();

            var submissions = new List<FormSubmissionModel>
            {
                new FormSubmissionModel
                {
                    FirstName = "Nicolai",
                    SurName = "Oksen",
                    Email = "nicolai@oksen.com",
                    PhoneNumber = "12345678",
                    DateOfBirth = DateTimeOffset.UtcNow,
                    ProductSerialNumber = Guid.NewGuid()
                },
                new FormSubmissionModel
                {
                    FirstName = "Another",
                    SurName = "Person",
                    Email = "nicolai@oksen.com",
                    PhoneNumber = "12345678",
                    DateOfBirth = DateTimeOffset.UtcNow,
                    ProductSerialNumber = Guid.NewGuid()
                }
            };

            submissionRepository.GetAllSubmissionsAsync().Returns(submissions);

            var controller = new FormSubmissionController(submissionRepository);

            var result = await controller.Index(null);

            result.Should().NotBeNull();
            result.Model.As<IPagedList<FormSubmissionViewModel>>().Count.Should().Be(2);
            result.Model.As<IPagedList<FormSubmissionViewModel>>().First().FirstName.ShouldBeEquivalentTo("Nicolai");
        }

        [Test(Description = "Get submissions page 2 with 12 submissions")]
        public async Task IndexTwoSubmissionsPageTwoAsync()
        {
            var submissionRepository = Substitute.For<ISubmissionsRepository>();

            var submissions = new List<FormSubmissionModel>();
            for (var i = 0; i < 12; i++)
            {
                submissions.Add(new FormSubmissionModel
                {
                    FirstName = $"Person{i.ToString().PadLeft(2,'0')}",
                    SurName = "Oksen",
                    Email = "nicolai@oksen.com",
                    PhoneNumber = "12345678",
                    DateOfBirth = DateTimeOffset.UtcNow,
                    ProductSerialNumber = Guid.NewGuid()
                });
            }

            submissionRepository.GetAllSubmissionsAsync().Returns(submissions);

            var controller = new FormSubmissionController(submissionRepository);

            var result = await controller.Index(2);

            result.Should().NotBeNull();
            result.Model.As<IPagedList<FormSubmissionViewModel>>().Count.Should().Be(2);
            result.Model.As<IPagedList<FormSubmissionViewModel>>().First().FirstName.ShouldBeEquivalentTo("Person10");
        }
    }
}
