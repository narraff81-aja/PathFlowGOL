using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PathFlowGOL.Models;
/// <summary>
/// Stato di Rendicontazione dell'Appuntamento
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatoAppuntamento {
    [EnumMember(Value = "Programmato")]
    Programmato,
    [EnumMember(Value = "Spostato")]
    Spostato,
    [EnumMember(Value = "Svolto")]
    Svolto,
    [EnumMember(Value = "Annullato")]
    Annullato
}