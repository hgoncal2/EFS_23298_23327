using EFS_23298_23327.Areas.Gerir.Controllers;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
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
            
            var reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a=>a.ListaAnfitrioes).Include(c=>c.Cliente).Include(s=>s.Sala).OrderBy(u => u.ReservaDate).ToListAsync();
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

        [HttpGet]
        public async Task<IActionResult> OrdenaDataDesc(string vari,string type, Dictionary<int, string> dic) {



            var reservas = new List<Reservas>();
            if (vari == "dataI") {

                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.ReservaDate).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.ReservaDate).ToListAsync();

                }
            }
            if(vari == "dataF") {
                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.ReservaEndDate).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.ReservaEndDate).ToListAsync();

                }
            }
            if (vari == "dataC") {
                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.DataCriacao).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();

                }
            }



            return PartialView("_partialReservasTabela",reservas);

        }
        [HttpGet]
        public async Task<IActionResult> Filter(Dictionary<string, string> dictVals, Dictionary<int, string> dic,String last) {



            var reservas = new List<Reservas>();
            if (dictVals.ContainsKey("dic")) {
                reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.ReservaDate).ToListAsync();

            } else {
                var query = "";
                var orderby = "";
                var orderType = "";
                foreach (var val in dictVals) {
                    if (!val.Key.ToLower().Contains("dat")) {
                        if(val.Value.ToLower().Contains("true") || val.Value.ToLower().Contains("false")) {
                            query += @val.Key.Replace("_", ".") + "==" + val.Value;
                            TempData["lastVal"]= val.Value;
                        } else {
                                query += @val.Key.Replace("_", ".") + ".toString().Contains(\"" + val.Value + "\")";
                        }
                       


                        if (!val.Equals(dictVals.Last())) {
                            query += " && ";
                        }
                    } else {
                        orderby = val.Key;
                        orderType = val.Value;
                    }

                }
                if (query.Trim().EndsWith("&&")) {
                    query = query.Substring(0,query.LastIndexOf("&")-1);
                   
                }
                if (query == "") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(orderby + " " + orderType).ToListAsync();

                } else {
                        if (orderby == "") {
                        reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).ToListAsync();

                    } else {

                        reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).OrderBy(orderby + " "+ orderType ).ToListAsync();


                    }
                }
               
                }
            






                    TempData["dicVals"] = dictVals;
        
            TempData["SalasCor"] =dic;
            TempData["Last"] = last;
           



            return PartialView("_partialReservasTabela", reservas);

        }
        [HttpGet]
        public async Task<IActionResult> OrdenaDataCresc() {

            var reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.ReservaDate).ToListAsync();
            return PartialView("_partialReservasTabela", reservas);

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


