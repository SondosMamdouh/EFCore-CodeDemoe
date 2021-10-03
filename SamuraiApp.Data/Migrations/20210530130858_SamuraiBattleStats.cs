﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SamuraiBattleStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE OR ALTER VIEW dbo.SamuraiBattleStats
                AS
                SELECT dbo.SamuraiBattle.SamuraiId, dbo.Samurais.Name,
                COUNT(dbo.SamuraiBattle.BattleId) AS NumbarOfBattles
                from dbo.SamuraiBattle INNER JOIN 
                dbo.Samurais ON dbo.SamuraiBattle.SamuraiId = dbo.Samurais.Id
                GROUP BY dbo.Samurais.Name , dbo.SamuraiBattle.SamuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP VIEW dbo.SamuraiBattleStats");
        }
    }
}
