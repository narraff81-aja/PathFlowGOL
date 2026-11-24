using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Dati ottenuti da:
/// - Regione, file: "1_STRUTTURA FILE EXCEL_PLL.xlsx";
/// - fogli: OMLS, file: "APPUNTAMENTI OR_ACC (1).xlsx";
/// - file: "LINK OR ACC T.S..xlsx"
/// </summary>
public class GestionePatto {
    // -- dati dal file xlsx --
    [Key]
    [MaxLength(100)]
    public string IdPatto { get; set; } = String.Empty; // PK

    [Required]
    public int IdCittadino { get; set; }// FK
    public Cittadino Cittadino { get; set; }= new Cittadino();
    /// <summary>
    /// Email fornita al CPI per il patto con la Regione
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string EmailRegione { get; set; } = String.Empty;
    /// <summary>
    /// Telefono fornito al CPI per il patto con la Regione
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TelefonoRegione { get; set; } = String.Empty;

    [Required]
    [MaxLength(100)]
    public string Intervento { get; set; } = "(perc.1,2,3) ORIENTAMENTO SPECIALISTICO GOL";// come da file xlsx
    
    // all'inizio è "ISCRIZIONE ASSEGNATA" per tutti, poi sarà modificabile
    [Required]
    [MaxLength(100)]
    public string StatoInterventoRegione { get; set; } = "ISCRIZIONE ASSEGNATA";// come da file xlsx
    [Required] 
    public DateOnly DataIscrizione { get; set; }

    /// <summary>
    /// Se OR = DataStato + 20g ; se ACC = Min(DataStato + 180g, 31/12/AAAA)
    /// </summary>
    public DateOnly? DataChiusura { get; set; }
    /// <summary>
    /// Data di presa in carico dall'ente = Data assegnazione
    /// </summary>
    public DateOnly? DataStato { get; set; }
    /* -- si suppone lo stesso Ente a cui è riferito il gestionale --
    public string CodiceEntePromotore { get; set; }
    public string EntePromotore { get; set; }
     */

    [MaxLength(100)]
    public string? CorsoScelto { get; set; }// non dovrebbe essere di tipo Corso per le varie edizioni
    /// <summary>
    /// 'Reinserimento lavorativo (1); Upskilling (2); Reskilling (3); Lavoro e Inclusione (4); Ricollocazione Collettiva (5)'
    /// </summary>
    [Required]
    public PercorsoGOL Percorso { get; set; } = PercorsoGOL.ReinserimentoLavorativo;
    /// <summary>
    /// CPI di appartenza del cittadino
    /// </summary>
    [Required]
    [MaxLength(100)]
    //[Column("cpi_titolare")]    
    public string CPITitolare { get; set; }= String.Empty;

    // -- dati scoperti ed inseriti dall'Operatore --

    /// <summary>
    /// Operatore da assegnato
    /// </summary>
    public int? IdOperatore { get; set; }// FK
    public ProfiloOperatore? Operatore { get; set; }

    // i dati di residenza e contatti sono qui perché un Cittadino può cambiare
    // residenza / contatti tra la fase di OR (Orientamento) e ACC (Accompagnamento al lavoro)

    [MaxLength(150)]
    public string? IndirizzoResidenza { get; set; }
    [Column(TypeName = "CHAR(5)")]
    public string? CapResidenza { get; set; }
    [MaxLength(150)]
    public string? CittaResidenza { get; set; }
    [Column(TypeName = "CHAR(2)")]
    public string? ProvinciaResidenza { get; set; }
    /// <summary>
    /// Email fornita all'Ente, per default è uguale a EmailRegione
    /// </summary>
    [MaxLength(255)]
    public string? EmailEnte { get; set; }
    /// <summary>
    /// Telefono fornito all'Ente, per default è uguale a TelefonoRegione
    /// </summary>
    [MaxLength(20)]
    public string? TelefonoEnte { get; set; }

    public int? IdTitoloStudio { get; set; }// FK
    public TitoloStudio? TitoloStudio { get; set; }
    public int? IdBeneficio { get; set; }// FK
    public TipoBeneficio? Beneficio { get; set; }

    public int? IdLavoroObiettivo { get; set; }// FK
    public LavoroObiettivo? LavoroObiettivo { get; set; }
    public int? IdStatoIntervento { get; set; }// FK
    public StatoIntervento? StatoIntervento { get; set; }
    [Column(TypeName = "SMALLINT")]
    // OR max 1/3 è 4 e 2/3 è 6; ACC max 10
    public int OreTotaliPreviste { get; set; } // 6h o 10h
    [Column(TypeName = "SMALLINT")]
    public int OreTotaliErogate { get; set; } // ad ogni appuntamento confermato si aggiorna
    /// <summary>
    /// Cosa è stato fatto: "non risponde alla mail", "non risponde al telefono", 
    /// "irreperibile", "non presente all'appuntamento", "appuntamento non completato", 
    /// "appuntamento effettuato"
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string? CosaFatto { get; set; }
    /// <summary>
    /// "interrotto", "chiuso in pll", "da interrompere"
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string? StatoFinale { get; set; }
    [Column(TypeName = "TEXT")]
    public string? Note { get; set; }

    public StatoRendicPatto StatoRendicontazione { get; set; } = StatoRendicPatto.NonRendicontato;

    // --- NAVIGAZIONE: GLI APPUNTAMENTI REALI (1 a N) ---
    public ICollection<Appuntamento> Appuntamenti { get; set; } = new List<Appuntamento>();

    // --- NAVIGAZIONE: Checklist (1 a 1) ---
    public int? IdChecklist { get; set; }
    public ChecklistDocumentale? Checklist { get; set; }
}
