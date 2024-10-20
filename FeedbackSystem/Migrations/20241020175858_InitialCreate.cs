using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FeedbackSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "Comment", "CreatedAt", "CustomerId", "ProductId", "Rating" },
                values: new object[,]
                {
                    { 1, "Atque sit autem.", new DateTime(2023, 12, 7, 3, 35, 19, 284, DateTimeKind.Local).AddTicks(4932), 1, 0, 4 },
                    { 2, "Placeat quia sit sint.", new DateTime(2023, 11, 5, 20, 48, 54, 971, DateTimeKind.Local).AddTicks(5858), 2, 0, 3 },
                    { 3, "Et labore repellat corporis iusto et.", new DateTime(2024, 5, 29, 4, 22, 40, 441, DateTimeKind.Local).AddTicks(2945), 3, 0, 2 },
                    { 4, "Aperiam totam officiis corrupti beatae recusandae ea et.", new DateTime(2024, 4, 11, 14, 11, 38, 195, DateTimeKind.Local).AddTicks(7994), 4, 0, 5 },
                    { 5, "Nam qui eius voluptate et reprehenderit quis ipsum voluptas.", new DateTime(2024, 4, 4, 6, 1, 11, 660, DateTimeKind.Local).AddTicks(5611), 5, 0, 4 },
                    { 6, "Consequatur qui rerum facilis minus velit doloremque architecto nobis est.", new DateTime(2024, 8, 23, 18, 39, 30, 635, DateTimeKind.Local).AddTicks(9388), 6, 0, 1 },
                    { 7, "Accusamus earum aperiam ut dolorum fuga.", new DateTime(2024, 6, 22, 23, 52, 41, 258, DateTimeKind.Local).AddTicks(9616), 7, 0, 1 },
                    { 8, "Iure minima reprehenderit laborum esse quo recusandae delectus quos consequuntur.", new DateTime(2024, 4, 17, 21, 19, 5, 923, DateTimeKind.Local).AddTicks(2038), 8, 0, 3 },
                    { 9, "Accusamus fuga mollitia est soluta similique sunt velit.", new DateTime(2024, 8, 13, 9, 3, 5, 761, DateTimeKind.Local).AddTicks(839), 9, 0, 5 },
                    { 10, "Ducimus ut ut qui blanditiis debitis omnis et.", new DateTime(2024, 6, 6, 4, 26, 53, 2, DateTimeKind.Local).AddTicks(7078), 10, 0, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");
        }
    }
}
