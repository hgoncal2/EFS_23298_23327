using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EFS_23298_23327.ViewModel;

namespace EFS_23298_23327.Controllers
{
    [Route("api/gerir/salas")]
    [ApiController]
    public class SalasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public SalasControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/gerir/salas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salas>>> GetSalas()
        {
            var salas = await _context.Salas
               .Where(s => !s.Deleted)
               .Include(s => s.ListaAnfitrioes)
               .Include(s => s.ListaReservas)
               .ToListAsync();

            var result = new List<AnfitriaoSalaViewModel>();
            foreach (var sala in salas)
            {
               
                var anfitrioes = sala.ListaAnfitrioes.Select(a => a.UserName).ToList();
                var reservas = sala.ListaReservas.Select(r => r.ReservaId).ToList(); // Lista de IDs das reservas
                //para não mandar os objetos pela API
                sala.ListaReservas.Clear();
                sala.ListaAnfitrioes.Clear();

                var viewModel = new AnfitriaoSalaViewModel(sala, anfitrioes, reservas);
                viewModel.Id = sala.SalaId;
                result.Add(viewModel);
            }
            return Ok(result);
        }

        // GET: api/gerir/salas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AnfitriaoSalaViewModel>> GetSala(int id)
        {
            var sala = await _context.Salas
                .Where(s => !s.Deleted)
                .Include(s => s.ListaAnfitrioes)
                .Include(s => s.ListaReservas)
                .FirstOrDefaultAsync(s => s.SalaId == id);

            if (sala == null)
            {
                return NotFound();
            }

            var anfitrioes = sala.ListaAnfitrioes.Select(a => a.UserName).ToList();
            var reservas = sala.ListaReservas.Select(r => r.ReservaId).ToList();

            // Limpar as listas do objeto sala
            sala.ListaReservas.Clear();
            sala.ListaAnfitrioes.Clear();

            var viewModel = new AnfitriaoSalaViewModel(sala, anfitrioes, reservas);
            viewModel.Id = sala.SalaId;
            return Ok(viewModel);
        }

    }
}
