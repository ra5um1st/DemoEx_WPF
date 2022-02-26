using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoEx.Domain.Migrations
{
    public partial class Image_LanguageServiceImages_Tables_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageServiceImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    LanguageServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageServiceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageServiceImages_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageServiceImages_LaguageServices_LanguageServiceId",
                        column: x => x.LanguageServiceId,
                        principalTable: "LaguageServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageServiceImages_ImageId",
                table: "LanguageServiceImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageServiceImages_LanguageServiceId",
                table: "LanguageServiceImages",
                column: "LanguageServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageServiceImages");

            migrationBuilder.DropTable(
                name: "Image");
        }
    }
}
