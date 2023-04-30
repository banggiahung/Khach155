namespace Khach155.Models
{
    public class LuuTruBan
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public decimal GiaBan { get; set; }
        public string BanChoAi { get; set; }
        public decimal SoLuongBan { get; set; }
        public decimal SoTienThanhToanBan { get; set; }


    }
}
