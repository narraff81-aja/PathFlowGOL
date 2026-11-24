using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Dati ottenuti da: 
/// - fogli: MESE, file: "APPUNTAMENTI OR_ACC (1).xlsx";
/// - file: "LINK OR ACC T.S..xlsx"
/// </summary>
public class Appuntamento {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string IdPatto { get; set; } = string.Empty;// FK
    public GestionePatto? Patto { get; set; }

    /// <summary>
    /// Link per videoconferenza (Meet/Zoom) o "IN PRESENZA"
    /// </summary>
    [MaxLength(255)]
    public string? Link { get; set; }// 'se il link è specifico per l'appuntamento
    [Required]
    public DateOnly Data { get; set; }
    [Required]
    public TimeOnly OrarioInizio { get; set; }
    [Required]
    [Column(TypeName = "SMALLINT")]
    public int Durata { get; set; } = 1;// default 1 h
    [Required]
    [MaxLength(1)]
    [Comment("'I' = 'individuale', 'G' = 'di gruppo'")]
    public string Tipologia { get; set; } = "I";
    [Required]
    public StatoAppuntamento StatoAppuntamento { get; set; } = StatoAppuntamento.Programmato;
    [Column(TypeName = "TEXT")]
    public string? Note { get; set; }
    [Required]
    public StatoRendicAppunt StatoRendicontazione { get; set; } = StatoRendicAppunt.NonRendicontabile;
}