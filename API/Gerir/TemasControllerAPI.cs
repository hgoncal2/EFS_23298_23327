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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;

namespace EFS_23298_23327.API.Gerir
{
    [Route("api/gerir/temas")]
    [CustomAuthorize(Roles ="Admin,Anfitriao")]

    [ApiController]
    public class TemasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TemasControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }
        //[CustomAuthorize(Roles = "Admin,Anfitriao")]
        // GET: api/gerir/temas
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemaDTO>>> GetTemas(bool showTemasSemSala)
        {

            var temas = new List<Temas>();
            if (showTemasSemSala) {
                temas = await _context.Temas.Where(t => !t.Deleted).Include(t => t.ListaFotos).ToListAsync();

            } else {
                temas = await _context.Temas.Where(t => !t.Deleted && t.SalaID != null).Include(t => t.ListaFotos).ToListAsync();
            }

            

            var result = new List<TemaDTO>();
            foreach (var tema in temas)
            {
                var fotos = tema.ListaFotos.Select(f => f.Nome).ToList();

                var dto = new TemaDTO(tema, fotos);

                result.Add(dto);
            }
            return Ok(result);
        }
        [AllowAnonymous]
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
        public async Task<IActionResult> PutTemas(int id, [FromBody] TemaDTO temas) {
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
                        Salas s = await _context.Salas.Where(s => s.SalaId == temas.SalaID && !s.Deleted).FirstOrDefaultAsync();
                        if (s != null)
                        {
                            if (await _context.Temas.Where(t => t.SalaID == temas.SalaID && !t.Deleted).FirstOrDefaultAsync() == null) {
                                tema.SalaID = temas.SalaID;
                                tema.Sala = s;
                        }
                        else {
                            tema.Sala = null;
                            tema.SalaID = null;
                        }
                        }
                        else
                        {
                            tema.Sala = null;
                            tema.SalaID = null;
                        }
                    List<Fotos> listaDeFotos = await _context.Fotos.Where(f => temas.ListaDeFotos.Contains(f.Nome)).ToListAsync();
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
        public async Task<ActionResult<Temas>> CreateTemas(TemaDTO temas) {
            if (ModelState.IsValid) {
                var temaExiste = _context.Temas.FirstOrDefault(t => t.Nome == temas.Nome && !t.Deleted);
                if (temaExiste != null)
                {
                    return BadRequest(new { Error = "Tema " + temas.Nome + " já existe!" });
                }
                Temas tema = new Temas(temas);
                if (temas.ListaDeFotos.Any()) {
                    foreach (var item in temas.ListaDeFotos)
                    {
                        tema.ListaFotosNome?.Add(item);
                    }
                }
                Salas s = await _context.Salas.Where(s => s.SalaId == tema.SalaID && !s.Deleted).FirstOrDefaultAsync();
                if (s != null) {
                    tema.Sala = s;
                }
                else
                {
                    tema.Sala = null;
                    tema.SalaID = null;
                }
                tema.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                tema.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Temas.Add(tema);
                await _context.SaveChangesAsync();
                var result = await GetTemas(tema.TemaId);
                // Retornar o resultado apropriado
            
                if (result.Result is NotFoundResult) {
                    return NotFound(); // Se o recurso não foi encontrado
                }

                return Ok(((ObjectResult)result.Result).Value); // Retorna o objeto atualizado
            }
            return BadRequest();
        }

        // DELETE: api/gerir/temas/{id}
        //[CustomAuthorize(Roles = "Admin,Anfitriao")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemas(int id)
        {
            var temas = await _context.Temas.FindAsync(id);
            if (temas == null)
            {
                return NotFound();
            }
            temas.SalaID = null;
            temas.Deleted = true;
            _context.Update(temas);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TemasExists(int id)
        {
            return _context.Temas.Any(e => e.TemaId == id && !e.Deleted);
        }
    }
}
