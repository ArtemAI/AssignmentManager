using System;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PL.Models;

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
        private readonly IUserService _userProfileService;

        public UsersController(IUserService userService)
        {
            _userProfileService = userService;
        }

        /// <summary>
        /// Gets the list of user profiles based on current user's role.
        /// </summary>
        /// <returns>List of user profiles.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUsersAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                return Ok(await _userProfileService.GetAllUsersAsync());
            }

            var currentUserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await _userProfileService.GetUserByIdAsync(currentUserId);

            return Ok(await _userProfileService.GetUserByProjectIdAsync(currentUser.ProjectId));
        }

        /// <summary>
        /// Gets a user's profile by ID.
        /// </summary>
        /// <param name="userId">A user identifier in the form of a GUID string.</param>
        /// <returns>A user record or null if user was not found.</returns>
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<ActionResult<UserProfileDto>> GetUserByIdAsync(Guid userId)
        {
            UserProfileDto user = await _userProfileService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404,
                    Message = "Could not find user with provided ID."
                });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync([FromBody] UserProfileDto user)
        {
            UserProfileDto createdUser = await _userProfileService.CreateUserProfileAsync(user);
            return CreatedAtRoute("GetUser", new { userId = createdUser.Id }, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUserAsync(Guid userId, [FromBody] UserProfileDto user)
        {
            UserProfileDto existingUser = await _userProfileService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404,
                    Message = "Could not find user with provided ID."
                });
            }

            await _userProfileService.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpPut("{userId}/role")]
        public async Task<ActionResult> SetUserRole(Guid userId, string role)
        {
            UserProfileDto existingUser = await _userProfileService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404,
                    Message = "Could not find user with provided ID."
                });
            }

            await _userProfileService.SetUserRole(userId, role);

            return NoContent();
        }
    }
}