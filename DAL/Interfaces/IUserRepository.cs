using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        UserProfile AddUserProfile(UserProfile item);
        void UpdateUserProfile(UserProfile item);

        Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId);
        Task<IEnumerable<UserProfile>> GetUserProfileByProjectIdAsync(Guid projectId);
        Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync();
    }
}