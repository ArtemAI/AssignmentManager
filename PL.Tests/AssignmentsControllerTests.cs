using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PL.Controllers;

namespace PL.Tests
{
    public class AssignmentsControllerTests
    {
        private Mock<IAssignmentService> _stubAssignmentService;
        private AssignmentsController _assignmentsController;

        [SetUp]
        public void Setup()
        {
            _stubAssignmentService = new Mock<IAssignmentService>();
            _assignmentsController = new AssignmentsController(_stubAssignmentService.Object);
        }

        [Test]
        public async Task CreateAssignmentAsync_ReturnsCreatedResponse_WhenPassedValidObject()
        {
            // Arrange
            var assignmentToCreate = new AssignmentDto
            {
                Name = "Assignment",
                Status = AssignmentStatus.ToDo,
                ProjectId = Guid.NewGuid(),
                AssigneeId = Guid.NewGuid()
            };
            _stubAssignmentService.Setup(x => x.CreateAssignmentAsync(assignmentToCreate))
                .ReturnsAsync(() => { assignmentToCreate.Id = Guid.NewGuid(); return assignmentToCreate; });

            // Act
            var actionResult = await _assignmentsController.CreateAssignmentAsync(assignmentToCreate);

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtRouteResult), actionResult);
        }

        [Test]
        public async Task UpdateAssignmentAsync_ReturnsNoContentResponse_WhenPassedValidObject()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignmentToUpdate = new AssignmentDto
            {
                Id = assignmentId,
                Name = "Assignment",
                Status = AssignmentStatus.ToDo,
                ProjectId = Guid.NewGuid(),
                AssigneeId = Guid.NewGuid()
            };
            _stubAssignmentService.Setup(x => x.UpdateAssignmentAsync(assignmentToUpdate))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _assignmentsController.UpdateAssignmentAsync(assignmentId, assignmentToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task UpdateAssignmentAsync_ReturnsBadRequestResponse_WhenPassedInvalidObject()
        {
            // Arrange
            var assignmentToUpdate = new AssignmentDto
            {
                Id = Guid.NewGuid(),
                Name = "Assignment",
                Status = AssignmentStatus.ToDo,
                ProjectId = Guid.NewGuid(),
                AssigneeId = Guid.NewGuid()
            };

            // Act
            var actionResult = await _assignmentsController.UpdateAssignmentAsync(Guid.NewGuid(), assignmentToUpdate);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task DeleteAssignmentAsync_ReturnsNoContentResponse_WhenPassedValidValue()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            _stubAssignmentService.Setup(x => x.RemoveAssignmentAsync(assignmentId))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _assignmentsController.DeleteAssignmentAsync(assignmentId);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }

        [Test]
        public async Task DeleteAssignmentAsync_ReturnsBadRequestResponse_WhenPassedInvalidValue()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            _stubAssignmentService.Setup(x => x.RemoveAssignmentAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _assignmentsController.DeleteAssignmentAsync(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
        }

        [Test]
        public async Task GetAssignmentByIdAsync_ReturnsAssignment_WhenAssignmentExists()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignmentDto = new AssignmentDto { Id = assignmentId, Name = "Assignment" };
            _stubAssignmentService.Setup(x => x.GetAssignmentByIdAsync(assignmentId))
                .ReturnsAsync(assignmentDto);

            // Act
            var actionResult = await _assignmentsController.GetAssignmentByIdAsync(assignmentId);
            var result = actionResult.Result as OkObjectResult;
            var assignment = result.Value as AssignmentDto;

            // Assert
            Assert.AreEqual(assignmentId, assignment.Id);
        }

        [Test]
        public async Task GetAssignmentByIdAsync_ReturnsNotFoundResponse_WhenAssignmentDoesNotExist()
        {
            // Arrange
            _stubAssignmentService.Setup(x => x.GetAssignmentByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var actionResult = await _assignmentsController.GetAssignmentByIdAsync(Guid.NewGuid());
            var result = actionResult.Result as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllAssignmentsAsync_ReturnsAssignmentList_Always()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignmentList = new List<AssignmentDto>
            {
                new AssignmentDto
                {
                    Id = assignmentId,
                    Name = "Assignment",
                    Status = AssignmentStatus.ToDo,
                    ProjectId = Guid.NewGuid(),
                    AssigneeId = Guid.NewGuid()
                }
            };
            _stubAssignmentService.Setup(x => x.GetAllAssignmentsAsync())
                .ReturnsAsync(assignmentList);

            // Act
            var actionResult = await _assignmentsController.GetAllAssignmentsAsync();
            var result = actionResult.Result as OkObjectResult;
            var assignments = result.Value as IEnumerable<AssignmentDto>;

            // Assert
            Assert.AreEqual(assignmentId, assignments.First().Id);
        }

        [TearDown]
        public void TearDown()
        {
            _stubAssignmentService.VerifyAll();
        }
    }
}