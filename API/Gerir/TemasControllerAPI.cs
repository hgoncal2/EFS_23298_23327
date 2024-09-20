using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EFS_23298_23327.API.Gerir
{
    [Route("api/gerir/temas")]

    [ApiController]
    public class TemasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TemasControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }
        [CustomAuthorize(Roles = "Admin,Anfitriao")]
        // GET: api/TemasControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Temas>>> GetTemas()
        {
            var applicationDbContext = await _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala).Where(s => s.Deleted != true).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao).ToListAsync();
            foreach (var item in applicationDbContext)
            {
                item.ListaFotosNome = item.ListaFotos.Select(f => f.Nome).ToList();
                item.ListaFotos = new HashSet<Fotos>();


            }
            return applicationDbContext;
        }

        // GET: api/TemasControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Temas>> GetTemas(int id)
        {
            var temas = await _context.Temas.Include(f => f.ListaFotos).FirstOrDefaultAsync(t => t.TemaId == id && !t.Deleted);

            if (temas == null)
            {
                return NotFound();
            }

            temas.ListaFotosNome = temas.ListaFotos.Select(f => f.Nome).ToList();
            temas.ListaFotos = new HashSet<Fotos>();
            return temas;
        }

        // PUT: api/gerir/temas/<id>
        //[CustomAuthorize(Roles = "Admin,Anfitriao")]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemas(int id, [FromBody] Temas temas)
        {
            if (id != temas.TemaId)
            {
                return NotFound();
            }
            ModelState.Remove("PrecoStr");

            var tema = await _context.Temas.FirstOrDefaultAsync(t => t.TemaId == temas.TemaId);
            if (tema == null)
            {
                tema = await _context.Temas.Include(t => t.ListaFotos).Where(t => t.TemaId == temas.TemaId).FirstOrDefaultAsync();
                try
                {

                    List<Fotos> listaDeFotos = await _context.Fotos.Where(f => tema.ListaFotosNome.Contains(f.Nome)).ToListAsync();

                    tema = temas;

                    tema.ListaFotos = listaDeFotos;

                    _context.Update(tema);
                    _context.Entry(tema).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(tema).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(tema).Property(t => t.CriadoPorUsername).IsModified = false;


                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {


                    if (!TemasExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var result = await GetTemas(tema.TemaId);

            // Retornar o resultado apropriado
            if (result.Result is NotFoundResult)
            {
                return NotFound(); // Se o recurso não foi encontrado
            }
            
            return Ok(result.Result); // Retorna o objeto atualizado
            }

            // POST: api/TemasControllerAPI
            [CustomAuthorize(Roles = "Admin,Anfitriao")]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Temas>> PostTemas(Temas temas)
        {
            _context.Temas.Add(temas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTemas", new { id = temas.TemaId }, temas);
        }

        // DELETE: api/TemasControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemas(int id)
        {
            var temas = await _context.Temas.FindAsync(id);
            if (temas == null)
            {
                return NotFound();
            }

            _context.Temas.Remove(temas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TemasExists(int id)
        {
            return _context.Temas.Any(e => e.TemaId == id && !e.Deleted);
        }
    }
}
