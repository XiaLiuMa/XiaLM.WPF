using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XiaLM.P101.Quartz.Migrations
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobName = table.Column<string>(nullable: true),
                    JobGroup = table.Column<string>(nullable: true),
                    Cron = table.Column<string>(nullable: true),
                    AssemblyName = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    RunTimes = table.Column<int>(nullable: false),
                    BeginTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    TriggerType = table.Column<int>(nullable: false),
                    IntervalSecond = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Valid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
