using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public class TitoloStudioService : ITitoloStudioService {
    private readonly AppDbContext _context;

    public TitoloStudioService(AppDbContext context) {
        _context = context;
    }

    public async Task<ServiceResult<IEnumerable<TitoloStudioReadDTO>>> GetAllAsync() {
        var list = await _context.TitoliStudio
            .AsNoTracking()
            .OrderBy(i => i.Denominazione)
            .Select(i => new TitoloStudioReadDTO {
                Id = i.Id,
                Denominazione = i.Denominazione
            })
            .ToListAsync();

        return ServiceResult<IEnumerable<TitoloStudioReadDTO>>.Ok(list);
    }

    public async Task<ServiceResult<TitoloStudioReadDTO>> GetByIdAsync(int id) {
        var titStudio = await _context.TitoliStudio
    .AsNoTracking()
    .FirstOrDefaultAsync(i => i.Id == id);
        if (titStudio is null) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("TitoloStudio non trovato con quell'id.", 404);
        }
        return ServiceResult<TitoloStudioReadDTO>.Ok(new TitoloStudioReadDTO {
            Id = titStudio.Id,
            Denominazione = titStudio.Denominazione
        });
    }

    public async Task<ServiceResult<TitoloStudioReadDTO>> CreateAsync(string denominazione) {
        // Blocco per mantenere UNIQUE
        var testoPulito = denominazione.Trim();
        bool esisteGia = await _context.TitoliStudio.AsNoTracking()
            .AnyAsync(x => x.Denominazione.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("Esiste già un TitoloStudio con questa denominazione.", 409);
        }

        var titStudio = new TitoloStudio { Denominazione = testoPulito };
        _context.TitoliStudio.Add(titStudio);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<TitoloStudioReadDTO>.Ok(new TitoloStudioReadDTO {
                Id = titStudio.Id,
                Denominazione = titStudio.Denominazione
            }, 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("Impossibile salvare: il TitoloStudio è già presente.", 409);
        }
    }

    public async Task<ServiceResult<TitoloStudioReadDTO>> UpdateAsync(int id, string denominazione) {
        var titStudio = await _context.TitoliStudio.FindAsync(id);
        if (titStudio is null) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("TitoloStudio non trovato con quell'id.", 404);
        }

        // Blocco per mantenere UNIQUE
        var testoPulito = denominazione.Trim();
        bool esisteGia = await _context.TitoliStudio
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Denominazione.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("Esiste già un TitoloStudio con questa denominazione.", 409);
        }
        // /Blocco

        titStudio.Denominazione = testoPulito;
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<TitoloStudioReadDTO>.Ok(new TitoloStudioReadDTO {
                Id = titStudio.Id,
                Denominazione = titStudio.Denominazione
            });
        }
        catch (DbUpdateException) {
            return ServiceResult<TitoloStudioReadDTO>.Fail("Impossibile salvare: il TitoloStudio è già presente.", 409);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) {
        var titStudio = await _context.TitoliStudio.FindAsync(id);
        if (titStudio == null)
            return ServiceResult<bool>.Fail("TitoloStudio non trovato con quell'id.", 404);

        // Controllo di sicurezza: verifica se è stato usato
        bool isUsed = await _context.GestionePatti
            .AsNoTracking()
            .AnyAsync(g => g.IdTitoloStudio == id);
        if (isUsed)
            return ServiceResult<bool>.Fail("Impossibile eliminare il TitoloStudio: risulta associato a uno o più patti.", 409);

        _context.TitoliStudio.Remove(titStudio);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true);
        }
        catch (DbUpdateException) {
            return ServiceResult<bool>.Fail("Impossibile eliminare il TitoloStudio perchè collegato ad altri dati.", 409);
        }
    }
}

