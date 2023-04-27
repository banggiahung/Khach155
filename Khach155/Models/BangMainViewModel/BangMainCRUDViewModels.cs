﻿using System.Data;

namespace Khach155.Models.BangMainViewModel
{
    public class BangMainCRUDViewModels
    {
        public int Id { get; set; }
        public string NguoiBan { get; set; }

        public decimal GiaCa { get; set; }
        public bool Cancel { get; set; }
        public int UserId { get; set; }

        public static implicit operator BangMainCRUDViewModels(BangMain _bangMain)
        {
            return new BangMainCRUDViewModels
            {
                Id = _bangMain.Id,
                NguoiBan = _bangMain.NguoiBan,
                GiaCa = _bangMain.GiaCa,
                Cancel = _bangMain.Cancel
            };
        }

        public static implicit operator BangMain(BangMainCRUDViewModels vm)
        {
            return new BangMain
            {
                Id = vm.Id,
                NguoiBan = vm.NguoiBan,
                GiaCa = vm.GiaCa,
                Cancel = vm.Cancel

            };
        }
    }
}
