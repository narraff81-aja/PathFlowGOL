using System.ComponentModel.DataAnnotations;

namespace PathFlowGOL.Models;
/// <summary>
/// Tabella di lookup
/// </summary>
public class StatoIntervento {
    [Key]
    public int Id { get; set; } // PK 
    [Required]
    [MaxLength(50)]
    public string Stato { get; set; } = String.Empty; // UNIQUE
    // Relazione 1 a Molti
    public ICollection<GestionePatto> GestionePatti { get; set; } = new List<GestionePatto>();
}