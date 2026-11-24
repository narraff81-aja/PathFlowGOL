using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Anagrafica pura e immutabile di un Cittadino
/// </summary>
[Comment("Anagrafica immutabile di un Cittadino")]
public class Cittadino {
    [Key]
    public int Id { get; set; }// PK
    [Required]
    [Column(TypeName ="CHAR(16)")]
    public string CodiceFiscale { get; set; } = String.Empty;// UNIQUE  // Potrebbe essere PK
    [Required]
    [MaxLength(120)]
    public string Nome { get; set; } = String.Empty;
    [Required]
    [MaxLength(120)]
    public string Cognome { get; set; } = String.Empty;
    /// <summary>
    /// M o F o ''
    /// </summary>    
    [Required]
    [MaxLength(1)]
    [Comment("M o F o ''")]
    public string Sesso { get; set; } = "";
    [Required]
    public DateOnly DataNascita { get; set; }
    /// <summary>
    /// Città o Nazione
    /// </summary>
    [MaxLength(120)]
    [Comment("Città o Nazione")]
    public string? LuogoNascita { get; set; }

    // Relazione 1 a Molti
    public ICollection<GestionePatto> Patti { get; set; } = new List<GestionePatto>();

}
