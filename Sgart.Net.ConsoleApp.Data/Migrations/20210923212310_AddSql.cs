using Microsoft.EntityFrameworkCore.Migrations;

namespace Sgart.Net.ConsoleApp.Data.Migrations
{
    public partial class AddSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // esempio di come eseguire un qualsiai T-SQL, ad esempio per aggiungere una store
            migrationBuilder.Sql(@"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[spu_GetTodos]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT *
	FROM [dbo].[Todos]
	ORDER BY [TodoId]

END
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
