using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    CompletionPercent = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    AssigneeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    AllowEmailNotifications = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_UserProfiles_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("3f866896-c858-4b0f-9bf3-3fa3dca36827"), "Tiny project with small development team.", null, "Project A" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), "Medium project with average development team.", null, "Project B" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("3f35412d-1a07-4333-ae60-1fcd642cac61"), "Large project with big development team.", null, "Project C" });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "AllowEmailNotifications", "FirstName", "LastName", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("33293875-ab05-495d-8883-8c23bfb66c2c"), false, "Andrey", "Andrienko", new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238") },
                    { new Guid("546c8c7c-4a94-464b-8cc7-fffb35bb0d29"), true, "Bogdan", "Bogdanov", new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238") },
                    { new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), true, "Artem", "Chepak", new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238") },
                    { new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), true, "Sergey", "Sergeev", new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238") }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssigneeId", "CompletionPercent", "Deadline", "Description", "Name", "Priority", "ProjectId", "Status" },
                values: new object[,]
                {
                    { new Guid("e6efe661-925a-40dd-9ea8-c194170e35ef"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 100, new DateTime(2020, 7, 19, 0, 0, 0, 0, DateTimeKind.Local), "Create project's GitHub repo.", "Setup project", 2, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 2 },
                    { new Guid("a5ae9821-fd63-447d-8210-c01451ce9ef7"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 45, new DateTime(2020, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), "Design high-level architecture of the project.", "Design system architecture", 3, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 1 },
                    { new Guid("0fadfa8b-d8df-458a-bb68-05b0a7956c89"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 0, new DateTime(2020, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), "Research and decide on unit test and mocking framework.", "Prepare for Unit Tests", 2, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 0 },
                    { new Guid("fbbcfbaf-283e-4029-be35-5e66095dfc9c"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 100, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), "Perform review of available software storage solutions.", "Review storage solutions", 0, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 2 },
                    { new Guid("680e01a4-57a1-4796-a585-fe6e56836348"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 75, new DateTime(2020, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), "Improve performance of software that is developed.", "Performance improvements", 3, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 1 },
                    { new Guid("153fa6dc-4320-4ec6-aa08-265593166618"), new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), 100, new DateTime(2020, 7, 20, 0, 0, 0, 0, DateTimeKind.Local), "Create plan of automated testing of developed systems.", "Automated testing plan", 1, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 2 },
                    { new Guid("7aa1ceeb-26f3-486e-b0f0-c4c6dc1c1db6"), new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), 50, new DateTime(2020, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), "Perform functional testing of developed system.", "Functional testing", 2, new Guid("529b493e-d3e6-4065-8ed7-4e3a8a7df238"), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssigneeId",
                table: "Assignments",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ProjectId",
                table: "Assignments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProjectId",
                table: "UserProfiles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_UserProfiles_AssigneeId",
                table: "Assignments",
                column: "AssigneeId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Projects_ProjectId",
                table: "Assignments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Projects_ProjectId",
                table: "UserProfiles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_UserProfiles_ManagerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
