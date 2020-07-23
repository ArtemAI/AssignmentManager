using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    /// <summary>
    /// Performs operations on Assignment entities.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignmentService(UserManager<ApplicationUser> userManager, IEmailService emailService,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(AssignmentDto assignment)
        {
            UserProfile assignee = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(assignment.AssigneeId);
            if (assignee.AllowEmailNotifications)
            {
                var assigneeUser = await _userManager.FindByIdAsync(assignee.Id.ToString());
                var recipientAddress = new EmailAddress
                {
                    Address = assigneeUser.Email, Name = $"{assignee.FirstName} {assignee.LastName}"
                };
                var infoMessage = new EmailMessage
                {
                    Subject = "New assignment",
                    Content = $"You have been assigned to new task: {assignment.Name}.\n" +
                              "Please visit your Assignments page for detailed information."
                };
                infoMessage.ToAddresses.Add(recipientAddress);
                await _emailService.Send(infoMessage);
            }

            Assignment assignmentToCreate = _mapper.Map<Assignment>(assignment);
            Assignment createdAssignment = _unitOfWork.Assignments.AddAssignment(assignmentToCreate);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<AssignmentDto>(createdAssignment);
        }

        public async Task<bool> UpdateAssignmentAsync(AssignmentDto assignment)
        {
            if (await _unitOfWork.Assignments.GetAssignmentByIdAsync((Guid)assignment.Id) == null)
            {
                return false;
            }

            var assignmentToUpdate = _mapper.Map<Assignment>(assignment);
            _unitOfWork.Assignments.UpdateAssignment(assignmentToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveAssignmentAsync(Guid assignmentId)
        {
            var assignmentToRemove = await _unitOfWork.Assignments.GetAssignmentByIdAsync(assignmentId);
            if (assignmentToRemove == null)
            {
                return false;
            }

            _unitOfWork.Assignments.RemoveAssignment(assignmentToRemove);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<AssignmentDto> GetAssignmentByIdAsync(Guid assignmentId)
        {
            Assignment assignment = await _unitOfWork.Assignments.GetAssignmentByIdAsync(assignmentId);
            return _mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentByUserIdAsync(Guid userId)
        {
            IEnumerable<Assignment> userAssignments = await _unitOfWork.Assignments.GetAssignmentsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(userAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentByProjectIdAsync(Guid projectId)
        {
            IEnumerable<Assignment> projectAssignments =
                await _unitOfWork.Assignments.GetAssignmentsByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(projectAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAllAssignmentsAsync()
        {
            IEnumerable<Assignment> assignments = await _unitOfWork.Assignments.GetAllAssignmentAsync();
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        #region IDisposable Support

        private bool _disposedValue;

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