using System;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Performs operations on Project entities.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project, Guid creatorId)
        {
            Project mappedProject = _mapper.Map<Project>(project);
            Project createdProject = _unitOfWork.Projects.AddProject(mappedProject);
            await SetProjectManagerById(createdProject.Id, creatorId);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task RemoveProjectAsync(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            _unitOfWork.Projects.RemoveProject(project);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProjectAsync(ProjectDto project)
        {
            Project mappedProject = _mapper.Map<Project>(project);
            _unitOfWork.Projects.UpdateProject(mappedProject);
            await _unitOfWork.SaveAsync();
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

        public async Task SetProjectManagerById(Guid projectId, Guid userId)
        {
            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            UserProfile user = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(userId);
            project.ManagerId = userId;
            user.ProjectId = projectId;
            _unitOfWork.Projects.UpdateProject(project);
            _unitOfWork.UserProfiles.UpdateUserProfile(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserProfileDto> GetProjectManagerById(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetProjectByIdAsync(projectId);
            if (project.ManagerId == null)
            {
                return null;
            }
            UserProfile user = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync((Guid) project.ManagerId);
            return _mapper.Map<UserProfileDto>(user);
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