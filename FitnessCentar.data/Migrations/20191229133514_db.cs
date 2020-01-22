using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessCentar.data.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KategorijaPlanIProgram",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Obrisan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorijaPlanIProgram", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KorisnickiNalog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KorisnickoIme = table.Column<string>(nullable: true),
                    Lozinka = table.Column<string>(nullable: true),
                    Tip = table.Column<string>(nullable: true),
                    ResetPasswordCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnickiNalog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NacinPlacanja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NacinPlacanja", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipClanarine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Cijena = table.Column<float>(nullable: false),
                    VrijemeTrajanja = table.Column<int>(nullable: false),
                    Obrisan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipClanarine", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vjezba",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true),
                    Slika = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vjezba", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Podkategorija",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    KategorijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podkategorija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Podkategorija_Kategorija_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanIProgram",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    KategorijaID = table.Column<int>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Obrisan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanIProgram", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanIProgram_KategorijaPlanIProgram_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "KategorijaPlanIProgram",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutorizacijskiToken",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Vrijednost = table.Column<string>(nullable: true),
                    KorisnickiNalogID = table.Column<int>(nullable: false),
                    VrijemeEvidentiranja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacijskiToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AutorizacijskiToken_KorisnickiNalog_KorisnickiNalogID",
                        column: x => x.KorisnickiNalogID,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    BrojTelefona = table.Column<string>(nullable: true),
                    DatumRodenja = table.Column<DateTime>(nullable: false),
                    JMBG = table.Column<string>(nullable: true),
                    Slika = table.Column<string>(nullable: true),
                    BrojKartice = table.Column<string>(nullable: true),
                    Spol = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    KorisnickiNalogID = table.Column<int>(nullable: true),
                    Obrisan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Korisnik_KorisnickiNalog_KorisnickiNalogID",
                        column: x => x.KorisnickiNalogID,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stavka",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Cijena = table.Column<float>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Slika = table.Column<string>(nullable: true),
                    PodkategorijaID = table.Column<int>(nullable: false),
                    Obrisan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stavka", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stavka_Podkategorija_PodkategorijaID",
                        column: x => x.PodkategorijaID,
                        principalTable: "Podkategorija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sedmica",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanIProgramID = table.Column<int>(nullable: false),
                    RedniBroj = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedmica", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sedmica_PlanIProgram_PlanIProgramID",
                        column: x => x.PlanIProgramID,
                        principalTable: "PlanIProgram",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adresa",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Grad = table.Column<string>(nullable: true),
                    Ulica = table.Column<string>(nullable: true),
                    PostanskiBroj = table.Column<string>(nullable: true),
                    KorisnikID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresa", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Adresa_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clanarina",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumDodavanja = table.Column<DateTime>(nullable: false),
                    DatumIsteka = table.Column<DateTime>(nullable: false),
                    TipClanarineID = table.Column<int>(nullable: false),
                    ClanID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clanarina", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Clanarina_Korisnik_ClanID",
                        column: x => x.ClanID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clanarina_TipClanarine_TipClanarineID",
                        column: x => x.TipClanarineID,
                        principalTable: "TipClanarine",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MjerenjeNapredka",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumMjerenja = table.Column<DateTime>(nullable: false),
                    Kilaza = table.Column<string>(nullable: true),
                    Visina = table.Column<string>(nullable: true),
                    ProcenatMasti = table.Column<string>(nullable: true),
                    ObimPrsa = table.Column<string>(nullable: true),
                    ObimRuku = table.Column<string>(nullable: true),
                    ObimStruka = table.Column<string>(nullable: true),
                    ObimNoge = table.Column<string>(nullable: true),
                    ClanID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MjerenjeNapredka", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MjerenjeNapredka_Korisnik_ClanID",
                        column: x => x.ClanID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosjecenostClana",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    VrijemeDolaska = table.Column<DateTime>(nullable: false),
                    VrijemeOdlaska = table.Column<DateTime>(nullable: true),
                    ClanID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosjecenostClana", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PosjecenostClana_Korisnik_ClanID",
                        column: x => x.ClanID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Racun",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumVrijeme = table.Column<DateTime>(nullable: false),
                    ZaposlenikID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racun", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Racun_Korisnik_ZaposlenikID",
                        column: x => x.ZaposlenikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skladiste",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Kolicina = table.Column<int>(nullable: false),
                    Velicina = table.Column<string>(nullable: true),
                    StavkaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skladiste", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Skladiste_Stavka_StavkaID",
                        column: x => x.StavkaID,
                        principalTable: "Stavka",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dan",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SedmicaID = table.Column<int>(nullable: false),
                    RedniBroj = table.Column<int>(nullable: false),
                    DuzinaTreninga = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dan_Sedmica_SedmicaID",
                        column: x => x.SedmicaID,
                        principalTable: "Sedmica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Narudzba",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumVrijeme = table.Column<DateTime>(nullable: false),
                    RacunID = table.Column<int>(nullable: true),
                    NacinPlacanjaID = table.Column<int>(nullable: false),
                    Cijena = table.Column<int>(nullable: false),
                    KorisnikID = table.Column<int>(nullable: false),
                    AdresaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzba", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Narudzba_Adresa_AdresaID",
                        column: x => x.AdresaID,
                        principalTable: "Adresa",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Narudzba_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Narudzba_NacinPlacanja_NacinPlacanjaID",
                        column: x => x.NacinPlacanjaID,
                        principalTable: "NacinPlacanja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Narudzba_Racun_RacunID",
                        column: x => x.RacunID,
                        principalTable: "Racun",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanVjezba",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DanID = table.Column<int>(nullable: false),
                    VjezbaID = table.Column<int>(nullable: false),
                    BrojPonavljanja = table.Column<int>(nullable: false),
                    BrojSetova = table.Column<int>(nullable: false),
                    DuzinaOdmora = table.Column<int>(nullable: true),
                    RedniBrojVjezbe = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanVjezba", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DanVjezba_Dan_DanID",
                        column: x => x.DanID,
                        principalTable: "Dan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanVjezba_Vjezba_VjezbaID",
                        column: x => x.VjezbaID,
                        principalTable: "Vjezba",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NarudzbaStavke",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NarudzbaID = table.Column<int>(nullable: false),
                    StavkaID = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    Cijena = table.Column<float>(nullable: false),
                    VelicinaID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarudzbaStavke", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NarudzbaStavke_Narudzba_NarudzbaID",
                        column: x => x.NarudzbaID,
                        principalTable: "Narudzba",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarudzbaStavke_Stavka_StavkaID",
                        column: x => x.StavkaID,
                        principalTable: "Stavka",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarudzbaStavke_Skladiste_VelicinaID",
                        column: x => x.VelicinaID,
                        principalTable: "Skladiste",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adresa_KorisnikID",
                table: "Adresa",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacijskiToken_KorisnickiNalogID",
                table: "AutorizacijskiToken",
                column: "KorisnickiNalogID");

            migrationBuilder.CreateIndex(
                name: "IX_Clanarina_ClanID",
                table: "Clanarina",
                column: "ClanID");

            migrationBuilder.CreateIndex(
                name: "IX_Clanarina_TipClanarineID",
                table: "Clanarina",
                column: "TipClanarineID");

            migrationBuilder.CreateIndex(
                name: "IX_Dan_SedmicaID",
                table: "Dan",
                column: "SedmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_DanVjezba_DanID",
                table: "DanVjezba",
                column: "DanID");

            migrationBuilder.CreateIndex(
                name: "IX_DanVjezba_VjezbaID",
                table: "DanVjezba",
                column: "VjezbaID");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_KorisnickiNalogID",
                table: "Korisnik",
                column: "KorisnickiNalogID",
                unique: true,
                filter: "[KorisnickiNalogID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_Email_BrojKartice_JMBG",
                table: "Korisnik",
                columns: new[] { "Email", "BrojKartice", "JMBG" },
                unique: true,
                filter: "[Email] IS NOT NULL AND [BrojKartice] IS NOT NULL AND [JMBG] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MjerenjeNapredka_ClanID",
                table: "MjerenjeNapredka",
                column: "ClanID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_AdresaID",
                table: "Narudzba",
                column: "AdresaID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_KorisnikID",
                table: "Narudzba",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_NacinPlacanjaID",
                table: "Narudzba",
                column: "NacinPlacanjaID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_RacunID",
                table: "Narudzba",
                column: "RacunID");

            migrationBuilder.CreateIndex(
                name: "IX_NarudzbaStavke_NarudzbaID",
                table: "NarudzbaStavke",
                column: "NarudzbaID");

            migrationBuilder.CreateIndex(
                name: "IX_NarudzbaStavke_StavkaID",
                table: "NarudzbaStavke",
                column: "StavkaID");

            migrationBuilder.CreateIndex(
                name: "IX_NarudzbaStavke_VelicinaID",
                table: "NarudzbaStavke",
                column: "VelicinaID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanIProgram_KategorijaID",
                table: "PlanIProgram",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Podkategorija_KategorijaID",
                table: "Podkategorija",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_PosjecenostClana_ClanID",
                table: "PosjecenostClana",
                column: "ClanID");

            migrationBuilder.CreateIndex(
                name: "IX_Racun_ZaposlenikID",
                table: "Racun",
                column: "ZaposlenikID");

            migrationBuilder.CreateIndex(
                name: "IX_Sedmica_PlanIProgramID",
                table: "Sedmica",
                column: "PlanIProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_Skladiste_StavkaID",
                table: "Skladiste",
                column: "StavkaID");

            migrationBuilder.CreateIndex(
                name: "IX_Stavka_PodkategorijaID",
                table: "Stavka",
                column: "PodkategorijaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorizacijskiToken");

            migrationBuilder.DropTable(
                name: "Clanarina");

            migrationBuilder.DropTable(
                name: "DanVjezba");

            migrationBuilder.DropTable(
                name: "MjerenjeNapredka");

            migrationBuilder.DropTable(
                name: "NarudzbaStavke");

            migrationBuilder.DropTable(
                name: "PosjecenostClana");

            migrationBuilder.DropTable(
                name: "TipClanarine");

            migrationBuilder.DropTable(
                name: "Dan");

            migrationBuilder.DropTable(
                name: "Vjezba");

            migrationBuilder.DropTable(
                name: "Narudzba");

            migrationBuilder.DropTable(
                name: "Skladiste");

            migrationBuilder.DropTable(
                name: "Sedmica");

            migrationBuilder.DropTable(
                name: "Adresa");

            migrationBuilder.DropTable(
                name: "NacinPlacanja");

            migrationBuilder.DropTable(
                name: "Racun");

            migrationBuilder.DropTable(
                name: "Stavka");

            migrationBuilder.DropTable(
                name: "PlanIProgram");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Podkategorija");

            migrationBuilder.DropTable(
                name: "KategorijaPlanIProgram");

            migrationBuilder.DropTable(
                name: "KorisnickiNalog");

            migrationBuilder.DropTable(
                name: "Kategorija");
        }
    }
}
