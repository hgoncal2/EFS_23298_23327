using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23306.Data;
using EFS_23298_23306.Models;
using System.Numerics;

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
            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.deleted!=true)).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Temas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos)
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
        public async Task<IActionResult> Create([Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas)
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
                Dictionary<Fotos, IFormFile> mapFotos = new Dictionary<Fotos, IFormFile>();
                
                if (Imagens != null)
                {
                    foreach(var Imagem in Imagens)
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
                                
                                f.TemaID = temas.TemaID;
                                f.Tema = temas;

                                temas.ListaFotos.Add(f);
                                mapFotos.Add(f,Imagem);
                                hasImagem = true;

                            }
                        }
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

            var temas = await _context.Temas.Include(m => m.ListaFotos.Where(f => f.deleted != true)).FirstOrDefaultAsync(m => m.TemaID == id);

            if (temas == null)
            {
                return NotFound();
            }
            ViewBag.TemaAntigo = temas.Nome;
           
            return View(temas);
        }

        // POST: Temas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas,String nomeAntigo)
        {
            if (id != temas.TemaID)
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

                                f.TemaID = temas.TemaID;
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
                var temasAtual = await _context.Temas.Include(m => m.ListaFotos).FirstOrDefaultAsync(m => m.TemaID == id);
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

            var temas = await _context.Temas.Include(m => m.ListaFotos).FirstOrDefaultAsync(m => m.TemaID == id);
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
                var fotos = await _context.Fotos.Where(f => f.TemaID == temas.TemaID).ToListAsync();
                foreach (var item in fotos)
                {
                   item.deleted = true;
                }

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

        // POST: Temas/EliminaFoto/5
        [HttpPost, ActionName("EliminaFoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminaFoto(int id)
        {
            

            var foto = await _context.Fotos.FindAsync(id);
            Temas tema;
            if (foto != null)
            {

               
                foto.deleted = true;
                 tema = await _context.Temas.FirstOrDefaultAsync(f => f.TemaID == foto.TemaID);
                if (tema != null)
                {
                    tema.ListaFotos.FirstOrDefault(f => f.FotoID == id).deleted = true;
                    await _context.SaveChangesAsync();

                    TempData["FotoEliminada"] = true;
                    return RedirectToAction(nameof(Edit),new { id = tema.TemaID});
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
            return _context.Temas.Any(e => e.TemaID == id);
        }
    }
}
