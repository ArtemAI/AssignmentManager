using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IProjectService : IDisposable
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto project);
        Task<bool> UpdateProjectAsync(ProjectDto project);
        Task<bool> RemoveProjectAsync(Guid projectId);
        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<bool> SetProjectManagerAsync(Guid projectId, Guid userId, Project createdProject = null);
    }
}