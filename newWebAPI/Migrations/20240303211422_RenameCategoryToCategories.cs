using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategoryToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catgory_CatgoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catgory",
                table: "Catgory");

            migrationBuilder.RenameTable(
                name: "Catgory",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CatgoryId",
                table: "Products",
                column: "CatgoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CatgoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Catgory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catgory",
                table: "Catgory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catgory_CatgoryId",
                table: "Products",
                column: "CatgoryId",
                principalTable: "Catgory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
