using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class sessaoTempo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataLogin",
                table: "SessoesUsuarios",
                newName: "UltimaRequisicao");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "SessoesUsuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpiracao",
                table: "SessoesUsuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "SessoesUsuarios");

            migrationBuilder.DropColumn(
                name: "DataExpiracao",
                table: "SessoesUsuarios");

            migrationBuilder.RenameColumn(
                name: "UltimaRequisicao",
                table: "SessoesUsuarios",
                newName: "DataLogin");
        }
    }
}
