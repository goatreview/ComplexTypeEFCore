using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplexTypeEFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Field_Size",
                table: "Goats",
                newName: "Field_Hectare");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Field_Hectare",
                table: "Goats",
                newName: "Field_Size");
        }
    }
}
