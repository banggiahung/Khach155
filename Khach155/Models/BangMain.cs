namespace Khach155.Models
{
    public class BangMain
    {
        public int Id { get; set; }
        public string NguoiBan { get; set; }

        public decimal GiaCa { get; set; }
        public bool Cancel { get; set; }
        public int? UserId { get; set; }

    }
}
