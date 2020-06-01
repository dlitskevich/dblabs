using Microsoft.EntityFrameworkCore.Migrations;

namespace dbLabsDummy.Migrations
{
    public partial class spviewproducts1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
              var sp = @"
	CREATE PROCEDURE `viewproducts`()
	BEGIN
	SELECT * FROM maskShop.ShopItems where amount>100;
	END;";

		migrationBuilder.Sql (sp);
                }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
		
        }
    }
}
