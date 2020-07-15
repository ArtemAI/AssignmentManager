using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Performs such account operations as registration, signing in and JWT generation.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtConfiguration _jwtConfiguration;
        private readonly IMapper _mapper;

        public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, IJwtConfiguration jwtConfiguration, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtConfiguration = jwtConfiguration;
            _mapper = mapper;
        }

        /// <summary>
        /// Performs a user login operation.
        /// </summary>
        /// <param name="username">User's name.</param>
        /// <param name="password">User's password.</param>
        /// <returns>An application user's record or null if login was not successful.</returns>
        public async Task<ApplicationUser> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            { 
                return await _userManager.FindByNameAsync(username);
            }

            return null;
        }

        /// <summary>
        /// Performs registration operation. Identity and domain data are being saved separately.
        /// </summary>
        /// <param name="user">Registered user.</param>
        /// <returns>An application user's record or null if registration was not successful.</returns>
        public async Task<ApplicationUser> Register(RegisterUserDto user)
        {
            var userProfile = _mapper.Map<UserProfile>(user);
            var identityUser = _mapper.Map<ApplicationUser>(user);
            var creationResult = await _userManager.CreateAsync(identityUser, user.Password);

            if (creationResult.Succeeded)
            {
                var registeredUser = await _userManager.FindByNameAsync(user.UserName);
                await _userManager.AddToRoleAsync(registeredUser, "Employee");
                await _signInManager.SignInAsync(registeredUser, false);

                userProfile.Id = registeredUser.Id;
                _unitOfWork.UserProfiles.AddUserProfile(userProfile);
                await _unitOfWork.SaveAsync();
                return registeredUser;
            }

            return null;
        }

        /// <summary>
        /// Performs generation of JWT that grants to user some permissions, based on his role.
        /// </summary>
        /// <param name="user">An application user's record.</param>
        /// <returns>JSON Web Token object.</returns>
        public async Task<object> GenerateJwt(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfiguration.ExpirationDays));

            var token = new JwtSecurityToken(
                _jwtConfiguration.Issuer,
                _jwtConfiguration.Audience,
                claims,
                expires: expirationDate,
                signingCredentials: credentials
            );

            return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }

        #region IDisposable Support
        private bool _disposedValue = false;

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