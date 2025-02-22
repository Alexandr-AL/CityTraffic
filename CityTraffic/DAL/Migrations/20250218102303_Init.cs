using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityTraffic.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stoppoints",
                columns: table => new
                {
                    StoppointId = table.Column<int>(type: "INTEGER", nullable: false),
                    StoppointName = table.Column<string>(type: "TEXT", nullable: true),
                    location = table.Column<string>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoppoints", x => x.StoppointId);
                });

            migrationBuilder.CreateTable(
                name: "TransportRoutes",
                columns: table => new
                {
                    RouteId = table.Column<string>(type: "TEXT", nullable: false),
                    RouteNumber = table.Column<string>(type: "TEXT", nullable: true),
                    RouteTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRoutes", x => x.RouteId);
                });

            migrationBuilder.CreateTable(
                name: "StoppointEntityTransportRouteEntity",
                columns: table => new
                {
                    RoutesRouteId = table.Column<string>(type: "TEXT", nullable: false),
                    StoppointsStoppointId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoppointEntityTransportRouteEntity", x => new { x.RoutesRouteId, x.StoppointsStoppointId });
                    table.ForeignKey(
                        name: "FK_StoppointEntityTransportRouteEntity_Stoppoints_StoppointsStoppointId",
                        column: x => x.StoppointsStoppointId,
                        principalTable: "Stoppoints",
                        principalColumn: "StoppointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoppointEntityTransportRouteEntity_TransportRoutes_RoutesRouteId",
                        column: x => x.RoutesRouteId,
                        principalTable: "TransportRoutes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoppointEntityTransportRouteEntity_StoppointsStoppointId",
                table: "StoppointEntityTransportRouteEntity",
                column: "StoppointsStoppointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoppointEntityTransportRouteEntity");

            migrationBuilder.DropTable(
                name: "Stoppoints");

            migrationBuilder.DropTable(
                name: "TransportRoutes");
        }
    }
}
