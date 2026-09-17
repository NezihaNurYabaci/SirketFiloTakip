using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SirketFiloTakip.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plaka = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelYili = table.Column<int>(type: "int", nullable: false),
                    Kilometre = table.Column<int>(type: "int", nullable: false),
                    YakitTuru = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    AracTuru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Araclar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calisanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pozisyon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ehliyet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calisanlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BakimKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BakimTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToplamUcret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kilometre = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SonrakiBakimKm = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakimKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BakimKayitlari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YakitKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Litre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToplamUcret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kilometre = table.Column<int>(type: "int", nullable: false),
                    YakitTuru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YakitKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YakitKayitlari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gorevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    CalisanId = table.Column<int>(type: "int", nullable: false),
                    Amac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KalkisYeri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VarisYeri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanlananCikis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlanlananDonus = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GercekCikis = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GercekDonus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaslangicKm = table.Column<int>(type: "int", nullable: true),
                    BitisKm = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorevler_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gorevler_Calisanlar_CalisanId",
                        column: x => x.CalisanId,
                        principalTable: "Calisanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HasarKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    GorevId = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maliyet = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HasarKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HasarKayitlari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HasarKayitlari_Gorevler_GorevId",
                        column: x => x.GorevId,
                        principalTable: "Gorevler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Araclar_Plaka",
                table: "Araclar",
                column: "Plaka",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BakimKayitlari_AracId",
                table: "BakimKayitlari",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_AracId",
                table: "Gorevler",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_CalisanId",
                table: "Gorevler",
                column: "CalisanId");

            migrationBuilder.CreateIndex(
                name: "IX_HasarKayitlari_AracId",
                table: "HasarKayitlari",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_HasarKayitlari_GorevId",
                table: "HasarKayitlari",
                column: "GorevId");

            migrationBuilder.CreateIndex(
                name: "IX_YakitKayitlari_AracId",
                table: "YakitKayitlari",
                column: "AracId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BakimKayitlari");

            migrationBuilder.DropTable(
                name: "HasarKayitlari");

            migrationBuilder.DropTable(
                name: "YakitKayitlari");

            migrationBuilder.DropTable(
                name: "Gorevler");

            migrationBuilder.DropTable(
                name: "Araclar");

            migrationBuilder.DropTable(
                name: "Calisanlar");
        }
    }
}
