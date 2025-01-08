using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdaKurumsal.Migrations
{
    /// <inheritdoc />
    public partial class first_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(80)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isConfirm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Iletisim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dil = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Telefon1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Telefon2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FooterAdresSatir = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    FooterAdresSatir2 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    AdresSatir1 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    AdresSatir2 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iletisim", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "RolName", "isActive" },
                values: new object[,]
                {
                    { 1, "Developer", true },
                    { 2, "Admin", true }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "Id", "RolId", "UserId", "isActive" },
                values: new object[] { 1, 1, 1, true });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Email", "Password", "UserName", "isActive", "isConfirm" },
                values: new object[] { 1, "kose.feyza@gmail.com", "$2a$11$fTu9FYpZCFyCi65kwPqWAeNELjMOcK.3Hie0.kynBG2fy29zkfuhm", "Feyza Köse", true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Iletisim");
        }
    }
}
