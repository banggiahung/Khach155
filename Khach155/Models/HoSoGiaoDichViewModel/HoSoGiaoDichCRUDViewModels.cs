
using Khach155.Models;
using SEMPJ2.Models.HoSoGiaoDichViewModels;
using System.ComponentModel.DataAnnotations;
namespace SEMPJ2.Models.HoSoGiaoDichViewModels
{
    public class HoSoGiaoDichCRUDViewModels
    {
        public int ID { get; set; }
        public int UserIDInfo { get; set; }

        public DateTime ThoiGianGiaoDich { get; set; }
        public bool TrangThaiGiaoDich { get; set; }
        public string? MoTa { get; set; }

        public decimal SoTienNap { get; set; }

        public string? TenUserDisplay { get;set; }
        public string CodeLenh { get; set; }
        public int BankId { get; set; }




        public static implicit operator HoSoGiaoDichCRUDViewModels(HoSoGiaoDich _HoSo)
        {
            return new HoSoGiaoDichCRUDViewModels
            {
                ID = _HoSo.ID,
                UserIDInfo = _HoSo.UserIDInfo,
                ThoiGianGiaoDich = _HoSo.ThoiGianGiaoDich,
                TrangThaiGiaoDich = _HoSo.TrangThaiGiaoDich,
                MoTa = _HoSo.MoTa,
                SoTienNap = _HoSo.SoTienNap,
                CodeLenh = _HoSo.CodeLenh,
                BankId = _HoSo.BankId,

            };
        }

        public static implicit operator HoSoGiaoDich(HoSoGiaoDichCRUDViewModels vm)
        {
            return new HoSoGiaoDich
            {
                ID = vm.ID,
                UserIDInfo = vm.UserIDInfo,
                ThoiGianGiaoDich = vm.ThoiGianGiaoDich,
                TrangThaiGiaoDich = vm.TrangThaiGiaoDich,
                MoTa = vm.MoTa,
                SoTienNap = vm.SoTienNap,
                CodeLenh = vm.CodeLenh,
                BankId = vm.BankId,



            };
        }

    }
}
