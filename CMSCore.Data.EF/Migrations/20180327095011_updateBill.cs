using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CMSCore.Data.EF.Migrations
{
    public partial class updateBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRD_Bills_SEC_AppUsers_CustomerId",
                table: "PRD_Bills");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "PRD_Bills",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_PRD_Bills_SEC_AppUsers_CustomerId",
                table: "PRD_Bills",
                column: "CustomerId",
                principalTable: "SEC_AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRD_Bills_SEC_AppUsers_CustomerId",
                table: "PRD_Bills");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "PRD_Bills",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PRD_Bills_SEC_AppUsers_CustomerId",
                table: "PRD_Bills",
                column: "CustomerId",
                principalTable: "SEC_AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
