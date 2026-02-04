using System.ComponentModel.DataAnnotations;

namespace FurryFriends.API.Models.DTO
{
    public class TraCuuDTO
    {
        [Required]
        public Guid HoaDonId { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
    }
}
