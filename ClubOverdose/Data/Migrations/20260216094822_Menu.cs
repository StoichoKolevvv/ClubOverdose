using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubOverdose.Data.Migrations
{
    /// <inheritdoc />
    public partial class Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Drinks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_MenuId",
                table: "Drinks",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Menus_MenuId",
                table: "Drinks",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Menus_MenuId",
                table: "Drinks");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_MenuId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Drinks");
        }
    }
}
