using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IProjectRepository
    {
        Project Add(Project item);
        void Update(Project item);
        void Remove(Project item);

        Task<Project> GetByIdAsync(Guid projectId);
        Task<IEnumerable<Project>> GetAllAsync();
    }
}