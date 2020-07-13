using System;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    /// <summary>
    /// Performs UserProfile operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto user)
        {
            UserProfile mappedUser = _mapper.Map<UserProfile>(user);
            UserProfile createdUser = _unitOfWork.UserProfiles.Add(mappedUser);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserProfileDto>(createdUser);
        }

        public async Task RemoveUserAsync(Guid userId)
        {
            UserProfile user = await _unitOfWork.UserProfiles.GetByIdAsync(userId);
            _unitOfWork.UserProfiles.Remove(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUserAsync(UserProfileDto user)
        {
            UserProfile mappedUser = _mapper.Map<UserProfile>(user);
            _unitOfWork.UserProfiles.Update(mappedUser);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserProfileDto> GetUserByIdAsync(Guid userProfileId)
        {
            UserProfile user = await _unitOfWork.UserProfiles.GetByIdAsync(userProfileId);
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<IEnumerable<UserProfileDto>> GetUserByProjectIdAsync(Guid projectId)
        {
            IEnumerable<UserProfile> userProfiles = await _unitOfWork.UserProfiles.GetByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            IEnumerable<UserProfile> userProfiles = await _unitOfWork.UserProfiles.GetAllAsync();
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);
        }

        public async Task SetUserRole(Guid userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.AddToRoleAsync(user, role);
        }

        #region IDisposable Support
        private bool _disposedValue = false;

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