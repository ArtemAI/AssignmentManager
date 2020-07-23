using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    /// <summary>
    /// The controller for performing operations on projects.
    /// </summary>
    [Authorize(Roles = "Employee,Manager,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userProfileService;

        public ProjectsController(IProjectService projectService, IUserService userProfileService)
        {
            _projectService = projectService;
            _userProfileService = userProfileService;
        }

        /// <summary>
        /// Gets the list of projects based on current user's role.
        /// </summary>
        /// <returns>List of projects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                return Ok(await _projectService.GetAllProjectsAsync());
            }

            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await _userProfileService.GetUserByIdAsync(currentUserId);

            return Ok(await _projectService.GetProjectByIdAsync(currentUser.ProjectId));
        }

        /// <summary>
        /// Gets a project by ID.
        /// </summary>
        /// <param name="projectId">A project identifier in the form of a GUID string.</param>
        /// <returns>A project record or null if project was not found.</returns>
        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<ActionResult<ProjectDto>> GetProjectByIdAsync(Guid projectId)
        {
            ProjectDto project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404, Message = "Could not find project with provided ID."
                });
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjectAsync([FromBody] ProjectDto project)
        {
            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var createdProject = await _projectService.CreateProjectAsync(project, currentUserId);

            return CreatedAtRoute("GetProject", new {projectId = createdProject.Id}, createdProject);
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult> UpdateProjectAsync(Guid projectId, [FromBody] ProjectDto project)
        {
            var result = await _projectService.UpdateProjectAsync(project);
            if (!result)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404, Message = "Could not find project with provided ID."
                });
            }

            return NoContent();
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProjectAsync(Guid projectId)
        {
            var result = await _projectService.RemoveProjectAsync(projectId);
            if (!result)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404, Message = "Could not find project with provided ID."
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Sets manager of specified project. User must be administrator or manager of selected project.
        /// </summary>
        /// <param name="projectId">A project identifier in the form of a GUID string.</param>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <returns>HTTP status code 201 if operation is successful, error response otherwise.</returns>
        [HttpPut("{projectId}/manager")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> SetProjectManager(Guid projectId, [FromBody] Guid userId)
        {
            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentManagerId = _projectService.GetProjectManagerById(projectId).Result?.Id;
            if (!User.IsInRole("Administrator") && currentManagerId != currentUserId)
            {
                return Unauthorized(new ErrorDetails
                {
                    StatusCode = 403, Message = "You do not have permission to perform this operation."
                });
            }

            var result = await _projectService.SetProjectManagerById(projectId, userId);
            if (!result)
            {
                return BadRequest(new ErrorDetails
                {
                    StatusCode = 400, Message = "Could not find project or user with provided IDs."
                });
            }

            return NoContent();
        }
    }
}