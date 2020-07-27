using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PL.Controllers;

namespace PL.Tests
{
    public class UsersControllerTests
    {
        private Mock<IUserService> _stubUserService;
        private UsersController _usersController;

        [SetUp]
        public void Setup()
        {
            _stubUserService = new Mock<IUserService>();
            _usersController = new UsersController(_stubUserService.Object);
        }

        [Test]
        public async Task CreateUserAsync_ReturnsCreatedResponse_WhenPassedValidObject()
        {
            // Arrange
            var userToCreate = new UserProfileDto
            {
                FirstName = "First name", LastName = "Last name", AllowEmailNotifications = true
            };
            _stubUserService.Setup(x => x.CreateUserProfileAsync(userToCreate))
                .ReturnsAsync(() => { userToCreate.Id = Guid.NewGuid(); return userToCreate; });

            // Act
            var actionResult = await _usersController.CreateUserAsync(userToCreate);

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtRouteResult), actionResult);
        }

        [Test]
        public async Task UpdateUserAsync_ReturnsNoContentResponse_WhenPassedValidObject()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToUpdate = new UserProfileDto
            {
                Id = userId, FirstName = "First name", LastName = "Last name", AllowEmailNotifications = true
            };
            _stubUserService.Setup(x => x.UpdateUserAsync(userToUpdate))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _usersController.UpdateUserAsync(userId, userToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task UpdateUserAsync_ReturnsBadRequestResponse_WhenPassedInvalidObject()
        {
            // Arrange
            var userToUpdate = new UserProfileDto
            {
                Id = Guid.NewGuid(),
                FirstName = "First name",
                LastName = "Last name",
                AllowEmailNotifications = true
            };

            // Act
            var actionResult = await _usersController.UpdateUserAsync(Guid.NewGuid(), userToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserProfileDto
            {
                Id = userId, FirstName = "First name", LastName = "Last name", AllowEmailNotifications = true
            };
            _stubUserService.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(userDto);

            // Act
            var actionResult = await _usersController.GetUserByIdAsync(userId);
            var result = actionResult.Result as OkObjectResult;
            var user = result.Value as UserProfileDto;

            // Assert
            Assert.AreEqual(userId, user.Id);
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsNotFoundResponse_WhenUserDoesNotExist()
        {
            // Arrange
            _stubUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var actionResult = await _usersController.GetUserByIdAsync(Guid.NewGuid());
            var result = actionResult.Result as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsUserList_Always()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userList = new List<UserProfileDto>
            {
                new UserProfileDto
                {
                    Id = userId,
                    FirstName = "First name",
                    LastName = "Last name",
                    AllowEmailNotifications = true
                }
            };
            _stubUserService.Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(userList);

            // Act
            var actionResult = await _usersController.GetAllUsersAsync();
            var result = actionResult.Result as OkObjectResult;
            var users = result.Value as IEnumerable<UserProfileDto>;

            // Assert
            Assert.AreEqual(userId, users.First().Id);
        }

        [Test]
        public async Task SetUserRoleAsync_ReturnsNoContentResponse_WhenPassedValidValues()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleToSet = "Administrator";
            var roleDictionary = new Dictionary<string, string> { { "role", roleToSet } };
            string roleObject = JsonConvert.SerializeObject(roleDictionary);
            _stubUserService.Setup(x => x.SetUserRoleAsync(userId, roleToSet))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _usersController.SetUserRole(userId, roleObject);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task SetUserRoleAsync_ReturnsBadRequestResponse_WhenPassedInvalidValues()
        {
            // Arrange
            var roleToSet = "Invalid role";
            var roleDictionary = new Dictionary<string, string> { { "role", roleToSet } };
            string roleObject = JsonConvert.SerializeObject(roleDictionary);
            _stubUserService.Setup(x => x.SetUserRoleAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _usersController.SetUserRole(Guid.NewGuid(), roleObject);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task AddUserToProjectAsync_ReturnsNoContentResponse_WhenPassedValidValues()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            _stubUserService.Setup(x => x.AddUserToProjectAsync(userId, projectId))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _usersController.AddUserToProjectAsync(userId, projectId);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task AddUserToProjectAsync_ReturnsBadRequestResponse_WhenPassedInvlidValues()
        {
            // Arrange
            _stubUserService.Setup(x => x.AddUserToProjectAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _usersController.AddUserToProjectAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task RemoveUserFromProjectAsync_ReturnsNoContentResponse_WhenPassedValidValue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _stubUserService.Setup(x => x.RemoveUserFromProjectAsync(userId))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _usersController.RemoveUserFromProjectAsync(userId);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task RemoveUserFromProjectAsync_ReturnsBadRequestResponse_WhenPassedInvalidValue()
        {
            // Arrange
            _stubUserService.Setup(x => x.RemoveUserFromProjectAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _usersController.RemoveUserFromProjectAsync(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [TearDown]
        public void TearDown()
        {
            _stubUserService.VerifyAll();
        }
    }
}