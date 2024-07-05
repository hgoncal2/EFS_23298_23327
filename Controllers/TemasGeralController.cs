using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;

namespace EFS_23298_23327.Controllers
{
    public class TemasGeralController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemasGeralController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TemasGeral
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);
            ICollection<TemasFotoViewModel> TmVm = new List<TemasFotoViewModel>();
            var lista = await applicationDbContext.ToListAsync();
            if (lista != null)
            {
                if (lista.Any())
                {
                    foreach (var tema in lista)
                    {
                        if (tema.ListaFotos.Any())
                        {
                            foreach (var foto in tema.ListaFotos)
                            {
                                TmVm.Add(new TemasFotoViewModel(tema.Nome, foto.Nome));
                            }
                        }

                    }
                }
            }
            return View(lista);
        }

        // GET: TemasGeral/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }

        // GET: TemasGeral/Create
        public IActionResult Create()
        {
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaId", "SalaId");
            return View();
        }

        // POST: TemasGeral/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID,DataCriacao,Deleted,CriadoPorOid,CriadoPorUsername")] Temas temas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(temas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaId", "SalaId", temas.SalaID);
            return View(temas);
        }

        // GET: TemasGeral/Edit/5
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
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaId", "SalaId", temas.SalaID);
            return View(temas);
        }

        // POST: TemasGeral/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID,DataCriacao,Deleted,CriadoPorOid,CriadoPorUsername")] Temas temas)
        {
            if (id != temas.TemaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temas);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaID"] = new SelectList(_context.Salas, "SalaId", "SalaId", temas.SalaID);
            return View(temas);
        }

        // GET: TemasGeral/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }

        // POST: TemasGeral/Delete/5
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
            return _context.Temas.Any(e => e.TemaId == id);
        }
    }
}
