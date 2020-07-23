using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    /// <summary>
    /// Performs operations on Project entities.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project, Guid userId)
        {
            Project mappedProject = _mapper.Map<Project>(project);
            Project createdProject = _unitOfWork.Projects.AddProject(mappedProject);
            await _unitOfWork.SaveAsync();
            await SetProjectManagerById(createdProject.Id, userId, createdProject);
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task<bool> UpdateProjectAsync(ProjectDto project)
        {
            if (await _unitOfWork.Projects.GetProjectByIdAsync((Guid)project.Id) == null)
            {
                return false;
            }

            var projectToUpdate = _mapper.Map<Project>(project);
            _unitOfWork.Projects.UpdateProject(projectToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveProjectAsync(Guid projectId)
        {
            var projectToRemove = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if (projectToRemove == null)
            {
                return false;
            }

            _unitOfWork.Projects.RemoveProject(projectToRemove);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            IEnumerable<Project> projects = await _unitOfWork.Projects.GetAllProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
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
        /// <returns></returns>
        public async Task<bool> SetProjectManagerById(Guid projectId, Guid userId, Project createdProject = null)
        {
            var project = createdProject ?? await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            var user = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userId);
            if (project == null || user == null)
            {
                return false;
            }

            project.ManagerId = userId;
            user.ProjectId = projectId;
            _unitOfWork.Projects.UpdateProject(project);
            _unitOfWork.UserProfiles.UpdateUserProfile(user);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<UserProfileDto> GetProjectManagerById(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return null;
            }

            UserProfile user = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync((Guid)project.ManagerId);
            return _mapper.Map<UserProfileDto>(user);
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