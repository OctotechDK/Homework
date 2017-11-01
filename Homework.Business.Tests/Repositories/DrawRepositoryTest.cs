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
    public class DrawRepositoryTest
    {
        [Test(Description = "Test invalid product serial number")]
        public async Task TestInvalidProductSerial()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var data = new List<DrawSerialNumberModel>
            {
                new DrawSerialNumberModel
                {
                    Id = Guid.NewGuid()
                }
            }.AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.DrawSerialNumbers.Returns(mockSet);

            var repository = new DrawRepository(dbContext);

            var result = await repository.IsValidDrawProductSerialNumber(Guid.NewGuid());

            result.Should().BeFalse();
        }

        [Test(Description = "Test invalid product serial number")]
        public async Task TestValidProductSerial()
        {
            var dbContext = Substitute.For<IApplicationDbContext>();
            var serial = Guid.NewGuid();
            var data = new List<DrawSerialNumberModel>
            {
                new DrawSerialNumberModel
                {
                    Id = serial
                }
            }.AsQueryable();

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);

            dbContext.DrawSerialNumbers.Returns(mockSet);

            var repository = new DrawRepository(dbContext);

            var result = await repository.IsValidDrawProductSerialNumber(serial);

            result.Should().BeTrue();
        }
    }
}
