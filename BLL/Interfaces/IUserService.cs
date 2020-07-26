using BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfile);
        Task<bool> UpdateUserAsync(UserProfileDto userProfile);
        Task<UserProfileDto> GetUserByIdAsync(Guid userProfileId);
        Task<IEnumerable<UserProfileDto>> GetAllUsersAsync();
        Task<bool> AddUserToProject(Guid userId, Guid projectId);
        Task<bool> RemoveUserFromProject(Guid userId);
        Task<bool> SetUserRoleAsync(Guid userId, string role);
        Task<List<string>> GetAllRoleNamesAsync();
    }
}