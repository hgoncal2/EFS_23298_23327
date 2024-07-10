using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFS_23298_23327.Controllers
{
    [CustomAuthorize(Roles="Admin,Anfitriao,Cliente")]
    public class PerfilController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private Utilizadores userLogado { get; set; }


        public PerfilController(
            UserManager<Utilizadores> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.Utilizadores.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            HashSet<string> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var vm = new UtilizadoresViewModel(user);
            vm.Roles = roles;
            return View(vm);
            
        }

        public async Task<IActionResult> DadosPessoais()
        {
            var user = await _context.Utilizadores.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            HashSet<string> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var vm = new UtilizadoresViewModel(user);
            vm.Roles = roles;
            return PartialView("_PartialViewDadosP", vm);

        }

        public async Task<IActionResult> Reservas()
        {
            var user = await _context.Utilizadores.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var c = await _context.Clientes.Where(c => c.UserName.Equals(user.UserName)).FirstAsync();
            var r = await _context.Reservas.Where(r => r.Cliente.UserName.Equals(user.UserName)).ToListAsync();
            var vm = new Reservas(c);
            return PartialView("_PartialViewReservas", r);

        }

        public async Task<IActionResult> Pref()
        {
            var user = await _context.Utilizadores.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            HashSet<string> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var vm = new UtilizadoresViewModel(user);
            vm.Roles = roles;
            return PartialView("_PartialViewPref", vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UtilizadoresViewModel userVM)
        {
            if (userVM == null)
            {
                return NotFound();
            }
            var user = await _context.Utilizadores.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == userVM.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user2 = await _context.Utilizadores.Where(u => u.Deleted == false).Where(u => u.UserName.Trim() == userVM.Username.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();

                if (userVM.Email != null && userVM.Email.Trim() != "")
                {
                    user2 = await _context.Utilizadores.Where(u => u.Deleted == false).Where(u => u.Email.Trim() == userVM.Email.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();
                    if (user2 != null)
                    {
                        ViewBag.UserExiste = "Utilizador com email \"<strong>" + user2.Email + "</strong>\" já existe!";
                        return RedirectToAction("Index");
                    }
                }                  

                //Se for cliente
                user.Email = userVM.Email;
                user.PrimeiroNome = userVM.PrimeiroNome;
                user.UltimoNome = userVM.UltimoNome;

                _context.Update(user);
                _context.Entry(user).Property(t => t.DataCriacao).IsModified = false;
                _context.Entry(user).Property(t => t.CriadoPorOid).IsModified = false;
                _context.Entry(user).Property(t => t.CriadoPorUsername).IsModified = false;
                await _context.SaveChangesAsync();                  
                ViewBag.ShowAlert = true;
                TempData["UtilizadorAlterado"] = "Alterações guardadas com sucesso!";
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
    }

   

}
