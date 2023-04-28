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
			var a = _context.BangMain.ToList();
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
					vm = await _context.BangMain.Where(x => x.Id == id).FirstOrDefaultAsync()?? new BangMain();
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
                int? userId = HttpContext.Session.GetInt32("Id");

                DataUser userData = await _context.DataUser.FindAsync(userId);
				if (userData == null || data.Id < 0)
				{
					return NotFound("Invalid user or data");
				}

				BangMain main = await _context.BangMain.FindAsync(data.Id);
				if (main == null)
				{
					return NotFound("Invalid main");
				}

				LuuTruMua luuMain = new();
				luuMain.UserId = userId;
				luuMain.GiaMua = main.GiaCa;
				luuMain.MuonBan = false;
				luuMain.MuaCuaAi = main.NguoiBan;

				_context.Add(luuMain);
				await _context.SaveChangesAsync();

				main.Cancel = true;
				_context.Update(main);
				await _context.SaveChangesAsync();

				if (main.Cancel == true)
				{
					_context.BangMain.Remove(main);
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


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}