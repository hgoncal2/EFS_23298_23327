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
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Utilizadores.OrderByDescending(m => m.DataCriacao);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.UtilizadorID == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UtilizadorID,Username,PrimeiroNome,UltimoNome,Password,Email,NumeroTelemovel")] Utilizadores utilizadores)
        {
            if (ModelState.IsValid)
            {
                var msgErro = "";
                var erro = false;

                var utilizador = _context.Utilizadores.FirstOrDefault(m => m.Username.Trim().ToLower() == utilizadores.Username.Trim().ToLower());
                if (utilizador != null)
                {
                    ViewBag.UtilizadorExistente = utilizador.Username;

                    return View(utilizadores);
                }

                if (erro)
                {
                    return View(utilizadores);
                }


                _context.Add(utilizadores);
                await _context.SaveChangesAsync();
                TempData["NomeUtilizadorCriado"] = utilizadores.Username;
                return RedirectToAction(nameof(Index));
            }
            return View(utilizadores);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }
            ViewBag.UtilizadorAntigo = utilizadores.Username;
            return View(utilizadores);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UtilizadorID,Username,PrimeiroNome,UltimoNome,Email,NumeroTelemovel")] Utilizadores utilizadores,String nomeAntigo)
        {
            if (id != utilizadores.UtilizadorID)
            {
                return NotFound();
            }
            if (ModelState.ContainsKey("confirmPassword"))
            {
                ModelState.Remove("confirmPassword");
            }
            if (ModelState.ContainsKey("nomeAntigo"))
            {
                ModelState.Remove("nomeAntigo");
            }
            if (ModelState.ContainsKey("Password"))
            {
                ModelState.Remove("Password");
            }
            if (ModelState.IsValid)
            {
                var utilizador = _context.Utilizadores.FirstOrDefault(m => m.Username.Trim().ToLower() == utilizadores.Username.Trim().ToLower() && m.UtilizadorID != utilizadores.UtilizadorID);
                if (utilizador != null)
                {
                    ViewBag.UtilizadorExistente = utilizador.Username;
                    utilizadores.Username = nomeAntigo;
                    ViewBag.UtilizadorAntigo = utilizadores.Username;
                    return View(utilizadores);

                }
                var msgErro = "";
                var erro = false;

                if (erro)
                {
                    return View(utilizadores);
                }

                try
                {
                    _context.Update(utilizadores);
                    _context.Entry(utilizadores).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(utilizadores).Property(t => t.Password).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizadores.UtilizadorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.UtilizadorAntigo = utilizadores.Username;
                ViewBag.ShowAlert = true;
                return View(utilizadores);
            }
            return View(utilizadores);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.UtilizadorID == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            String? utilizador = null;
            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores != null)
            {
                utilizador = utilizadores.Username;
                _context.Utilizadores.Remove(utilizadores);
            }

            await _context.SaveChangesAsync();
            if (utilizador != null)
            {
                TempData["UtilizadorApagado"] = utilizador;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.UtilizadorID == id);
        }
    }
}
