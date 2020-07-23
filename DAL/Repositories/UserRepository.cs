using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository pattern implementation that allows to perform CRUD operations and select UserProfile records.
    /// </summary>
    internal class UserRepository : IUserRepository
    {
        private readonly AssignmentManagerContext _dbContext;
        private readonly DbSet<UserProfile> _dbSet;

        public UserRepository(AssignmentManagerContext context)
        {
            _dbContext = context;
            _dbSet = context.UserProfiles;
        }

        public UserProfile AddUserProfile(UserProfile item)
        {
            _dbSet.Add(item);
            return item;
        }

        public void UpdateUserProfile(UserProfile item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userProfileId);
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfileByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}