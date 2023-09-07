using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddListRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Material_RequestMaterialId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Request_RequestMaterialId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestMaterialId",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_RequestId",
                table: "Materials",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Request_RequestId",
                table: "Materials",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Request_RequestId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Material_RequestId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Materials");

            migrationBuilder.AddColumn<int>(
                name: "RequestMaterialId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Request_RequestMaterialId",
                table: "Requests",
                column: "RequestMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Material_RequestMaterialId",
                table: "Requests",
                column: "RequestMaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
