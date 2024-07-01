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
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace EFS_23298_23306.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin,Anfitriao")]
    [Area("Gerir")]
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

            var salas = await _context.Salas.Include(s=> s.ListaAnfitrioes)
                .FirstOrDefaultAsync(m => m.SalaId == id);
            if (salas == null)
            {
                return NotFound();
            }

            return View(salas);
        }

        // GET: Salas/Create
        public async Task<IActionResult> Create()
        {

            var ViewModel = new AnfitriaoSalaViewModel(new Salas(), new HashSet<string>());
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
                if (salaExiste != null)
                {
                    ViewBag.SalaExistente = salaExiste.Numero;
                    ViewBag.SelectionIdList = new MultiSelectList(_context.Anfitrioes.Where(a => a.Deleted == false), "Id", "UserName", salasAnf.ListaAnfitrioes);

                    return View(salasAnf);
                }
                Salas sala = salasAnf.Sala;
                var anfs = new HashSet<Anfitrioes>();
                if (salasAnf.ListaAnfitrioes.Any())
                {

                    foreach (var item in salasAnf.ListaAnfitrioes)
                    {
                        var anf = await _context.Anfitrioes.FirstOrDefaultAsync(m => m.Id == item);
                        if (anf != null)
                        {
                            anfs.Add(anf);
                        }
                    }



                }
                sala.ListaAnfitrioes = anfs;

                sala.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sala.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
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

            var salas = await _context.Salas.Include(s => s.ListaAnfitrioes).Where(s => s.SalaId == id).FirstOrDefaultAsync();
            if (salas == null)
            {
                return NotFound();
            }

            List<String>anfs = salas.ListaAnfitrioes.Select(a => a.Id).ToList();
            var ViewModel = new AnfitriaoSalaViewModel(salas, anfs);
            ViewBag.SelectionIdList = new MultiSelectList(_context.Anfitrioes.Where(a => a.Deleted == false), "Id", "UserName", ViewModel.ListaAnfitrioes);

            return View(ViewModel);
          
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sala,ListaAnfitrioes")] AnfitriaoSalaViewModel salaVM,int numeroAntigo)
        {
            if (id != salaVM.Sala.SalaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Salas? s = null;
                var sala = _context.Salas.FirstOrDefault(m => m.Numero == salaVM.Sala.Numero && m.SalaId != salaVM.Sala.SalaId);
                if (sala != null) {
                    ViewBag.SalaExistente = sala.Numero;
                    salaVM.Sala.Numero = numeroAntigo;
                    ViewBag.SalaAntiga = salaVM.Sala.Numero;
                    return View(salaVM);

                }
                try
                {
                     s =await _context.Salas.Include(s=> s.ListaAnfitrioes).Where(s=> s.SalaId == salaVM.Sala.SalaId).FirstOrDefaultAsync();
                    s.Numero = salaVM.Sala.Numero;
                    s.Area = salaVM.Sala.Area;
                    s.ListaAnfitrioes = await _context.Anfitrioes.Where(m => salaVM.ListaAnfitrioes.Contains(m.Id)).ToListAsync();
                    _context.Update(s);
                    _context.Entry(s).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(s).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(s).Property(t => t.CriadoPorUsername).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalasExists(salaVM.Sala.SalaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.TemaAntigo =s.Numero ;
                ViewBag.ShowAlert = true;
                ViewBag.SelectionIdList = new MultiSelectList(_context.Anfitrioes.Where(a => a.Deleted == false), "Id", "UserName", salaVM.ListaAnfitrioes);

                return View(salaVM);
            }
            return View(salaVM);
        }

        // GET: Salas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas
                .FirstOrDefaultAsync(m => m.SalaId == id);
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
                try {
                    _context.Salas.Remove(salas); await _context.SaveChangesAsync();

                    TempData["SalaApagada"] = salas.Numero;
                } catch (SqlException e) {

                    var tema = await _context.Temas.Where(t => t.SalaID == salas.SalaId).FirstOrDefaultAsync();
                    if (tema != null) {
                        TempData["ErroEliminar"] = tema.Nome;
                        return RedirectToAction(nameof(Index));
                    }


                } catch (DbUpdateException ex) {
                    var tema = await _context.Temas.Where(t => t.SalaID == salas.SalaId).FirstOrDefaultAsync();
                    if (tema != null) {
                        TempData["ErroEliminar"] = tema.Nome;
                        return RedirectToAction(nameof(Index));
                    }
                }

                }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalasExists(int id)
        {
            return _context.Salas.Any(e => e.SalaId == id);
        }
    }
}
