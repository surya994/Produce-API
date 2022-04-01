using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class add_education_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiling_EducationId",
                table: "Profiling",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiling_Education_EducationId",
                table: "Profiling",
                column: "EducationId",
                principalTable: "Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiling_Education_EducationId",
                table: "Profiling");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropIndex(
                name: "IX_Profiling_EducationId",
                table: "Profiling");
        }
    }
}
