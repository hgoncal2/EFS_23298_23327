using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EFS_23298_23327.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;


       public ReservasController(ApplicationDbContext context) {
            _context = context;
        }

        [CustomAuthorize(Roles = "Anfitriao")]

        public async Task<IActionResult> Index() {
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u => u.Cores).FirstOrDefaultAsync();
            TempData["userLoggedNome"] = u.PrimeiroNome + " " + u.UltimoNome;
            var r = await _context.Reservas.Include(c => c.Cliente).Include(a => a.Sala).Include(a => a.ListaAnfitrioes).Where(a => a.ListaAnfitrioes.Contains(u)).OrderBy(r => r.ReservaDate).ToListAsync();
            var vm = new ReservasDashboardViewModel(u.userPrefsAn, r);
            if (TempData["Save"] != null) {
                TempData["Save"] = TempData["Save"];

            }


            return View(vm);
        }


       

        [HttpPost]
        public async Task<IActionResult> SaveCor(Dictionary<string, string> dic, string showCancel) {

            var dik = dic;
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u => u.Cores).FirstOrDefaultAsync();
            if (u.userPrefsAn == null) {
                var pref = new UserPrefsAnf();
                pref.AnfId = u.Id;
                pref.Anfitriao = u;
                u.userPrefsAn = pref;

            }
            var newListaCores = u.userPrefsAn.Cores;
            ;
            var corDict = new Dictionary<int, string>();

            var newDIct = new HashSet<UserPrefAnfCores>();


            foreach (var i in dic) {
                var cor = newListaCores.Where(c => c.Key == int.Parse(i.Key)).FirstOrDefault();
                if (cor != null) {
                    cor.Value = i.Value;
                    newDIct.Add(cor);
                } else {
                    UserPrefAnfCores c = new UserPrefAnfCores();
                    c.Key = int.Parse(i.Key);
                    c.Value = i.Value;
                    newDIct.Add(c);
                }

            }
            if (showCancel == "true") {
                u.userPrefsAn.mostrarCanceladas = true;
            } else {
                u.userPrefsAn.mostrarCanceladas = false;
            }

            u.userPrefsAn.Cores = newDIct;
            _context.Update(u);
            await _context.SaveChangesAsync();
            TempData["Save"] = "Preferências guardadas com sucesso!";

            return PartialView("_partialSave");

        }
    }
}
