using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApp.Data.Migrations
{
    public partial class AddProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblProduct",
                columns: tbl => new
                {
                    ID = tbl.Column<int>(type: "int", nullable: false),
                    ProductTitle = tbl.Column<string>(type: "nvarchar(50)", nullable: false),
                    ProductDescription = tbl.Column<string>(type: "nvarchar(250)", nullable: true),
                    DisplayImage = tbl.Column<byte[]>(type: "varbinary(max)", nullable: true),
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblProduct");
        }
    }
}
