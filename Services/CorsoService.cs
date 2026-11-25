using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public class CorsoService : ICorsoService {
    private readonly AppDbContext _context;

    public CorsoService(AppDbContext context) {
        _context = context;
    }
    public async Task<ServiceResult<IEnumerable<CorsoReadDTO>>> GetAllAsync() {
        //return await _context.Corsi.AsNoTracking().OrderBy(c => c.Denominazione).ToListAsync();
        var list = await _context.Corsi
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .Select(i => MapToReadDto(i))
            .ToListAsync();

        return ServiceResult<IEnumerable<CorsoReadDTO>>.Ok(list);
    }

    public async Task<ServiceResult<CorsoReadDTO>> GetByIdAsync(int id) {
        //return await _context.Corsi.FindAsync(id);
        var corso = await _context.Corsi
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
        if (corso is null) {
            return ServiceResult<CorsoReadDTO>.Fail("Corso non trovato.", 404);
        }
        return ServiceResult<CorsoReadDTO>.Ok(MapToReadDto(corso));
    }

    public async Task<ServiceResult<CorsoReadDTO>> CreateAsync(CorsoCreateUpdateDTO dto) {
        var corso = new Corso {
            Denominazione = dto.Denominazione.Trim(),
            Settore = string.IsNullOrWhiteSpace(dto.Settore) ? null : dto.Settore.Trim(),
            Percorso = string.IsNullOrWhiteSpace(dto.Percorso) ? null : dto.Percorso.Trim(),
            CodiceCorso = string.IsNullOrWhiteSpace(dto.CodiceCorso) ? null : dto.CodiceCorso.Trim(),
            OreCorso = dto.OreCorso == null ? 1 : dto.OreCorso.Value,
            OreTirocinio = dto.OreTirocinio,
            TitoloStudioRichiesto = string.IsNullOrWhiteSpace(dto.TitoloStudioRichiesto) ? null : dto.TitoloStudioRichiesto.Trim(),
            RequisitoLingua = string.IsNullOrWhiteSpace(dto.RequisitoLingua) ? null : dto.RequisitoLingua.Trim(),
            TipologiaQualifica = string.IsNullOrWhiteSpace(dto.TipologiaQualifica) ? null : dto.TipologiaQualifica.Trim()
        };

        _context.Corsi.Add(corso);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<CorsoReadDTO>.Ok(MapToReadDto(corso), 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<CorsoReadDTO>.Fail("Impossibile salvare il Corso perchè collegato ad altri dati.", 409);
        }
        //return corso;
    }

    public async Task<ServiceResult<CorsoReadDTO>> UpdateAsync(int id, CorsoCreateUpdateDTO dto) {
        var corso = await _context.Corsi.FindAsync(id);
        //if (corso == null) return false;
        if (corso is null) {
            return ServiceResult<CorsoReadDTO>.Fail("Corso non trovato.", 404);
        }
        corso.Denominazione = dto.Denominazione.Trim();
        corso.Settore = string.IsNullOrWhiteSpace(dto.Settore) ? null : dto.Settore.Trim();
        corso.Percorso = string.IsNullOrWhiteSpace(dto.Percorso) ? null : dto.Percorso.Trim();
        corso.CodiceCorso = string.IsNullOrWhiteSpace(dto.CodiceCorso) ? null : dto.CodiceCorso.Trim();
        corso.OreCorso = dto.OreCorso == null ? 1 : dto.OreCorso.Value;
        corso.OreTirocinio = dto.OreTirocinio;
        corso.TitoloStudioRichiesto = string.IsNullOrWhiteSpace(dto.TitoloStudioRichiesto) ? null : dto.TitoloStudioRichiesto.Trim();
        corso.RequisitoLingua = string.IsNullOrWhiteSpace(dto.RequisitoLingua) ? null : dto.RequisitoLingua.Trim();
        corso.TipologiaQualifica = string.IsNullOrWhiteSpace(dto.TipologiaQualifica) ? null : dto.TipologiaQualifica.Trim();

        try {
            await _context.SaveChangesAsync();
            return ServiceResult<CorsoReadDTO>.Ok(MapToReadDto(corso), 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<CorsoReadDTO>.Fail("Impossibile salvare il Corso perchè collegato ad altri dati.", 409);
        }
        //return true;
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) {
        var corso = await _context.Corsi.FindAsync(id);
        //if (corso == null) return (false, "Corso non trovato.");
        if (corso is null) {
            return ServiceResult<bool>.Fail("Corso non trovato.", 404);
        }

        // Controllo di sicurezza: verifica se è stato usato - NON ha collegamento diretto

        _context.Corsi.Remove(corso);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true);
        }
        catch (Exception) {
            return ServiceResult<bool>.Fail("Impossibile eliminare il Corso perchè collegato ad altri dati.", 409);
        }
        //return (true, "Corso eliminato con successo.");
    }
    private static CorsoReadDTO MapToReadDto(Corso c) => new() {
        Id = c.Id,
        Denominazione = c.Denominazione,
        Settore = c.Settore,
        Percorso = c.Percorso,
        CodiceCorso = c.CodiceCorso,
        OreCorso = c.OreCorso,
        OreTirocinio = c.OreTirocinio,
        TitoloStudioRichiesto = c.TitoloStudioRichiesto,
        RequisitoLingua = c.RequisitoLingua,
        TipologiaQualifica = c.TipologiaQualifica,
    };
}

