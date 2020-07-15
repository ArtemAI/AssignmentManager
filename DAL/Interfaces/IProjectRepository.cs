using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IProjectRepository
    {
        Project AddProject(Project item);
        void UpdateProject(Project item);
        void RemoveProject(Project item);

        Task<Project> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}