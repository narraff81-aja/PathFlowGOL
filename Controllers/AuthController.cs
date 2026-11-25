using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;
using System.Security.Claims;

namespace PathFlowGOL.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Utente> _passwordHasher;

    public AuthController(AppDbContext context, IPasswordHasher<Utente> passwordHasher) {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model) {
        // 1. Cerca l'operatore tramite Username (che corrisponde alla mail o user-id)
        var utente = await _context.Utenti
            .Include(o => o.ProfiloOperatore)
            .FirstOrDefaultAsync(o => o.Username == model.Username.Trim());

        if (utente == null) {
            return Unauthorized(new { message = "Credenziali non valide (Username inesistente)." });
        }

        // 2. Verifica se l'operatore è attivo
        if (utente.Stato != StatoUtente.Attivo) {
            return Unauthorized(new { message = "Account non attivo o sospeso." });
        }

        // 3. Verifica la password con l'Hasher sicuro
        var result = _passwordHasher.VerifyHashedPassword(utente, utente.Password, model.Password.Trim());
        if (result == PasswordVerificationResult.Failed) {
            return Unauthorized(new { message = "Credenziali non valide (Password errata)." });
        }

        // 4. Crea i Claims (le informazioni salvate nel cookie/sessione)
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString()),
            new Claim(ClaimTypes.Name, utente.Username),
            new Claim(ClaimTypes.Role, utente.Ruolo.ToString()), // "Operatore", "Admin", "SuperAdmin"
            //new Claim("IdOperatore", utente.IdOperatore == null? "": utente.IdOperatore.Value.ToString()),
            new Claim("NomeCompleto", utente.ProfiloOperatore != null ? $"{utente.ProfiloOperatore.Nome} {utente.ProfiloOperatore.Cognome}" : utente.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 5. Effettua il Login emettendo il Cookie
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Ok(new { message = "Login effettuato con successo!", ruolo = utente.Ruolo.ToString() });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logout effettuato con successo." });
    }
    /*
    [HttpGet("test-hash")]
    public IActionResult TestHash([FromQuery] string password = "PasswordDiProva123") {
        // 1. Istanziamo l'hasher di ASP.NET Core
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Utente>();

        // 2. Creiamo un finto operatore (serve solo come riferimento per l'hasher)
        var dummyOperatore = new Utente { Username = "test" };

        // 3. Generiamo l'hash
        string hashed = hasher.HashPassword(dummyOperatore, password);

        return Ok(new {
            PasswordInChiaro = password,
            PasswordCifrataHash = hashed
        });
    }*/
}
