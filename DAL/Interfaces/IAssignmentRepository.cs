using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAssignmentRepository
    {
        Assignment AddAssignment(Assignment item);
        void UpdateAssignment(Assignment item);
        void RemoveAssignment(Assignment item);

        Task<Assignment> GetAssignmentByIdAsync(Guid assignmentId);
        Task<IEnumerable<Assignment>> GetAssignmentsByUserIdAsync(Guid employeeId);
        Task<IEnumerable<Assignment>> GetAssignmentsByProjectIdAsync(Guid projectId);
        Task<IEnumerable<Assignment>> GetAllAssignmentAsync();
    }
}