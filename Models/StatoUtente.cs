using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace PathFlowGOL.Models;
/// <summary>
/// Stato dell'account dell'Operatore
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]// invia/riceve le stringhe via API invece degli interi
public enum StatoUtente {
    [EnumMember(Value = "Attivo")]
    [Description("Operatore Attivo")]
    Attivo,
    [EnumMember(Value = "Non Attivo")]
    [Description("Operatore non attivo (in generale)")]
    NonAttivo,
    [EnumMember(Value = "Sospeso")]
    [Description("Operatore sospeso")]
    Sospeso,
    [EnumMember(Value = "In Ferie")]
    [Description("Operatore in ferie")]
    InFerie
}