using EFS_23298_23306.Data;
using EFS_23298_23306.Models;
using EFS_23298_23306.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFS_23298_23306.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly ApplicationDbContext _context;
        private Utilizadores userLogado { get; set; }

        // GET: AccountController

        public AccountController(UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager,
            ApplicationDbContext context)
        {
            _context = context;

            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public ActionResult Login(bool unauth)

        {

            if (unauth)
            {
                ViewBag.Unauth = true;
            }
            return View(new LoginViewModel());
        }
        




        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View(loginVM);

            }
            var u = await _userManager.FindByNameAsync(loginVM.Username);
            if (u != null)
            {
                var pass = await _userManager.CheckPasswordAsync(u, loginVM.Password);
                if (pass)
                {
                    bool hasChanges = false;
                    bool adminAnf = false;
                    var result = await _signInManager.PasswordSignInAsync(u, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        if(u.UserName == "admin" && !User.IsInRole("Admin"))
                        {
                            hasChanges = true;
                            adminAnf = true;
                             await _userManager.AddToRoleAsync(u, "Admin");
                            
                        }
                        var userAnf = await _context.Anfitrioes.Where(m => m.Deleted != true).Where(a=> a.UserName.Equals(u.UserName)).FirstOrDefaultAsync();
                        if (userAnf != null && !User.IsInRole("Anfitriao")) {
                            hasChanges = true;
                            adminAnf = true;
                            await _userManager.AddToRoleAsync(u, "Anfitriao");
                        }
                        if ((!User.IsInRole("Admin") && !User.IsInRole("Anfitriao")) && !adminAnf) {
                            hasChanges = true;
                            await _userManager.AddToRoleAsync(u, "Cliente");
                        }
                        userLogado = u;
                        if (hasChanges) {
                            await _context.SaveChangesAsync();
                        }
                        
                        return RedirectToAction("Index", "Temas", new { area = "Gerir" });
                    }
                }
            }

            ViewBag.Erro = true;
            return View(loginVM);


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
