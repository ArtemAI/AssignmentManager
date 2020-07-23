using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    /// <summary>
    /// The controller for performing account operations.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm] LoginUserDto user)
        {
            var loggedInUser = await _accountService.Login(user.UserName, user.Password);

            if (loggedInUser == null)
            {
                return BadRequest(new ErrorDetails {StatusCode = 400, Message = "Invalid username or password."});
            }

            var token = await _accountService.GenerateJwt(loggedInUser);
            return Ok(new {token});
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromForm] RegisterUserDto user)
        {
            var registeredUser = await _accountService.Register(user);

            if (registeredUser == null)
            {
                return BadRequest(new ErrorDetails
                {
                    StatusCode = 400, Message = "Could not create user. Please check entered data."
                });
            }

            var token = await _accountService.GenerateJwt(registeredUser);
            return Ok(new {token});
        }
    }
}