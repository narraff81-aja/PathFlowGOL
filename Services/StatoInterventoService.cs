using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public class StatoInterventoService : IStatoInterventoService {
    private readonly AppDbContext _context;

    public StatoInterventoService(AppDbContext context) {
        _context = context;
    }

    public async Task<ServiceResult<IEnumerable<StatoInterventoReadDTO>>> GetAllAsync() {
        var list = await _context.StatiIntervento
            .AsNoTracking()
            .OrderBy(i => i.Stato)
            .Select(i => new StatoInterventoReadDTO {
                Id = i.Id,
                Stato = i.Stato
            })
            .ToListAsync();

        return ServiceResult<IEnumerable<StatoInterventoReadDTO>>.Ok(list);
    }

    public async Task<ServiceResult<StatoInterventoReadDTO>> GetByIdAsync(int id) {
        var statoInt = await _context.StatiIntervento
    .AsNoTracking()
    .FirstOrDefaultAsync(i => i.Id == id);
        if (statoInt is null) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("StatoIntervento non trovato.", 404);
        }
        return ServiceResult<StatoInterventoReadDTO>.Ok(new StatoInterventoReadDTO {
            Id = statoInt.Id,
            Stato = statoInt.Stato
        });
    }

    public async Task<ServiceResult<StatoInterventoReadDTO>> CreateAsync(string stato) {
        // Blocco per mantenere UNIQUE
        var testoPulito = stato.Trim();
        bool esisteGia = await _context.StatiIntervento.AsNoTracking()
            .AnyAsync(x => x.Stato.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("Esiste già uno StatoIntervento con questa denominazione.", 409);
        }

        var statoInt = new StatoIntervento { Stato = testoPulito };
        _context.StatiIntervento.Add(statoInt);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<StatoInterventoReadDTO>.Ok(new StatoInterventoReadDTO {
                Id = statoInt.Id,
                Stato = statoInt.Stato
            }, 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("Impossibile salvare: lo StatoIntervento è già presente.", 409);
        }
    }

    public async Task<ServiceResult<StatoInterventoReadDTO>> UpdateAsync(int id, string stato) {
        // Blocco per mantenere UNIQUE
        var testoPulito = stato.Trim();
        bool esisteGia = await _context.StatiIntervento
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Stato.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("Esiste già uno StatoIntervento con questa denominazione.", 409);
        }


        var statoInt = await _context.StatiIntervento.FindAsync(id);
        if (statoInt is null) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("StatoIntervento non trovato.", 404);
        }
        statoInt.Stato = testoPulito;
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<StatoInterventoReadDTO>.Ok(new StatoInterventoReadDTO {
                Id = statoInt.Id,
                Stato = statoInt.Stato
            }, 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<StatoInterventoReadDTO>.Fail("Impossibile salvare: lo StatoIntervento è già presente.", 409);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) {
        var statoInt = await _context.StatiIntervento.FindAsync(id);
        if (statoInt == null)
            return ServiceResult<bool>.Fail("StatoIntervento non trovato.", 404);

        // Controllo di sicurezza: verifica se è stato usato
        bool isUsed = await _context.GestionePatti
            .AsNoTracking()
            .AnyAsync(g => g.IdStatoIntervento == id);
        if (isUsed)
            return ServiceResult<bool>.Fail("Impossibile eliminare lo StatoIntervento: risulta associato a uno o più patti.", 409);

        _context.StatiIntervento.Remove(statoInt);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true);
        }
        catch (Exception) {
            return ServiceResult<bool>.Fail("Impossibile eliminare lo StatoIntervento perchè collegato ad altri dati.", 409);
        }
    }
}

