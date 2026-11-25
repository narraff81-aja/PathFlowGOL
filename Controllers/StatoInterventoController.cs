using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PathFlowGOL.Controllers;

[Route("api/interventi")]
[ApiController]
[Authorize] // Richiede che l'utente sia autenticato per qualsiasi rotta qui dentro
public class StatoInterventoController : ControllerBase {
    private readonly IStatoInterventoService _service;

    public StatoInterventoController(IStatoInterventoService statoInterventoService) {
        _service = statoInterventoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var statiInt = await _service.GetAllAsync();
        return Ok(statiInt);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var statoInt = await _service.GetByIdAsync(id);
        if (statoInt == null) return NotFound(new { message = "StatoIntervento non trovato." });
        return Ok(statoInt);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(50)] string stato) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(stato))
            return BadRequest(new { message = "Lo stato dell'intervento non può essere vuoto." });

        var nuovoStatoInt = await _service.CreateAsync(stato);
        if (!nuovoStatoInt.Successo)
            return StatusCode(nuovoStatoInt.StatusCode, new { message = nuovoStatoInt.MsgErrore });

        return CreatedAtAction(nameof(GetById), new { id = nuovoStatoInt.Data!.Id }, nuovoStatoInt.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Update(int id, [FromBody, Required, MaxLength(50)] string stato) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(stato))
            return BadRequest(new { message = "Lo stato dell'intervento non può essere vuoto." });

        var updated = await _service.UpdateAsync(id, stato);
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
        return Ok("StatoIntervento eliminato con successo.");
    }

    private bool TryGetCurrentUser(out RuoloUtente ruolo) {
        ruolo = RuoloUtente.Operatore;
        var roleClaim = User.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<RuoloUtente>(roleClaim, out ruolo);
    }
}

