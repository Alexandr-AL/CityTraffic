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
                    stoppoint_id = table.Column<int>(type: "INTEGER", nullable: false)
                        /*.Annotation("Sqlite:Autoincrement", true)*/,
                    favorite_stoppoint = table.Column<bool>(type: "INTEGER", nullable: false),
                    stoppoint_name = table.Column<string>(type: "TEXT", nullable: true),
                    note = table.Column<string>(type: "TEXT", nullable: true),
                    location = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoppoints", x => x.stoppoint_id);
                });

            migrationBuilder.CreateTable(
                name: "TransportRoutes",
                columns: table => new
                {
                    route_id = table.Column<string>(type: "TEXT", nullable: false),
                    favorite_transport_route = table.Column<bool>(type: "INTEGER", nullable: false),
                    route_number = table.Column<string>(type: "TEXT", nullable: true),
                    route_type_id = table.Column<int>(type: "INTEGER", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRoutes", x => x.route_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stoppoints");

            migrationBuilder.DropTable(
                name: "TransportRoutes");
        }
    }
}
