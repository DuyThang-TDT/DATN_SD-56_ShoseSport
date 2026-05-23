using ShoseSport.API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.Web.Services.IService
{
    public interface IGiamGiaService
    {
        Task<IEnumerable<GiamGiaDTO>> GetAllAsync();
        Task<GiamGiaDTO> GetByIdAsync(Guid id);
        Task<GiamGiaDTO> CreateAsync(GiamGiaDTO dto);
        Task<bool> UpdateAsync(Guid id, GiamGiaDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}