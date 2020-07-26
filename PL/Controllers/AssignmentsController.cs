using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAssignmentAsync([FromBody] AssignmentDto assignment)
        {
            AssignmentDto createdAssignment = await _assignmentService.CreateAssignmentAsync(assignment);

            return CreatedAtRoute("GetAssignment", 
                new {assignmentId = createdAssignment.Id}, createdAssignment);
        }

        [HttpPut("{assignmentId}")]
        public async Task<ActionResult> UpdateAssignmentAsync(Guid assignmentId, [FromBody] AssignmentDto assignment)
        {
            if (assignmentId != assignment.Id)
            {
                return BadRequest("Specified assignment ID is invalid.");
            }

            var result = await _assignmentService.UpdateAssignmentAsync(assignment);
            if (!result)
            {
                return NotFound("Could not find assignment with provided ID.");
            }

            return NoContent();
        }

        [HttpDelete("{assignmentId}")]
        public async Task<ActionResult> DeleteAssignmentAsync(Guid assignmentId)
        {
            var result = await _assignmentService.RemoveAssignmentAsync(assignmentId);
            if (!result)
            {
                return NotFound("Could not find assignment with provided ID.");
            }

            return NoContent();
        }

        [HttpGet("{assignmentId}", Name = "GetAssignment")]
        public async Task<ActionResult<AssignmentDto>> GetAssignmentByIdAsync(Guid assignmentId)
        {
            AssignmentDto assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound("Could not find assignment with provided ID.");
            }

            return Ok(assignment);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAllAssignmentsAsync()
        {
            return Ok(await _assignmentService.GetAllAssignmentsAsync());
        }
    }
}