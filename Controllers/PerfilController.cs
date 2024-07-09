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
using System.Diagnostics;
using System.Security.Claims;
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
            HashSet<string> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var vm = new UtilizadoresViewModel(user);
            vm.Roles = roles;
            return PartialView("_PartialViewReservas", vm);

        }

        public async Task<IActionResult> Pref()
        {
            var user = await _context.Utilizadores.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            HashSet<string> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var vm = new UtilizadoresViewModel(user);
            vm.Roles = roles;
            return PartialView("_PartialViewPref", vm);

        }
    }

   

}
