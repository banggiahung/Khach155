using Khach155.Data;
using Khach155.Models;
using Khach155.Models.BangMainViewModel;
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

    }
}
