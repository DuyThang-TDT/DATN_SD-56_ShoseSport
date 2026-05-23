using ShoseSport.API.Models.DTO;

namespace ShoseSport.API.Services.IServices
{
    public interface IMauSacService
    {
        Task<IEnumerable<MauSacDTO>> GetAllAsync();
        Task<MauSacDTO> GetByIdAsync(Guid id);
        Task<MauSacDTO> CreateAsync(MauSacDTO dto);
        Task<bool> UpdateAsync(Guid id, MauSacDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
