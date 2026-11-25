using System.ComponentModel.DataAnnotations;

namespace PathFlowGOL.DTOs;

public class CorsoReadDTO {
    public int Id { get; set; }
    public string Denominazione { get; set; } = string.Empty;
    public string? Settore { get; set; }
    public string? Percorso { get; set; }
    public string? CodiceCorso { get; set; }
    public int OreCorso { get; set; }
    public int? OreTirocinio { get; set; }
    public string? TitoloStudioRichiesto { get; set; }
    public string? RequisitoLingua { get; set; }
    public string? TipologiaQualifica { get; set; }
}

public class CorsoCreateUpdateDTO {
    [Required(ErrorMessage = "La Denominazione del corso è obbligatoria")]
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
    public string? CodiceCorso { get; set; }
    /// <summary>
    /// Ore del corso al netto del tirocinio
    /// </summary>
    [Required(ErrorMessage = "Le ore del corso sono obbligatorie")]
    [Range(1, 1500, ErrorMessage = "Le ore del corso devono essere almeno 1.")]
    public int? OreCorso { get; set; }// <- reso nullable per verificare se è stato passato o meno
    [Range(0, 1000)]
    public int? OreTirocinio { get; set; }
    /// <summary>
    /// Titolo di studio minimo richiesto per accedere al corso
    /// </summary>
    [MaxLength(100)]
    public string? TitoloStudioRichiesto { get; set; }
    [MaxLength(100)]
    public string? RequisitoLingua { get; set; }
    /// <summary>
    /// Tipologia di qualifica al superamento del corso: Attestato o Qualifica
    /// </summary>
    [MaxLength(60)]
    public string? TipologiaQualifica { get; set; }

}