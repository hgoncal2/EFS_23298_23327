using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace EFS_23298_23327.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly IUserStore<Utilizadores> _userStore;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserEmailStore<Utilizadores> _emailStore;
       
        private readonly IEmailSender _emailSender;
        private Utilizadores userLogado { get; set; }
        public string ReturnUrl { get; private set; }

        // GET: AccountController

        public AccountController(UserManager<Utilizadores> userManager,
            IUserStore<Utilizadores> userStore,
            
            SignInManager<Utilizadores> signInManager,
            ApplicationDbContext context,
            
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
           
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(bool unauth)

        {

            if (unauth)
            {
                ViewBag.Unauth = true;
            }
            return View(new LoginViewModel());
        }




        [AllowAnonymous]
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
                   
                    var result = await _signInManager.PasswordSignInAsync(u, loginVM.Password,false,false);
                 

                    if (result.Succeeded)
                    {
                        
                        if (u.UserName == "admin" && !User.IsInRole("Admin"))
                        {
                            //Mete o email como tendo sido confirmado
                            await _emailStore.SetEmailConfirmedAsync(u, true, CancellationToken.None).ConfigureAwait(false);
                              _context.Update(u);
                            await _context.SaveChangesAsync();
                            await _userManager.AddToRoleAsync(u, "Admin");
                            
                            await _signInManager.PasswordSignInAsync(u, loginVM.Password, false, false);

                        }
                        TempData["NomeUtilizadorLogado"] = u.UserName;
                        if (User.IsInRole("Admin") && !User.IsInRole("Anfitriao")) {
                            return RedirectToAction("Index", "Temas", new { area = "Gerir" });
                        }
                        if (User.IsInRole("Anfitriao")) {
                            return RedirectToAction("Index", "Reservas");
                        }
                       
                        
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    var confirmed = await _userManager.IsEmailConfirmedAsync(u);
                    if (!confirmed) {
                        TempData["EmailNotConfirmed"] = "Por favor confirme o seu email para conseguir dar login!";
                    }
                }
                ViewBag.Erro = true;
                return View(loginVM);
            }

            ViewBag.Erro = true;
            return View(loginVM);


        }

        [AllowAnonymous]
        [HttpGet]
        public  ActionResult Register(string returnUrl=null) {
            ReturnUrl = returnUrl;
           
            

            var ViewModel = new RegisterViewModel();
            return View(new RegisterViewModel());

        }
       

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm, string returnUrl = null) {
            if (ModelState.IsValid) {

                if(vm.Password != vm.ConfirmPassword) {
                    ModelState.AddModelError("ConfirmPassword", "A password e a password de confirmação não são iguais!");
                    vm.Password = "";
                    vm.ConfirmPassword = "";
                    return View(vm);
                }
                var user = await _context.Utilizadores.Where(a => !a.Deleted).Where(a => a.UserName == vm.Username).FirstOrDefaultAsync();

                if (user != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user.UserName + "</strong>\" já existe!";

                    return View(vm);
                } else {
                    if (vm.Email != null && vm.Email.Trim() != "") {
                        var userEm = _context.Utilizadores.Where(u => u.Email.Trim() == vm.Email.Trim()).FirstOrDefault();
                        if (userEm != null) {


                            ViewBag.UserExiste = "Utilizador com email \"<strong>" + userEm.Email + "</strong>\" já existe!";
                            return View(vm);
                        }
                    }
                }
                returnUrl ??= Url.Content("~/");
                var u = CreateUser();
                await _userStore.SetUserNameAsync(u, vm.Username, CancellationToken.None);
                
                await _emailStore.SetEmailAsync(u, vm.Email, CancellationToken.None);
                
                var result = await _userManager.CreateAsync(u, vm.Password);
                //_context.Add(u);
                //       await _context.SaveChangesAsync();
                //     await _userManager.AddToRoleAsync(u, "Cliente");
                //   TempData["NomeUtilizadorCriado"] = u.UserName;
                // return await Login(new LoginViewModel(vm.Username, vm.Password));


                if (result.Succeeded) {
                    u.PrimeiroNome = vm.PrimeiroNome;
                    u.UltimoNome = vm.UltimoNome;
                    await _userManager.AddToRoleAsync(u, "Cliente");
                    _context.Update(u);
                    await _context.SaveChangesAsync();

                    var userId = await _userManager.GetUserIdAsync(u);
                    var location = new Uri($"{Request.Scheme}://{Request.Host}");
                    //O base path da aplicação
                    var url = location.AbsoluteUri;
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(u);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                        "ConfirmarEmail", "Account",values: new { area = "", userId = userId, code = code}, protocol: HttpContext.Request.Scheme
                        );
                        await _emailSender.SendEmailAsync(vm.Email, "Confirme o seu email",
                      $"Por favor verifique a sua conta clicando no link:\n{callbackUrl}");



                   

                    if (_userManager.Options.SignIn.RequireConfirmedAccount) {
                        TempData["ConfirmEmail"] = "Por favor verifique o seu email antes de poder continuar!";
                        return RedirectToAction("Index","Home");
                    } else {
                        await _signInManager.SignInAsync(u, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


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

        [CustomAuthorize]
        [HttpGet]
        public ActionResult ResetPassword() {

           
            return View(new LoginViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(LoginViewModel l) {
            ModelState.Remove("Password");
            
            if (ModelState.IsValid) {
                TempData["ResetPasswordSucc"] = "Pedido de reset password enviado!Por favor verifique o seu email!";
                var user = await _userManager.FindByNameAsync(l.Username);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(new LoginViewModel());
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ResetPasswordConfirm", "Account",
                   
                    values: new { area = "", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"Por favor clique no link para dar reset à password:\n {callbackUrl}");

                
            }
            TempData["ResetPasswordErr"] = "Erro indesperado,por favor tente mais tarde!";
            return View(new LoginViewModel());
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirm(string code = null) {
            if (code == null) {
                return BadRequest("Deve ser fornecido um código");
            } else {
                var r = new RegisterViewModel();
                TempData["Code"] = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                
                return View(r);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("Password,ConfirmPassword,Username")] RegisterViewModel u,String code) {
          

            var user = await _userManager.FindByNameAsync(u.Username);
            if (user == null) {
                
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)) ,u.Password);
            if (result.Succeeded) {
                TempData["EmailSucc"] = "Reset à password com sucesso!";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }


        private Utilizadores CreateUser() {
            try {
                return Activator.CreateInstance<Clientes>();
            } catch {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Clientes)}'. " +
                    $"Ensure that '{nameof(Clientes)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Utilizadores> GetEmailStore() {
            if (!_userManager.SupportsUserEmail) {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Utilizadores>)_userStore;
        }
        public async Task<IActionResult> ConfirmarEmail(string userId, string code) {
            if (userId == null || code == null) {
                return RedirectToAction("Index","Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) {
                TempData["EmailSucc"] = "Email confirmado com sucesso!";
               
            } else {
                TempData["EmailErr"] = "Erro ao confirmar Email!!";
            }
            return RedirectToAction(nameof(Login));
        }
    

}
}
