using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<Specialization> GetByIdAsync(int id);
        Task<Specialization> GetByNameAsync(string name);
        Task AddAsync(Specialization specialization);
    }
}
