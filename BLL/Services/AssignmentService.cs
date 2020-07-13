using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Performs assignments operations.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(AssignmentDto assignment)
        {
            Assignment mappedAssignment = _mapper.Map<Assignment>(assignment);
            Assignment createdAssignment = _unitOfWork.Assignments.Add(mappedAssignment);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<AssignmentDto>(createdAssignment);
        }

        public async Task RemoveAssignmentAsync(Guid assignmentId)
        {
            Assignment assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            _unitOfWork.Assignments.Remove(assignment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAssignmentAsync(AssignmentDto assignment)
        {
            Assignment mappedAssignment = _mapper.Map<Assignment>(assignment);
            _unitOfWork.Assignments.Update(mappedAssignment);
            await _unitOfWork.SaveAsync();
        }

        public async Task<AssignmentDto> GetAssignmentByIdAsync(Guid assignmentId)
        {
            Assignment assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            return _mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentByUserIdAsync(Guid userId)
        {
            IEnumerable<Assignment> userAssignments = await _unitOfWork.Assignments.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(userAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentByProjectIdAsync(Guid projectId)
        {
            IEnumerable<Assignment> userAssignments = await _unitOfWork.Assignments.GetByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(userAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAllAssignmentsAsync()
        {
            IEnumerable<Assignment> assignments = await _unitOfWork.Assignments.GetAllAsync();
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}