using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    /// <summary>
    /// Performs operations on UserProfile entities.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto user)
        {
            UserProfile userToCreate = _mapper.Map<UserProfile>(user);
            UserProfile createdUser = _unitOfWork.UserProfiles.AddUserProfile(userToCreate);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserProfileDto>(createdUser);
        }

        public async Task<bool> UpdateUserAsync(UserProfileDto user)
        {
            if (await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(user.Id) == null)
            {
                return false;
            }

            UserProfile userToUpdate = _mapper.Map<UserProfile>(user);
            _unitOfWork.UserProfiles.UpdateUserProfile(userToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<UserProfileDto> GetUserByIdAsync(Guid userProfileId)
        {
            UserProfile user = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userProfileId);
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<IEnumerable<UserProfileDto>> GetUserByProjectIdAsync(Guid projectId)
        {
            IEnumerable<UserProfile> projectUserProfiles =
                await _unitOfWork.UserProfiles.GetUserProfileByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<UserProfileDto>>(projectUserProfiles);
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            IEnumerable<UserProfile> userProfiles = await _unitOfWork.UserProfiles.GetAllUserProfilesAsync();
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);
        }

        public async Task<bool> SetUserRole(Guid userId, string role)
        {
            var userToSetRole = await _userManager.FindByIdAsync(userId.ToString());
            if (userToSetRole == null)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(userToSetRole, role);
            return true;
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}