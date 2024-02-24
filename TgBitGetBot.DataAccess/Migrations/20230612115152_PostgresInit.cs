using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBitGetBot.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class PostgresInit : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "text", nullable: true),
					TelegramId = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserStates",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					TelegramId = table.Column<long>(type: "bigint", nullable: false),
					State = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserStates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserApiInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Token = table.Column<string>(type: "text", nullable: true),
					Passphrase = table.Column<string>(type: "text", nullable: true),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserApiInfos", x => x.Id);
					table.ForeignKey(
						name: "FK_UserApiInfos_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserApiInfos_UserId",
				table: "UserApiInfos",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserApiInfos");

			migrationBuilder.DropTable(
				name: "UserStates");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
