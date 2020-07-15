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

        public Assignment AddAssignment(Assignment item)
        {
            _dbSet.Add(item);
            return item;
        }

        public void UpdateAssignment(Assignment item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void RemoveAssignment(Assignment item)
        {
            _dbSet.Remove(item);
        }

        public async Task<Assignment> GetAssignmentByIdAsync(Guid assignmentId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == assignmentId);
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByUserIdAsync(Guid employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.AssigneeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(x => x.CompletionPercent)
                .ToListAsync();
        }
    }
}