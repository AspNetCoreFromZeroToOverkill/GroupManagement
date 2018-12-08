using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingMilitia.PlayBall.GroupManagement.Data.Migrations
{
    public partial class AddConcurrencyToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "public",
                table: "Groups",
                type: "xid",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "public",
                table: "Groups");
        }
    }
}
