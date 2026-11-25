using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PathFlowGOL.Controllers;

[Route("api/titolistudio")]
[ApiController]
[Authorize] // Richiede che l'utente sia autenticato per qualsiasi rotta qui dentro
public class TitoloStudioController : ControllerBase {
    private readonly ITitoloStudioService _service;

    public TitoloStudioController(ITitoloStudioService titoloStudioService) {
        _service = titoloStudioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var titoliStudio = await _service.GetAllAsync();
        return Ok(titoliStudio);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var titoloStudio = await _service.GetByIdAsync(id);
        if (titoloStudio == null) return NotFound(new { message = "TitoloStudio non trovato." });
        return Ok(titoloStudio);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(70)] string denominazione) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(denominazione))
            return BadRequest(new { message = "La denominazione del Titolo di Studio non può essere vuota." });

        var nuovoTitoloStudio = await _service.CreateAsync(denominazione);
        if (!nuovoTitoloStudio.Successo)
            return StatusCode(nuovoTitoloStudio.StatusCode, new { message = nuovoTitoloStudio.MsgErrore });

        return CreatedAtAction(nameof(GetById), new { id = nuovoTitoloStudio.Data!.Id }, nuovoTitoloStudio.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Update(int id, [FromBody, Required, MaxLength(70)] string denominazione) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(denominazione))
            return BadRequest(new { message = "La denominazione del Titolo di Studio non può essere vuota." });

        var updated = await _service.UpdateAsync(id, denominazione);
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
        return Ok("TitoloStudio eliminato con successo.");
    }

    private bool TryGetCurrentUser(out RuoloUtente ruolo) {
        ruolo = RuoloUtente.Operatore;
        var roleClaim = User.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<RuoloUtente>(roleClaim, out ruolo);
    }
}

