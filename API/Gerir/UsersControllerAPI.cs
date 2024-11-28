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
using Microsoft.AspNetCore.Authorization;

namespace EFS_23298_23327.Controllers
{
    [Route("api/gerir/users")]
    [CustomAuthorize(Roles ="Admin,Anfitriao")]
    [ApiController]
    public class UsersControllerAPI : ControllerBase {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<Utilizadores> _userManager;

        public UsersControllerAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager, RoleManager<IdentityRole> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/gerir/anfs
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> getUsers() {
            //todos os utilizadores não apagados
            var users = await _context.Utilizadores.Where(u => u.Deleted != true).OrderByDescending(u => u.DataCriacao).ToListAsync();
            ICollection<UtilizadoresViewModel> listaU = new List<UtilizadoresViewModel>();

            if (users != null || users.Any()) {
                //Ir buscar os roles a que um utilizador pertence,para cada utilizador
                foreach (var u in users) {

                    var a = new UtilizadoresViewModel(u);
                    a.Roles = _userManager.GetRolesAsync(u).Result.ToHashSet();
                    if (a.Roles.Contains("Cliente")) {
                        listaU.Add(a);
                    }



                }
            }

            return Ok(listaU);
        }
      

    }



}





