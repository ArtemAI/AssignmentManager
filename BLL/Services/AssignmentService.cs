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
        private readonly Session _session;

        public AssignmentService(IEmailService emailService, IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager, SessionProvider sessionProvider)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _session = sessionProvider.Session;
        }

        /// <summary>
        /// Creates new assignment and informs assignee with email.
        /// </summary>
        /// <param name="assignment">Assignment to be added.</param>
        /// <returns>Created assignment DTO.</returns>
        public async Task<AssignmentDto> CreateAssignmentAsync(AssignmentDto assignment)
        {
            UserProfile assigneeUser = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(assignment.AssigneeId);
            if (assigneeUser.AllowEmailNotifications)
            {
                ApplicationUser assigneeUserProfile = await _userManager.FindByIdAsync(assigneeUser.Id.ToString());
                var recipientAddress = new EmailAddress
                {
                    Address = assigneeUserProfile.Email, Name = $"{assigneeUser.FirstName} {assigneeUser.LastName}"
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
            if (await _unitOfWork.Assignments.GetAssignmentByIdAsync(assignment.Id.Value) == null)
            {
                return false;
            }

            Assignment assignmentToUpdate = _mapper.Map<Assignment>(assignment);
            _unitOfWork.Assignments.UpdateAssignment(assignmentToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveAssignmentAsync(Guid assignmentId)
        {
            Assignment assignmentToRemove = await _unitOfWork.Assignments.GetAssignmentByIdAsync(assignmentId);
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

        /// <summary>
        /// Gets the list of assignments based on current user's role.
        /// </summary>
        /// <returns>Sequence of assignments.</returns>
        public async Task<IEnumerable<AssignmentDto>> GetAllAssignmentsAsync()
        {
            ApplicationUser currentUser = _session.User;
            if (await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            {
                var assignments = await _unitOfWork.Assignments.GetAllAssignmentAsync();
                return _mapper.Map<List<AssignmentDto>>(assignments);
            }

            var assignmentList = new List<AssignmentDto>();
            var currentUserProfile = await _unitOfWork.UserProfiles.GetUserProfileByIdAsync(currentUser.Id);
            if (currentUserProfile.ProjectId.HasValue)
            {
                if (await _userManager.IsInRoleAsync(currentUser, "Manager"))
                {
                    var projectAssignments =
                        await _unitOfWork.Assignments.GetAssignmentsByProjectIdAsync(currentUserProfile.ProjectId.Value);
                    assignmentList = _mapper.Map<List<AssignmentDto>>(projectAssignments);
                }
                else
                {
                    var userAssignments = 
                        await _unitOfWork.Assignments.GetAssignmentsByUserIdAsync(currentUser.Id);
                    assignmentList = _mapper.Map<List<AssignmentDto>>(userAssignments);
                }
            }
            
            return assignmentList;
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