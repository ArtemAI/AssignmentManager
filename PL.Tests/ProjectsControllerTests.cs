using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PL.Controllers;

namespace PL.Tests
{
    public class ProjectsControllerTests
    {
        private Mock<IProjectService> _stubProjectService;
        private ProjectsController _projectsController;

        [SetUp]
        public void Setup()
        {
            _stubProjectService = new Mock<IProjectService>();
            _projectsController = new ProjectsController(_stubProjectService.Object);
        }

        [Test]
        public async Task CreateProjectAsync_ReturnsCreatedResponse_WhenPassedValidObject()
        {
            // Arrange
            var projectToCreate = new ProjectDto { Name = "Project" };
            _stubProjectService.Setup(x => x.CreateProjectAsync(projectToCreate))
                .ReturnsAsync(() => { projectToCreate.Id = Guid.NewGuid(); return projectToCreate; });

            // Act
            var actionResult = await _projectsController.CreateProjectAsync(projectToCreate);

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtRouteResult), actionResult);
        }

        [Test]
        public async Task UpdateProjectAsync_ReturnsNoContentResponse_WhenPassedValidObject()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectToUpdate = new ProjectDto { Id = projectId, Name = "Project" };
            _stubProjectService.Setup(x => x.UpdateProjectAsync(projectToUpdate))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _projectsController.UpdateProjectAsync(projectId, projectToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task UpdateProjectAsync_ReturnsBadRequestResponse_WhenPassedInvalidObject()
        {
            // Arrange
            var projectToUpdate = new ProjectDto
            {
                Id = Guid.NewGuid(),
                Name = "Project"
            };

            // Act
            var actionResult = await _projectsController.UpdateProjectAsync(Guid.NewGuid(), projectToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task DeleteProjectAsync_ReturnsNoContentResponse_WhenPassedValidValue()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _stubProjectService.Setup(x => x.RemoveProjectAsync(projectId))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _projectsController.DeleteProjectAsync(projectId);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task DeleteProjectAsync_ReturnsBadRequestResponse_WhenPassedInvalidValue()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _stubProjectService.Setup(x => x.RemoveProjectAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _projectsController.DeleteProjectAsync(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
        }

        [Test]
        public async Task GetProjectByIdAsync_ReturnsProject_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectDto = new ProjectDto { Id = projectId, Name = "Project" };
            _stubProjectService.Setup(x => x.GetProjectByIdAsync(projectId))
                .ReturnsAsync(projectDto);

            // Act
            var actionResult = await _projectsController.GetProjectByIdAsync(projectId);
            var result = actionResult.Result as OkObjectResult;
            var project = result.Value as ProjectDto;

            // Assert
            Assert.AreEqual(projectId, project.Id);
        }

        [Test]
        public async Task GetProjectByIdAsync_ReturnsNotFoundResponse_WhenProjectDoesNotExist()
        {
            // Arrange
            _stubProjectService.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var actionResult = await _projectsController.GetProjectByIdAsync(Guid.NewGuid());
            var result = actionResult.Result as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllProjectsAsync_ReturnsProjectList_Always()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectList = new List<ProjectDto> { new ProjectDto { Id = projectId, Name = "Project" } };
            _stubProjectService.Setup(x => x.GetAllProjectsAsync())
                .ReturnsAsync(projectList);

            // Act
            var actionResult = await _projectsController.GetAllProjectsAsync();
            var result = actionResult.Result as OkObjectResult;
            var projects = result.Value as IEnumerable<ProjectDto>;

            // Assert
            Assert.AreEqual(projectId, projects.First().Id);
        }

        [Test]
        public async Task SetProjectManagerAsync_ReturnsNoContentResponse_WhenPassedValidValues()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _stubProjectService.Setup(x => x.SetProjectManagerAsync(projectId, userId, null))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _projectsController.SetProjectManagerAsync(projectId, userId);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task SetProjectManagerAsync_ReturnsBadRequestResponse_WhenPassedInvalidValues()
        {
            // Arrange
            _stubProjectService.Setup(x => x.SetProjectManagerAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), null))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _projectsController.SetProjectManagerAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [TearDown]
        public void TearDown()
        {
            _stubProjectService.VerifyAll();
        }
    }
}