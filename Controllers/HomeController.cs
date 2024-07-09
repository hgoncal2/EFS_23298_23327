using EFS_23298_23327.Data;
using EFS_23298_23327.Migrations;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Common;
using System.Diagnostics;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFS_23298_23327.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        private readonly ApplicationDbContext _context;
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated) {
                var u = await _context.Utilizadores.Where(a => a.UserName == User.Identity.Name).FirstOrDefaultAsync();
                if (u != null) { 
                
                }

            }
            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);
            ICollection<TemasFotoViewModel> TmVm = new List<TemasFotoViewModel>();
            var lista = await applicationDbContext.ToListAsync();
            if (lista != null)
            {
                if (lista.Any())
                {
                    foreach (var tema in lista)
                    {
                        if (tema.ListaFotos.Any()) {
                            foreach(var foto in tema.ListaFotos)
                        {
                                TmVm.Add(new TemasFotoViewModel(tema.Nome, foto.Nome));
                            }
                        }
                        
                    }
                }
            }
            return View(TmVm);
            
        }
        [CustomAuthorize(Roles ="Anfitriao")]
        public async Task<IActionResult> Privacy() {

            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var u = await  _context.Anfitrioes.Where(u=>u.Id == ud).Include(u=> u.userPrefsAn).ThenInclude(u=>u.Cores).FirstOrDefaultAsync();
            TempData["userLoggedNome"] = u.PrimeiroNome + " " + u.UltimoNome;
            var r = await _context.Reservas.Include(c=>c.Cliente).Include(a=>a.Sala).Include(a=>a.ListaAnfitrioes).Where(a=>a.ListaAnfitrioes.Contains(u)).OrderBy(r => r.ReservaDate).ToListAsync();
            var vm = new ReservasDashboardViewModel(u.userPrefsAn,r);
            if (TempData["Save"] != null) {
                TempData["Save"] = TempData["Save"];

            }
            

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
           var s= HttpContext.Response.StatusCode;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCor(Dictionary<string,string> dic,string showCancel) {

            var dik = dic;
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u=> u.Cores).FirstOrDefaultAsync();
            if (u.userPrefsAn == null) {
                var pref= new UserPrefsAnf();
                pref.AnfId=u.Id;
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
