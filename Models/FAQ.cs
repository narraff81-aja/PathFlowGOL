using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PathFlowGOL.Models;

public class FAQ {
    // -- Domanda --
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Es: 'or', 'acc', 'corsi', 'varie'
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string Argomento { get; set; } = string.Empty;
    /// <summary>
    /// Fonte della domanda: "wapp", "mail", "diretta"
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string FonteDomanda { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "TEXT")]
    public string Domanda { get; set; } = string.Empty;// supera i 255 caratteri
    [Required]
    public int IdOperatoreDomanda { get; set; }// FK
    [Required]
    public ProfiloOperatore OperatoreDomanda { get; set; } = new();

    // -- Risposta --
    [Column(TypeName = "TEXT")]
    public string? Risposta { get; set; }// supera i 255 caratteri
    public DateOnly DataRisposta { get; set; }
    /// <summary>
    /// note ulteriori a chiarimento
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string? NoteUlteriori { get; set; }
    /// <summary>
    /// url di riferimenti normativi
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string? RiferimentiNormativi { get; set; }

    public int? IdOperatoreRisposta { get; set; }// FK
    public ProfiloOperatore? OperatoreRisposta { get; set; }

}
