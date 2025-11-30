using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentBazaar.Web.Migrations
{
    /// <inheritdoc />
    public partial class addprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_StudyYears_StudyYearId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StudyYearId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StudyYearId",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "StudyYearId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StudyYearId",
                table: "Products",
                column: "StudyYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_StudyYears_StudyYearId",
                table: "Products",
                column: "StudyYearId",
                principalTable: "StudyYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
