namespace Khach155.Models
{
    public class LuuTruMua
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public decimal GiaMua { get; set; }
        public bool MuonBan { get; set; }
        public string MuaCuaAi { get; set; }
        public decimal SoLuongMua { get; set; }
        public decimal SoTienThanhToan { get; set; }


    }
}
