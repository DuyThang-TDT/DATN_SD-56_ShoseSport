using ShoseSport.API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Services.IServices
{
    public interface IChatLieuService
    {
        Task<IEnumerable<ChatLieuDTO>> GetAllAsync();
        Task<ChatLieuDTO> GetByIdAsync(Guid id);
        Task<ChatLieuDTO> CreateAsync(ChatLieuDTO dto);
        Task<bool> UpdateAsync(Guid id, ChatLieuDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}