using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public class TipoBeneficioService : ITipoBeneficioService {
    private readonly AppDbContext _context;

    public TipoBeneficioService(AppDbContext context) {
        _context = context;
    }

    public async Task<ServiceResult<IEnumerable<TipoBeneficioReadDTO>>> GetAllAsync() {
        var list = await _context.TipiBenefici
            .AsNoTracking()
            .OrderBy(i => i.Beneficio)
            .Select(i => new TipoBeneficioReadDTO {
                Id = i.Id,
                Beneficio = i.Beneficio
            })
            .ToListAsync();

        return ServiceResult<IEnumerable<TipoBeneficioReadDTO>>.Ok(list);
    }

    public async Task<ServiceResult<TipoBeneficioReadDTO>> GetByIdAsync(int id) {
        var benef = await _context.TipiBenefici
    .AsNoTracking()
    .FirstOrDefaultAsync(i => i.Id == id);
        if (benef is null) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("TipoBeneficio non trovato con quell'id.", 404);
        }
        return ServiceResult<TipoBeneficioReadDTO>.Ok(new TipoBeneficioReadDTO {
            Id = benef.Id,
            Beneficio = benef.Beneficio
        });
    }

    public async Task<ServiceResult<TipoBeneficioReadDTO>> CreateAsync(string beneficio) {
        // Blocco per mantenere UNIQUE
        var testoPulito = beneficio.Trim();
        bool esisteGia = await _context.TipiBenefici.AsNoTracking()
            .AnyAsync(x => x.Beneficio.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("Esiste già un TipoBeneficio con questa denominazione.", 409);
        }

        var benef = new TipoBeneficio { Beneficio = testoPulito };
        _context.TipiBenefici.Add(benef);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<TipoBeneficioReadDTO>.Ok(new TipoBeneficioReadDTO {
                Id = benef.Id,
                Beneficio = benef.Beneficio
            }, 201);
        }
        catch (DbUpdateException) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("Impossibile salvare: il TipoBeneficio è già presente.", 409);
        }
    }

    public async Task<ServiceResult<TipoBeneficioReadDTO>> UpdateAsync(int id, string beneficio) {
        var benef = await _context.TipiBenefici.FindAsync(id);
        if (benef is null) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("TipoBeneficio non trovato con quell'id.", 404);
        }

        // Blocco per mantenere UNIQUE
        var testoPulito = beneficio.Trim();
        bool esisteGia = await _context.TipiBenefici
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Beneficio.ToLower() == testoPulito.ToLower());
        if (esisteGia) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("Esiste già un TipoBeneficio con questa denominazione.", 409);
        }
        // /Blocco

        benef.Beneficio = testoPulito;
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<TipoBeneficioReadDTO>.Ok(new TipoBeneficioReadDTO {
                Id = benef.Id,
                Beneficio = benef.Beneficio
            });
        }
        catch (DbUpdateException) {
            return ServiceResult<TipoBeneficioReadDTO>.Fail("Impossibile salvare: il TipoBeneficio è già presente.", 409);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) {
        var benef = await _context.TipiBenefici.FindAsync(id);
        if (benef == null)
            return ServiceResult<bool>.Fail("TipoBeneficio non trovato con quell'id.", 404);

        // Controllo di sicurezza: verifica se è stato usato
        bool isUsed = await _context.GestionePatti
            .AsNoTracking()
            .AnyAsync(g => g.IdBeneficio == id);
        if (isUsed)
            return ServiceResult<bool>.Fail("Impossibile eliminare il TipoBeneficio: risulta associato a uno o più patti.", 409);

        _context.TipiBenefici.Remove(benef);
        try {
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true);
        }
        catch (DbUpdateException) {
            return ServiceResult<bool>.Fail("Impossibile eliminare il TipoBeneficio perchè collegato ad altri dati.", 409);
        }
    }
}

