using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_projekat.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Karoserije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karoserije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Placevi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Telefon = table.Column<int>(type: "int", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kapacitet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placevi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prodavci",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrLicneKarte = table.Column<long>(type: "bigint", maxLength: 9, nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Telefon = table.Column<int>(type: "int", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavci", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vozilo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marka = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RegistarskaTablica = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    GodinaProizvodnje = table.Column<int>(type: "int", nullable: false),
                    Kilometraza = table.Column<int>(type: "int", nullable: false),
                    ZapreminaMotora = table.Column<int>(type: "int", nullable: false),
                    SnagaMotora = table.Column<int>(type: "int", nullable: false),
                    KaroserijaID = table.Column<int>(type: "int", nullable: true),
                    NazivPlacaID = table.Column<int>(type: "int", nullable: true),
                    VlasnikID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozilo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vozilo_Karoserije_KaroserijaID",
                        column: x => x.KaroserijaID,
                        principalTable: "Karoserije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vozilo_Placevi_NazivPlacaID",
                        column: x => x.NazivPlacaID,
                        principalTable: "Placevi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vozilo_Prodavci_VlasnikID",
                        column: x => x.VlasnikID,
                        principalTable: "Prodavci",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_KaroserijaID",
                table: "Vozilo",
                column: "KaroserijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_NazivPlacaID",
                table: "Vozilo",
                column: "NazivPlacaID");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_VlasnikID",
                table: "Vozilo",
                column: "VlasnikID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vozilo");

            migrationBuilder.DropTable(
                name: "Karoserije");

            migrationBuilder.DropTable(
                name: "Placevi");

            migrationBuilder.DropTable(
                name: "Prodavci");
        }
    }
}
