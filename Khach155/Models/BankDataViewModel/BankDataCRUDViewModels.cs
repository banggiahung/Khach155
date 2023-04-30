using Khach155.Models;

namespace Khach155.Models.BankDataViewModel
{
    public class BankDataCRUDViewModels
    {
          public int Id { get; set; }
        public string BankName { get; set; }
        public string TaiKhoanBank { get; set; }
        public string TokenBank { get; set; }
        public string TenTaiKhoan { get; set; }

        public static implicit operator BankDataCRUDViewModels(BankData _bank)
        {
            return new BankDataCRUDViewModels
            {
                Id = _bank.Id,
                BankName = _bank.BankName,
                TaiKhoanBank = _bank.TaiKhoanBank,
                TokenBank = _bank.TokenBank,
                TenTaiKhoan = _bank.TenTaiKhoan

            };
        }

        public static implicit operator BankData(BankDataCRUDViewModels vm)
        {
            return new BankData
            {
                Id = vm.Id,
                BankName = vm.BankName,
                TaiKhoanBank = vm.TaiKhoanBank,
                TokenBank = vm.TokenBank,
                TenTaiKhoan = vm.TenTaiKhoan


            };
        }
    }
}
