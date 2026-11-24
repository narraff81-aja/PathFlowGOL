using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PathFlowGOL.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Utente> Utenti => Set<Utente>();
    public DbSet<Cittadino> Cittadini => Set<Cittadino>();
    public DbSet<GestionePatto> GestionePatti => Set<GestionePatto>();
    public DbSet<TipoBeneficio> TipiBenefici => Set<TipoBeneficio>();
    public DbSet<LavoroObiettivo> LavoriObiettivo => Set<LavoroObiettivo>();
    public DbSet<StatoIntervento> StatiIntervento => Set<StatoIntervento>();
    public DbSet<TitoloStudio> TitoliStudio => Set<TitoloStudio>();
    public DbSet<ProfiloOperatore> ProfiliOperatori => Set<ProfiloOperatore>();
    public DbSet<Corso> Corsi => Set<Corso>();
    public DbSet<Appuntamento> Appuntamenti => Set<Appuntamento>();
    public DbSet<ChecklistDocumentale> ChecklistDocumentale => Set<ChecklistDocumentale>();
    public DbSet<FAQ> FAQs => Set<FAQ>();
    public DbSet<Logging> Logging => Set<Logging>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Logging>()
            .Property(l => l.Istante)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relazione 1-1
        modelBuilder.Entity<Utente>()
            .HasOne(u => u.ProfiloOperatore)
            .WithOne(p => p.Utente)
            .HasForeignKey<Utente>(u => u.IdOperatore)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ChecklistDocumentale>()
            .HasOne(i => i.Patto)
            .WithOne(c => c.Checklist)
            .HasForeignKey<GestionePatto>(c => c.IdChecklist);

        // Relazioni 1-N ambigue
        modelBuilder.Entity<Appuntamento>()
            .HasOne(a => a.Patto)
            .WithMany(g => g.Appuntamenti)
            .HasForeignKey(a => a.IdPatto)
            .HasPrincipalKey(g => g.IdPatto);
        modelBuilder.Entity<FAQ>()
            .HasOne(f => f.OperatoreDomanda)
            .WithMany(o => o.DomandeFAQs)
            .HasForeignKey(f => f.IdOperatoreDomanda)
            .HasPrincipalKey(o => o.Id);
        modelBuilder.Entity<FAQ>()
            .HasOne(f => f.OperatoreRisposta)
            .WithMany(o => o.RisposteFAQs)
            .HasForeignKey(f => f.IdOperatoreRisposta)
            .HasPrincipalKey(o => o.Id);
        modelBuilder.Entity<GestionePatto>()
            .HasOne(g => g.Cittadino)
            .WithMany(c => c.Patti)
            .HasForeignKey(g => g.IdCittadino)
            .HasPrincipalKey(c => c.Id);
        modelBuilder.Entity<GestionePatto>()
           .HasOne(g => g.Operatore)
           .WithMany(o => o.Patti)
           .HasForeignKey(g => g.IdOperatore)
           .HasPrincipalKey(o => o.Id);
        modelBuilder.Entity<GestionePatto>()
           .HasOne(g => g.TitoloStudio)
           .WithMany(t => t.GestionePatti)
           .HasForeignKey(g => g.IdTitoloStudio)
           .HasPrincipalKey(o => o.Id);
        modelBuilder.Entity<GestionePatto>()
           .HasOne(g => g.Beneficio)
           .WithMany(b => b.GestionePatti)
           .HasForeignKey(g => g.IdBeneficio)
           .HasPrincipalKey(b => b.Id);
        modelBuilder.Entity<GestionePatto>()
           .HasOne(g => g.LavoroObiettivo)
           .WithMany(l => l.GestionePatti)
           .HasForeignKey(g => g.IdLavoroObiettivo)
           .HasPrincipalKey(l => l.Id);
        modelBuilder.Entity<GestionePatto>()
           .HasOne(g => g.StatoIntervento)
           .WithMany(s => s.GestionePatti)
           .HasForeignKey(g => g.IdStatoIntervento)
           .HasPrincipalKey(s => s.Id);

        // campi UNIQUE
        modelBuilder.Entity<Utente>()
            .HasIndex(i => i.Username).IsUnique();
        modelBuilder.Entity<Cittadino>()
            .HasIndex(i => i.CodiceFiscale).IsUnique();
        modelBuilder.Entity<LavoroObiettivo>()
            .HasIndex(i => i.Lavoro).IsUnique();
        modelBuilder.Entity<StatoIntervento>()
            .HasIndex(i => i.Stato).IsUnique();
        modelBuilder.Entity<TipoBeneficio>()
            .HasIndex(i => i.Beneficio).IsUnique();
        modelBuilder.Entity<TitoloStudio>()
            .HasIndex(i => i.Denominazione).IsUnique();

        // --- CONVERSIONE ENUM IN STRINGA E DEFAULT SUL DB ---
        // -- Appuntamento --
        modelBuilder.Entity<Appuntamento>()
            .Property(a => a.StatoAppuntamento)
            .HasConversion<string>().HasMaxLength(20)
            .HasDefaultValue(StatoAppuntamento.Programmato);
        // Converter per far digerire gli spazi degli Enum [EnumMember] su MySQL
        var statoRendicAppuntConverter = new ValueConverter<StatoRendicAppunt, string>(
            v => StatoRendicAppuntToString(v),
            v => StringToStatoRendicAppunt(v)
        );
        modelBuilder.Entity<Appuntamento>()
            .Property(a => a.StatoRendicontazione)
            .HasConversion(statoRendicAppuntConverter).HasMaxLength(30)
            .HasDefaultValue(StatoRendicAppunt.NonRendicontabile);

        // -- GestionePatto --
        var percorsoGOLConverter = new ValueConverter<PercorsoGOL, string>(
            v => PercorsoGOLToString(v),
            v => StringToPercorsoGOL(v)
        );
        modelBuilder.Entity<GestionePatto>()
            .Property(g => g.Percorso)
            .HasConversion(percorsoGOLConverter).HasMaxLength(40)
            .HasSentinel(PercorsoGOL.ReinserimentoLavorativo) // Deve usare questo anziché il '0'
            .HasDefaultValue(PercorsoGOL.ReinserimentoLavorativo);
        var statoRendicPattoConverter = new ValueConverter<StatoRendicPatto, string>(
            v => StatoRendicPattoToString(v),
            v => StringToStatoRendicPatto(v)
        );
        modelBuilder.Entity<GestionePatto>()
            .Property(g => g.StatoRendicontazione)
            .HasConversion(statoRendicPattoConverter).HasMaxLength(30)
            .HasDefaultValue(StatoRendicPatto.NonRendicontato);

        // -- Operatore --
        modelBuilder.Entity<Utente>()
            .Property(o => o.Ruolo)
            .HasSentinel(RuoloUtente.Operatore)// Deve usare questo anziché il '0'
            .HasDefaultValue(RuoloUtente.Operatore);
        //.HasConversion<string>().HasMaxLength(20) // NO string
        var statoOperatoreConverter = new ValueConverter<StatoUtente, string>(
            v => StatoUtenteToString(v),
            v => StringToStatoUtente(v)
        );
        modelBuilder.Entity<Utente>()
            .Property(o => o.Stato)
            .HasConversion(statoOperatoreConverter).HasMaxLength(15)
            //.HasSentinel(StatoOperatore.Attivo)
            .HasDefaultValue(StatoUtente.Attivo);


        base.OnModelCreating(modelBuilder);
    }
    static string StatoRendicAppuntToString(StatoRendicAppunt stato) {
        return stato switch {
            StatoRendicAppunt.DaRendicontare => "Da Rendicontare",
            StatoRendicAppunt.Rendicontato => "Rendicontato",
            _ => "Non Rendicontabile",
        };
    }
    static StatoRendicAppunt StringToStatoRendicAppunt(string str) {
        return str.Trim().ToLower() switch {
            "non rendicontabile" => StatoRendicAppunt.NonRendicontabile,
            "da rendicontare" => StatoRendicAppunt.DaRendicontare,
            "rendicontato" => StatoRendicAppunt.Rendicontato,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str,
                "Stringa per Stato di Rendicontazione dell'Appuntamento non riconosciuta."),
        };
    }
    static string PercorsoGOLToString(PercorsoGOL percorso) {
        return percorso switch {
            PercorsoGOL.Upskilling => "2 - Upskilling",
            PercorsoGOL.Reskilling => "3 - Reskilling",
            PercorsoGOL.LavoroInclusione => "4 - Lavoro e Inclusione",
            PercorsoGOL.RicollocazioneCollettiva => "5 - Ricollocazione Collettiva",
            _ => "1 - Reinserimento lavorativo"
        };
    }
    static PercorsoGOL StringToPercorsoGOL(string str) {
        return str.Trim().ToLower() switch {
            "1 - reinserimento lavorativo" => PercorsoGOL.ReinserimentoLavorativo,
            "2 - upskilling" => PercorsoGOL.Upskilling,
            "3 - reskilling" => PercorsoGOL.Reskilling,
            "4 - lavoro e inclusione" => PercorsoGOL.LavoroInclusione,
            "5 - ricollocazione collettiva" => PercorsoGOL.RicollocazioneCollettiva,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, "Stringa PercorsoGOL non riconosciuta.")
        };
    }
    static string StatoRendicPattoToString(StatoRendicPatto stato) {
        return stato switch {
            StatoRendicPatto.InAttesa => "In Attesa",
            StatoRendicPatto.InParteRendic => "In Parte",
            StatoRendicPatto.Rendicontato => "Rendicontato",
            _ => "Non Rendicontato",
        };
    }
    static StatoRendicPatto StringToStatoRendicPatto(string str) {
        return str.Trim().ToLower() switch {
            "non rendicontato" => StatoRendicPatto.NonRendicontato,
            "in attesa" => StatoRendicPatto.InAttesa,
            "in parte" => StatoRendicPatto.InParteRendic,
            "rendicontato" => StatoRendicPatto.Rendicontato,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str,
                "Stringa per Stato di Rendicontazione del Patto non riconosciuta."),
        };
    }
    static string StatoUtenteToString(StatoUtente stato) {
        return stato switch {
            StatoUtente.NonAttivo => "Non Attivo",
            StatoUtente.Sospeso => "Sospeso",
            StatoUtente.InFerie => "In Ferie",
            _ => "Attivo",
        };
    }
    static StatoUtente StringToStatoUtente(string str) {
        return str.Trim().ToLower() switch {
            "non attivo" => StatoUtente.NonAttivo,
            "sospeso" => StatoUtente.Sospeso,
            "in ferie" => StatoUtente.InFerie,
            "attivo" => StatoUtente.Attivo,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, "Stringa StatoUtente non riconosciuta.")
        };
    }


}
