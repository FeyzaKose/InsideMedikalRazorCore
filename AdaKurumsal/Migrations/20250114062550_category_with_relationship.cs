using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdaKurumsal.Migrations
{
    /// <inheritdoc />
    public partial class category_with_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainCategoryCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.UniqueConstraint("AK_Categories_Code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_MainCategoryCode",
                        column: x => x.MainCategoryCode,
                        principalTable: "Categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$eiGAbwlElSWksebI63TwSu/L90NWiffYVe4dlGnTyPJwCj/Dfz4Le");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MainCategoryCode",
                table: "Categories",
                column: "MainCategoryCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$QnPXcU3/yjGO8lgKGLh0PesFsj.5YeF7hUlwaw29UiFgvTzfJ0UHG");
        }
    }
}
