using System;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository pattern implementation that allows to perform CRUD operations and select UserProfile records.
    /// </summary>
    class UserRepository : IUserRepository
    {
        private readonly AssignmentManagerContext _dbContext;
        private readonly DbSet<UserProfile> _dbSet;

        public UserRepository(AssignmentManagerContext context)
        {
            _dbContext = context;
            _dbSet = context.UserProfiles;
        }

        public UserProfile Add(UserProfile item)
        {
            _dbSet.Add(item);
            return item;
        }

        public void Update(UserProfile item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Remove(UserProfile item)
        {
            _dbSet.Remove(item);
        }

        public async Task<UserProfile> GetByIdAsync(Guid userProfileId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userProfileId);
        }

        public async Task<IEnumerable<UserProfile>> GetByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}