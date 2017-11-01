using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Homework.Business.Models;

namespace Homework.Business.Repositories
{
    public class DrawRepository : IDrawRepository
    {
        private readonly IApplicationDbContext context;

        public DrawRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsValidDrawProductSerialNumber(Guid productSerialNumber)
        {
            return await context.DrawSerialNumbers.AnyAsync(x => x.Id == productSerialNumber);
        }
    }
}
