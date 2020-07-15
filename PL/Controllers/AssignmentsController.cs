using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PL.Controllers
{
    /// <summary>
    /// The controller for performing operations on assignments.
    /// </summary>
    [Authorize(Roles = "Employee,Manager,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IUserService _userProfileService;

        public AssignmentsController(IAssignmentService assignmentService, IUserService userProfileService)
        {
            _assignmentService = assignmentService;
            _userProfileService = userProfileService;
        }

        /// <summary>
        /// Gets the list of assignments based on current user's role.
        /// </summary>
        /// <returns>List of assignments.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAllAssignmentsAsync()
        {
            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (User.IsInRole("Administrator"))
            {
                return Ok(await _assignmentService.GetAllAssignmentsAsync());
            }
            if (User.IsInRole("Manager"))
            {
                var currentUser = await _userProfileService.GetUserByIdAsync(currentUserId);
                return Ok(await _assignmentService.GetAssignmentByProjectIdAsync(currentUser.ProjectId));
            }

            return Ok(await _assignmentService.GetAssignmentByUserIdAsync(currentUserId));
        }

        /// <summary>
        /// Gets an assignment by ID.
        /// </summary>
        /// <param name="assignmentId">An assignment identifier in the form of a GUID string.</param>
        /// <returns>An assignment record or null if assignment was not found.</returns>
        [HttpGet("{assignmentId}", Name = "GetAssignment")]
        public async Task<ActionResult<AssignmentDto>> GetAssignmentByIdAsync(Guid assignmentId)
        {
            AssignmentDto assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAssignmentAsync([FromBody] AssignmentDto assignment)
        {
            AssignmentDto createdAssignment = await _assignmentService.CreateAssignmentAsync(assignment);
            if(createdAssignment == null)
            {
                return BadRequest("Assignment could not be created.");
            }

            return CreatedAtRoute("GetAssignment", new { assignmentId = createdAssignment.Id }, createdAssignment);
        }

        [HttpPut("{assignmentId}")]
        public async Task<ActionResult> UpdateAssignmentAsync(Guid assignmentId, [FromBody] AssignmentDto assignment)
        {
            AssignmentDto existingAssignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (existingAssignment == null)
            {
                return NotFound();
            }

            await _assignmentService.UpdateAssignmentAsync(assignment);

            return NoContent();
        }

        [HttpDelete("{assignmentId}")]
        public async Task<ActionResult> DeleteAssignmentAsync(Guid assignmentId)
        {
            if (await _assignmentService.GetAssignmentByIdAsync(assignmentId) == null)
            {
                return NotFound();
            }

            await _assignmentService.RemoveAssignmentAsync(assignmentId);

            return NoContent();
        }
    }
}