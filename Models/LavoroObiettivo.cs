using System.ComponentModel.DataAnnotations;

namespace PathFlowGOL.Models;
/// <summary>
/// tabella di supporto
/// </summary>
public class LavoroObiettivo {
    [Key]
    public int Id { get; set; } // PK 
    [Required]
    [MaxLength(130)]
    public string Lavoro { get; set; } = String.Empty; // UNIQUE
    // Relazione 1 a Molti
    public ICollection<GestionePatto> GestionePatti { get; set; } = new List<GestionePatto>();
}
