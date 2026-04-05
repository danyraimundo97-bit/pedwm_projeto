using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectType = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    HolidayHours = table.Column<double>(type: "REAL", nullable: true),
                    AllocatedHours = table.Column<int>(type: "INTEGER", nullable: true),
                    ClientName = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectStatus = table.Column<string>(type: "TEXT", nullable: true),
                    ManagerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MedicalCertificateId = table.Column<string>(type: "TEXT", nullable: true),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: true),
                    MissedHours = table.Column<double>(type: "REAL", nullable: true),
                    CourseName = table.Column<string>(type: "TEXT", nullable: true),
                    CertificationLink = table.Column<string>(type: "TEXT", nullable: true),
                    Hours = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TaskType = table.Column<int>(type: "INTEGER", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: true),
                    Environment = table.Column<string>(type: "TEXT", nullable: true),
                    StoryPoints = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
