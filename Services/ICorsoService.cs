using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public interface ICorsoService {
    Task<ServiceResult<IEnumerable<CorsoReadDTO>>> GetAllAsync();
    Task<ServiceResult<CorsoReadDTO>> GetByIdAsync(int id);
    Task<ServiceResult<CorsoReadDTO>> CreateAsync(CorsoCreateUpdateDTO dto);
    Task<ServiceResult<CorsoReadDTO>> UpdateAsync(int id, CorsoCreateUpdateDTO dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);

    /*Task<IEnumerable<Corso>> GetAllAsync();
    Task<Corso?> GetByIdAsync(int id);
    Task<Corso> CreateAsync(CorsoCreateUpdateDTO dto);
    Task<bool> UpdateAsync(int id, CorsoCreateUpdateDTO dto);
    Task<(bool success, string message)> DeleteAsync(int id);*/

}
