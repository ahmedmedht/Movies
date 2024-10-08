using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Migrations
{
    /// <inheritdoc />
    public partial class checkedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_ImageInfo_ImageInfoId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageInfo",
                table: "ImageInfo");

            migrationBuilder.RenameTable(
                name: "ImageInfo",
                newName: "Images");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Images_ImageInfoId",
                table: "Movies",
                column: "ImageInfoId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Images_ImageInfoId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ImageInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageInfo",
                table: "ImageInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_ImageInfo_ImageInfoId",
                table: "Movies",
                column: "ImageInfoId",
                principalTable: "ImageInfo",
                principalColumn: "Id");
        }
    }
}
