using Microsoft.EntityFrameworkCore.Migrations;

namespace dbLabsDummy.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var sp = @"CREATE PROCEDURE [maskshop].[viewproducts]
                    @amount_min int
                AS
                BEGIN
                    SET NOCOUNT ON;
					SELECT * FROM maskShop.ShopItems where amount>@amount_min
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
