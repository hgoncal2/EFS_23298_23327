using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using EFS_23298_23327.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Humanizer;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EFS_23298_23327.Controllers
{
    [CustomAuthorize(Roles ="Cliente,Admin,Anfitriao")]
    public class TemasGeralController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ClassHub> _progressHubContext;
        private readonly IEmailSender _emailSender;
        public TemasGeralController(ApplicationDbContext context, IHubContext<ClassHub> progressHubContext, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _progressHubContext = progressHubContext;
            _context = context;
        }

        // GET: TemasGeral
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //Usado para testes ws
           // await _progressHubContext.Clients.All.SendAsync("tema", "system", "teste");

            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Where(m => m.Deleted != true).Where(t=>t.ListaFotos.Where(f=>!f.Deleted).Any()).Where(t=> t.SalaID!=null).OrderByDescending(m => m.DataCriacao);
            ICollection<TemasFotoViewModel> TmVm = new List<TemasFotoViewModel>();
            var lista = await applicationDbContext.ToListAsync();
            if (lista != null)
            {
                if (lista.Any())
                {
                    foreach (var tema in lista)
                    {
                        if (tema.ListaFotos.Any())
                        {
                            foreach (var foto in tema.ListaFotos)
                            {
                                TmVm.Add(new TemasFotoViewModel(tema.Nome, foto.Nome));
                            }
                        }

                    }
                }
            }
            return View(lista);
        }


   


     


        [AllowAnonymous]
        public async Task<IActionResult> Reserva(int? id) {

            if(id == null) {
                return NotFound();
            }

            var tema = await _context.Temas.Include(f => f.ListaFotos.Where(f => f.Deleted == false)).Include(s => s.Sala).ThenInclude(s => s.ListaReservas.Where(r=>!r.Cancelada || !r.Deleted)).ThenInclude(s => s.Cliente).Where(r => !r.Deleted).Where(s => s.SalaID == id).FirstOrDefaultAsync();
            if (tema == null) {
                return NotFound();
            }


           
            ReservaViewModel rvm = new ReservaViewModel(tema.Sala, tema);
            rvm.Sala.ListaReservas = rvm.Sala.ListaReservas.Where(r => !r.Cancelada).ToList();
            if (TempData["viewStart"] != null && TempData["viewEnd"] != null) {
                rvm.viewStart = (DateTime)TempData["viewStart"];
                rvm.viewEnd = (DateTime)TempData["viewEnd"];
            }

            return View(rvm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Cancela(int resId) {


            var reserva = await _context.Reservas.Where(r => r.ReservaId == resId && !r.Deleted && !r.Cancelada && r.CriadoPorOid == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();

            if (reserva == null) {
                return Unauthorized();
            }

            var u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                TempData["ErroCliente"] = "Erro! Apenas clientes podem fazer reservas!";
                return RedirectToAction(nameof(Reserva), new { id = reserva.SalaId });

            }

            
            reserva.Cancelada = true;
            reserva.DataCancel = DateTime.Now;
            var callbackUrl = Url.Action(
                        "Index", "Perfil", values: new { area = "", resId = reserva.ReservaId }, protocol: HttpContext.Request.Scheme
                        );
            await _emailSender.SendEmailAsync(reserva.Cliente.Email, "Nova reserva criada!!",
          $"A sua reserva para o dia {reserva.ReservaDate} foi cancelada com sucesso! Clique no link para ver mais detalhes:\n{callbackUrl}");
            _context.Update(reserva);
            await _context.SaveChangesAsync();

            TempData["ReservaCancel"] = "Reserva <strong>" + reserva.ReservaId + "</strong> cancelada com sucesso!";
            return RedirectToAction(nameof(Reserva), new { id = reserva.SalaId });






        }


        [HttpPost]
        public async Task<IActionResult> ReservaData(DateTime dateI, int salaId, string viewType, DateTime viewStart, DateTime viewEnd) {
            if (User.Identity.IsAuthenticated==false) {
                ViewBag.erroUserAuth = true;
                return PartialView("_erroUserPartial");
            }

            var tema = await _context.Temas.Include(f => f.ListaFotos.Where(f => f.Deleted == false)).Include(s => s.Sala).ThenInclude(s => s.ListaReservas).ThenInclude(s => s.Cliente).Where(r => !r.Deleted).Where(s => s.SalaID == salaId).FirstOrDefaultAsync();


            if (tema == null) {
                return NotFound();
            }
            ReservaViewModel rvm = new ReservaViewModel(tema.Sala, tema);
            rvm.dataI = dateI;
            rvm.viewType = viewType;
            rvm.viewStart = viewStart;
            rvm.viewStart = viewEnd;



            return PartialView("_reservasModals", rvm);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Reserva(int id,ReservaViewModel rvm) {

            if (User.Identity.IsAuthenticated == false) {
                ViewBag.erroUserAuth = true;
                return View(new RegisterViewModel());
            }

            Clientes u = null;

            u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                TempData["ErroCliente"] = "Erro! Apenas clientes podem fazer reservas!";
                return RedirectToAction(nameof(Reserva), rvm.Sala.SalaId);

            }

            var r = new Reservas(u);
            r.ListaAnfitrioes=rvm.Sala.ListaAnfitrioes;
            var tema = await _context.Temas.Include(s => s.Sala).Where(r => !r.Deleted).Where(s => s.SalaID == rvm.Sala.SalaId).FirstOrDefaultAsync();
            if (tema == null) {
                TempData["ErroCliente"] = "Erro! Não é possível reservar uma sala sem tema atribuído!";
                return RedirectToAction(nameof(Reserva), rvm.Sala.SalaId);

            }

            var endDate = rvm.dataI.AddMinutes(tema.TempoEstimado);
            var sala = await _context.Salas.Where(s=>s.SalaId==tema.SalaID).Include(s=>s.ListaAnfitrioes).FirstOrDefaultAsync();
            r.SalaId = sala.SalaId;
            r.NumPessoas = rvm.nPessoas;
            r.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            r.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
            r.DataCriacao = DateTime.Now;
            r.Sala = tema.Sala;
            r.ListaAnfitrioes = sala.ListaAnfitrioes;
            r.ReservaDate = rvm.dataI;
            var precoStr = (tema.Preco * rvm.nPessoas).ToString();
            r.TotalPreco = Convert.ToDecimal(precoStr.Replace('.', ','));
            r.TemaNome = tema.Nome;
            r.TemaDif = tema.Dificuldade;
            r.ReservaEndDate = endDate;
            tema.Sala.ListaReservas.Add(r);
            _context.Update(tema.Sala);
           
            await _context.SaveChangesAsync();

             var callbackUrl = Url.Action(
                        "Index", "Perfil", values: new { area = "", resId = r.ReservaId }, protocol: HttpContext.Request.Scheme
                        );
            await _emailSender.SendEmailAsync(r.Cliente.Email, "Nova reserva criada!",
          $"A sua reserva para o dia {r.ReservaDate} foi criada com sucesso! Clique no link para ver mais detalhes:\n{callbackUrl}");

            TempData["viewEnd"] = rvm.viewEnd;
            TempData["viewStart"] = rvm.viewStart;

            TempData["ReservaSucesso"] = "Reserva para " + r.ReservaDate.ToString("dd-MM-yyyy HH:mm:ss") + " efetuada com sucesso";
            await _progressHubContext.Clients.Group("Anfitrioes").SendAsync("reserva", "system", r.ReservaId + "," + @TimeSpan.FromHours((r.ReservaDate - DateTime.Now).TotalHours).Humanize() + "," + r.Sala.Numero);

            return RedirectToAction(nameof(Reserva),rvm.Sala.SalaId);






        }


   




    }
}
