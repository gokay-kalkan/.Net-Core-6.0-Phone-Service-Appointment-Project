using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneService.Migrations
{
    public partial class problemandpricesrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProblemId",
                table: "Prices",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Problems_ProblemId",
                table: "Prices",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Problems_ProblemId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ProblemId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Prices");
        }
    }
}
