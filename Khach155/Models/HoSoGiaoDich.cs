using System.ComponentModel.DataAnnotations;

namespace Khach155.Models
{
    public class HoSoGiaoDich
    {
        public int ID { get; set; }
        public int UserIDInfo { get; set; }

        public DateTime ThoiGianGiaoDich { get; set; }
        public bool TrangThaiGiaoDich { get; set; }
        public string? MoTa { get; set; }

        public decimal SoTienNap { get; set; }
        public string CodeLenh { get; set; }
        public int BankId { get; set; }


    }
}
