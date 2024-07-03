using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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


            var r = await _context.Salas.Include(s => s.ListaReservas).ThenInclude(s=> s.Cliente).Where(r => !r.Deleted).Where(s => s.SalaId == 1).FirstOrDefaultAsync();
            return View(r);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
           var s= HttpContext.Response.StatusCode;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<JsonResult> Reserva(DateTime date) {
            Clientes u = null;
           
                 u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                TempData["ErroCliente"] = "Erro! Apenas clientes podem fazer reservas!";
                return Json("erro");
            }
                var r = new Reservas(u);
               
                var tema = await _context.Temas.Include(s => s.Sala).Where(r => !r.Deleted).Where(s => s.SalaID == 1).FirstOrDefaultAsync();
            if (tema == null) {
                TempData["ErroCliente"] = "Erro! Não é possível reservar uma sala sem tema atribuído!";
                return Json("erro");
            }
            var endDate = date.AddMinutes(tema.TempoEstimado);
            var sala =await _context.Salas.FindAsync(tema.SalaID);
                r.SalaId = sala.SalaId;
                r.Sala = tema.Sala;
            r.ReservaDate = date;
            r.ReservaEndDate = endDate;
                tema.Sala.ListaReservas.Add(r);

                _context.Update(tema.Sala);
                await _context.SaveChangesAsync();
                return Json("ok");
            
            
        }

    }
}