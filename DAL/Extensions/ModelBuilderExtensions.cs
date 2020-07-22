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
            Guid[] projectIds = new Guid[]
            {
                Guid.Parse("3F866896-C858-4B0F-9BF3-3FA3DCA36827"),
                Guid.Parse("529B493E-D3E6-4065-8ED7-4E3A8A7DF238"),
                Guid.Parse("3F35412D-1A07-4333-AE60-1FCD642CAC61")
            };

            Guid[] userIds = new Guid[]
            {
                Guid.Parse("33293875-AB05-495D-8883-8C23BFB66C2C"),
                Guid.Parse("546C8C7C-4A94-464B-8CC7-FFFB35BB0D29"),
                Guid.Parse("1E547CAB-DBED-41AA-95C9-4AFC3BA183A4"),
                Guid.Parse("07D28657-02A1-47AD-BE2F-7D0E0B4E45B1")
            };

            Guid[] assignmentIds = new Guid[]
            {
                Guid.Parse("E6EFE661-925A-40DD-9EA8-C194170E35EF"),
                Guid.Parse("A5AE9821-FD63-447D-8210-C01451CE9EF7"),
                Guid.Parse("0FADFA8B-D8DF-458A-BB68-05B0A7956C89"),
                Guid.Parse("FBBCFBAF-283E-4029-BE35-5E66095DFC9C"),
                Guid.Parse("680E01A4-57A1-4796-A585-FE6E56836348"),
                Guid.Parse("153FA6DC-4320-4EC6-AA08-265593166618"),
                Guid.Parse("7AA1CEEB-26F3-486E-B0F0-C4C6DC1C1DB6")
            };

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile
                {
                    Id = userIds[0],
                    FirstName = "Andrey",
                    LastName = "Andrienko",
                    ProjectId = projectIds[1],
                    AllowEmailNotifications = false
                },
                new UserProfile
                {
                    Id = userIds[1],
                    FirstName = "Bogdan",
                    LastName = "Bogdanov",
                    ProjectId = projectIds[1],
                    AllowEmailNotifications = true
                }, new UserProfile
                {
                    Id = userIds[2],
                    FirstName = "Artem",
                    LastName = "Chepak",
                    ProjectId = projectIds[1],
                    AllowEmailNotifications = true
                }, new UserProfile
                {
                    Id = userIds[3],
                    FirstName = "Sergey",
                    LastName = "Sergeev",
                    ProjectId = projectIds[1],
                    AllowEmailNotifications = true
                }
            );

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = projectIds[0],
                    Name = "Project A",
                    Description = "Tiny project with small development team."
                },
                new Project
                {
                    Id = projectIds[1],
                    Name = "Project B",
                    Description = "Medium project with average development team."
                },
                new Project
                {
                    Id = projectIds[2],
                    Name = "Project C",
                    Description = "Large project with big development team."
                }
            );

            modelBuilder.Entity<Assignment>().HasData(
                new Assignment
                {
                    Id = assignmentIds[0],
                    Name = "Setup project",
                    Description = "Create project's GitHub repo.",
                    Priority = 2,
                    CompletionPercent = 100,
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(-3),
                    AssigneeId = userIds[2],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[1],
                    Name = "Design system architecture",
                    Description = "Design high-level architecture of the project.",
                    Priority = 3,
                    CompletionPercent = 45,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(5),
                    AssigneeId = userIds[2],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[2],
                    Name = "Prepare for Unit Tests",
                    Description = "Research and decide on unit test and mocking framework.",
                    Priority = 2,
                    Status = AssignmentStatus.ToDo,
                    Deadline = DateTime.Today.AddDays(3),
                    AssigneeId = userIds[2],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[3],
                    Name = "Review storage solutions",
                    Description = "Perform review of available software storage solutions.",
                    Priority = 0,
                    CompletionPercent = 100,
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(10),
                    AssigneeId = userIds[2],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[4],
                    Name = "Performance improvements",
                    Description = "Improve performance of software that is developed.",
                    Priority = 3,
                    CompletionPercent = 75,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(7),
                    AssigneeId = userIds[2],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[5],
                    Name = "Automated testing plan",
                    Description = "Create plan of automated testing of developed systems.",
                    Priority = 1,
                    CompletionPercent = 100,
                    Status = AssignmentStatus.Done,
                    Deadline = DateTime.Today.AddDays(-2),
                    AssigneeId = userIds[3],
                    ProjectId = projectIds[1]
                },
                new Assignment
                {
                    Id = assignmentIds[6],
                    Name = "Functional testing",
                    Description = "Perform functional testing of developed system.",
                    Priority = 2,
                    CompletionPercent = 50,
                    Status = AssignmentStatus.InProgress,
                    Deadline = DateTime.Today.AddDays(3),
                    AssigneeId = userIds[3],
                    ProjectId = projectIds[1]
                }
            );
        }
    }
}