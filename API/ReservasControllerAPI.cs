using EFS_23298_23327.API.DTOs;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using System.Security.Policy;

namespace EFS_23298_23327.API
{
    [Route("api/reservas")]
    [ApiController]
    public class ReservasControllerAPI:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly IUserStore<Utilizadores> _userStore;
        private readonly SignInManager<Utilizadores> _signInManager;


        public ReservasControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager, IUserStore<Utilizadores> userStore,

          SignInManager<Utilizadores> signInManager) {
            _context = context;
            _userManager = userManager;

            _userStore = userStore;

            _signInManager = signInManager;

        }


        /// <summary>
        /// id da reserva
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/reserva")]

        [HttpGet("{id}")]
        public async Task<IActionResult> VerReserva(int id) {

            if (id == null) {
                return NotFound();
            }


            var res = await _context.Reservas.Include(s => s.ListaAnfitrioes.Where(a => !a.Deleted)).Include(s => s.Sala).Include(c => c.Cliente).Where(s => s.ReservaId == id && !s.Deleted && !s.Cancelada).FirstOrDefaultAsync();
            if (res == null) {
                return NotFound();
            }



            var r = new ReservasWrapper(res);
            foreach (var item2 in res.ListaAnfitrioes) {
                r.Anfitrioes.Add(new AnfsWrapper(item2));
            }




            return Ok(new {
                TemaNome = res.TemaNome,
                TemaDiff = DifficultiesValue.GetDifficultyColor((int)res.TemaDif),
                SalaNumero = res.Sala.Numero,
                Reserva = r

            });
        }

        /// <summary>
        ///faz reserva baseado id da sala
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FazReserva([FromBody] ReservaViewModel rvm) {

            Clientes u = null;

            u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                
                return Unauthorized(new {Error="Apenas clientes podem fazer reservas!"});

            }

            var r = new Reservas(u);

           
            if (rvm.SalaId == null) {
                return BadRequest();
            }
            var sala = await _context.Salas.Where(s => s.SalaId == rvm.SalaId && !s.Deleted).Include(a=>a.ListaAnfitrioes).FirstOrDefaultAsync();
            r.ListaAnfitrioes = sala.ListaAnfitrioes;
            var tema = await _context.Temas.Include(s => s.Sala).Where(r => !r.Deleted).Where(s => s.SalaID == rvm.SalaId).FirstOrDefaultAsync();
            if (tema == null) {

                return BadRequest(new { Error = "Sala não tem tema associado!" });

            }

            var endDate = rvm.dataI.AddHours(1).AddMinutes(tema.TempoEstimado);
            r.SalaId = sala.SalaId;
            r.NumPessoas = rvm.nPessoas;
            r.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            r.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
            r.DataCriacao = DateTime.Now;
            r.Sala = tema.Sala;
            r.ListaAnfitrioes = sala.ListaAnfitrioes;
            r.ReservaDate = rvm.dataI.AddHours(1);
            var precoStr = (tema.Preco * rvm.nPessoas).ToString();
            r.TotalPreco = Convert.ToDecimal(precoStr.Replace('.', ','));
            r.TemaNome = tema.Nome;
            r.TemaDif = tema.Dificuldade;
            r.ReservaEndDate = endDate;
            tema.Sala.ListaReservas.Add(r);
            _context.Update(tema.Sala);
            await _context.SaveChangesAsync();




            return Ok(new {
                Msg = "Reserva criada com sucesso!"

            });
        }

        [HttpPut("{resId}")]
        public async Task<IActionResult> CancelaReserva(int resId) {

           
            var reserva = await _context.Reservas.Include(c => c.Cliente).Where(r => r.ReservaId == resId && !r.Deleted && !r.Cancelada && DateTime.Now.AddHours(48) < r.ReservaDate && r.ClienteID == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
            var u = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (reserva == null || reserva.ClienteID != u) {
                return Unauthorized(new {Error= "Não está autorizado!"});
            }

            //Notificar utilizador de cancelamento da reserva
           
            reserva.Cancelada = true;
            reserva.DataCancel = DateTime.Now;

            _context.Update(reserva);
            await _context.SaveChangesAsync();

            return Ok();






        }





    }




}
