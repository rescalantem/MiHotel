using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiHotel.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class index2key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accesos_Estancias_EstanciaHabitacionId_EstanciaHuespedId",
                table: "Accesos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estancias",
                table: "Estancias");

            migrationBuilder.DropIndex(
                name: "IX_Accesos_EstanciaHabitacionId_EstanciaHuespedId",
                table: "Accesos");

            migrationBuilder.DropColumn(
                name: "EstanciaHabitacionId",
                table: "Accesos");

            migrationBuilder.DropColumn(
                name: "EstanciaHuespedId",
                table: "Accesos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estancias",
                table: "Estancias",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Hoteles",
                columns: new[] { "Id", "RazonSocial" },
                values: new object[] { 1, "Hotel León" });

            migrationBuilder.InsertData(
                table: "Huespedes",
                columns: new[] { "Id", "Nombre", "Telefono" },
                values: new object[] { 1, "Judith Rangel Diaz", "1234567890" });

            migrationBuilder.InsertData(
                table: "Habitaciones",
                columns: new[] { "Id", "Foto", "HotelId", "NumeroStr", "Ocupada" },
                values: new object[,]
                {
                    { 1, null, 1, "102", false },
                    { 2, null, 1, "202", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estancias_HabitacionId_HuespedId",
                table: "Estancias",
                columns: new[] { "HabitacionId", "HuespedId" });

            migrationBuilder.CreateIndex(
                name: "IX_Accesos_EstanciaId",
                table: "Accesos",
                column: "EstanciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accesos_Estancias_EstanciaId",
                table: "Accesos",
                column: "EstanciaId",
                principalTable: "Estancias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accesos_Estancias_EstanciaId",
                table: "Accesos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estancias",
                table: "Estancias");

            migrationBuilder.DropIndex(
                name: "IX_Estancias_HabitacionId_HuespedId",
                table: "Estancias");

            migrationBuilder.DropIndex(
                name: "IX_Accesos_EstanciaId",
                table: "Accesos");

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Huespedes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EstanciaHabitacionId",
                table: "Accesos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstanciaHuespedId",
                table: "Accesos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estancias",
                table: "Estancias",
                columns: new[] { "HabitacionId", "HuespedId" });

            migrationBuilder.CreateIndex(
                name: "IX_Accesos_EstanciaHabitacionId_EstanciaHuespedId",
                table: "Accesos",
                columns: new[] { "EstanciaHabitacionId", "EstanciaHuespedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Accesos_Estancias_EstanciaHabitacionId_EstanciaHuespedId",
                table: "Accesos",
                columns: new[] { "EstanciaHabitacionId", "EstanciaHuespedId" },
                principalTable: "Estancias",
                principalColumns: new[] { "HabitacionId", "HuespedId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
