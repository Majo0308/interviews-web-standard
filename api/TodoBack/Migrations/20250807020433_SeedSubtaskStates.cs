using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoBack.Migrations
{
    /// <inheritdoc />
    public partial class SeedSubtaskStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "SubtaskStates",
            columns: new[] { "SubtaskStateId", "StateName" },
            values: new object[,]
            {
                { 1, "Pending" },
                { 2, "In Progress" },
                { 3, "Completed" }
            });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubtaskStates",
                keyColumn: "SubtaskStateId",
                keyValues: new object[]
                {
                    1,
                    2,
                    3
                });
        }
    }
}
