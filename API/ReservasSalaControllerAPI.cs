using EFS_23298_23327.API.DTOs;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;

namespace EFS_23298_23327.API
{
    [Route("api/reservasSala")]
    [ApiController]
    public class ReservasSalaControllerAPI:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public ReservasSalaControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: Devolve reservas de uma sala,poder especificar se quer mostrar as canceladas ou não
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id da sala</param>
        /// <param name="showCanc">Permite escolher se mostra reservas canceladas</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id,bool showCanc) {

            if (id == null) {
                return NotFound();
            }


            var tema = await _context.Temas.Include(f => f.ListaFotos.Where(f => f.Deleted == false)).Include(s => s.Sala).ThenInclude(s => s.ListaReservas.Where(r =>!r.Deleted)).ThenInclude(s => s.Cliente).Where(r => !r.Deleted).Where(s => s.SalaID == id).FirstOrDefaultAsync();
            if (tema == null) {
                return NotFound();
            }



            ReservaDTO rvm = new ReservaDTO(tema.Sala, tema);


            foreach (var item in tema.Sala.ListaReservas)
            {
                
                if(showCanc) {
                    var res = new ReservasWrapper(item);
                    rvm.ListaReservas?.Add(res);
                } else {
                    if (!item.Cancelada) {
                        var res = new ReservasWrapper(item);
                        rvm.ListaReservas?.Add(res);
                    }
                }
               
                
            };
           


            return Ok(rvm);
        }


        /// <summary>
        /// cria uma reserva baseado no id da sala
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rvm"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        
        public async Task<IActionResult> FazReserva(int id, FazReservaDTO rvm) {


            if (User.Identity.IsAuthenticated == false) {
                return Unauthorized(new {Error = "Necessita de estar autenticado para fazer reservas!"});
            }

            Clientes u = null;

            u = await _context.Clientes.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (u == null) {
                return Unauthorized(new { Error = "Apenas clientes podem fazer reservas!" });

            }

            var r = new Reservas(u);
           
            var tema = await _context.Temas.Include(s => s.Sala).Where(r => !r.Deleted).Where(s => s.SalaID == rvm.SalaId).FirstOrDefaultAsync();
            if (tema == null) {
                return BadRequest(new { Error = "Não é possível reservar uma sala sem tema atribuído!" });


            }

            //falta fazer verificação se já existe reserva para aquela data

            var endDate = rvm.ReservaDate.AddMinutes(tema.TempoEstimado);
            var sala = await _context.Salas.Where(s => s.SalaId == tema.SalaID).Include(s => s.ListaAnfitrioes).FirstOrDefaultAsync();
            r.SalaId = sala.SalaId;
            r.NumPessoas = rvm.NumPessoas;
            r.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            r.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
            r.DataCriacao = DateTime.Now;
            r.Sala = tema.Sala;
            r.ListaAnfitrioes = sala.ListaAnfitrioes;
            r.ReservaDate = rvm.ReservaDate;

            var precoStr = (tema.Preco * rvm.NumPessoas).ToString();
            r.TotalPreco = Convert.ToDecimal(precoStr.Replace('.', ','));
            r.TemaNome = tema.Nome;
            r.TemaDif = tema.Dificuldade;
            r.ReservaEndDate = endDate;
            tema.Sala.ListaReservas.Add(r);
            _context.Update(tema.Sala);

            await _context.SaveChangesAsync();




            return Ok();






        }

    }




}
