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

        /// <summary>
        /// Asynchronously adds a new specialization to the database.
        /// </summary>
        /// <param name="specialization">The specialization object to add to the database.</param>
        /// <remarks>
        /// This method adds the specified specialization object to the database and saves the changes asynchronously.
        /// It assumes that the 'specialization' parameter is not null and the database context (_veeztaDbContext) is properly configured.
        /// </remarks>
        public async Task AddAsync(Specialization specialization)
        {
            await _veeztaDbContext.Specializations.AddAsync(specialization);
            await _veeztaDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a specialization from the database by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the specialization to be retrieved.</param>
        /// <returns>
        /// The specialization object with the specified ID if found, or null if no specialization matches the ID.
        /// </returns>
        /// <remarks>
        /// This method searches for a specialization in the database based on the provided 'id'.
        /// If a matching specialization is found, it is returned as the result; otherwise, null is returned.
        /// </remarks>
        public async Task<Specialization> GetByIdAsync(int id)
        {
            return await _veeztaDbContext.Specializations.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves a specialization from the database by its name.
        /// </summary>
        /// <param name="name">The name of the specialization to be retrieved.</param>
        /// <returns>
        /// The specialization object with the specified name if found, or null if no specialization matches the name.
        /// </returns>
        /// <remarks>
        /// This method queries the database to find a specialization based on the provided 'name'.
        /// If a matching specialization is found (based on specialization name), it is returned as the result; otherwise, null is returned.
        /// </remarks>
        public async Task<Specialization> GetByNameAsync(string name)
        {
            return await _veeztaDbContext.Specializations
                              .FirstOrDefaultAsync(s => s.SpecializationName == name);
        }
    }
}
