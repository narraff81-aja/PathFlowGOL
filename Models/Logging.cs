using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;

public class Logging {
    [Key]
    public int Id { get; set; }// PK
    [Required]
    public DateTime Istante { get; set; }// current_timestamp() ON UPDATE current_timestamp(),
    [Required]
    public int IdOperatore { get; set; }
    [Column(TypeName = "VARCHAR(45)")]
    [Comment("IPv4 o IPv6")]
    public string IP { get; set; } = string.Empty;
    /// <summary>
    /// tabella che verrà modificata
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string NomeTabella { get; set; } = string.Empty;
    /// <summary>
    /// campo della tabella che verrà modificato
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string NomeCampo { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string ValoreVecchio { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string ValoreNuovo { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    public string Azione { get; set; } = string.Empty;
}