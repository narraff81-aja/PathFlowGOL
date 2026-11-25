using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFlowGOL.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChecklistDocumentale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MailFinaleInviata = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    NomeCognomeInOrdine = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    FirmaAutografa = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    OrientUnicoFilePDF = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    OrientPrivacyFirmata = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    OrientCVFirmato = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    OrientSchedaProfiloWord = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    BilancioCompetenzePDF = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AccompUnicoFilePDF = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AccompPrivacyFirmata = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AccompCVFirmato = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AccompSchedaProfiloWord = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ModuloConclusivoFirmato = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistDocumentale", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cittadini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodiceFiscale = table.Column<string>(type: "CHAR(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cognome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sesso = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, comment: "M o F o ''")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascita = table.Column<DateOnly>(type: "date", nullable: false),
                    LuogoNascita = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true, comment: "Città o Nazione")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cittadini", x => x.Id);
                },
                comment: "Anagrafica immutabile di un Cittadino")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Corsi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Denominazione = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Settore = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Percorso = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodiceCorso = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Codice ATECO")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OreCorso = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Ore del corso al netto del tirocinio"),
                    OreTirocinio = table.Column<short>(type: "SMALLINT", nullable: true),
                    TitoloStudioRichiesto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "Titolo di studio minimo richiesto per accedere al corso")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequisitoLingua = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipologiaQualifica = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true, comment: "Tipologia di qualifica al superamento del corso: Attestato o Qualifica")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corsi", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LavoriObiettivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Lavoro = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LavoriObiettivo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Istante = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdOperatore = table.Column<int>(type: "int", nullable: false),
                    IP = table.Column<string>(type: "VARCHAR(45)", nullable: false, comment: "IPv4 o IPv6")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeTabella = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeCampo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoreVecchio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoreNuovo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Azione = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logging", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfiliOperatori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cognome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColoreCSS = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfiliOperatori", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StatiIntervento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Stato = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatiIntervento", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipiBenefici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Beneficio = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipiBenefici", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TitoliStudio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Denominazione = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitoliStudio", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Argomento = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FonteDomanda = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Domanda = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdOperatoreDomanda = table.Column<int>(type: "int", nullable: false),
                    Risposta = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataRisposta = table.Column<DateOnly>(type: "date", nullable: false),
                    NoteUlteriori = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RiferimentiNormativi = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdOperatoreRisposta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQs_ProfiliOperatori_IdOperatoreDomanda",
                        column: x => x.IdOperatoreDomanda,
                        principalTable: "ProfiliOperatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAQs_ProfiliOperatori_IdOperatoreRisposta",
                        column: x => x.IdOperatoreRisposta,
                        principalTable: "ProfiliOperatori",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ruolo = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Stato = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "Attivo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdOperatore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utenti_ProfiliOperatori_IdOperatore",
                        column: x => x.IdOperatore,
                        principalTable: "ProfiliOperatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Utente del sito web")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GestionePatti",
                columns: table => new
                {
                    IdPatto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdCittadino = table.Column<int>(type: "int", nullable: false),
                    EmailRegione = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoRegione = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Intervento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoInterventoRegione = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataIscrizione = table.Column<DateOnly>(type: "date", nullable: false),
                    DataChiusura = table.Column<DateOnly>(type: "date", nullable: true),
                    DataStato = table.Column<DateOnly>(type: "date", nullable: true),
                    CorsoScelto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Percorso = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false, defaultValue: "1 - Reinserimento lavorativo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CPITitolare = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdOperatore = table.Column<int>(type: "int", nullable: true),
                    IndirizzoResidenza = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapResidenza = table.Column<string>(type: "CHAR(5)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CittaResidenza = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaResidenza = table.Column<string>(type: "CHAR(2)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailEnte = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoEnte = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdTitoloStudio = table.Column<int>(type: "int", nullable: true),
                    IdBeneficio = table.Column<int>(type: "int", nullable: true),
                    IdLavoroObiettivo = table.Column<int>(type: "int", nullable: true),
                    IdStatoIntervento = table.Column<int>(type: "int", nullable: true),
                    OreTotaliPreviste = table.Column<short>(type: "SMALLINT", nullable: false),
                    OreTotaliErogate = table.Column<short>(type: "SMALLINT", nullable: false),
                    CosaFatto = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoFinale = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoRendicontazione = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: "Non Rendicontato")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdChecklist = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GestionePatti", x => x.IdPatto);
                    table.ForeignKey(
                        name: "FK_GestionePatti_ChecklistDocumentale_IdChecklist",
                        column: x => x.IdChecklist,
                        principalTable: "ChecklistDocumentale",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GestionePatti_Cittadini_IdCittadino",
                        column: x => x.IdCittadino,
                        principalTable: "Cittadini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GestionePatti_LavoriObiettivo_IdLavoroObiettivo",
                        column: x => x.IdLavoroObiettivo,
                        principalTable: "LavoriObiettivo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GestionePatti_ProfiliOperatori_IdOperatore",
                        column: x => x.IdOperatore,
                        principalTable: "ProfiliOperatori",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GestionePatti_StatiIntervento_IdStatoIntervento",
                        column: x => x.IdStatoIntervento,
                        principalTable: "StatiIntervento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GestionePatti_TipiBenefici_IdBeneficio",
                        column: x => x.IdBeneficio,
                        principalTable: "TipiBenefici",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GestionePatti_TitoliStudio_IdTitoloStudio",
                        column: x => x.IdTitoloStudio,
                        principalTable: "TitoliStudio",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Appuntamenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPatto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    OrarioInizio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Durata = table.Column<short>(type: "SMALLINT", nullable: false),
                    Tipologia = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, comment: "'I' = 'individuale', 'G' = 'di gruppo'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoAppuntamento = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Programmato")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoRendicontazione = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: "Non Rendicontabile")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appuntamenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appuntamenti_GestionePatti_IdPatto",
                        column: x => x.IdPatto,
                        principalTable: "GestionePatti",
                        principalColumn: "IdPatto",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Appuntamenti_IdPatto",
                table: "Appuntamenti",
                column: "IdPatto");

            migrationBuilder.CreateIndex(
                name: "IX_Cittadini_CodiceFiscale",
                table: "Cittadini",
                column: "CodiceFiscale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_IdOperatoreDomanda",
                table: "FAQs",
                column: "IdOperatoreDomanda");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_IdOperatoreRisposta",
                table: "FAQs",
                column: "IdOperatoreRisposta");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdBeneficio",
                table: "GestionePatti",
                column: "IdBeneficio");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdChecklist",
                table: "GestionePatti",
                column: "IdChecklist",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdCittadino",
                table: "GestionePatti",
                column: "IdCittadino");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdLavoroObiettivo",
                table: "GestionePatti",
                column: "IdLavoroObiettivo");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdOperatore",
                table: "GestionePatti",
                column: "IdOperatore");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdStatoIntervento",
                table: "GestionePatti",
                column: "IdStatoIntervento");

            migrationBuilder.CreateIndex(
                name: "IX_GestionePatti_IdTitoloStudio",
                table: "GestionePatti",
                column: "IdTitoloStudio");

            migrationBuilder.CreateIndex(
                name: "IX_LavoriObiettivo_Lavoro",
                table: "LavoriObiettivo",
                column: "Lavoro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatiIntervento_Stato",
                table: "StatiIntervento",
                column: "Stato",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipiBenefici_Beneficio",
                table: "TipiBenefici",
                column: "Beneficio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TitoliStudio_Denominazione",
                table: "TitoliStudio",
                column: "Denominazione",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_IdOperatore",
                table: "Utenti",
                column: "IdOperatore",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_Username",
                table: "Utenti",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appuntamenti");

            migrationBuilder.DropTable(
                name: "Corsi");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Logging");

            migrationBuilder.DropTable(
                name: "Utenti");

            migrationBuilder.DropTable(
                name: "GestionePatti");

            migrationBuilder.DropTable(
                name: "ChecklistDocumentale");

            migrationBuilder.DropTable(
                name: "Cittadini");

            migrationBuilder.DropTable(
                name: "LavoriObiettivo");

            migrationBuilder.DropTable(
                name: "ProfiliOperatori");

            migrationBuilder.DropTable(
                name: "StatiIntervento");

            migrationBuilder.DropTable(
                name: "TipiBenefici");

            migrationBuilder.DropTable(
                name: "TitoliStudio");
        }
    }
}
