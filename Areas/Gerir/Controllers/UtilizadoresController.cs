using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23306.Data;
using EFS_23298_23306.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace EFS_23298_23306.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Area("Gerir")]
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UtilizadoresViewModels
        public async Task<IActionResult> Index()
        {

            var users = await _context.Utilizadores.ToListAsync();
            ICollection<UtilizadoresViewModel> listaU = new List<UtilizadoresViewModel>();
            if (users != null || users.Any())
            {
                foreach (var u in users)
                {
                    var uVM = new UtilizadoresViewModel(u.UserName, u.PrimeiroNome, u.UltimoNome, u.DataCriacao);
                    listaU.Add(uVM);
                }
            }
            return View(listaU);
        }

        // GET: UtilizadoresViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadoresViewModel = await _context.UtilizadoresViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadoresViewModel == null)
            {
                return NotFound();
            }

            return View(utilizadoresViewModel);
        }

        // GET: UtilizadoresViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UtilizadoresViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrimeiroNome,UltimoNome,DataCriacao")] UtilizadoresViewModel utilizadoresViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizadoresViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizadoresViewModel);
        }

        // GET: UtilizadoresViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadoresViewModel = await _context.UtilizadoresViewModel.FindAsync(id);
            if (utilizadoresViewModel == null)
            {
                return NotFound();
            }
            return View(utilizadoresViewModel);
        }

        // POST: UtilizadoresViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimeiroNome,UltimoNome,DataCriacao")] UtilizadoresViewModel utilizadoresViewModel)
        {
            if (id != utilizadoresViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizadoresViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresViewModelExists(utilizadoresViewModel.Id))
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
            return View(utilizadoresViewModel);
        }

        // GET: UtilizadoresViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadoresViewModel = await _context.UtilizadoresViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadoresViewModel == null)
            {
                return NotFound();
            }

            return View(utilizadoresViewModel);
        }

        // POST: UtilizadoresViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizadoresViewModel = await _context.UtilizadoresViewModel.FindAsync(id);
            if (utilizadoresViewModel != null)
            {
                _context.UtilizadoresViewModel.Remove(utilizadoresViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresViewModelExists(int id)
        {
            return _context.UtilizadoresViewModel.Any(e => e.Id == id);
        }
    }
}
