using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Week.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserToken");

            migrationBuilder.RenameColumn(
                name: "IActive",
                table: "UserToken",
                newName: "IsActive");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserToken",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserTokenId",
                table: "UserToken",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CodeRefreshToken",
                table: "UserToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToken",
                table: "UserToken",
                column: "UserTokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToken",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "UserTokenId",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "CodeRefreshToken",
                table: "UserToken");

            migrationBuilder.RenameTable(
                name: "UserToken",
                newName: "UserTokens");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "UserTokens",
                newName: "IActive");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserTokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                column: "UserId");
        }
    }
}
