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
using EFS_23298_23327.API.DTOs;
using EFS_23298_23327.Data.Enum;

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
        // GET: api/gerir/temas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemaDTO>>> GetTemas()
        {
            var temas = await _context.Temas.Where(t => !t.Deleted).Include(t => t.ListaFotos).ToListAsync();

            var result = new List<TemaDTO>();
            foreach (var tema in temas)
            {
                var fotos = tema.ListaFotos.Select(f => f.Nome).ToList();

                var dto = new TemaDTO(tema, fotos);

                result.Add(dto);
            }
            return Ok(result);
        }

        // GET: api/gerir/temas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TemaDTO>> GetTemas(int id)
        {
            var tema = await _context.Temas
               .Where(t => !t.Deleted)
               .Include(t => t.ListaFotos)
               .FirstOrDefaultAsync(t => t.TemaId == id);

            if (tema == null)
            {
                return NotFound();
            }

            var fotos = tema.ListaFotos?.Select(f => f.Nome).ToList();



            var viewModel = new TemaDTO(tema, fotos);

            return Ok(viewModel);
        }
    



        // PUT: api/gerir/temas/<id>
        //[CustomAuthorize(Roles = "Admin,Anfitriao")]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemas(int id, [FromBody][Bind("Nome,Descricao,TempoEstimado,Preco,Icone,MinPessoas,MaxPessoas,Dificuldade,ListaDeFotos")] TemaDTO temas) {
            if (id != temas.TemaId) {
                return NotFound();
            }

            //Estes campos devem permanecer iguais,então não nos vamos preocupar se estão a ser mandados ou não
            ModelState.Remove("TemaId");
            ModelState.Remove("DataCriacao");
            ModelState.Remove("CriadoPorOid");
            ModelState.Remove("CriadoPorUsername");
            ModelState.Remove("ListaDeFotos");

            if (ModelState.IsValid)
            {

                var tema = await _context.Temas.FirstOrDefaultAsync(t => t.TemaId == temas.TemaId);
                if (tema == null)
                {
                    return NotFound();
                }
                    try
                    {
                        tema = await _context.Temas.Include(t => t.ListaFotos).Where(t => t.TemaId == temas.TemaId).FirstOrDefaultAsync();
                        tema.Nome = temas.Nome;
                        tema.Descricao = temas.Descricao;
                        tema.TempoEstimado = temas.TempoEstimado;
                        tema.Preco = temas.Preco;
                        tema.Icone = temas.Icone;
                        tema.MinPessoas = temas.MinPessoas;
                        tema.MaxPessoas = temas.MaxPessoas;
                        tema.Dificuldade = temas.Dificuldade;
                        List<Fotos> listaDeFotos = await _context.Fotos.Where(f => tema.ListaFotosNome.Contains(f.Nome)).ToListAsync();
                        tema.ListaFotos = listaDeFotos;
                        _context.Update(tema);
                        _context.Entry(tema).Property(t => t.TemaId).IsModified = false;
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
            var result = await GetTemas(temas.TemaId);

            // Retornar o resultado apropriado
            if (result.Result is NotFoundResult)
            {
                return NotFound(); // Se o recurso não foi encontrado
            }

            return Ok(((ObjectResult)result.Result).Value); // Retorna o objeto atualizado
        }

        // POST: api/TemasControllerAPI
        //    [CustomAuthorize(Roles = "Admin,Anfitriao")]
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
