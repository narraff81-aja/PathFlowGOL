using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public interface IStatoInterventoService {
    Task<ServiceResult<IEnumerable<StatoInterventoReadDTO>>> GetAllAsync();
    Task<ServiceResult<StatoInterventoReadDTO>> GetByIdAsync(int id);
    Task<ServiceResult<StatoInterventoReadDTO>> CreateAsync(string stato);
    Task<ServiceResult<StatoInterventoReadDTO>> UpdateAsync(int id, string stato);
    Task<ServiceResult<bool>> DeleteAsync(int id);

}
