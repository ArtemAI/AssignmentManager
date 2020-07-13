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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("961881bc-fa7e-419f-968e-f852792c51d4"), "Tiny project with small development team.", null, "Project A" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), "Medium project with average development team.", null, "Project B" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ManagerId", "Name" },
                values: new object[] { new Guid("86e85a6d-2a0e-4964-a606-3680985888a7"), "Large project with big development team.", null, "Project C" });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "AllowEmailNotifications", "FirstName", "LastName", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("33293875-ab05-495d-8883-8c23bfb66c2c"), false, "Andrey", "Andrienko", new Guid("4d56d43a-092b-4023-82e0-870484a1223d") },
                    { new Guid("546c8c7c-4a94-464b-8cc7-fffb35bb0d29"), true, "Bogdan", "Bogdanov", new Guid("4d56d43a-092b-4023-82e0-870484a1223d") },
                    { new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), true, "Artem", "Chepak", new Guid("4d56d43a-092b-4023-82e0-870484a1223d") },
                    { new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), true, "Sergey", "Sergeev", new Guid("4d56d43a-092b-4023-82e0-870484a1223d") }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssigneeId", "CompletionPercent", "Deadline", "Description", "Name", "Priority", "ProjectId", "Status" },
                values: new object[,]
                {
                    { new Guid("5916ef2d-2075-44dc-a070-fa34a161f4cc"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 100, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Local), "Create project's GitHub repo.", "Setup project", 1, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 2 },
                    { new Guid("e60bc80c-21bb-481d-b248-34f6ebfea153"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 45, new DateTime(2020, 5, 8, 0, 0, 0, 0, DateTimeKind.Local), "Design high-level architecture of the project.", "Design system architecture", 3, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 1 },
                    { new Guid("30e00597-cf30-4430-8899-ce43a9f91702"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 0, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), "Research and decide on unit test and mocking framework.", "Prepare for Unit Tests", 2, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 0 },
                    { new Guid("01e37ff1-cf73-4602-9816-cf0753a42fb4"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 100, new DateTime(2020, 5, 13, 0, 0, 0, 0, DateTimeKind.Local), "Perform review of available software storage solutions.", "Review storage solutions", 0, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 2 },
                    { new Guid("87d0cf39-219e-49e2-9d73-167b8bd19f4d"), new Guid("1e547cab-dbed-41aa-95c9-4afc3ba183a4"), 75, new DateTime(2020, 5, 10, 0, 0, 0, 0, DateTimeKind.Local), "Improve performance of software that is developed.", "Performance improvements", 3, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 1 },
                    { new Guid("894f91e1-2a0a-40a6-962d-cbe518deefe6"), new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), 100, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Local), "Create plan of automated testing of developed systems.", "Automated testing plan", 1, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 2 },
                    { new Guid("f7600c74-4eb0-41ac-9eff-4ff01a72c2b4"), new Guid("07d28657-02a1-47ad-be2f-7d0e0b4e45b1"), 50, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), "Perform functional testing of developed system.", "Functional testing", 2, new Guid("4d56d43a-092b-4023-82e0-870484a1223d"), 1 }
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
