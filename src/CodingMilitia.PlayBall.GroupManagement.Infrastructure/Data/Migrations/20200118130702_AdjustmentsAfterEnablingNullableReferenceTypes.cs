using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Migrations
{
    public partial class AdjustmentsAfterEnablingNullableReferenceTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_CreatorId",
                schema: "public",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Groups_GroupId",
                schema: "public",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                schema: "public",
                table: "Players",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                schema: "public",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_CreatorId",
                schema: "public",
                table: "Groups",
                column: "CreatorId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Groups_GroupId",
                schema: "public",
                table: "Players",
                column: "GroupId",
                principalSchema: "public",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_CreatorId",
                schema: "public",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Groups_GroupId",
                schema: "public",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Players",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                schema: "public",
                table: "Players",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Groups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                schema: "public",
                table: "Groups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_CreatorId",
                schema: "public",
                table: "Groups",
                column: "CreatorId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Groups_GroupId",
                schema: "public",
                table: "Players",
                column: "GroupId",
                principalSchema: "public",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
