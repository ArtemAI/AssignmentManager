using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PL.Controllers
{
    /// <summary>
    /// The controller for performing operations on user profiles.
    /// </summary>
    [Authorize(Roles = "Employee,Manager,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync([FromBody] UserProfileDto user)
        {
            UserProfileDto createdUser = await _userService.CreateUserProfileAsync(user);
            return CreatedAtRoute("GetUser", new {userId = createdUser.Id}, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUserAsync(Guid userId, [FromBody] UserProfileDto user)
        {
            if(userId != user.Id)
            {
                return BadRequest("Specified user ID is invalid.");
            }

            var result = await _userService.UpdateUserAsync(user);
            if (!result)
            {
                return NotFound("Could not find user with provided ID.");
            }

            return NoContent();
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<ActionResult<UserProfileDto>> GetUserByIdAsync(Guid userId)
        {
            UserProfileDto user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Could not find user with provided ID.");
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> GetAllRoleNamesAsync()
        {
            return Ok(await _userService.GetAllRoleNamesAsync());
        }

        /// <summary>
        /// Sets a role of specified user. 
        /// </summary>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <param name="jsonRole">JSON object that contains name of new role.</param>
        /// <returns>HTTP status code 204 if operation is successful, error response otherwise.</returns>
        [HttpPut("{userId}/role")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> SetUserRoleAsync(Guid userId, [FromBody] object jsonRole)
        {
            var roleDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonRole.ToString());
            var result = await _userService.SetUserRoleAsync(userId, roleDictionary["role"]);
            if(!result)
            {
                return BadRequest("Could not set a role of specified user.");
            }

            return NoContent();
        }

        [HttpPut("{userId}/project/{projectId}")]
        public async Task<ActionResult> AddUserToProjectAsync(Guid userId, Guid projectId)
        {
            var result = await _userService.AddUserToProjectAsync(userId, projectId);
            if (!result)
            {
                return BadRequest("Could not add user to project with provided ID.");
            }

            return NoContent();
        }

        [HttpDelete("{userId}/project")]
        public async Task<ActionResult> RemoveUserFromProjectAsync(Guid userId)
        {
            var result = await _userService.RemoveUserFromProjectAsync(userId);
            if (!result)
            {
                return BadRequest("Could not find user with provided IDs.");
            }

            return NoContent();
        }
    }
}