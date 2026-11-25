using PathFlowGOL.DTOs;
using PathFlowGOL.Models;

namespace PathFlowGOL.Services;

public interface ILavoroObiettivoService {
    Task<ServiceResult<IEnumerable<LavoroObiettivoReadDTO>>> GetAllAsync();
    Task<ServiceResult<LavoroObiettivoReadDTO>> GetByIdAsync(int id);
    Task<ServiceResult<LavoroObiettivoReadDTO>> CreateAsync(string lavoro);
    Task<ServiceResult<LavoroObiettivoReadDTO>> UpdateAsync(int id, string lavoro);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    
    /*Task<IEnumerable<LavoroObiettivo>> GetAllAsync();
    Task<LavoroObiettivo?> GetByIdAsync(int id);
    Task<LavoroObiettivo> CreateAsync(string lavoro);
    Task<bool> UpdateAsync(int id, string lavoro);
    Task<(bool success, string message)> DeleteAsync(int id);*/     
}
