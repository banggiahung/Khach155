using Khach155.Data;
using Khach155.Models;
using Khach155.Models.BangMainViewModel;
using Khach155.Models.LuuTruMuaViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Khach155.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminFbController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AdminFbController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(BangMainCRUDViewModels model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = 1;
                model.Cancel = false;
                BangMain _bang = new BangMain();
                _bang.Id = model.Id;
                _bang.NguoiBan = model.NguoiBan;
                _bang.GiaCa = model.GiaCa;
                _bang.Cancel = model.Cancel;
                _bang.UserId = model.UserId;
                _context.Add(_bang);
                await _context.SaveChangesAsync();

            }
            return Ok(model);

        }
        [HttpGet]
        public IActionResult GetDataMua()
        {
			var a = from obj in _context.LuuTruMua
					join _user in _context.DataUser on obj.UserId equals _user.Id
					where obj.MuonBan == false
					select new LuuTruMuaCRUDViewModels
					{
						Id = obj.Id,
						UserId = _user.Id,
						GiaMua = obj.GiaMua,
						MuonBan = obj.MuonBan,
						MuaCuaAi = obj.MuaCuaAi,
						TenUser = _user.UserName,
                        SoLuongMua = obj.SoLuongMua,
                        SoTienThanhToan = obj.SoTienThanhToan
					};
			return Ok(a.ToList());
		}
         
        [HttpGet]
        public IActionResult GetDataBan()
        {
            var a = from obj in _context.LuuTruBan
                    join _user in _context.DataUser on obj.UserId equals _user.Id
                    select new LuuTruMuaCRUDViewModels
                    {
                        Id = obj.Id,
                        UserId = _user.Id,
                        GiaMua = obj.GiaBan,
                        MuaCuaAi = obj.BanChoAi,
                        TenUser = _user.UserName,
                        SoLuongMua = obj.SoLuongBan,
                        SoTienThanhToan = obj.SoTienThanhToanBan
                    };
            return Ok(a.ToList());
        }

    }
}
