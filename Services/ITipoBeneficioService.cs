using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public interface ITipoBeneficioService {
    Task<ServiceResult<IEnumerable<TipoBeneficioReadDTO>>> GetAllAsync();
    Task<ServiceResult<TipoBeneficioReadDTO>> GetByIdAsync(int id);
    Task<ServiceResult<TipoBeneficioReadDTO>> CreateAsync(string beneficio);
    Task<ServiceResult<TipoBeneficioReadDTO>> UpdateAsync(int id, string beneficio);
    Task<ServiceResult<bool>> DeleteAsync(int id);

}
