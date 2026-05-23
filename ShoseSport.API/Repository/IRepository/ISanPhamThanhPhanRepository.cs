using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
	public interface ISanPhamThanhPhanRepository : IRepository<SanPhamThanhPhan>
	{
		Task<IEnumerable<SanPhamThanhPhan>> GetBySanPhamIdAsync(Guid sanPhamId);
		Task<IEnumerable<SanPhamThanhPhan>> GetByThanhPhanIdAsync(Guid thanhPhanId);
		Task DeleteBySanPhamIdAsync(Guid sanPhamId);
		Task DeleteByThanhPhanIdAsync(Guid thanhPhanId);
	}
}
