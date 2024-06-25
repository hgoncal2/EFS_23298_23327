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

        public TemasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Temas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Temas.Include(t => t.Foto).Include(t => t.Sala);
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
        public async Task<IActionResult> Create([Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas)
        {

            if (ModelState.IsValid)
            {

                var msgErro = "";
                var erro = false;

                var tema = _context.Temas.FirstOrDefault(acc => acc.Nome == temas.Nome);
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
                if (erro)
                {
                    return View(temas);
                }
                
                _context.Add(temas);
                await _context.SaveChangesAsync();
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
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID", temas.FotoID);
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID", temas.SalaID);
            return View(temas);
        }

        // POST: Temas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaID,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade")] Temas temas)
        {
            if (id != temas.TemaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                if (erro)
                {
                    return View(temas);
                }
                try
                {
                    _context.Update(temas);
                    await _context.SaveChangesAsync();
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
                ViewBag.ShowAlert = true;
                return View(temas);
            }
            ViewData["FotoID"] = new SelectList(_context.Fotos, "FotoID", "FotoID", temas.FotoID);
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaID", "SalaID", temas.SalaID);
            return View(temas);
        }

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
            var temas = await _context.Temas.FindAsync(id);
            if (temas != null)
            {
                _context.Temas.Remove(temas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemasExists(int id)
        {
            return _context.Temas.Any(e => e.TemaID == id);
        }
    }
}
