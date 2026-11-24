using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathFlowGOL.Models;
/// <summary>
/// Dati ottenuti da fogli: MESE, file: "APPUNTAMENTI OR_ACC (1).xlsx";
/// </summary>
public class ChecklistDocumentale {
    [Key]
    public int Id { get; set; }

    // Relazione 1 a 1
    public GestionePatto? Patto { get; set; } 

    /// <summary>
    /// mail finale inviata a utente?
    /// </summary>
    public bool? MailFinaleInviata { get; set; }
    /// <summary>
    /// SKILL GAP 1^ domanda: nome e cognome non scambiati?
    /// </summary>
    public bool? NomeCognomeInOrdine { get; set; }
    /// <summary>
    /// SKILL GAP 2^ domanda: firma autografa?
    /// </summary>
    public bool? FirmaAutografa { get; set; }
    /// <summary>
    /// Orient.: Unico file in pdf caricato?
    /// </summary>
    public bool? OrientUnicoFilePDF { get; set; }
    /// <summary>
    /// Orient. Privacy: data e firma autografa inserita?
    /// </summary>
    public bool? OrientPrivacyFirmata { get; set; }
    /// <summary>
    /// Orient. CV: data e firma autografa inserita?
    /// </summary>
    public bool? OrientCVFirmato { get; set; }
    /// <summary>
    /// Orient. Scheda Profilo in formato word?
    /// </summary>
    public bool? OrientSchedaProfiloWord { get; set; }
    /// <summary>
    /// Bilancio Competenze in formato pdf?
    /// </summary>
    public bool? BilancioCompetenzePDF { get; set; }
    /// <summary>
    /// Accomp.: Unico file in pdf caricato?
    /// </summary>
    public bool? AccompUnicoFilePDF { get; set; }
    /// <summary>
    /// Accomp. Privacy: data e firma autografa inserita?
    /// </summary>
    public bool? AccompPrivacyFirmata { get; set; }
    /// <summary>
    /// Accomp. CV: data e firma autografa inserita?
    /// </summary>
    public bool? AccompCVFirmato { get; set; }
    /// <summary>
    /// Accomp. Scheda Profilo in formato word?
    /// </summary>
    public bool? AccompSchedaProfiloWord { get; set; }
    /// <summary>
    /// Modulo Conclusivo firmato solo dall'utente?
    /// </summary>
    public bool? ModuloConclusivoFirmato { get; set; }
}