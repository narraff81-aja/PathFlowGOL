using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace PathFlowGOL.Models;
/// <summary>
/// Stato di Rendicontazione dell'Appuntamento
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatoRendicAppunt {
    [EnumMember(Value = "Non Rendicontabile")]
    [Description("Appuntamento Programmato o Spostato o Annullato")]
    NonRendicontabile,
    [EnumMember(Value = "Da Rendicontare")]
    DaRendicontare,
    [EnumMember(Value = "Rendicontato")]
    Rendicontato
}