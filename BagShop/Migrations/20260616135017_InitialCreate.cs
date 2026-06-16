using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BagShop.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Bags",
                columns: table => new
                {
                    BagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bags", x => x.BagId);
                    table.ForeignKey(
                        name: "FK_Bags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FullName", "Password" },
                values: new object[] { 1, "sara@gmail.com", "Sara Ahmad", "123" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FullName", "Password" },
                values: new object[] { 2, "rawan@gmail.com", "Rawan Ali", "456" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FullName", "Password" },
                values: new object[] { 3, "noor@gmail.com", "Noor Omar", "789" });

            migrationBuilder.InsertData(
                table: "Bags",
                columns: new[] { "BagId", "BagName", "Brand", "Color", "Price", "UserId" },
                values: new object[,]
                {
                    { 1, "Classic Leather Bag", "Zara", "Black", 45.99m, 1 },
                    { 2, "Mini Crossbody Bag", "Mango", "Beige", 29.99m, 1 },
                    { 3, "Travel Backpack", "Nike", "Gray", 65.00m, 2 },
                    { 4, "Elegant Handbag", "Guess", "Brown", 75.50m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bags_UserId",
                table: "Bags",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
