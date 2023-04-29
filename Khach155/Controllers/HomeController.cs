using Khach155.Data;
using Khach155.Models;
using Khach155.Models.BangMainViewModel;
using Khach155.Models.DataUserViewModel;
using Khach155.Models.LuuTruMuaViewModel;
using Khach155.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using static Azure.Core.HttpHeader;

namespace Khach155.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ICommon common)
        {
            _logger = logger;
            _context = context;
            _iCommon = common;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        [HttpGet]
        public IActionResult GetDataBangMain()
        {
            var a = _context.BangMain
                .Where(x=>x.SoLuongTaiKhoan > 0)
                .ToList();
            return Ok(a);
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View();

            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(DataUserCRUDViewModels model)
        {
            var user = await _context.DataUser.SingleOrDefaultAsync(u => u.UserName == model.UserName);
            model.Password = _iCommon.GetMD5(model.Password);
            if (user != null && user.Password == model.Password)
            {
                // Đăng nhập thành công
                HttpContext.Session.SetInt32("Id", user.Id ?? 0);
                //HttpContext.Session.SetString("UserRole", user.Role);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Login = "Sai tài khoản hoặc mật khẩu";
                // Không đăng nhập được
                ModelState.AddModelError("", "Lỗi");
                return View(model);
            }
        }
        // đăng ký
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(DataUserCRUDViewModels model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem UserName đã có trong cơ sở dữ liệu chưa
                var existingUser = await _context.DataUser.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã có");
                    ViewBag.UserName = "Tài Khoản đã có";
                    return View(model);
                }

                // Thêm người dùng mới vào cơ sở dữ liệu
                model.Password = _iCommon.GetMD5(model.Password);
                model.TienDangCo = 0;
                model.SoDiem = 0;

                _context.DataUser.Add(model);
                await _context.SaveChangesAsync();

                // Đăng nhập người dùng mới và chuyển hướng đến trang chủ
                //HttpContext.Session.SetInt32("Id", model.Id ?? 0);
                return RedirectToAction("Login", "Home");
            }


            // Hiển thị thông báo lỗi nếu dữ liệu đăng ký không hợp lệ
            return View(model);
        }
        public IActionResult Logout()
        {
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MuaFb(int id)
        {
            BangMainCRUDViewModels vm = new BangMainCRUDViewModels();

            if (id > 0)
            {
                try
                {
                    vm = await _context.BangMain.Where(x => x.Id == id).FirstOrDefaultAsync() ?? new BangMain();
                    if (vm == null)
                    {
                        return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return NotFound();
            }


            return Ok(vm);
        }
        [HttpPost]
        public async Task<IActionResult> MuaFb([FromForm] BangMainCRUDViewModels data)
        {
            try
            {
				//int? userId = HttpContext.Session.GetInt32("Id");
				int? userId = 1;
                

				DataUser userData = await _context.DataUser.FindAsync(userId);
                if (userData == null || data.Id < 0 )
                {
                    return NotFound("Invalid user or data");
                }
                

				BangMain main = await _context.BangMain.FindAsync(data.Id);
                if (main == null)
                {
                    return NotFound("Invalid main");
                }
				else if (main.UserId == userId)
				{
					return NotFound("Không được mua của chính mình");

				}
				LuuTruMua luuMain = new();
                luuMain.UserId = userId;
                luuMain.GiaMua = main.GiaCa;
                luuMain.MuonBan = false;
                luuMain.MuaCuaAi = main.NguoiBan;
                luuMain.SoLuongMua = data.SoLuongUserMua;
                 
                _context.Add(luuMain);
                await _context.SaveChangesAsync();

                main.SoLuongTaiKhoan = main.SoLuongTaiKhoan - data.SoLuongUserMua;
                main.Cancel = false;
                _context.Update(main);
                await _context.SaveChangesAsync();
               
                userData.TienDangCo = userData.TienDangCo - luuMain.GiaMua;
                userData.SoDiem = userData.SoDiem + luuMain.SoLuongMua;
                _context.Update(userData);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(data);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }
        // lấy ra ra bảng để bán
        [HttpGet]
        public IActionResult GetDataProfileMain()
        {
            int? userId = HttpContext.Session.GetInt32("Id");

            var a = from obj in _context.LuuTruMua
                    join _user in _context.DataUser on obj.UserId equals _user.Id
                    where obj.MuonBan == false && _user.Id == userId
                    select new LuuTruMuaCRUDViewModels
                    {
                        Id = obj.Id,
                        UserId = userId,
                        GiaMua = obj.GiaMua,
                        MuonBan = obj.MuonBan,
                        MuaCuaAi = obj.MuaCuaAi,
                        TenUser = _user.UserName
                    };
            return Ok(a.ToList());
        }
        [HttpGet]
        public IActionResult GetDataProfileMainTien()
        {
            int? userId = HttpContext.Session.GetInt32("Id");

            var a = from _user in _context.DataUser
                    where _user.Id == userId
                    select new DataUserCRUDViewModels
                    {
                        Id = userId,
                        TienDangCo = _user.TienDangCo
                    };
            return Ok(a);
        }

        // lấy ra id của lưu trữ mua
        [HttpGet]
        public async Task<IActionResult> BanFb(int id)
        {
            LuuTruMuaCRUDViewModels vm = new LuuTruMuaCRUDViewModels();

            if (id > 0)
            {
                try
                {
                    vm = await _context.LuuTruMua.Where(x => x.Id == id).FirstOrDefaultAsync() ?? new LuuTruMua();
                    if (vm == null)
                    {
                        return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return NotFound();
            }


            return Ok(vm);
        }
        // post vào bảng chính để đăng mua

        [HttpPost]
        public async Task<IActionResult> BanFb([FromForm] LuuTruMuaCRUDViewModels data)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("Id");
                DataUser userData = await _context.DataUser.FindAsync(userId);
                if (userData == null || data.Id < 0)
                {
                    return NotFound("Invalid user or data");
                }

                LuuTruMua main = await _context.LuuTruMua.FindAsync(data.Id);
                if (main == null)
                {
                    return NotFound("Invalid main");
                }

                BangMain pushMain = new();
                pushMain.NguoiBan = userData.UserName;
                pushMain.GiaCa = data.GiaMua;
                pushMain.UserId = userId;
                pushMain.Cancel = false;
               

                _context.Add(pushMain);
                await _context.SaveChangesAsync();

                main.MuonBan = true;
                _context.Update(main);
                await _context.SaveChangesAsync();

                if (main.MuonBan == true)
                {
                    _context.LuuTruMua.Remove(main);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound("Main is not cancelled");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(data);
        }
        [HttpGet]
        public IActionResult GetBankApi()
        {
            var a = _context.BankData.ToList();
            return Ok(a);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}