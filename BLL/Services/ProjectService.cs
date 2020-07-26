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
    /// Performs operations on Project entities.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Session _session;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager, SessionProvider sessionProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _session = sessionProvider.Session;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project)
        {
            ApplicationUser currentUser = _session.User;
            Project projectToCreate = _mapper.Map<Project>(project);
            Project createdProject = _unitOfWork.Projects.AddProject(projectToCreate);
            await _unitOfWork.SaveAsync();
            await SetProjectManagerAsync(createdProject.Id, currentUser.Id, createdProject);
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task<bool> UpdateProjectAsync(ProjectDto project)
        {
            if (await _unitOfWork.Projects.GetProjectByIdAsync(project.Id.Value) == null)
            {
                return false;
            }

            Project projectToUpdate = _mapper.Map<Project>(project);
            _unitOfWork.Projects.UpdateProject(projectToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveProjectAsync(Guid projectId)
        {
            Project projectToRemove = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if (projectToRemove == null)
            {
                return false;
            }

            _unitOfWork.Projects.RemoveProject(projectToRemove);
            await _unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Gets the list of projects based on current user's role.
        /// </summary>
        /// <returns>Sequence of projects.</returns>
        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            ApplicationUser currentUser = _session.User;
            if (await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            {
                var projects = await _unitOfWork.Projects.GetAllProjectsAsync();
                return _mapper.Map<IEnumerable<ProjectDto>>(projects);
            }

            var userProjects = new List<ProjectDto>();
            UserProfile currentUserProfile = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(currentUser.Id);
            if(currentUserProfile.ProjectId.HasValue)
            {
                var project = await GetProjectByIdAsync(currentUserProfile.ProjectId.Value);
                userProjects.Add(project);
            }
            
            return userProjects;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(project);
        }

        /// <summary>
        /// Sets the manager of specified project.
        /// The project entity has to be provided as third parameter in case it was just created.
        /// </summary>
        /// <param name="projectId">A project identifier in the form of a GUID string.</param>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <param name="createdProject">A project entity (optional).</param>
        /// <returns>True if operation is successful, false otherwise.</returns>
        public async Task<bool> SetProjectManagerAsync(Guid projectId, Guid userId, Project createdProject = null)
        {
            ApplicationUser currentUser = _session.User;
            UserProfile userToUpdate = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userId);
            Project projectToUpdate = createdProject ?? await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if (userToUpdate == null || projectToUpdate == null)
            {
                return false;
            }

            UserProfile currentManager = projectToUpdate.Manager;
            if (currentManager != null)
            {
                if (!await _userManager.IsInRoleAsync(currentUser, "Administrator") && currentManager.Id != currentUser.Id)
                {
                    return false;
                }
            }

            projectToUpdate.ManagerId = userId;
            userToUpdate.ProjectId = projectId;
            _unitOfWork.Projects.UpdateProject(projectToUpdate);
            _unitOfWork.UserProfiles.UpdateUserProfile(userToUpdate);
            await _unitOfWork.SaveAsync();
            await _userService.SetUserRoleAsync(userId, "Manager");
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