using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlBodega.Migrations
{
    /// <inheritdoc />
    public partial class ConsumoPropsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumos_Egresos_EgresoIdEgreso",
                table: "Consumos");

            migrationBuilder.DropForeignKey(
                name: "FK_EgresosAlimentacion_Plantas_IdPlanta",
                table: "EgresosAlimentacion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgresosAlimentacion",
                table: "EgresosAlimentacion");

            migrationBuilder.RenameTable(
                name: "EgresosAlimentacion",
                newName: "EgresoAlimentaciones");

            migrationBuilder.RenameIndex(
                name: "IX_EgresosAlimentacion_IdPlanta",
                table: "EgresoAlimentaciones",
                newName: "IX_EgresoAlimentaciones_IdPlanta");

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Inventarios",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "EgresoIdEgreso",
                table: "Consumos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Consumos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPlanta",
                table: "Consumos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlantaIdPlanta",
                table: "Consumos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "EgresoAlimentaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgresoAlimentaciones",
                table: "EgresoAlimentaciones",
                column: "IdEgresoA");

            migrationBuilder.CreateIndex(
                name: "IX_Consumos_PlantaIdPlanta",
                table: "Consumos",
                column: "PlantaIdPlanta");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumos_Egresos_EgresoIdEgreso",
                table: "Consumos",
                column: "EgresoIdEgreso",
                principalTable: "Egresos",
                principalColumn: "IdEgreso");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumos_Plantas_PlantaIdPlanta",
                table: "Consumos",
                column: "PlantaIdPlanta",
                principalTable: "Plantas",
                principalColumn: "IdPlanta");

            migrationBuilder.AddForeignKey(
                name: "FK_EgresoAlimentaciones_Plantas_IdPlanta",
                table: "EgresoAlimentaciones",
                column: "IdPlanta",
                principalTable: "Plantas",
                principalColumn: "IdPlanta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumos_Egresos_EgresoIdEgreso",
                table: "Consumos");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumos_Plantas_PlantaIdPlanta",
                table: "Consumos");

            migrationBuilder.DropForeignKey(
                name: "FK_EgresoAlimentaciones_Plantas_IdPlanta",
                table: "EgresoAlimentaciones");

            migrationBuilder.DropIndex(
                name: "IX_Consumos_PlantaIdPlanta",
                table: "Consumos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgresoAlimentaciones",
                table: "EgresoAlimentaciones");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "IdPlanta",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "PlantaIdPlanta",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "EgresoAlimentaciones");

            migrationBuilder.RenameTable(
                name: "EgresoAlimentaciones",
                newName: "EgresosAlimentacion");

            migrationBuilder.RenameIndex(
                name: "IX_EgresoAlimentaciones_IdPlanta",
                table: "EgresosAlimentacion",
                newName: "IX_EgresosAlimentacion_IdPlanta");

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Inventarios",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<int>(
                name: "EgresoIdEgreso",
                table: "Consumos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgresosAlimentacion",
                table: "EgresosAlimentacion",
                column: "IdEgresoA");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumos_Egresos_EgresoIdEgreso",
                table: "Consumos",
                column: "EgresoIdEgreso",
                principalTable: "Egresos",
                principalColumn: "IdEgreso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EgresosAlimentacion_Plantas_IdPlanta",
                table: "EgresosAlimentacion",
                column: "IdPlanta",
                principalTable: "Plantas",
                principalColumn: "IdPlanta");
        }
    }
}
