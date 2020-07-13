using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions
{
    static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Guid firstProjectId = Guid.NewGuid(), secondProjectId = Guid.NewGuid(), thirdProjectId = Guid.NewGuid();
            Guid firstUserId = Guid.Parse("33293875-AB05-495D-8883-8C23BFB66C2C"),
                secondUserId = Guid.Parse("546C8C7C-4A94-464B-8CC7-FFFB35BB0D29"),
                thirdUserId = Guid.Parse("1E547CAB-DBED-41AA-95C9-4AFC3BA183A4"),
                fourthUserId = Guid.Parse("07D28657-02A1-47AD-BE2F-7D0E0B4E45B1");

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile
                {
                    Id = firstUserId,
                    FirstName = "Andrey",
                    LastName = "Andrienko",
                    ProjectId = secondProjectId,
                    AllowEmailNotifications = false
                },
                new UserProfile
                {
                    Id = secondUserId,
                    FirstName = "Bogdan",
                    LastName = "Bogdanov",
                    ProjectId = secondProjectId,
                    AllowEmailNotifications = true
                }, new UserProfile
                {
                    Id = thirdUserId,
                    FirstName = "Artem",
                    LastName = "Chepak",
                    ProjectId = secondProjectId,
                    AllowEmailNotifications = true
                }, new UserProfile
                {
                    Id = fourthUserId,
                    FirstName = "Sergey",
                    LastName = "Sergeev",
                    ProjectId = secondProjectId,
                    AllowEmailNotifications = true
                }
            );

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = firstProjectId,
                    Name = "Project A",
                    Description = "Tiny project with small development team."
                },
                new Project
                {
                    Id = secondProjectId,
                    Name = "Project B",
                    Description = "Medium project with average development team."
                },
                new Project
                { 
                    Id = thirdProjectId,
                    Name = "Project C",
                    Description = "Large project with big development team."
                }
            );

            modelBuilder.Entity<Assignment>().HasData(
                new Assignment 
                { 
                    Id = Guid.NewGuid(),
                    Name = "Setup project",
                    Description = "Create project's GitHub repo.",
                    Priority = 2,
                    CompletionPercent = 100, 
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(-3),
                    AssigneeId = thirdUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Design system architecture",
                    Description = "Design high-level architecture of the project.",
                    Priority = 3,
                    CompletionPercent = 45,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(5),
                    AssigneeId = thirdUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Prepare for Unit Tests",
                    Description = "Research and decide on unit test and mocking framework.",
                    Priority = 2,
                    Status = AssignmentStatus.ToDo,
                    Deadline = DateTime.Today.AddDays(3),
                    AssigneeId = thirdUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Review storage solutions",
                    Description = "Perform review of available software storage solutions.",
                    Priority = 0,
                    CompletionPercent = 100,
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(10),
                    AssigneeId = thirdUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Performance improvements",
                    Description = "Improve performance of software that is developed.",
                    Priority = 3,
                    CompletionPercent = 75,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(7),
                    AssigneeId = thirdUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Automated testing plan",
                    Description = "Create plan of automated testing of developed systems.",
                    Priority = 1,
                    CompletionPercent = 100,
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(-2),
                    AssigneeId = fourthUserId,
                    ProjectId = secondProjectId
                },
                new Assignment
                {
                    Id = Guid.NewGuid(),
                    Name = "Functional testing",
                    Description = "Perform functional testing of developed system.",
                    Priority = 2,
                    CompletionPercent = 50,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(3),
                    AssigneeId = fourthUserId,
                    ProjectId = secondProjectId
                }
            );
        }
    }
}