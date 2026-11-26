using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public class LavoroObiettivoService : ILavoroObiettivoService {
    private readonly AppDbContext _context;

    public LavoroObiettivoService(AppDbContext context) {
        _context = context;
    }

    public async Task<ServiceResult<IEnumerable<LavoroObiettivoReadDTO>>> GetAllAsync() {
        //return await _context.LavoriObiettivo.AsNoTracking().OrderBy(c => c.Lavoro).ToListAsync();
        var list = await _context.LavoriObiettivo
            .AsNoTracking()
            .OrderBy(i => i.Lavoro)
            .Select(i => new LavoroObiettivoReadDTO {
                Id = i.Id,
                Lavoro = i.Lavoro
            })
            .ToListAsync();

        return ServiceResult<IEnumerable<LavoroObiettivoReadDTO>>.Ok(list);
    }

    public async Task<ServiceResult<LavoroObiettivoReadDTO>> GetByIdAsync(int id) {
        //return await _context.LavoriObiettivo.FindAsync(id);
        var lav = await _context.LavoriObiettivo
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
        if (lav is null) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("LavoroObiettivo non trovato con quell'id.", 404);
        }
        return ServiceResult<LavoroObiettivoReadDTO>.Ok(new LavoroObiettivoReadDTO {
            Id = lav.Id,
            Lavoro = lav.Lavoro
        });
    }

    public async Task<ServiceResult<LavoroObiettivoReadDTO>> CreateAsync(string lavoro) {
        /*var lav = new LavoroObiettivo { Lavoro = lavoro };
        _context.LavoriObiettivo.Add(lav);
        await _context.SaveChangesAsync();
        return lav;*/


        // Blocco per mantenere UNIQUE
        var testoPulito = lavoro.Trim();
        bool esisteGia = await _context.LavoriObiettivo.AsNoTracking()
            .AnyAsync(x => x.Lavoro.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("Esiste già un LavoroObiettivo con questa denominazione.", 409);
        }

        var lav = new LavoroObiettivo { Lavoro = testoPulito };
        _context.LavoriObiettivo.Add(lav);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<LavoroObiettivoReadDTO>.Ok(new LavoroObiettivoReadDTO {
                Id = lav.Id,
                Lavoro = lav.Lavoro
            }, 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("Impossibile salvare: il LavoroObiettivo è già presente.", 409);
        }
    }

    public async Task<ServiceResult<LavoroObiettivoReadDTO>> UpdateAsync(int id, string lavoro) {
        /*var lav = await _context.LavoriObiettivo.FindAsync(id);
        if (lav == null) return false;

        lav.Lavoro = lavoro;
        await _context.SaveChangesAsync();
        return true;*/
        var lav = await _context.LavoriObiettivo.FindAsync(id);
        if (lav is null) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("LavoroObiettivo non trovato con quell'id.", 404);
        }

        // Blocco per mantenere UNIQUE
        var testoPulito = lavoro.Trim();
        bool esisteGia = await _context.LavoriObiettivo
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Lavoro.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("Esiste già un LavoroObiettivo con questa denominazione.", 409);
        }
        // /Blocco

        lav.Lavoro = testoPulito;
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<LavoroObiettivoReadDTO>.Ok(new LavoroObiettivoReadDTO {
                Id = lav.Id,
                Lavoro = lav.Lavoro
            });
        }
        catch (DbUpdateException) {
            return ServiceResult<LavoroObiettivoReadDTO>.Fail("Impossibile salvare: il LavoroObiettivo è già presente.", 409);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) {
        /*
        var lav = await _context.LavoriObiettivo.FindAsync(id);
        if (lav == null) return (false, "Lavoro Obiettivo non trovato.");

        // Controllo di sicurezza: verifica se è stato usato
        bool isUsed = await _context.LavoriObiettivo.AsNoTracking().AnyAsync(g => g.IdLavoroObiettivo == id);
        if (isUsed) return (false, "Impossibile eliminare il LavoroObiettivo: risulta associato a uno o più patti.");

        _context.LavoriObiettivo.Remove(lav);
        await _context.SaveChangesAsync();
        return (true, "Lavoro Obiettivo eliminato con successo.");*/

        var lav = await _context.LavoriObiettivo.FindAsync(id);
        if (lav == null)
            return ServiceResult<bool>.Fail("LavoroObiettivo non trovato con quell'id.", 404);

        // Controllo di sicurezza: verifica se è stato usato
        bool isUsed = await _context.GestionePatti
            .AsNoTracking()
            .AnyAsync(g => g.IdLavoroObiettivo == id);
            // oppure .AnyAsync(g => g.IdLavoroObiettivo.HasValue && g.IdLavoroObiettivo.Value == id);
        if (isUsed)
            return ServiceResult<bool>.Fail("Impossibile eliminare il LavoroObiettivo: risulta associato a uno o più patti.", 409);

        _context.LavoriObiettivo.Remove(lav);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true);
        }
        catch (DbUpdateException) {
            return ServiceResult<bool>.Fail("Impossibile eliminare il LavoroObiettivo perchè collegato ad altri dati.", 409);
        }
    }
}

