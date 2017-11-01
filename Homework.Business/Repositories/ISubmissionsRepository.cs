using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Business.Models;

namespace Homework.Business.Repositories
{
    public interface ISubmissionsRepository
    {
        Task<IEnumerable<FormSubmissionModel>> GetAllSubmissionsAsync();

        Task<int> GetTotalSubmissionCountForProductSerial(Guid productSerialNumber);

        Task AddSubmissionAsync(FormSubmissionViewModel submission);
    }
}