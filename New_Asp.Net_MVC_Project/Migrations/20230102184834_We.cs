using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewAsp.NetMVCProject.Migrations
{
    /// <inheritdoc />
    public partial class We : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Register_RegisterId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "Register");

            migrationBuilder.DropIndex(
                name: "IX_Notes_RegisterId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Register",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Register", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_RegisterId",
                table: "Notes",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Register_RegisterId",
                table: "Notes",
                column: "RegisterId",
                principalTable: "Register",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
