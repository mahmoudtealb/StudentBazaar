using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentBazaar.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ownerId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ownerId",
                table: "Products",
                column: "ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_ownerId",
                table: "Products",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_ownerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ownerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ownerId",
                table: "Products");
        }
    }
}
