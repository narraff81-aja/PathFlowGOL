using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public interface ITitoloStudioService {
    Task<ServiceResult<IEnumerable<TitoloStudioReadDTO>>> GetAllAsync();
    Task<ServiceResult<TitoloStudioReadDTO>> GetByIdAsync(int id);
    Task<ServiceResult<TitoloStudioReadDTO>> CreateAsync(string denominazione);
    Task<ServiceResult<TitoloStudioReadDTO>> UpdateAsync(int id, string denominazione);
    Task<ServiceResult<bool>> DeleteAsync(int id);

}
