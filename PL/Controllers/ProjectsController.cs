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
    /// The controller for performing operations on projects.
    /// </summary>
    [Authorize(Roles = "Employee,Manager,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjectAsync([FromBody] ProjectDto project)
        {
            var createdProject = await _projectService.CreateProjectAsync(project);
            return CreatedAtRoute("GetProject", new {projectId = createdProject.Id}, createdProject);
        }

        [HttpPut("{projectId}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> UpdateProjectAsync(Guid projectId, [FromBody] ProjectDto project)
        {
            if (projectId != project.Id)
            {
                return BadRequest("Specified project ID is invalid.");
            }

            var result = await _projectService.UpdateProjectAsync(project);
            if (!result)
            {
                return NotFound("Could not find project with provided ID.");
            }

            return NoContent();
        }

        [HttpDelete("{projectId}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> DeleteProjectAsync(Guid projectId)
        {
            var result = await _projectService.RemoveProjectAsync(projectId);
            if (!result)
            {
                return NotFound("Could not find project with provided ID.");
            }

            return NoContent();
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<ActionResult<ProjectDto>> GetProjectByIdAsync(Guid projectId)
        {
            ProjectDto project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Could not find project with provided ID.");
            }

            return Ok(project);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
        {
            return Ok(await _projectService.GetAllProjectsAsync());
        }

        /// <summary>
        /// Sets manager of specified project. User must be administrator or manager of selected project.
        /// </summary>
        /// <param name="projectId">A project identifier in the form of a GUID string.</param>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <returns>HTTP status code 201 if operation is successful, error response otherwise.</returns>
        [HttpPut("{projectId}/manager")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> SetProjectManagerAsync(Guid projectId, [FromBody] Guid userId)
        {
            var result = await _projectService.SetProjectManagerAsync(projectId, userId);
            if (!result)
            {
                return BadRequest("Could not find project or user with provided IDs.");
            }

            return NoContent();
        }
    }
}