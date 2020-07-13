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
    /// Performs project operations.
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

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project)
        {
            Project mappedProject = _mapper.Map<Project>(project);
            Project createdProject = _unitOfWork.Projects.Add(mappedProject);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task RemoveProjectAsync(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            _unitOfWork.Projects.Remove(project);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProjectAsync(ProjectDto project)
        {
            Project mappedProject = _mapper.Map<Project>(project);
            _unitOfWork.Projects.Update(mappedProject);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            IEnumerable<Project> projects = await _unitOfWork.Projects.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task SetProjectManagerById(Guid projectId, Guid userId)
        {
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            UserProfile user = await _unitOfWork.UserProfiles.GetByIdAsync(userId);

            project.ManagerId = userId;
            user.ProjectId = projectId;

            _unitOfWork.Projects.Update(project);
            _unitOfWork.UserProfiles.Update(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserProfileDto> GetProjectManagerById(Guid projectId)
        {
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);

            if (project.ManagerId == null)
            {
                return null;
            }

            UserProfile user = await _unitOfWork.UserProfiles.GetByIdAsync((Guid) project.ManagerId);
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