using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFlowGOL.Models;
using PathFlowGOL.Services;
using System.ComponentModel.DataAnnotations;

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
        if (!benefici.Successo)
            return StatusCode(benefici.StatusCode, new { message = benefici.MsgErrore });

        return Ok(benefici.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var beneficio = await _service.GetByIdAsync(id);
        if (!beneficio.Successo)
            return StatusCode(beneficio.StatusCode, new { message = beneficio.MsgErrore });

        return Ok(beneficio.Data);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RuoloUtente.SuperAdmin)},{nameof(RuoloUtente.Admin)}")] // Solo Admin e SuperAdmin possono scrivere
    public async Task<IActionResult> Create([FromBody, Required, MaxLength(60)] string beneficio) {
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
        var result = await _service.DeleteAsync(id);
        if (!result.Successo) {
            return StatusCode(result.StatusCode, new { message = result.MsgErrore });
        }
        return Ok(new { message = "TipoBeneficio eliminato con successo." });
    }
}

