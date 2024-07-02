using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace EFS_23298_23327.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private Utilizadores userLogado { get; set; }

        // GET: AccountController

        public AccountController(UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [CustomAuthorize]
        [HttpGet]
        public ActionResult Login(bool unauth)

        {

            if (unauth)
            {
                ViewBag.Unauth = true;
            }
            return View(new LoginViewModel());
        }




        [CustomAuthorize]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View(loginVM);

            }
            var u = await  _context.Utilizadores.Where(a => !a.Deleted).Where(a => a.UserName == loginVM.Username).FirstOrDefaultAsync();
            if (u != null)
            {
                var user =await _context.Utilizadores.Where(a => a.Deleted).Where(a => a.Id == u.Id).FirstOrDefaultAsync();
                
                if(user != null) {
                    ViewBag.Erro = true;
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                var pass = await _userManager.CheckPasswordAsync(u, loginVM.Password);
                if (pass)
                {
                   
                    var result = await _signInManager.PasswordSignInAsync(u, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        if(u.UserName == "admin" && !User.IsInRole("Admin"))
                        {
                           
                             await _userManager.AddToRoleAsync(u, "Admin");
                            
                        }
                        TempData["NomeUtilizadorLogado"] = u.UserName;
                        if (User.IsInRole("Admin") || User.IsInRole("Anfitriao")){
                            return RedirectToAction("Index", "Temas", new { area = "Gerir" });
                        }
                       
                        
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                ViewBag.Erro = true;
                return View(loginVM);
            }

            ViewBag.Erro = true;
            return View(loginVM);


        }

        [CustomAuthorize]
        [HttpGet]
        public  ActionResult Register() {
            
            var ViewModel = new RegisterViewModel();
            return View(new RegisterViewModel());

        }

       
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm) {
            if (ModelState.IsValid) {

                var user = await _context.Utilizadores.Where(a => !a.Deleted).Where(a => a.UserName == vm.Username).FirstOrDefaultAsync();

                if (user != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user.UserName + "</strong>\" já existe!";

                    return View(vm);
                } else {
                    if (vm.Email != null && vm.Email.Trim() != "") {
                        user = _context.Utilizadores.Where(u => u.Email.Trim() == vm.Email.Trim()).FirstOrDefault();
                        if (user != null) {


                            ViewBag.UserExiste = "Utilizador com email \"<strong>" + user.Email + "</strong>\" já existe!";
                            return View(vm);
                        }
                    }

                }
                var u = new Clientes(vm);
                _context.Add(u);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(u, "Cliente");
                TempData["NomeUtilizadorCriado"] = u.UserName;
                return await Login(new LoginViewModel(vm.Username, vm.Password));
            }


                return View(new RegisterViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            TempData["logOut"] = true;
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToAction("Index", "Home", new {area = ""});
            }
        }



    }
}
