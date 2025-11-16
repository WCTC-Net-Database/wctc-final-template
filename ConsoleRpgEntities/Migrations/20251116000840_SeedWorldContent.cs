using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class SeedWorldContent : BaseMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            RunSql(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            RunSqlRollback(migrationBuilder);
        }
    }
}
