using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;

namespace PathFlowGOL.Controllers;

[Route("api/lavoriobiettivo")]
[ApiController]
[Authorize] // Richiede che l'utente sia autenticato per qualsiasi rotta qui dentro
public class LavoroObiettivoController : ControllerBase {
    private readonly ILavoroObiettivoService _service;

    public LavoroObiettivoController(ILavoroObiettivoService lavoroObiettivoService) {
        _service = lavoroObiettivoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var lavori = await _service.GetAllAsync();
        //return Ok(lavori);
        if (!lavori.Successo)
            return StatusCode(lavori.StatusCode, new { message = lavori.MsgErrore });

        return Ok(lavori.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var lavoro = await _service.GetByIdAsync(id);
        /*if (lavoro == null) return NotFound(new { message = "LavoroObiettivo non trovato." });
        return Ok(lavoro);*/
        if (!lavoro.Successo)
            return StatusCode(lavoro.StatusCode, new { message = lavoro.MsgErrore });

        return Ok(lavoro.Data);
    }

    [HttpPost]
    //[Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(130)] string lavoro) {
        if (string.IsNullOrWhiteSpace(lavoro))
            return BadRequest(new { message = "La denominazione del lavoro non può essere vuota." });

        var nuovoLavoro = await _service.CreateAsync(lavoro);
        //return CreatedAtAction(nameof(GetById), new { id = nuovoLavoro.Id }, nuovoLavoro);
        if (!nuovoLavoro.Successo)
            return StatusCode(nuovoLavoro.StatusCode, new { message = nuovoLavoro.MsgErrore });

        return CreatedAtAction(nameof(GetById), new { id = nuovoLavoro.Data!.Id }, nuovoLavoro.Data);
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")]
    public async Task<IActionResult> Update(int id, [FromBody, Required, MaxLength(130)] string lavoro) {
        if (string.IsNullOrWhiteSpace(lavoro))
            return BadRequest(new { message = "La denominazione del lavoro non può essere vuota." });

        var updated = await _service.UpdateAsync(id, lavoro);
        /*if (!updated) 
            return NotFound(new { message = "LavoroObiettivo non trovato." });
        return Ok(new { message = "Lavoro Obiettivo aggiornato con successo." });*/
        if (!updated.Successo)
            return StatusCode(updated.StatusCode, new { message = updated.MsgErrore });
        return StatusCode(updated.StatusCode, updated.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono eliminare
    public async Task<IActionResult> Delete(int id) {
        /*var (success, message) = await _service.DeleteAsync(id);
        if (!success) return BadRequest(new { message });
        return Ok(new { message });*/
        var result = await _service.DeleteAsync(id);
        if (!result.Successo) {
            return StatusCode(result.StatusCode, new { message = result.MsgErrore });
        }
        return Ok(new { message = "LavoroObiettivo eliminato con successo." });
    }
}

