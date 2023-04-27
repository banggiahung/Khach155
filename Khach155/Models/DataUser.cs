using System.ComponentModel.DataAnnotations;

namespace Khach155.Models
{
    public class DataUser
    {
        public int? Id { get; set; }
        public decimal? TienDangCo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

	}
}
