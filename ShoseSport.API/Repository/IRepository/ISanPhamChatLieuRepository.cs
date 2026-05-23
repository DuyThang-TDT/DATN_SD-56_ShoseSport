using ShoseSport.API.Models;
using System.Linq.Expressions;

namespace ShoseSport.API.Repository.IRepository
{
	public interface ISanPhamChatLieuRepository : IRepository<SanPhamChatLieu>
	{
		Task<IEnumerable<SanPhamChatLieu>> GetBySanPhamIdAsync(Guid sanPhamId);
		Task<IEnumerable<SanPhamChatLieu>> GetByChatLieuIdAsync(Guid chatLieuId);
		Task DeleteBySanPhamIdAsync(Guid sanPhamId);
		Task DeleteByChatLieuIdAsync(Guid chatLieuId);
	}
}
