using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Authorization;
using EFS_23298_23306.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Area("Gerir")]
    public class UtilizadoresController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context, UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
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
                   
                   
                   HashSet<String> roles =  _userManager.GetRolesAsync(u).Result.ToHashSet();
                    var uVM = new UtilizadoresViewModel(u);
                    uVM.Roles = roles;
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

            var ViewModel = new RegisterViewModel();
            ViewBag.SelectionIdList = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", ViewModel.Roles);
            return View(new RegisterViewModel());
        }

        // POST: UtilizadoresViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Utilizadores.Where(u=> u.UserName == rvm.Username).FirstOrDefault();
                if (user != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user.UserName + "</strong>\" já existe!";
                    ViewBag.SelectionIdList = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", rvm.Roles);

                    return View(rvm);
                } else {
                    user = _context.Utilizadores.Where(u => u.Email == rvm.Email).FirstOrDefault();
                    if (user != null) {
                        ViewBag.SelectionIdList = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", rvm.Roles);

                        ViewBag.UserExiste = "Utilizador com email \"<strong>" + user.Email + "</strong>\" já existe!";
                        return View(rvm);
                    }
                }
                Utilizadores u = new Utilizadores(rvm);
                u.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                u.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(u);
                await _context.SaveChangesAsync();
                foreach (var item in rvm.Roles) {
                    await _userManager.AddToRoleAsync(u, item);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rvm);
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
