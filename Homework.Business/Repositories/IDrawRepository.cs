using System;
using System.Threading.Tasks;

namespace Homework.Business.Repositories
{
    public interface IDrawRepository
    {
        Task<bool> IsValidDrawProductSerialNumber(Guid productSerialNumber);
    }
}