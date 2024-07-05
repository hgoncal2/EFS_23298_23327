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

        /// <summary>
        /// Retorna página "index" das reservas
        /// </summary>
        /// <param name="salaId">ID da sala para fazer reserva</param>
        /// <returns>Viewmodel criada para reservas</returns>
        
        public async Task<IActionResult> Index(int? salaId) {

            var tema = await _context.Temas.Include(f => f.ListaFotos.Where(f => f.Deleted == false)).Include(s => s.Sala).ThenInclude(s => s.ListaReservas).ThenInclude(s => s.Cliente).Where(r => !r.Deleted).Where(s => s.SalaID == salaId).FirstOrDefaultAsync();
            if (tema == null) {
                return NotFound();
            }

            ReservaViewModel rvm = new ReservaViewModel(tema.Sala, tema);
            if (TempData["viewStart"] != null && TempData["viewEnd"] != null) {
                rvm.viewStart = (DateTime)TempData["viewStart"];
                rvm.viewEnd = (DateTime)TempData["viewEnd"];
            }

            return View(rvm);
        }

        /// <summary>
        /// Usado para passar variáveis de JS para o modelo,que irá ser usado para a reserva
        /// </summary>
        /// <param name="dateI">Data de inicio da reserva</param>
        /// <param name="salaId">Id da sala</param>
        /// <param name="viewType">Viewtype do objeto "view" do fullcalendar,de momento não está a ser usado</param>
        /// <param name="viewStart">activeStart do objeto "view" do fullcalendar,irá ser usado para "repor" o calendário ao estado anterior ao POST da reserva</param>
        /// <param name="viewEnd">>activeEnd do objeto "view" do fullcalendar,irá ser usado para "repor" o calendário ao estado anterior ao POST da reserva</param>
        /// <returns>Partial view que contém o Modal para confirmar a reserva,juntamente com o novo modelo com os atributos definidos</returns>
        [HttpPost]
        public async Task<IActionResult> ReservaData(DateTime dateI, int salaId, string viewType, DateTime viewStart, DateTime viewEnd) {


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

        /// <summary>
        /// Método que efetivamente cria a reserva
        /// </summary>
        /// <param name="rvm">Viewmodel da reserva</param>
        /// <returns>View da reserva(index)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id,ReservaViewModel rvm) {
            Clientes u = null;

            u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                TempData["ErroCliente"] = "Erro! Apenas clientes podem fazer reservas!";
                return NotFound();
            }

            var r = new Reservas(u);

            var tema = await _context.Temas.Include(s => s.Sala).Where(r => !r.Deleted).Where(s => s.SalaID == rvm.Sala.SalaId).FirstOrDefaultAsync();
            if (tema == null) {
                TempData["ErroCliente"] = "Erro! Não é possível reservar uma sala sem tema atribuído!";
                return View("Privacy");
            }
            var endDate = rvm.dataI.AddMinutes(tema.TempoEstimado);
            var sala = await _context.Salas.FindAsync(tema.SalaID);
            r.SalaId = sala.SalaId;
            r.NumPessoas = rvm.nPessoas;
            r.Sala = tema.Sala;
            r.ReservaDate = rvm.dataI;
            r.ReservaEndDate = endDate;
            tema.Sala.ListaReservas.Add(r);

            _context.Update(tema.Sala);
            await _context.SaveChangesAsync();
            TempData["viewEnd"] = rvm.viewEnd;
            TempData["viewStart"] = rvm.viewStart;

            TempData["ReservaSucesso"] = "Reserva para " + r.ReservaDate.ToString("dd-MM-yyyy HH:mm:ss") + " efetuada com sucesso";
            return View(rvm);

        }
    }
}

