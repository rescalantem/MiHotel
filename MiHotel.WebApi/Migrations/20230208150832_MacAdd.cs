using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiHotel.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class MacAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Habitaciones");

            migrationBuilder.AddColumn<string>(
                name: "EspMacAdd",
                table: "Habitaciones",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "EspMacAdd",
                value: "00:00:00:00:00:01");

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 2,
                column: "EspMacAdd",
                value: "00:00:00:00:00:02");

            migrationBuilder.CreateIndex(
                name: "IX_Habitaciones_EspMacAdd",
                table: "Habitaciones",
                column: "EspMacAdd",
                unique: true,
                filter: "[EspMacAdd] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Habitaciones_EspMacAdd",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "EspMacAdd",
                table: "Habitaciones");

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Habitaciones",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Foto",
                value: null);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 2,
                column: "Foto",
                value: null);
        }
    }
}
