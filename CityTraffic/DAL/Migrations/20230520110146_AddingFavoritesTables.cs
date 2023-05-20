using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityTraffic.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingFavoritesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "Stoppoints");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TransportRoutes",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TransportRoutes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RouteTypeId",
                table: "TransportRoutes",
                newName: "route_type_id");

            migrationBuilder.RenameColumn(
                name: "RouteNumber",
                table: "TransportRoutes",
                newName: "route_number");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "TransportRoutes",
                newName: "route_id");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Stoppoints",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Stoppoints",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stoppoints",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StoppointName",
                table: "Stoppoints",
                newName: "stoppoint_name");

            migrationBuilder.RenameColumn(
                name: "StoppointId",
                table: "Stoppoints",
                newName: "stoppoint_id");

            migrationBuilder.CreateTable(
                name: "FavoritesStoppoints",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    stoppoint_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesStoppoints", x => x.id);
                    table.ForeignKey(
                        name: "FK_FavoritesStoppoints_Stoppoints_stoppoint_id",
                        column: x => x.stoppoint_id,
                        principalTable: "Stoppoints",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritesTransportRoutes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    transport_route_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesTransportRoutes", x => x.id);
                    table.ForeignKey(
                        name: "FK_FavoritesTransportRoutes_TransportRoutes_transport_route_id",
                        column: x => x.transport_route_id,
                        principalTable: "TransportRoutes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesStoppoints_stoppoint_id",
                table: "FavoritesStoppoints",
                column: "stoppoint_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesTransportRoutes_transport_route_id",
                table: "FavoritesTransportRoutes",
                column: "transport_route_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritesStoppoints");

            migrationBuilder.DropTable(
                name: "FavoritesTransportRoutes");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "TransportRoutes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TransportRoutes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "route_type_id",
                table: "TransportRoutes",
                newName: "RouteTypeId");

            migrationBuilder.RenameColumn(
                name: "route_number",
                table: "TransportRoutes",
                newName: "RouteNumber");

            migrationBuilder.RenameColumn(
                name: "route_id",
                table: "TransportRoutes",
                newName: "RouteId");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Stoppoints",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Stoppoints",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Stoppoints",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "stoppoint_name",
                table: "Stoppoints",
                newName: "StoppointName");

            migrationBuilder.RenameColumn(
                name: "stoppoint_id",
                table: "Stoppoints",
                newName: "StoppointId");

            migrationBuilder.AddColumn<bool>(
                name: "Favorites",
                table: "TransportRoutes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Favorites",
                table: "Stoppoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
