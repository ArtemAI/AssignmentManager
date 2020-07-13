﻿using BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfile);
        Task UpdateUserAsync(UserProfileDto userProfile);

        Task<UserProfileDto> GetUserByIdAsync(Guid userProfileId);
        Task<IEnumerable<UserProfileDto>> GetUserByProjectIdAsync(Guid projectId);
        Task<IEnumerable<UserProfileDto>> GetAllUsersAsync();
        Task SetUserRole(Guid userId, string role);
    }
}