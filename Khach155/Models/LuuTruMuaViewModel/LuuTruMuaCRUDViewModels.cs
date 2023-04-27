using System.Data;

namespace Khach155.Models.LuuTruMuaViewModel
{
    public class LuuTruMuaCRUDViewModels
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public decimal GiaMua { get; set; }
        public bool MuonBan { get; set; }
        public string MuaCuaAi { get; set; }
        public string TenUser { get; set; }
        

        public static implicit operator LuuTruMuaCRUDViewModels(LuuTruMua _luuTru)
        {
            return new LuuTruMuaCRUDViewModels
            {
                Id = _luuTru.Id,
                UserId = _luuTru.UserId,
                GiaMua = _luuTru.GiaMua,
                MuonBan = _luuTru.MuonBan,
                MuaCuaAi = _luuTru.MuaCuaAi,
            };
        }

        public static implicit operator LuuTruMua(LuuTruMuaCRUDViewModels vm)
        {
            return new LuuTruMua
            {
                Id = vm.Id,
                UserId = vm.UserId,
                GiaMua = vm.GiaMua,
                MuonBan = vm.MuonBan,
                MuaCuaAi = vm.MuaCuaAi,

            };
        }
    }
}
