using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EFS_23298_23327.ViewModel;
using EFS_23298_23327.API.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFS_23298_23327.Controllers
{
    [Route("api/gerir/anfs")]
    [ApiController]
    public class AnfitrioesControllerAPI : ControllerBase {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<Utilizadores> _userManager;

        public AnfitrioesControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager, RoleManager<IdentityRole> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/gerir/anfs
        [HttpGet]
        public async Task<IActionResult> getAnfs() {
            //todos os utilizadores não apagados
            var users = await _context.Anfitrioes.Where(u => u.Deleted != true).OrderByDescending(u => u.DataCriacao).ToListAsync();
            Dictionary<String, HashSet<String>> DictRolesUser = new Dictionary<String, HashSet<String>>();
            ICollection<UtilizadoresViewModel> listaU = new List<UtilizadoresViewModel>();

            if (users != null || users.Any()) {
                //Ir buscar os roles a que um utilizador pertence,para cada utilizador
                foreach (var u in users) {

                    var a = new UtilizadoresViewModel(u);
                    a.Roles = _userManager.GetRolesAsync(u).Result.ToHashSet();
                    if (a.Roles.Contains("Anfitriao")) {
                        listaU.Add(a);
                    }



                }
            }

            return Ok(listaU);
        }

        // GET: api/gerir/salas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> getAnf(string id) {
            //todos os utilizadores não apagados
            var user = await _context.Anfitrioes.Where(u => u.Deleted != true && u.Id == id).OrderByDescending(u => u.DataCriacao).FirstOrDefaultAsync();
            if (user == null) {
                return NotFound();
            }
            var u = new UtilizadoresViewModel(user);
            u.Roles = _userManager.GetRolesAsync(user).Result.ToHashSet();


            return Ok(u);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RegisterViewModel rvm) {
            ModelState.Remove("confirmPassword");
            if (ModelState.IsValid) {
                var user = _context.Utilizadores.Where(u => u.UserName.Trim() == rvm.Username.Trim()).FirstOrDefault();

                if (user != null) {

                    return BadRequest(new { Error = "Username existe!" });
                } else {
                    if (rvm.Email != null && rvm.Email.Trim() != "") {
                        user = _context.Utilizadores.Where(u => u.Email.Trim() == rvm.Email.Trim()).FirstOrDefault();
                        if (user != null) {


                            return BadRequest(new { Error = "Email já está a ser usado!" });
                        }
                    }

                }

                //Só vamos criar anfitrioes por agora
                rvm.Roles.Clear();
                rvm.Roles.Add("Anfitriao");
                if (rvm.Roles.Contains("Anfitriao") && rvm.Roles.Contains("Cliente")) {
                    return BadRequest("Um utilizador não pode ser cliente e anfitrião");
                }
                Utilizadores u = null;
                if (rvm.Roles.Contains("Anfitriao")) {
                    u = new Anfitrioes(rvm);
                } else {
                    if (rvm.Roles.Contains("Cliente")) {
                        u = new Clientes(rvm);
                    } else {
                        u = new Utilizadores(rvm);
                    }
                }



                u.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                u.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(u);
                await _context.SaveChangesAsync();
                foreach (var item in rvm.Roles) {
                    await _userManager.AddToRoleAsync(u, item);
                }
                return Ok();
            }
            return BadRequest();
        }
        [CustomAuthorize(Roles = "Admin,Anfitriao")]
        [HttpPut("{id}")]

        public async Task<IActionResult> Edit(string id, [FromForm] UtilizadoresViewModel userVM) {
            if (userVM == null || id == null) {
                return NotFound();
            }
            var user = await _context.Anfitrioes.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == userVM.Id);

            if (user == null) {
                return NotFound();
            }


            var sendingUser = await _context.Utilizadores.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user.Id != User.FindFirstValue(ClaimTypes.NameIdentifier) && !_userManager.GetRolesAsync(sendingUser).Result.ToHashSet().Contains("Admin")) {

                return Unauthorized();

            }


            if (ModelState.IsValid) {


                if (user.UserName == "admin" && userVM.Username != "admin") {
                    return Unauthorized();
                }

                var user2 = await _context.Utilizadores.Where(u => u.Deleted == false).Where(u => u.UserName.Trim() == userVM.Username.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();


                if (user2 != null) {
                    return BadRequest(new { Error = "Utilizador " + user2.UserName + "já existe" });



                } else {
                    if (userVM.Email != null && userVM.Email.Trim() != "") {
                        user2 = await _context.Utilizadores.Where(u => u.Deleted == false).Where(u => u.Email.Trim() == userVM.Email.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();
                        if (user2 != null) {

                            return BadRequest(new { Error = "Utilizador com email" + user2.Email + "já existe" });
                        }
                    }






                   ;
                    if ((userVM.Password != null && userVM.Password != "") && userVM.Password == userVM.ConfirmPassword) {
                        var hasher = new PasswordHasher<Utilizadores>();
                        user.PasswordHash = hasher.HashPassword(null, userVM.Password);

                    }
                    user.Email = userVM.Email;
                    user.PrimeiroNome = userVM.PrimeiroNome;
                    user.UltimoNome = userVM.UltimoNome;

                    _context.Update(user);
                    _context.Entry(user).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(user).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(user).Property(t => t.CriadoPorUsername).IsModified = false;
                    await _context.SaveChangesAsync();

                    return Ok();

                }

            }
            return BadRequest();
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(String id) {
            

                        var sendingUser = await _context.Utilizadores.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(sendingUser == null) {
                return Unauthorized();
            }
            if (!_userManager.GetRolesAsync(sendingUser).Result.ToHashSet().Contains("Admin")) {
                return Unauthorized();
            }


                var utilizador = await _context.Utilizadores.FindAsync(id);

            if (utilizador != null) {
                if (utilizador.UserName == "admin") {
                    return Unauthorized();
                }
                var r = await _context.Reservas.Where(r=> (r.Deleted || r.Cancelada) && r.ReservaDate>DateTime.Now ).Include(r=>r.ListaAnfitrioes).Where(a=> a.ListaAnfitrioes.Select(a=>a.Id).Contains(utilizador.Id)).FirstOrDefaultAsync();
                if (r != null) {
                    return Unauthorized(new { Error = "Não pode eliminar anfitriões com reservas ativas!" });
                }
                utilizador.Deleted = true;


            } else {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return Ok();

        }

    }



}





