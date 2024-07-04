using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using System.Numerics;
using System.Security.Claims;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin,Anfitriao")]

    [Area("Gerir")]
    public class TemasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string? temaAntigo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TemasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Temas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala).Where(s => s.Deleted != true).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Temas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos).Include(s=>s.Sala)
.FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }

        // GET: Temas/Create
        public async Task<ActionResult> Create()
        {
           List<Salas> a = await _context.Temas.Where(s => !s.Deleted).Select(s => s.Sala).ToListAsync();
           List<Salas> s =await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s.Except(a).ToList();
            return View();
        }

        // POST: Temas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID")] Temas temas)
        {
            var Imagens = HttpContext.Request.Form.Files;
            if (ModelState.ContainsKey("Imagem"))
            {
                ModelState.Remove("Imagem");
            }
            if (ModelState.IsValid)
            {

                var msgErro = "";
                var erro = false;

                var tema = _context.Temas.FirstOrDefault(m => m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower());
                if (tema != null)
                {
                    ViewBag.TemaExistente = tema.Nome;

                    return View(temas);
                }

                if (temas.MaxPessoas <= temas.MinPessoas)
                {
                    msgErro = "Erro!Máximo de pessoas tem que ser maior que o mínimo de pessoas";
                    ModelState.AddModelError("MaxPessoas", msgErro);
                    erro = true;

                }
                if (temas.MinPessoas >= temas.MaxPessoas)
                {
                    msgErro = "Erro!Mínimo de pessoas tem que ser menor que o máximo de pessoas";

                    ModelState.AddModelError("MinPessoas", msgErro);
                    erro = true;

                }

                var hasImagem = false;
                string nomeImagem = "";
                Dictionary<Fotos, IFormFile> mapFotos = new Dictionary<Fotos, IFormFile>();

                if (Imagens != null)
                {
                    foreach (var Imagem in Imagens)
                    {
                        if (Imagem != null)
                        {
                            if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg"))
                            {
                                msgErro = "Erro!Ficheiro de imagem tem que ser png ou jpeg!";
                                ModelState.AddModelError("Foto", msgErro);
                                erro = true;
                            }
                            else
                            {
                               
                                
                                Guid g = Guid.NewGuid();
                                nomeImagem = g.ToString();
                                string extensaoImagem = Path.GetExtension(Imagem.FileName).ToLowerInvariant();
                                nomeImagem += extensaoImagem;
                                Fotos f = new Fotos(nomeImagem);
                                
                               
                                f.TemaId = temas.TemaId;
                                f.Tema = temas;
                                f.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                f.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                                temas.ListaFotos.Add(f);
                               
                                mapFotos.Add(f, Imagem);
                                hasImagem = true;

                            }
                        }
                    }

                }



                if (erro)
                {
                    return View(temas);
                }
                //https://stackoverflow.com/a/71882405
                temas.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                temas.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(temas);
                await _context.SaveChangesAsync();
                if (hasImagem)
                {
                    string localizacaoImagem = _webHostEnvironment.WebRootPath;
                    localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }
                    foreach (KeyValuePair<Fotos, IFormFile> i in mapFotos)
                    {

                        localizacaoImagem = Path.Combine(localizacaoImagem, i.Key.Nome);
                        using var stream = new FileStream(
                      localizacaoImagem, FileMode.Create
                      );
                        await i.Value.CopyToAsync(stream);
                        localizacaoImagem = _webHostEnvironment.WebRootPath;
                        localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                    }

                    // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens

                    // atribuir ao caminho o nome da imagem

                }
                TempData["NomeTemaCriado"] = temas.Nome;
                return RedirectToAction(nameof(Index));
            }

            return View(temas);
        }

        // GET: Temas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala).FirstOrDefaultAsync(m => m.TemaId == id);

            if (temas == null)
            {
                return NotFound();
            }
            List<Salas> a = await _context.Temas.Where(s => !s.Deleted && s.TemaId != id).Select(s => s.Sala).ToListAsync();
            List<Salas> s = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s.Except(a).ToList();
            ViewBag.TemaAntigo = temas.Nome;

            return View(temas);
        }

        // POST: Temas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID,Icone,Preco")] Temas temas, string nomeAntigo)
        {
            if (id != temas.TemaId)
            {
                return NotFound();
            }
            if (ModelState.ContainsKey("nomeAntigo"))
            {
                ModelState.Remove("nomeAntigo");
            }
            if (ModelState.ContainsKey("Imagem"))
            {
                ModelState.Remove("Imagem");
            }
            var Imagens = HttpContext.Request.Form.Files;
            if (ModelState.IsValid)
            {
                var tema = _context.Temas.FirstOrDefault(m => m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower() && m.TemaId != temas.TemaId);
                if (tema != null)
                {
                    ViewBag.TemaExistente = tema.Nome;
                    temas.Nome = nomeAntigo;
                    ViewBag.TemaAntigo = temas.Nome;
                    return View(temas);

                }
                var msgErro = "";
                var erro = false;

                if (temas.MaxPessoas <= temas.MinPessoas)
                {
                    msgErro = "Erro!Máximo de pessoas tem que ser maior que o mínimo de pessoas";
                    ModelState.AddModelError("MaxPessoas", msgErro);
                    erro = true;

                }
                if (temas.MinPessoas >= temas.MaxPessoas)
                {
                    msgErro = "Erro!Mínimo de pessoas tem que ser menor que o máximo de pessoas";

                    ModelState.AddModelError("MinPessoas", msgErro);
                    erro = true;

                }

                var hasImagem = false;
                string nomeImagem = "";
                Dictionary<Fotos, IFormFile> mapFotos = new Dictionary<Fotos, IFormFile>();

                if (Imagens != null)
                {
                    foreach (var Imagem in Imagens)
                    {
                        if (Imagem != null)
                        {
                            if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg"))
                            {
                                msgErro = "Erro!Ficheiro de imagem tem que ser png ou jpeg!";
                                ModelState.AddModelError("Foto", msgErro);
                                erro = true;
                            }
                            else
                            {

                                Guid g = Guid.NewGuid();
                                nomeImagem = g.ToString();
                                string extensaoImagem = Path.GetExtension(Imagem.FileName).ToLowerInvariant();
                                nomeImagem += extensaoImagem;
                                Fotos f = new Fotos(nomeImagem);

                                f.TemaId = temas.TemaId;
                                f.Tema = temas;

                                temas.ListaFotos.Add(f);
                                mapFotos.Add(f, Imagem);
                                hasImagem = true;

                            }
                        }
                    }

                }



                if (erro)
                {
                    List<Salas> sa = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
                    ViewBag.s = sa;
                    return View(temas);
                }
                try
                {

                    _context.Update(temas);
                    _context.Entry(temas).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(temas).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(temas).Property(t => t.CriadoPorUsername).IsModified = false;

                    await _context.SaveChangesAsync();
                    if (hasImagem)
                    {
                        string localizacaoImagem = _webHostEnvironment.WebRootPath;
                        localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                        if (!Directory.Exists(localizacaoImagem))
                        {
                            Directory.CreateDirectory(localizacaoImagem);
                        }
                        foreach (KeyValuePair<Fotos, IFormFile> i in mapFotos)
                        {

                            localizacaoImagem = Path.Combine(localizacaoImagem, i.Key.Nome);
                            using var stream = new FileStream(
                          localizacaoImagem, FileMode.Create
                          );
                            await i.Value.CopyToAsync(stream);
                            localizacaoImagem = _webHostEnvironment.WebRootPath;
                            localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                        }

                        // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens

                        // atribuir ao caminho o nome da imagem

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemasExists(temas.TemaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.TemaAntigo = temas.Nome;
                ViewBag.ShowAlert = true;
                var temasAtual = await _context.Temas.Include(m => m.ListaFotos).FirstOrDefaultAsync(m => m.TemaId == id);
                List<Salas> s =await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s;
                return View(temasAtual);
            }

            return View(temas);
        }
        //função criada por defeito,não está a ser usada
        // GET: Temas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos).FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }

        // POST: Temas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? tema = null;

            var temas = await _context.Temas.FindAsync(id);

            if (temas != null)
            {

                tema = temas.Nome;
                temas.Deleted = true;
                temas.SalaID = null;
                var fotos = await _context.Fotos.Where(f => f.TemaId == temas.TemaId).ToListAsync();
                foreach (var item in fotos)
                {
                    item.Deleted = true;
                    
                }

            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            if (tema != null)
            {
                TempData["TemaApagado"] = tema;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Temas/EliminaFoto/5
        [HttpPost, ActionName("EliminaFoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminaFoto(int id)
        {


            var foto = await _context.Fotos.FindAsync(id);
            Temas tema;
            if (foto != null)
            {


                foto.Deleted = true;
                tema = await _context.Temas.FirstOrDefaultAsync(f => f.TemaId == foto.TemaId);
                if (tema != null)
                {
                    tema.ListaFotos.FirstOrDefault(f => f.FotoId == id).Deleted = true;
                    await _context.SaveChangesAsync();

                    TempData["FotoEliminada"] = true;
                    return RedirectToAction(nameof(Edit), new { id = tema.TemaId });
                }

            }
            else
            {
                return NotFound();
            }

            return NotFound();
        }



        private bool TemasExists(int id)
        {
            return _context.Temas.Any(e => e.TemaId == id);
        }
    }
}
