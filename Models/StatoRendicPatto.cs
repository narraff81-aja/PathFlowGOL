using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace PathFlowGOL.Models;
/// <summary>
/// Stato di Rendicontazione del Patto
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatoRendicPatto {
    [EnumMember(Value = "Non Rendicontato")]
    NonRendicontato,
    [EnumMember(Value = "In Attesa")]
    [Description("Svolto e pronto per Rendicontazione")]
    InAttesa,
    [EnumMember(Value = "In Parte")]
    [Description("Parzialmente Rendicontato")]
    InParteRendic,
    [EnumMember(Value = "Rendicontato")]
    Rendicontato
}