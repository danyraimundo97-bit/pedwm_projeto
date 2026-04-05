using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Projects");

            migrationBuilder.AlterColumn<double>(
                name: "AllocatedHours",
                table: "Projects",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Projects",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "AllocatedHours",
                table: "Projects",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectStatus",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }
    }
}
