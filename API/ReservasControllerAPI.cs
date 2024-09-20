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
    [Route("api/reservas")]
    [ApiController]
    public class ReservasControllerAPI:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public ReservasControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager) {
            _context = context;
            _userManager = userManager;
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





            return Ok(new {
                TemaNome = res.TemaNome,
                TemaDiff = DifficultiesValue.GetDifficultyColor((int)res.TemaDif),
                SalaNumero = res.Sala.Numero,
                Reserva = r

            });
        }


    }




}
