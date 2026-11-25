namespace PathFlowGOL.Services;
// Ha l'obiettivo di uniformare le risposte dei vari Service
public record ServiceResult<T>(bool Successo, T? Data = default, string? MsgErrore = null, int StatusCode = 200) {
    public static ServiceResult<T> Ok(T data, int statusCode = 200) => new(true, data, null, statusCode);
    public static ServiceResult<T> Fail(string messaggio, int statusCode = 400) => new(false, default, messaggio, statusCode);
}