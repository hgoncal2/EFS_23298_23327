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
        
        public async Task<IActionResult> Privacy() {

            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
           var s= HttpContext.Response.StatusCode;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       








    }

   

}
