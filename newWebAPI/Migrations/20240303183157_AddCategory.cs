using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatgoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Catgory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catgory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CatgoryId",
                table: "Products",
                column: "CatgoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catgory_CatgoryId",
                table: "Products",
                column: "CatgoryId",
                principalTable: "Catgory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catgory_CatgoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Catgory");

            migrationBuilder.DropIndex(
                name: "IX_Products_CatgoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CatgoryId",
                table: "Products");
        }
    }
}
