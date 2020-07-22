using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions
{
    /// <summary>
    /// Performs database seeding with test data.
    /// </summary>
    static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Guid firstProjectId = Guid.Parse("3F866896-C858-4B0F-9BF3-3FA3DCA36827"),
                secondProjectId = Guid.Parse("529B493E-D3E6-4065-8ED7-4E3A8A7DF238"),
                thirdProjectId = Guid.Parse("3F35412D-1A07-4333-AE60-1FCD642CAC61");

            Guid firstUserId = Guid.Parse("33293875-AB05-495D-8883-8C23BFB66C2C"),
                secondUserId = Guid.Parse("546C8C7C-4A94-464B-8CC7-FFFB35BB0D29"),
                thirdUserId = Guid.Parse("1E547CAB-DBED-41AA-95C9-4AFC3BA183A4"),
                fourthUserId = Guid.Parse("07D28657-02A1-47AD-BE2F-7D0E0B4E45B1");

            Guid firstAssignmentId = Guid.Parse("E6EFE661-925A-40DD-9EA8-C194170E35EF"),
                secondAssignmentId = Guid.Parse("A5AE9821-FD63-447D-8210-C01451CE9EF7"),
                thirdAssignmentId = Guid.Parse("0FADFA8B-D8DF-458A-BB68-05B0A7956C89"),
                fourthAssignmentId = Guid.Parse("FBBCFBAF-283E-4029-BE35-5E66095DFC9C"),
                fifthAssignmentId = Guid.Parse("680E01A4-57A1-4796-A585-FE6E56836348"),
                sixthAssignmentId = Guid.Parse("153FA6DC-4320-4EC6-AA08-265593166618"),
                seventhAssignmentId = Guid.Parse("7AA1CEEB-26F3-486E-B0F0-C4C6DC1C1DB6");

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
                    Id = firstAssignmentId,
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
                    Id = secondAssignmentId,
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
                    Id = thirdAssignmentId,
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
                    Id = fourthAssignmentId,
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
                    Id = fifthAssignmentId,
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
                    Id = sixthAssignmentId,
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
                    Id = seventhAssignmentId,
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