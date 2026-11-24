using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Profilo Operatore
/// </summary>
public class ProfiloOperatore {
    [Key]
    public int Id { get; set; } // PK
    [Required]
    [MaxLength(120)]
    public string Nome { get; set; } = String.Empty;
    [Required]
    [MaxLength(120)]
    public string Cognome { get; set; } = String.Empty;
    [EmailAddress(ErrorMessage = "Formato email non valido.")]
    [MaxLength(255)]
    public string? Email { get; set; }// 'alcuni non hanno indicato la mail'

    /// <summary>
    /// Colore distintivo nel front-end es: yellow, #ff0000, rgb(0,255,0)
    /// </summary>
    [MaxLength(100)]
    public string? ColoreCSS { get; set; }

    /// <summary>
    /// Link per videoconferenza (Meet/Zoom) o "IN PRESENZA"
    /// </summary>
    //[Url(ErrorMessage = "Formato link non valido.")]
    [MaxLength(255)]
    public string? Link { get; set; }// 'se il link è assegnato individualmente

    /* -- ne vale la pena? --
    [Column(TypeName = "DECIMAL(5,2)")]
    public decimal? CompensoOrario { get; set; }
    /// <summary>
    /// 'si azzera a ogni rendicontazione'
    /// </summary>
    public int ContatoreOreRendicontazione { get; set; } = 0;
    */

    // Relazione 1 a 1
    public Utente? Utente { get; set; }

    // Relazione 1 a Molti
    public ICollection<GestionePatto> Patti { get; set; } = new List<GestionePatto>();
    public ICollection<FAQ> DomandeFAQs { get; set; } = new List<FAQ>();
    public ICollection<FAQ> RisposteFAQs { get; set; } = new List<FAQ>();
}
