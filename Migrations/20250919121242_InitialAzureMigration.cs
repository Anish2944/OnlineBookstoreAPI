using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineBookstoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialAzureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://tse4.mm.bing.net/th/id/OIP.s-upM5SIYvp6QxN-naltXQHaLH?rs=1&pid=ImgDetMain&o=7&rm=3");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://pictures.abebooks.com/isbn/9780593043165-uk.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6d/Sapiens-_A_Brief_History_of_Humankind.png/800px-Sapiens-_A_Brief_History_of_Humankind.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/1984.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/brief_history.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/sapiens.jpg");
        }
    }
}
