using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KReca.Data.Migrations
{
    /// <inheritdoc />
    public partial class SiteAyarlariVeIletisim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IletisimMesajlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    Mesaj = table.Column<string>(type: "text", nullable: false),
                    Okundu = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IletisimMesajlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteAyarlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KargoUcreti = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UcretsizKargoLimiti = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UcretsizKargoAktif = table.Column<bool>(type: "boolean", nullable: false),
                    DuyuruMetni = table.Column<string>(type: "text", nullable: true),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyarlari", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "SiteAyarlari",
                columns: new[] { "Id", "DuyuruMetni", "GuncellenmeTarihi", "KargoUcreti", "UcretsizKargoAktif", "UcretsizKargoLimiti" },
                values: new object[] { 1, null, new DateTime(2026, 7, 6, 11, 26, 54, 967, DateTimeKind.Utc).AddTicks(6901), 150m, false, 1000m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IletisimMesajlari");

            migrationBuilder.DropTable(
                name: "SiteAyarlari");

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 6, 23, 22, 51, 14, 242, DateTimeKind.Utc).AddTicks(1448));

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 2,
                column: "OlusturulmaTarihi",
                value: new DateTime(2026, 6, 23, 22, 51, 14, 242, DateTimeKind.Utc).AddTicks(1451));
        }
    }
}
