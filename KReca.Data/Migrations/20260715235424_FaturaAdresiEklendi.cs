using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KReca.Data.Migrations
{
    /// <inheritdoc />
    public partial class FaturaAdresiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaturaAdresi",
                table: "Siparisler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaturaSehri",
                table: "Siparisler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirmaAdi",
                table: "Siparisler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TcVeyaVergiNo",
                table: "Siparisler",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 7, 15, 23, 54, 23, 593, DateTimeKind.Utc).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 2,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 7, 15, 23, 54, 23, 593, DateTimeKind.Utc).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "SiteAyarlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuncellenmeTarihi",
                value: new DateTime(2026, 7, 15, 23, 54, 23, 593, DateTimeKind.Utc).AddTicks(4790));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaturaAdresi",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "FaturaSehri",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "FirmaAdi",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "TcVeyaVergiNo",
                table: "Siparisler");

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 7, 6, 11, 26, 54, 967, DateTimeKind.Utc).AddTicks(6772));

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 2,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 7, 6, 11, 26, 54, 967, DateTimeKind.Utc).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "SiteAyarlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "GuncellenmeTarihi",
                value: new DateTime(2026, 7, 6, 11, 26, 54, 967, DateTimeKind.Utc).AddTicks(6901));
        }
    }
}
