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
using EFS_23298_23327.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Runtime.Intrinsics.Arm;

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

            var users = await _context.Utilizadores.Where(u=> u.Deleted != true).OrderByDescending(u=> u.DataCriacao).ToListAsync();
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
        public async Task<IActionResult> Details(String? id)
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
            
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            ViewBag.SelectionIdList = allRoles;
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
                var user = _context.Utilizadores.Where(u=> u.UserName.Trim() == rvm.Username.Trim()).FirstOrDefault();
                HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
                ViewBag.SelectionIdList = allRoles;
                if (user != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user.UserName + "</strong>\" já existe!";

                    return View(rvm);
                } else {
                    if (rvm.Email!=null && rvm.Email.Trim() != "") {
                        user = _context.Utilizadores.Where(u => u.Email.Trim() == rvm.Email.Trim()).FirstOrDefault();
                        if (user != null) {
                            

                            ViewBag.UserExiste = "Utilizador com email \"<strong>" + user.Email + "</strong>\" já existe!";
                            return View(rvm);
                        }
                    }
                    
                }
                if(rvm.Roles.Contains("Anfitriao") && rvm.Roles.Contains("Cliente")) {
                    ViewBag.UserExiste = "Erro! \"<strong>" + rvm.Username + "</strong>\" não pode ser Ciente <strong>e</strong> Anfitrião!";
                    return View(rvm);
                }
                Utilizadores u = null;
                if (rvm.Roles.Contains("Anfitriao")) {
                     u = new Anfitrioes(rvm);
                }
                else{
                     u = new Utilizadores(rvm);
                }
               
                u.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                u.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(u);
                await _context.SaveChangesAsync();
                foreach (var item in rvm.Roles) {
                    await _userManager.AddToRoleAsync(u, item);
                }
                TempData["NomeUtilizadorCriado"] = u.UserName;
                return RedirectToAction(nameof(Index));
            }
            return View(rvm);
        }

        // GET: UtilizadoresViewModels/Edit/5
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Utilizadores.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            HashSet<String> allRoles = _roleManager.Roles.Select(r=> r.Name).ToHashSet();
            HashSet<String> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var ViewModel = new UtilizadoresViewModel(user);
            ViewModel.Roles= roles;
            ViewBag.SelectionIdList = allRoles;  
            return View(ViewModel);
           
        }

        // POST: UtilizadoresViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UtilizadoresViewModel userVM) {
            if (userVM == null) {
                return NotFound();
            }
            var user = await _context.Utilizadores.FindAsync(userVM.Id);
            if (user == null) {
                return NotFound();
            }
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            ViewBag.SelectionIdList = allRoles;

            if (ModelState.IsValid) {

                
                if (user.UserName == "admin" && userVM.Username != "admin") {
                    ViewBag.UserExiste = "Erro! Não é possivel mudar o username da conta \"<strong>" + "admin" + "</strong>\"!";
                    userVM.Username = user.UserName;
                    ViewBag.UserAntigo = user.UserName;


                    return View(userVM);
                }
                
                var user2 = _context.Utilizadores.Where(u => u.UserName.Trim() == userVM.Username.Trim() && u.Id != userVM.Id).FirstOrDefault();
               
                    
                    if (user2 != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user2.UserName + "</strong>\" já existe!";
                  
                    userVM.Username = user.UserName;
                    ViewBag.UserAntigo = user.UserName;


                    return View(userVM);
                } else {
                    if (userVM.Email != null && userVM.Email.Trim() != "") {
                        user2 = _context.Utilizadores.Where(u => u.Email.Trim() == userVM.Email.Trim() && u.Id != userVM.Id).FirstOrDefault();
                        if (user2 != null) {
           
                            ViewBag.UserExiste = "Utilizador com email \"<strong>" + user.Email + "</strong>\" já existe!";

                            userVM.Username = user.UserName;
                            ViewBag.UserAntigo = user.UserName;

                            return View(userVM);
                        }
                    }



                    user.UserName = userVM.Username;
                    user.Email=userVM.Email;
                    user.PrimeiroNome = userVM.PrimeiroNome;
                    user.UltimoNome = userVM.UltimoNome;
                    
                    _context.Update(user);
                    _context.Entry(user).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(user).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(user).Property(t => t.CriadoPorUsername).IsModified = false;
                    await _context.SaveChangesAsync();
                    HashSet<String> rolesAntigos = _userManager.GetRolesAsync(user).Result.ToHashSet();
                    HashSet<String> rolesNovos = userVM.Roles.ToHashSet();
                    HashSet<String> rolesAdicionar = rolesNovos.Except(rolesAntigos).ToHashSet();
                    HashSet<String> rolesRemover = rolesAntigos.Except(rolesNovos).ToHashSet();
                    if(user.UserName == "admin" && rolesRemover.Contains("Admin")){
                        ViewBag.ShowAlert = true;
                        ViewBag.UserExiste = "Erro!Não é possível remover a role 'Admin' ao utilizador \"<strong>" + "admin" + "</strong>\"!";
                        var roles =await _userManager.GetRolesAsync(user);
                        userVM.Roles = roles.ToHashSet();

                        return View(userVM);
                    }
                    if (rolesRemover != null) {
                       await _userManager.RemoveFromRolesAsync(user, rolesRemover);
                    }
                    if (rolesAdicionar != null) {
                        await _userManager.AddToRolesAsync(user, rolesAdicionar);
                    }
                    ViewBag.UserAntigo = userVM.Username;
                    ViewBag.ShowAlert = true;
                    return View(userVM);
                   
                }

            }
            return View(userVM);
        }
        // GET: UtilizadoresViewModels/Delete/5
        public async Task<IActionResult> Delete(String? id)
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
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            string? user = null;

            var utilizador = await _context.Utilizadores.FindAsync(id);

            if (utilizador != null) {
                if (utilizador.UserName == "admin") {
                    TempData["ErroAdmin"] = "Erro! Não é possivel eliminar o utilizador \"<strong>admin</strong>\"";
                    return RedirectToAction(nameof(Index));
                }
                user = utilizador.UserName;
                utilizador.Deleted = true;
      

            } else {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            if (user != null) {
                TempData["UtilizadorApagado"] = user;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresViewModelExists(String id)
        {
            return _context.UtilizadoresViewModel.Any(e => e.Id == id);
        }

        
    }
}
