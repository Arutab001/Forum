using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueForumAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateThreadsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Thread_ThreadId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Thread",
                table: "Thread");

            migrationBuilder.RenameTable(
                name: "Thread",
                newName: "Threads");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Threads",
                table: "Threads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Threads_ThreadId",
                table: "Posts",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Threads_ThreadId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Threads",
                table: "Threads");

            migrationBuilder.RenameTable(
                name: "Threads",
                newName: "Thread");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Thread",
                table: "Thread",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Thread_ThreadId",
                table: "Posts",
                column: "ThreadId",
                principalTable: "Thread",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
