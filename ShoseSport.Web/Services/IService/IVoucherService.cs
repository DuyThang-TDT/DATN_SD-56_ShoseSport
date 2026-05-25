using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.Web.Services.IService
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Voucher voucher);
        Task<bool> UpdateAsync(Guid id, Voucher voucher);
        Task<bool> DeleteAsync(Guid id);
        Task<string> ValidateVoucherAsync(string voucherCode, decimal tongTienHang);
        Task<string> GetAvailableVouchersAsync(Guid khachHangId, decimal tongTienHang);
    }
}