using EFS_23298_23306.Data;
using EFS_23298_23306.Models;
using EFS_23298_23306.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Login()

        {
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
                    var result = await _signInManager.PasswordSignInAsync(u, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        if(u.UserName == "admin")
                        {
                            await _userManager.AddToRoleAsync(u, "Admin");
                        }
                        userLogado = u;
                        return RedirectToAction("Index", "Temas");
                    }
                }
            }

            ViewBag.Erro = true;
            return View(loginVM);


        }



    }
}
