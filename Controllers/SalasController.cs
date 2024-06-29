using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23306.Data;
using EFS_23298_23306.Models;
using EFS_23298_23306.ViewModel;

namespace EFS_23298_23306.Controllers
{
    public class SalasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Salas.Include(m => m.ListaAnfitrioes.Where(f => f.Deleted != true)).OrderByDescending(m => m.DataCriacao);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Salas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas
                .FirstOrDefaultAsync(m => m.SalaID == id);
            if (salas == null)
            {
                return NotFound();
            }

            return View(salas);
        }

        // GET: Salas/Create
        public async Task<IActionResult> Create()
        {
           
                var ViewModel = new AnfitriaoSalaViewModel(new Salas(), new HashSet<String>());
            ViewBag.SelectionIdList = new MultiSelectList(_context.Anfitrioes.Where(a => a.Deleted == false), "Id", "UserName", ViewModel.ListaAnfitrioes);

            return View(ViewModel);
        }

        // POST: Salas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sala,ListaAnfitrioes")] AnfitriaoSalaViewModel salasAnf)
        {
            if (ModelState.IsValid)
            {
                var salaExiste = _context.Salas.FirstOrDefault(m => m.Numero == salasAnf.Sala.Numero);
                if (salaExiste != null) {
                    ViewBag.SalaExistente = salaExiste.Numero;
                    ViewBag.SelectionIdList = new MultiSelectList(_context.Anfitrioes.Where(a => a.Deleted == false), "Id", "UserName", salasAnf.ListaAnfitrioes);

                    return View(salasAnf);
                }
                Salas sala = salasAnf.Sala;
                var anfs = new HashSet<Anfitrioes>();
                if (salasAnf.ListaAnfitrioes.Any()) {
                   

                    
                   
                    foreach (var item in salasAnf.ListaAnfitrioes)
                    {
                        var anf = await _context.Anfitrioes.FirstOrDefaultAsync(m => m.Id == item); 
                        if(anf != null) {
                            anfs.Add(anf);
                        }
                    }
                    


                }
                sala.ListaAnfitrioes = anfs;
                

                _context.Add(sala);
                await _context.SaveChangesAsync();
                TempData["NumeroSalaCriada"] = sala.Numero;
                return RedirectToAction(nameof(Index));
            }
            return View(salasAnf);
        }

        // GET: Salas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas.FindAsync(id);
            if (salas == null)
            {
                return NotFound();
            }
            return View(salas);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaID,Area,Numero")] Salas salas)
        {
            if (id != salas.SalaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalasExists(salas.SalaID))
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
            return View(salas);
        }

        // GET: Salas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas
                .FirstOrDefaultAsync(m => m.SalaID == id);
            if (salas == null)
            {
                return NotFound();
            }

            return View(salas);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salas = await _context.Salas.FindAsync(id);
            if (salas != null)
            {
                _context.Salas.Remove(salas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalasExists(int id)
        {
            return _context.Salas.Any(e => e.SalaID == id);
        }
    }
}
