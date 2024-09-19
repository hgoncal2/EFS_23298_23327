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
using EFS_23298_23327.API.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public async Task<ActionResult<IEnumerable<SalaDTO>>> GetSalas()
        {
            var salas = await _context.Salas
               .Where(s => !s.Deleted)
               .Include(s => s.ListaAnfitrioes)
               .Include(s => s.ListaReservas)
               .ToListAsync();

            var result = new List<SalaDTO>();
            foreach (var sala in salas)
            {
               
                var anfitrioes = sala.ListaAnfitrioes.Select(a => a.UserName).ToList();
                var reservas = sala.ListaReservas.Select(r => r.ReservaId).ToList(); // Lista de IDs das reservas
                //para não mandar os objetos pela API
               

                var dto = new SalaDTO(sala, anfitrioes, reservas);
               
                result.Add(dto);
            }
            return Ok(result);
        }

        // GET: api/gerir/salas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDTO>> GetSala(int id)
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

            var anfitrioes = sala.ListaAnfitrioes?.Select(a => a.UserName).ToList();
            var reservas = sala.ListaReservas?.Select(r => r.ReservaId).ToList();

           

            var viewModel = new SalaDTO(sala, anfitrioes, reservas);
       
            return Ok(viewModel);
        }

        // PUT: api/gerir/salas/<id>
      //  [CustomAuthorize(Roles = "Admin,Anfitriao")]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSala(int id,[FromBody] SalaDTO s) {
            if (id != s.SalaId) {
                return NotFound();
            }

            //Estes campos devem permanecer iguais,então não nos vamos preocupar se estão a ser mandados ou não
            ModelState.Remove("dataCriacao");
            ModelState.Remove("criadoPorOid");
            ModelState.Remove("criadoPorUsername");
            
            ModelState.Remove("listaReservas");



            if (ModelState.IsValid) {
               
                var sala = _context.Salas.FirstOrDefault(m => m.Numero == s.Numero && m.SalaId != s.SalaId);
                if (sala != null) {
                    //Se número da sala já existir
                    return BadRequest(new { Error = "Sala " + s.Numero+ " já existe!" });

                }
                try {
                    sala = await _context.Salas.Include(a => a.ListaAnfitrioes).Include(a=>a.ListaReservas).Where(a => a.SalaId == s.SalaId).FirstOrDefaultAsync();
                    sala.Numero = s.Numero;
                    sala.Area = s.Area;
                    List<Anfitrioes> listaUsers = await _context.Anfitrioes.Where(m => s.ListaAnfitrioes.Contains(m.UserName)).ToListAsync();
                    sala.ListaAnfitrioes = listaUsers;
                    _context.Update(sala);
                    _context.Entry(sala).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(sala).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(sala).Property(t => t.CriadoPorUsername).IsModified = false;
                    //Não vai ser possivel remover reservas a partir daqui
                   
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!SalasExists(s.SalaId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
            }



            var result = await GetSala(s.SalaId);

            // Retornar o resultado apropriado
            if (result.Result is NotFoundResult) {
                return NotFound(); // Se o recurso não foi encontrado
            }

            return Ok(result.Result.); // Retorna o objeto atualizado
        }
        private bool SalasExists(int id) {
            return _context.Salas.Any(e => e.SalaId == id);
        }

    }


}
