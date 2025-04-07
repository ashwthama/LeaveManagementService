using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveServiceMS.Migrations
{
    /// <inheritdoc />
    public partial class fouthMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "LeaveRequests");
        }
    }
}
