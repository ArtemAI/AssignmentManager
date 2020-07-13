using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IProjectService : IDisposable
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto project);
        Task UpdateProjectAsync(ProjectDto project);
        Task RemoveProjectAsync(Guid projectId);
        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task SetProjectManagerById(Guid projectId, Guid userId);
        Task<UserProfileDto> GetProjectManagerById(Guid projectId);
    }
}