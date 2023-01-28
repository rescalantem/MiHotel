using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiHotel.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class IndiceTelefono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Huespedes_Telefono",
                table: "Huespedes");

            migrationBuilder.CreateIndex(
                name: "IX_Huespedes_Telefono",
                table: "Huespedes",
                column: "Telefono",
                unique: true,
                filter: "[Telefono] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Huespedes_Telefono",
                table: "Huespedes");

            migrationBuilder.CreateIndex(
                name: "IX_Huespedes_Telefono",
                table: "Huespedes",
                column: "Telefono");
        }
    }
}
