using System;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository pattern implementation that allows to perform CRUD operations and select Assignment records.
    /// </summary>
    class AssignmentRepository : IAssignmentRepository
    {
        private readonly AssignmentManagerContext _dbContext;
        private readonly DbSet<Assignment> _dbSet;

        public AssignmentRepository(AssignmentManagerContext context)
        {
            _dbContext = context;
            _dbSet = context.Assignments;
        }

        public Assignment Add(Assignment item)
        {
            _dbSet.Add(item);
            return item;
        }

        public void Update(Assignment item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Remove(Assignment item)
        {
            _dbSet.Remove(item);
        }

        public async Task<Assignment> GetByIdAsync(Guid assignmentId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == assignmentId);
        }

        public async Task<IEnumerable<Assignment>> GetByUserIdAsync(Guid employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.AssigneeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(x => x.CompletionPercent)
                .ToListAsync();
        }
    }
}