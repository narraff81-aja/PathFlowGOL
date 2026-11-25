using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PathFlowGOL.Controllers;

[Route("api/benefici")]
[ApiController]
[Authorize] // Richiede che l'utente sia autenticato per qualsiasi rotta qui dentro
public class TipoBeneficioController : ControllerBase {
    private readonly ITipoBeneficioService _service;

    public TipoBeneficioController(ITipoBeneficioService tipoBeneficioService) {
        _service = tipoBeneficioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var benefici = await _service.GetAllAsync();
        return Ok(benefici);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var beneficio = await _service.GetByIdAsync(id);
        if (beneficio == null) return NotFound(new { message = "TipoBeneficio non trovato." });
        return Ok(beneficio);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(60)] string beneficio) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(beneficio))
            return BadRequest(new { message = "Il beneficio non può essere vuoto." });

        var nuovoBeneficio = await _service.CreateAsync(beneficio);
        if (!nuovoBeneficio.Successo)
            return StatusCode(nuovoBeneficio.StatusCode, new { message = nuovoBeneficio.MsgErrore });

        return CreatedAtAction(nameof(GetById), new { id = nuovoBeneficio.Data!.Id }, nuovoBeneficio.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Update(int id, [FromBody, Required, MaxLength(60)] string beneficio) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(beneficio))
            return BadRequest(new { message = "Il beneficio non può essere vuoto." });

        var updated = await _service.UpdateAsync(id, beneficio);
        if (!updated.Successo)
            return StatusCode(updated.StatusCode, new { message = updated.MsgErrore });
        return StatusCode(updated.StatusCode, updated.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Delete(int id) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        var result = await _service.DeleteAsync(id);
        if (!result.Successo) {
            return StatusCode(result.StatusCode, new { message = result.MsgErrore });
        }
        return Ok("TipoBeneficio eliminato con successo.");
    }

    private bool TryGetCurrentUser(out RuoloUtente ruolo) {
        ruolo = RuoloUtente.Operatore;
        var roleClaim = User.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<RuoloUtente>(roleClaim, out ruolo);
    }
}

