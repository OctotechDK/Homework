using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Homework.Business.Models;

namespace Homework.Business.Repositories
{
    public class SubmissionsRepository : ISubmissionsRepository
    {
        private readonly IApplicationDbContext context;

        public SubmissionsRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FormSubmissionModel>> GetAllSubmissionsAsync()
        {
            return await context.Submissions.ToListAsync();
        }

        public async Task<int> GetTotalSubmissionCountForProductSerial(Guid productSerialNumber)
        {
            return await context.Submissions.CountAsync(x => x.ProductSerialNumber == productSerialNumber);
        }

        public async Task AddSubmissionAsync(FormSubmissionViewModel submission)
        {
            if(!submission.DateOfBirth.HasValue)
                throw new NullReferenceException("No Date of Birth provided, value is null");

            if(!submission.ProductSerialNumber.HasValue)
                throw new NullReferenceException("No product serial number provided, value is null");

            context.Submissions.Add(new FormSubmissionModel
            {
                Id = Guid.NewGuid(),
                FirstName = submission.FirstName,
                SurName = submission.SurName,
                Email = submission.Email,
                PhoneNumber = submission.PhoneNumber,
                DateOfBirth = submission.DateOfBirth.Value,
                ProductSerialNumber = submission.ProductSerialNumber.Value
            });

            await context.SaveChangesAsync();
        }
    }
}
