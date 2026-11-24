using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Utente del sito web
/// </summary>
[Comment("Utente del sito web")]
public class Utente {
    [Key]
    public int Id { get; set; } // PK

    /// <summary>
    /// Email come Username
    /// </summary>
    [Required(ErrorMessage = "L'Username è obbligatorio! Puoi usare la mail")]
    [MaxLength(100)]
    public string Username { get; set; } = String.Empty; // UNIQUE
    /// <summary>
    /// Hash Password
    /// </summary>
    [Required(ErrorMessage = "La Password è obbligatoria! Subirà l'hash")]
    [MaxLength(255)]
    public string Password { get; set; } = String.Empty;

    /// <summary>
    /// 1=Operatore, 13= Admin, 47= Super Admin
    /// </summary>
    public RuoloUtente Ruolo { get; set; } = RuoloUtente.Operatore;
    /// <summary>
    /// Attivo, Non attivo
    /// </summary>
    public StatoUtente Stato { get; set; } = StatoUtente.Attivo;

    public int? IdOperatore { get; set; } // FK
    public ProfiloOperatore? ProfiloOperatore { get; set; }
}
