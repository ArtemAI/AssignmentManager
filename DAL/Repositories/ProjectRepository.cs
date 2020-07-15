using System;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository pattern implementation that allows to perform CRUD operations and select Project records.
    /// </summary>
    class ProjectRepository : IProjectRepository
    {
        private readonly AssignmentManagerContext _dbContext;
        private readonly DbSet<Project> _dbSet;

        public ProjectRepository(AssignmentManagerContext context)
        {
            _dbContext = context;
            _dbSet = context.Projects;
        }

        public Project AddProject(Project item)
        {
            _dbSet.Add(item);
            return item;
        }

        public void UpdateProject(Project item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void RemoveProject(Project item)
        {
            _dbSet.Remove(item);
        }

        public async Task<Project> GetProjectByIdAsync(Guid projectId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}