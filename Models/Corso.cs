using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;

public class Corso {
    [Key]
    public int Id { get; set; }// PK
    [Required]
    [MaxLength(160)]
    public string Denominazione { get; set; } = string.Empty;    
    /// <summary>
    /// Raggruppamento corsi
    /// </summary>
    [MaxLength(80)]
    public string? Settore { get; set; }
    [MaxLength(80)]
    public string? Percorso { get; set; }
    /// <summary>
    /// Codice ATECO
    /// </summary>
    [MaxLength(50)]
    [Comment("Codice ATECO")]
    public string? CodiceCorso { get; set; }
    /// <summary>
    /// Ore del corso al netto del tirocinio
    /// </summary>
    [Required]
    [Column(TypeName = "SMALLINT")]
    [Range(1,1500)]
    [Comment("Ore del corso al netto del tirocinio")]
    public int OreCorso { get; set; }
    [Column(TypeName = "SMALLINT")]
    [Range(0, 1000)]
    public int? OreTirocinio { get; set; }
    /// <summary>
    /// Titolo di studio minimo richiesto per accedere al corso
    /// </summary>
    [MaxLength(100)]
    [Comment("Titolo di studio minimo richiesto per accedere al corso")]
    public string? TitoloStudioRichiesto { get; set; }
    [MaxLength(100)]
    public string? RequisitoLingua { get; set; }
    /// <summary>
    /// Tipologia di qualifica al superamento del corso
    /// </summary>
    [MaxLength(60)]
    [Comment("Tipologia di qualifica al superamento del corso: Attestato o Qualifica")]
    public string? TipologiaQualifica { get; set; }

}
