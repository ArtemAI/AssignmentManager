using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Session _session;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, SessionProvider sessionProvider)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _session = sessionProvider.Session;
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

        /// <summary>
        /// Gets the list of user profiles based on current user's role.
        /// </summary>
        /// <returns>Sequence of user profiles.</returns>
        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            ApplicationUser currentUser = _session.User;
            if(await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            {
                IEnumerable<UserProfile> userProfiles = await _unitOfWork.UserProfiles.GetAllUserProfilesAsync();
                return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);
            }

            var currentUserProfile = await GetUserByIdAsync(currentUser.Id);
            var projectUserProfiles =
                await _unitOfWork.UserProfiles.GetUserProfilesByProjectIdAsync(currentUserProfile.ProjectId);
            return _mapper.Map<IEnumerable<UserProfileDto>>(projectUserProfiles);
        }

        /// <summary>
        /// Adds user to selected project.
        /// </summary>
        /// <param name="userId">User identifier in the form of a GUID string.</param>
        /// <param name="projectId">Project identifier in the form of a GUID string.</param>
        /// <returns>True if operation is successful, false otherwise.</returns>
        public async Task<bool> AddUserToProject(Guid userId, Guid projectId)
        {
            UserProfile userToUpdate = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userId);
            if (userToUpdate == null)
            {
                return false;
            }

            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if(project == null)
            {
                return false;
            }

            ApplicationUser currentUser = _session.User;
            if (await _userManager.IsInRoleAsync(currentUser, "Administrator") == false)
            {
                var currentUserProfile = await GetUserByIdAsync(currentUser.Id);
                if (currentUserProfile.ProjectId != projectId)
                {
                    return false;
                }

                if (await _userManager.IsInRoleAsync(currentUser, "Manager") == false)
                {
                    return false;
                }
            }

            userToUpdate.ProjectId = projectId;
            _unitOfWork.UserProfiles.UpdateUserProfile(userToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Removes user from the project.
        /// </summary>
        /// <param name="userId">User identifier in the form of a GUID string.</param>
        /// <returns>True if operation is successful, false otherwise.</returns>
        public async Task<bool> RemoveUserFromProject(Guid userId)
        {
            UserProfile userToUpdate = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userId);
            if(userToUpdate == null)
            {
                return false;
            }

            ApplicationUser currentUser = _session.User;
            if (await _userManager.IsInRoleAsync(currentUser, "Administrator") == false)
            {
                var currentUserProfile = await GetUserByIdAsync(currentUser.Id);
                if (currentUserProfile.ProjectId != userToUpdate.ProjectId)
                {
                    return false;
                }

                if (await _userManager.IsInRoleAsync(currentUser, "Manager") == false && userId != currentUser.Id)
                {
                    return false;
                }
            }

            userToUpdate.ProjectId = null;
            _unitOfWork.UserProfiles.UpdateUserProfile(userToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Adds user to specified role.
        /// </summary>
        /// <param name="userId">User identifier in the form of a GUID string.</param>
        /// <param name="role">Name of the role.</param>
        /// <returns>True if operation is successful, false otherwise.</returns>
        public async Task<bool> SetUserRoleAsync(Guid userId, string role)
        {
            ApplicationUser currentUser = _session.User;
            if (userId == currentUser.Id)
            {
                return false;
            }

            if (role == "Administrator" && await _userManager.IsInRoleAsync(currentUser, "Administrator") == false)
            {
                return false;
            }

            ApplicationUser userToSetRole = await _userManager.FindByIdAsync(userId.ToString());
            if (userToSetRole == null || await _roleManager.FindByNameAsync(role) == null)
            {
                return false;
            }

            var rolesToRemoveFrom = await _userManager.GetRolesAsync(userToSetRole);
            await _userManager.RemoveFromRolesAsync(userToSetRole, rolesToRemoveFrom);
            await _userManager.AddToRoleAsync(userToSetRole, role);
            return true;
        }

        public Task<List<string>> GetAllRoleNamesAsync()
        {
            var roles = _roleManager.Roles;
            return roles.Select(x => x.Name).ToListAsync();
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