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
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
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
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "laterality",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    laterality_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_laterality", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "view_position",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    view_position_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_view_position", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FolderDoctorEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderDoctorEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderDoctorEntities_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderDoctorEntities_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FotografPath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    laterality_id = table.Column<int>(type: "int", nullable: false),
                    view_position_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Image_laterality_laterality_id",
                        column: x => x.laterality_id,
                        principalTable: "laterality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Image_view_position_view_position_id",
                        column: x => x.view_position_id,
                        principalTable: "view_position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BreastBiradsEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    BreastBiradsId = table.Column<int>(type: "int", nullable: true),
                    DoctorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreastBiradsEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreastBiradsEntities_BreastBirads_BreastBiradsId",
                        column: x => x.BreastBiradsId,
                        principalTable: "BreastBirads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BreastBiradsEntities_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreastBiradsEntities_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FindingCategoriesEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    FindingCategoriesId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingCategoriesEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingCategoriesEntities_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingCategoriesEntities_FindingCategories_FindingCategorie~",
                        column: x => x.FindingCategoriesId,
                        principalTable: "FindingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingCategoriesEntities_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
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
                table: "Doctor",
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
                    { 5, "Suspicious Calcification" },
                    { 6, "Focal Asymmetry" },
                    { 7, "Asymmetry" }
                });

            migrationBuilder.InsertData(
                table: "Folder",
                columns: new[] { "Id", "FolderPath" },
                values: new object[] { 1, "0a0c5108270e814818c1ad002482ce74" });

            migrationBuilder.InsertData(
                table: "laterality",
                columns: new[] { "id", "laterality_name" },
                values: new object[,]
                {
                    { 1, "R" },
                    { 2, "L" }
                });

            migrationBuilder.InsertData(
                table: "view_position",
                columns: new[] { "id", "view_position_name" },
                values: new object[,]
                {
                    { 1, "CC" },
                    { 2, "MLO" }
                });

            migrationBuilder.InsertData(
                table: "FolderDoctorEntities",
                columns: new[] { "Id", "DoctorId", "FolderId" },
                values: new object[,]
                {
                    { 1, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 1 },
                    { 2, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 1 }
                });

            migrationBuilder.InsertData(
                table: "Image",
                columns: new[] { "Id", "FolderId", "FotografPath", "laterality_id", "view_position_id" },
                values: new object[,]
                {
                    { 1, 1, "0a6a90bdc088e0cc62df8d2d58d14840.png", 1, 2 },
                    { 2, 1, "1b66d3ea1dae116b7c0e87e3caab3340.png", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "BreastBiradsEntities",
                columns: new[] { "Id", "BreastBiradsId", "DoctorId", "ImageId" },
                values: new object[,]
                {
                    { 1, 1, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 1 },
                    { 2, 2, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 1 },
                    { 3, 4, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 2 },
                    { 4, 3, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 2 }
                });

            migrationBuilder.InsertData(
                table: "FindingCategoriesEntities",
                columns: new[] { "Id", "DoctorId", "FindingCategoriesId", "ImageId" },
                values: new object[,]
                {
                    { 1, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 1, 1 },
                    { 2, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 2, 1 },
                    { 3, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 3, 1 },
                    { 4, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreastBiradsEntities_BreastBiradsId",
                table: "BreastBiradsEntities",
                column: "BreastBiradsId");

            migrationBuilder.CreateIndex(
                name: "IX_BreastBiradsEntities_DoctorId",
                table: "BreastBiradsEntities",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_BreastBiradsEntities_ImageId",
                table: "BreastBiradsEntities",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategoriesEntities_DoctorId",
                table: "FindingCategoriesEntities",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategoriesEntities_FindingCategoriesId",
                table: "FindingCategoriesEntities",
                column: "FindingCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategoriesEntities_ImageId",
                table: "FindingCategoriesEntities",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderDoctorEntities_DoctorId",
                table: "FolderDoctorEntities",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderDoctorEntities_FolderId",
                table: "FolderDoctorEntities",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_FolderId",
                table: "Image",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_laterality_id",
                table: "Image",
                column: "laterality_id");

            migrationBuilder.CreateIndex(
                name: "IX_Image_view_position_id",
                table: "Image",
                column: "view_position_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreastBiradsEntities");

            migrationBuilder.DropTable(
                name: "FindingCategoriesEntities");

            migrationBuilder.DropTable(
                name: "FolderDoctorEntities");

            migrationBuilder.DropTable(
                name: "BreastBirads");

            migrationBuilder.DropTable(
                name: "FindingCategories");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "laterality");

            migrationBuilder.DropTable(
                name: "view_position");
        }
    }
}
