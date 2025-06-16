using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDoktorSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Doktor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Soyad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktor", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Etiket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EtiketAd = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiket", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fotograf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FotografPath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotograf", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FotografEtiket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FotografId = table.Column<int>(type: "int", nullable: false),
                    EtiketId = table.Column<int>(type: "int", nullable: true),
                    DoktorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EtiketTarihi = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotografEtiket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotografEtiket_Doktor_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FotografEtiket_Etiket_EtiketId",
                        column: x => x.EtiketId,
                        principalTable: "Etiket",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FotografEtiket_Fotograf_FotografId",
                        column: x => x.FotografId,
                        principalTable: "Fotograf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Doktor",
                columns: new[] { "Id", "Ad", "Email", "Soyad" },
                values: new object[,]
                {
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "Ali", "ali@example.com", "Yılmaz" },
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c5555"), "Fatma", "fatma@example.com", "Çelik" },
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), "Ayşe", "ayse@example.com", "Kaya" },
                    { new Guid("3f2504e0-abcd-11d3-9a0c-0305e82c1111"), "Mehmet", "mehmet@example.com", "Demir" }
                });

            migrationBuilder.InsertData(
                table: "Etiket",
                columns: new[] { "Id", "EtiketAd" },
                values: new object[,]
                {
                    { 1, "A" },
                    { 2, "B" },
                    { 3, "C" }
                });

            migrationBuilder.InsertData(
                table: "Fotograf",
                columns: new[] { "Id", "FotografPath" },
                values: new object[,]
                {
                    { 1, "cat.1.jpg" },
                    { 2, "cat.2.jpg" },
                    { 3, "cat.3.jpg" },
                    { 4, "cat.4.jpg" }
                });

            migrationBuilder.InsertData(
                table: "FotografEtiket",
                columns: new[] { "Id", "DoktorId", "EtiketId", "EtiketTarihi", "FotografId" },
                values: new object[,]
                {
                    { 1, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 1, new DateOnly(2028, 5, 21), 1 },
                    { 2, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 2, new DateOnly(2022, 2, 5), 1 },
                    { 3, new Guid("3f2504e0-abcd-11d3-9a0c-0305e82c1111"), 2, new DateOnly(2021, 1, 2), 2 },
                    { 4, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c5555"), 3, new DateOnly(2023, 3, 8), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_DoktorId",
                table: "FotografEtiket",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_EtiketId",
                table: "FotografEtiket",
                column: "EtiketId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_FotografId",
                table: "FotografEtiket",
                column: "FotografId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotografEtiket");

            migrationBuilder.DropTable(
                name: "Doktor");

            migrationBuilder.DropTable(
                name: "Etiket");

            migrationBuilder.DropTable(
                name: "Fotograf");
        }
    }
}
