using System.ComponentModel.DataAnnotations;

namespace PathFlowGOL.Models;
/// <summary>
/// Tabella di lookup
/// </summary>
public class TipoBeneficio {
    [Key]
    public int Id { get; set; }//PK 
    /// <summary>
    /// 'Adi, Naspi, Sfl, ...'
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string Beneficio { get; set; } = String.Empty;// NO UNIQUE apposta
    // Relazione 1 a Molti
    public ICollection<GestionePatto> GestionePatti { get; set; } = new List<GestionePatto>();
}
