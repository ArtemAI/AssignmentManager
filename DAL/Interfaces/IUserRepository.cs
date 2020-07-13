using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        UserProfile Add(UserProfile item);
        void Update(UserProfile item);
        void Remove(UserProfile item);

        Task<UserProfile> GetByIdAsync(Guid userProfileId);
        Task<IEnumerable<UserProfile>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<UserProfile>> GetAllAsync();
    }
}