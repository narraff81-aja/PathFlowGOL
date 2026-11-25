using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.Security.Claims;

namespace PathFlowGOL.Controllers;

[Route("api/corsi")]
[ApiController]
[Authorize] // Richiede che l'utente sia autenticato per qualsiasi rotta qui dentro
public class CorsoController : ControllerBase {
    private readonly ICorsoService _service;

    public CorsoController(ICorsoService corsoService) {
        _service = corsoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var corsi = await _service.GetAllAsync();
        return Ok(corsi);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var corso = await _service.GetByIdAsync(id);
        if (corso == null) return NotFound(new { message = "Corso non trovato." });
        return Ok(corso);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody] CorsoCreateUpdateDTO dto) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(dto.Denominazione))
            return BadRequest(new { message = "La denominazione del corso è obbligatoria." });
        if (!dto.OreCorso.HasValue)
            return BadRequest(new { message = "Le ore del corso sono obbligatorie, almeno 1." });

        var nuovoCorso = await _service.CreateAsync(dto);
        //return CreatedAtAction(nameof(GetById), new { id = nuovoCorso.Id }, nuovoCorso);
        if (!nuovoCorso.Successo)
            return StatusCode(nuovoCorso.StatusCode, new { message = nuovoCorso.MsgErrore });        

        return CreatedAtAction(nameof(GetById), new { id = nuovoCorso.Data!.Id }, nuovoCorso.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CorsoCreateUpdateDTO dto) {
        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(dto.Denominazione))
            return BadRequest(new { message = "La denominazione del corso è obbligatoria." });

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated.Successo)
            return StatusCode(updated.StatusCode, new { message = updated.MsgErrore });

        //return Ok(new { message = "Corso aggiornato con successo." });
        return StatusCode(updated.StatusCode, updated.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Delete(int id) {
        /*var (success, message) = await _service.DeleteAsync(id);
        if (!success) return BadRequest(new { message });
        return Ok(new { message });*/

        if (!TryGetCurrentUser(out var ruolo)) {
            return Unauthorized();
        }

        var result = await _service.DeleteAsync(id);
        if (!result.Successo) {
            return StatusCode(result.StatusCode, new { message = result.MsgErrore });
        }
        return Ok( "Corso eliminato con successo." );
    }

    private bool TryGetCurrentUser(out RuoloUtente ruolo) {
        ruolo = RuoloUtente.Operatore;
        var roleClaim = User.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<RuoloUtente>(roleClaim, out ruolo);
    }
}

