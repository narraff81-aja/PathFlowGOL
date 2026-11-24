using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace PathFlowGOL.Models;
/// <summary>
/// Ruolo dell'Operatore rispetto al sito web
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuoloUtente {
    [Description("Operatore")]
    Operatore = 1,
    [Description("Operatore con poteri maggiori")]
    Admin=13,
    [Description("Admin con poteri maggiori")]
    SuperAdmin=47
}
