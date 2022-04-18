using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChemSolution_re_API.Migrations
{
    public partial class fix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementValence");

            migrationBuilder.DropTable(
                name: "Valences");

            migrationBuilder.CreateTable(
                name: "ElementValences",
                columns: table => new
                {
                    Valence = table.Column<byte>(type: "tinyint", nullable: false),
                    ElementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementValences", x => new { x.ElementId, x.Valence });
                    table.ForeignKey(
                        name: "FK_ElementValences_Elements_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementValences");

            migrationBuilder.CreateTable(
                name: "Valences",
                columns: table => new
                {
                    ValenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valences", x => x.ValenceId);
                });

            migrationBuilder.CreateTable(
                name: "ElementValence",
                columns: table => new
                {
                    ElementsElementId = table.Column<int>(type: "int", nullable: false),
                    ValencesValenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementValence", x => new { x.ElementsElementId, x.ValencesValenceId });
                    table.ForeignKey(
                        name: "FK_ElementValence_Elements_ElementsElementId",
                        column: x => x.ElementsElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementValence_Valences_ValencesValenceId",
                        column: x => x.ValencesValenceId,
                        principalTable: "Valences",
                        principalColumn: "ValenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Valences",
                column: "ValenceId",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementValence_ValencesValenceId",
                table: "ElementValence",
                column: "ValencesValenceId");
        }
    }
}
