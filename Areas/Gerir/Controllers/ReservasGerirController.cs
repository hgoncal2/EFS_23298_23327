using EFS_23298_23327.Areas.Gerir.Controllers;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles ="Admin,Anfitriao")]
    [Area("Gerir")]
    public class ReservasGerirController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public ReservasGerirController(ApplicationDbContext context, UserManager<Utilizadores> userManager,
     RoleManager<IdentityRole> roleManager) {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            //todos os utilizadores não apagados
            
            var reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a=>a.ListaAnfitrioes).Include(c=>c.Cliente).Include(s=>s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u => u.Cores).FirstOrDefaultAsync();
            if (u != null) {
                if (u.userPrefsAn != null) {
                    Dictionary<int, string> coresSalas = new Dictionary<int, string>();

                    foreach (var i in u.userPrefsAn.Cores) {
                        var sNum=reservas.Where(s => s.SalaId == i.Key).Select(s => s.Sala.Numero).First();


                        coresSalas.Add(sNum, i.Value);
                    }
                    TempData["SalasCor"] = coresSalas;


                }
            }
          
            

            return View(reservas);
        }
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            var reserva = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala)
.FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reserva == null) {
                return NotFound();
            }
            TempData["Cancelavel"] = DateTime.Now.AddHours(48) < reserva.ReservaDate;

            return View(reserva);
        }
    }
}


