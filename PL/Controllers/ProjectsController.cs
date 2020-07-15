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
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjectAsync([FromBody] ProjectDto project)
        {
            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            ProjectDto createdProject = await _projectService.CreateProjectAsync(project, currentUserId);

            return CreatedAtRoute("GetProject", new {projectId = createdProject.Id}, createdProject);
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult> UpdateProjectAsync(Guid projectId, [FromBody] ProjectDto project)
        {
            ProjectDto existingProject = await _projectService.GetProjectByIdAsync(projectId);
            if (existingProject == null)
            {
                return NotFound();
            }

            await _projectService.UpdateProjectAsync(project);

            return NoContent();
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProjectAsync(Guid projectId)
        {
            if (await _projectService.GetProjectByIdAsync(projectId) == null)
            {
                return NotFound();
            }

            await _projectService.RemoveProjectAsync(projectId);

            return NoContent();
        }

        /// <summary>
        /// Sets manager of specified project.
        /// </summary>
        /// <param name="projectId">A project identifier in the form of a GUID string.</param>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <returns></returns>
        [HttpPut("{projectId}/manager")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> SetProjectManager(Guid projectId, [FromBody] Guid userId)
        {
            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentManagerId = _projectService.GetProjectManagerById(projectId).Result?.Id;
            if (!User.IsInRole("Administrator") && currentManagerId != currentUserId)
            {
                return Unauthorized();
            }

            await _projectService.SetProjectManagerById(projectId, userId);

            return NoContent();
        }
    }
}