using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IProjectService : IDisposable
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto project, Guid userId);
        Task UpdateProjectAsync(ProjectDto project);
        Task RemoveProjectAsync(Guid projectId);

        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<bool> SetProjectManagerById(Guid projectId, Guid userId, Project createdProject = null);
        Task<UserProfileDto> GetProjectManagerById(Guid projectId);
    }
}