using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IAssignmentService : IDisposable
    {
        Task<AssignmentDto> CreateAssignmentAsync(AssignmentDto assignment);
        Task<bool> UpdateAssignmentAsync(AssignmentDto assignment);
        Task<bool> RemoveAssignmentAsync(Guid assignmentId);
        Task<AssignmentDto> GetAssignmentByIdAsync(Guid assignmentId);
        Task<IEnumerable<AssignmentDto>> GetAllAssignmentsAsync();
    }
}