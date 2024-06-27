using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23306.Data;
using EFS_23298_23306.Models;

namespace EFS_23298_23306.Controllers
{
    public class TemasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private String? temaAntigo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TemasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Temas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Temas.Include(t => t.Foto).Include(t => t.Sala).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Temas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas
                .Include(t => t.Foto)
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.TemaID == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }

        // GET: Temas/Create
        public IActionResult Create()
        {
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID");
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID");
            return View();
        }

        // POST: Temas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas, IFormFile Imagem)
        {

            if (ModelState.IsValid)
            {

                var msgErro = "";
                var erro = false;

                var tema = _context.Temas.FirstOrDefault(m =>  m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower());
                if (tema != null)
                {
                    ViewBag.TemaExistente = tema.Nome;

                    return View(temas);
                }

                if (temas.MaxPessoas <= temas.MinPessoas)
                {
                    msgErro = "Erro!Máximo de pessoas tem que ser maior que o mínimo de pessoas";
                    ModelState.AddModelError("MaxPessoas",msgErro);
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
                        temas.FotoID = f.FotoID;
                        temas.Foto = f;
                        hasImagem = true;

                    }
                }


                if (erro)
                {
                    return View(temas);
                }
               
                
                _context.Add(temas);
                await _context.SaveChangesAsync();
                if (hasImagem)
                {
                    string localizacaoImagem = _webHostEnvironment.WebRootPath;
                    // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens
                    localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }
                    // atribuir ao caminho o nome da imagem
                    localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);
                    using var stream = new FileStream(
                  localizacaoImagem, FileMode.Create
                  );
                    await Imagem.CopyToAsync(stream);
                }
                TempData["NomeTemaCriado"] = temas.Nome;
                return RedirectToAction(nameof(Index));
            }
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID", temas.FotoID);
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID", temas.SalaID);
            return View(temas);
        }

        // GET: Temas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.FindAsync(id);

            if (temas == null)
            {
                return NotFound();
            }
            ViewBag.TemaAntigo = temas.Nome;
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID", temas.FotoID);
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID", temas.SalaID);
            return View(temas);
        }

        // POST: Temas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas,String nomeAntigo, IFormFile Imagem)
        {
            if (id != temas.TemaID)
            {
                return NotFound();
            }
            if (ModelState.ContainsKey("nomeAntigo"))
            {
                ModelState.Remove("nomeAntigo");
            }
           
            if (ModelState.IsValid)
            {
                var tema = _context.Temas.FirstOrDefault(m => m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower() && m.TemaID != temas.TemaID);
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
                        temas.FotoID = f.FotoID;
                        temas.Foto = f;
                        hasImagem = true;

                    }
                }



                if (erro)
                {
                    return View(temas);
                }
                try
                {
                    
                    _context.Update(temas);
                    _context.Entry(temas).Property(t => t.DataCriacao).IsModified = false;
                    await _context.SaveChangesAsync();
                    if (hasImagem)
                    {
                        string localizacaoImagem = _webHostEnvironment.WebRootPath;
                        // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens
                        localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                        if (!Directory.Exists(localizacaoImagem))
                        {
                            Directory.CreateDirectory(localizacaoImagem);
                        }
                        // atribuir ao caminho o nome da imagem
                        localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);
                        using var stream = new FileStream(
                      localizacaoImagem, FileMode.Create
                      );
                        await Imagem.CopyToAsync(stream);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemasExists(temas.TemaID))
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
                return View(temas);
            }
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID", temas.FotoID);
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID", temas.SalaID);
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

            var temas = await _context.Temas
                .Include(t => t.Foto)
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.TemaID == id);
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
            String? tema = null;

            var temas = await _context.Temas.FindAsync(id);
            if (temas != null)
            {
                 tema = temas.Nome;
                temas.Deleted = true;

            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            if(tema != null)
            {
                TempData["TemaApagado"] = tema;
            }
           
            return RedirectToAction(nameof(Index));
        }

        private bool TemasExists(int id)
        {
            return _context.Temas.Any(e => e.TemaID == id);
        }
    }
}
