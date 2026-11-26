using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;

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
        if (!titoliStudio.Successo)
            return StatusCode(titoliStudio.StatusCode, new { message = titoliStudio.MsgErrore });

        return Ok(titoliStudio.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var titoloStudio = await _service.GetByIdAsync(id);
        if (!titoloStudio.Successo)
            return StatusCode(titoloStudio.StatusCode, new { message = titoloStudio.MsgErrore });

        return Ok(titoloStudio.Data);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(70)] string denominazione) {
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
        var result = await _service.DeleteAsync(id);
        if (!result.Successo) {
            return StatusCode(result.StatusCode, new { message = result.MsgErrore });
        }
        return Ok(new { message = "TitoloStudio eliminato con successo." });
    }
}

