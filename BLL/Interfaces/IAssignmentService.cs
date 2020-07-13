using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IAssignmentService : IDisposable
    {
        Task<AssignmentDto> CreateAssignmentAsync(AssignmentDto assignment);
        Task UpdateAssignmentAsync(AssignmentDto assignment);
        Task RemoveAssignmentAsync(Guid assignmentId);

        Task<AssignmentDto> GetAssignmentByIdAsync(Guid assignmentId);
        Task<IEnumerable<AssignmentDto>> GetAssignmentByUserIdAsync(Guid userId);
        Task<IEnumerable<AssignmentDto>> GetAssignmentByProjectIdAsync(Guid projectId);
        Task<IEnumerable<AssignmentDto>> GetAllAssignmentsAsync();
    }
}