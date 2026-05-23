using System.ComponentModel.DataAnnotations;

namespace ShoseSport.Web.ViewModels
{
    public class AddToCartViewModel
    {
        [Required]
        public Guid SanPhamChiTietId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }
    }

}
