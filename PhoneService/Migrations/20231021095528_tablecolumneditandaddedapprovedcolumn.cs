using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneService.Migrations
{
    public partial class tablecolumneditandaddedapprovedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Admins");
        }
    }
}
