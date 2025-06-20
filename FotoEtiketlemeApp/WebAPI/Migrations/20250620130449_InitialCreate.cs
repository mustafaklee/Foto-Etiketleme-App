using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BreastBirads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreastBirads", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Doktor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktor", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FindingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FolderPath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DoktorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folder_Doktor_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fotograf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FotografPath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FolderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotograf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotograf_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FotografEtiket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    imageId = table.Column<int>(type: "int", nullable: false),
                    breast_biradsId = table.Column<int>(type: "int", nullable: true),
                    finding_categoriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotografEtiket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotografEtiket_BreastBirads_breast_biradsId",
                        column: x => x.breast_biradsId,
                        principalTable: "BreastBirads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FotografEtiket_FindingCategories_finding_categoriesId",
                        column: x => x.finding_categoriesId,
                        principalTable: "FindingCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FotografEtiket_Fotograf_imageId",
                        column: x => x.imageId,
                        principalTable: "Fotograf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "BreastBirads",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "BI-RADS 1" },
                    { 2, "BI-RADS 2" },
                    { 3, "BI-RADS 3" },
                    { 4, "BI-RADS 4" },
                    { 5, "BI-RADS 5" },
                    { 6, "BI-RADS 6" }
                });

            migrationBuilder.InsertData(
                table: "Doktor",
                columns: new[] { "Id", "Email" },
                values: new object[,]
                {
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "ali@example.com" },
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c5555"), "fatma@example.com" },
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), "ayse@example.com" },
                    { new Guid("3f2504e0-abcd-11d3-9a0c-0305e82c1111"), "mehmet@example.com" }
                });

            migrationBuilder.InsertData(
                table: "FindingCategories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Mass" },
                    { 2, "Global Asymmetry" },
                    { 3, "Architectural Distortion" },
                    { 4, "Nipple Retraction, Mass" },
                    { 5, "Suspicious Calcification,Focal Asymmetry" },
                    { 6, "Focal Asymmetry" },
                    { 7, "Asymmetry" }
                });

            migrationBuilder.InsertData(
                table: "Folder",
                columns: new[] { "Id", "DoktorId", "FolderPath" },
                values: new object[] { 1, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "memography/0a0c5108270e814818c1ad002482ce74" });

            migrationBuilder.InsertData(
                table: "Fotograf",
                columns: new[] { "Id", "FolderId", "FotografPath" },
                values: new object[,]
                {
                    { 1, 1, "0a6a90bdc088e0cc62df8d2d58d14840.png" },
                    { 2, 1, "1b66d3ea1dae116b7c0e87e3caab3340.png" },
                    { 3, 1, "7a3df96890c90370590984ca196d1b40.png" },
                    { 4, 1, "cb8a1b1282b4b16c0f322e9fc89a9c35.png" }
                });

            migrationBuilder.InsertData(
                table: "FotografEtiket",
                columns: new[] { "Id", "breast_biradsId", "finding_categoriesId", "imageId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 1, 2 },
                    { 3, 3, 5, 3 },
                    { 4, 3, 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folder_DoktorId",
                table: "Folder",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotograf_FolderId",
                table: "Fotograf",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_breast_biradsId",
                table: "FotografEtiket",
                column: "breast_biradsId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_finding_categoriesId",
                table: "FotografEtiket",
                column: "finding_categoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografEtiket_imageId",
                table: "FotografEtiket",
                column: "imageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotografEtiket");

            migrationBuilder.DropTable(
                name: "BreastBirads");

            migrationBuilder.DropTable(
                name: "FindingCategories");

            migrationBuilder.DropTable(
                name: "Fotograf");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "Doktor");
        }
    }
}
