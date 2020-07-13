using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAssignmentRepository
    {
        Assignment Add(Assignment item);
        void Update(Assignment item);
        void Remove(Assignment item);

        Task<Assignment> GetByIdAsync(Guid assignmentId);
        Task<IEnumerable<Assignment>> GetByUserIdAsync(Guid employeeId);
        Task<IEnumerable<Assignment>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<Assignment>> GetAllAsync();
    }
}