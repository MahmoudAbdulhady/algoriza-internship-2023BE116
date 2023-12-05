using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Repositories
{
    public class SpecailizationRepoistory : ISpecializationRepository
    {
        private readonly VeeztaDbContext _veeztaDbContext;


        public SpecailizationRepoistory(VeeztaDbContext veeztaDbContext)
        {
            _veeztaDbContext = veeztaDbContext;
        }

        public async Task AddAsync(Specialization specialization)
        {
            await _veeztaDbContext.Specializations.AddAsync(specialization);
            await _veeztaDbContext.SaveChangesAsync();
        }

        public async Task<Specialization> GetByIdAsync(int id)
        {
            return await _veeztaDbContext.Specializations.FindAsync(id);
        }

        public  async Task<Specialization> GetByNameAsync(string name)
        {
            return await _veeztaDbContext.Specializations
                              .FirstOrDefaultAsync(s => s.SpecializationName == name);
        }
    }
}
