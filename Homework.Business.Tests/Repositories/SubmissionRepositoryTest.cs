using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Homework.Business.Models;
using Homework.Business.Repositories;
using Homework.Business.Tests.TestingUtils;
using NSubstitute;
using NUnit.Framework;

namespace Homework.Business.Tests.Repositories
{
    [TestFixture]
    public class SubmissionRepositoryTest
    {
        [Test(Description = "Get all submission when empty")]
        public async Task TestGetAllSubmissionsNoSubmissionAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>().AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);

            var result = await repository.GetAllSubmissionsAsync();

            result.Should().BeEmpty();
        }

        [Test(Description = "Get two submissions")]
        public async Task TestGetAllSubmissionsTwoSubmissionAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>
            {
                new FormSubmissionModel
                {
                    FirstName = "Nicolai"
                },
                new FormSubmissionModel
                {
                    FirstName = "Niels"
                }
            }.AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);

            var result = await repository.GetAllSubmissionsAsync();

            result.Should().NotBeEmpty();
            result.Count().ShouldBeEquivalentTo(2);
            result.First().FirstName.ShouldBeEquivalentTo("Nicolai");
            result.Last().FirstName.ShouldBeEquivalentTo("Niels");
        }

        [Test(Description = "Get count of 0 when supplying invalid serial")]
        public async Task TestGetTotalSubmissionCountForProductSerialNoMatchAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>
            {
                new FormSubmissionModel
                {
                    FirstName = "Nicolai"
                },
                new FormSubmissionModel
                {
                    FirstName = "Niels"
                }
            }.AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);

            var result = await repository.GetTotalSubmissionCountForProductSerial(Guid.NewGuid());

            result.ShouldBeEquivalentTo(0);
        }

        [Test(Description = "Get count of 2 when supplying valid serial")]
        public async Task TestGetTotalSubmissionCountForProductSerialTwoMatchesAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var serial = Guid.NewGuid();
            var data = new List<FormSubmissionModel>
            {
                new FormSubmissionModel
                {
                    FirstName = "Nicolai", ProductSerialNumber = serial
                },
                new FormSubmissionModel
                {
                    FirstName = "Niels", ProductSerialNumber = serial
                }
            }.AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);

            var result = await repository.GetTotalSubmissionCountForProductSerial(serial);

            result.ShouldBeEquivalentTo(2);
        }

        [Test(Description = "Get NullReference Exception when supplying invalid Date of Birth")]
        public void TestAddSubmissionInvalidDoBAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>().AsQueryable();
            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            var submission = new FormSubmissionViewModel();

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);


            repository.Awaiting(async x => await x.AddSubmissionAsync(submission))
                .ShouldThrow<NullReferenceException>()
                .WithMessage("No Date of Birth provided, value is null");
        }

        [Test(Description = "Get NullReference Exception when supplying invalid Product Serial Number")]
        public void TestAddSubmissionInvalidProductSerialAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>().AsQueryable();
            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            var submission = new FormSubmissionViewModel
            {
                DateOfBirth = DateTimeOffset.Now
            };

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);


            repository.Awaiting(async x => await x.AddSubmissionAsync(submission))
                .ShouldThrow<NullReferenceException>()
                .WithMessage("No product serial number provided, value is null");
        }

        [Test]
        public async Task TestAddSubmissionSuccessAsync()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<FormSubmissionModel>().AsQueryable();
            var mockSet = NSubstituteUtils.CreateMockDbSet(data);
            var serial = Guid.NewGuid();
            var submission = new FormSubmissionViewModel
            {
                FirstName = "Nicolai",
                SurName = "Oksen",
                Email = "nicolai@oksen.com",
                PhoneNumber = "12345678",
                DateOfBirth = DateTimeOffset.Now,
                ProductSerialNumber = serial
            };

            dbContext.Submissions.Returns(mockSet);

            var repository = new SubmissionsRepository(dbContext);

            await repository.AddSubmissionAsync(submission);

            mockSet.Received(1).Add(Arg.Any<FormSubmissionModel>());
            await dbContext.Received(1).SaveChangesAsync();
        }
    }
}
