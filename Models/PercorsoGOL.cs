using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace PathFlowGOL.Models;
/// <summary>
/// Percorso del programma GOL
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PercorsoGOL {
    [EnumMember(Value = "1 - Reinserimento lavorativo")]
    [Description("Reinserimento lavorativo (1)")]
    ReinserimentoLavorativo=1,
    [EnumMember(Value = "2 - Upskilling")]
    [Description("Upskilling (2)")]
    Upskilling,
    [EnumMember(Value = "3 - Reskilling")]
    [Description("Reskilling (3)")]
    Reskilling,
    [EnumMember(Value = "4 - Lavoro e Inclusione")]
    [Description("Lavoro e Inclusione (4)")]
    LavoroInclusione,
    [EnumMember(Value = "5 - Ricollocazione Collettiva")]
    [Description("Ricollocazione Collettiva (5)")]
    RicollocazioneCollettiva

    /*
    "Accompagnamento"
    "Acc. all'avvio d'impresa"
     */
}