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
using System.Linq.Dynamic.Core;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Area("Gerir")]
    public class UtilizadoresController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
        
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        //Construtor para o controller Utilizadores,irá receber uma instancia userManager(usada para gerir utilizadores) e roleManager(para gerir roles)
        public UtilizadoresController(ApplicationDbContext context, UserManager<Utilizadores> userManager,
             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        /// <summary>
        /// Get Index,devolve uma lista de UtilizadoresViewModel(para mostrar os roles dos utilizadores)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //todos os utilizadores não apagados
            var users = await _context.Utilizadores.Where(u=> u.Deleted != true).OrderByDescending(u=> u.DataCriacao).ToListAsync();
            Dictionary<String,HashSet<String>> DictRolesUser = new Dictionary<String,HashSet<String>>();
            ICollection<UtilizadoresViewModel> listaU = new List<UtilizadoresViewModel>();
          
            if (users != null || users.Any())
            {
                //Ir buscar os roles a que um utilizador pertence,para cada utilizador
                foreach (var u in users)
                { 
                   
                   
                DictRolesUser.Add(u.Id,_userManager.GetRolesAsync(u).Result.ToHashSet());
               //     var uVM = new UtilizadoresViewModel(u);
                 //   uVM.Roles = roles;
                  //  listaU.Add(uVM);
                }
            }

            TempData["DictRolesUser"] = DictRolesUser;
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            TempData["AllRoles"] = allRoles;
            return View(users);
        }

        // GET: UtilizadoresViewModels/Details/5
        public async Task<IActionResult> Details(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Utilizadores.Where(m => m.Deleted == false)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            HashSet<String> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            var uVM = new UtilizadoresViewModel(user);
            uVM.Roles = roles;
           

            return View(uVM);
        }

        // GET: UtilizadoresViewModels/Create
        public IActionResult Create()
        {
            //cria lista com todos os roles possíveis
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            ViewBag.SelectionIdList = allRoles;
            return View(new RegisterViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> Filter(Dictionary<string, string> dictVals, Dictionary<string, HashSet<String>> dicRoles, String last, String roles)
        {

            //Se tem filtro de Anfitriões
            var hasRoles = false;
            Dictionary<String, HashSet<String>> DictRolesUser = new Dictionary<String, HashSet<String>>();

            var Utilizadores = new List<Utilizadores>();
            //Inicializa query varia Anfitriões
            var queryRoles = "";
            var ListaRoles = new List<string>();
            var usersInRoles = new HashSet<string>();
            if (roles != "" && roles != null)
            {

                hasRoles = true;
                ListaRoles = roles.Split(",").ToList();
                //Interseção da lista de anfitriões da reserva com a lista de Ids do filtro.Se o tamanho da interseção das duas listas for igual ao tamanho da lista do filtro,devolve true(todos os elementos do filtro pertencem à lista de anfitriões)
               
                foreach(var role in ListaRoles)
                {
                    if (!usersInRoles.Any())
                    {
                        usersInRoles = usersInRoles.Concat(_userManager.GetUsersInRoleAsync(role).Result.Select(u => u.Id).ToHashSet<string>()).ToHashSet<string>();

                    }
                    else
                    {
                        usersInRoles = usersInRoles.Intersect(_userManager.GetUsersInRoleAsync(role).Result.Select(u => u.Id).ToHashSet<string>()).ToHashSet<string>();

                    }
                }
                queryRoles = "@0.Contains(Id)";
                // Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where("ListaAnfitrioes.Any(a => @0.Contains(a.Id))", Listaroles).ToListAsync();





            }
            //O dicionário contém "dic" se não houver valores filtrados(excepto anfitriões,não arranjei forma de passar o array no dicionário).Se não houver filtros de texto e não houver filtro de Anfitriões,devolve lista normal

            if (dictVals.ContainsKey("dic") && !dictVals.ContainsKey("roles"))
            {
                Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).OrderByDescending(u => u.DataCriacao).ToListAsync();

                //Se só houver filtro de anfitriões
            }
            if (dictVals.ContainsKey("dic") && dictVals.ContainsKey("roles"))
            {
                //Pode acontecer ter a key "roles" mas o Value estar vazio.Vamos certificar-nos que apanhamos esses casos
                if (hasRoles)
                {
                    //Devolve lista com filtro Anfitriões
                    Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(queryRoles , usersInRoles).ToListAsync();

                }
                else
                {
                    //Devolve lista normal,pois não caiu no primeiro "if"
                    Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).OrderByDescending(u => u.DataCriacao).ToListAsync();


                }
                //Se houver "mais" filtros na tabela(qualquer um excepto anfitrião
            }
            else
            {
                //Inicializa variáveis a string vazia.Vão ser necessárias para saber que tipo de filtro precisamos(importante para os OrderBys)
                var query = "";
                
                var orderby = "";
                var orderType = "";
                //Por cada valor no dicionário(Campo,Valor a filtrar)
                foreach (var val in dictVals)
                {
                    //Se o campo for data/roles,vamos ignorar por agora
                    if (!val.Key.ToLower().Contains("dat") && !val.Key.ToLower().Contains("roles"))
                    {
                        //Se for um campo boolean,a expressão vai ser diferente,temos que ter isso em conta
                        if (val.Value.ToLower().Contains("true") || val.Value.ToLower().Contains("false"))
                        {
                            query += @val.Key.Replace("_", ".") + "==" + val.Value;
                            //Guarda id do campo que foi submetido em ultimo.Esta linha de código é extremamente importante devido ao listener que está na dropdown(on change)que ative cada vez que se dá render da partial view
                            //Se este tempdata não tiver nulo,significa que já foi filtrado,e não preciisamos de chamar esta função outra vez.Esta verificação está a ser feita no lado do cliente
                            //Sem esta linha de código,vai entra em loop infinito!
                            TempData["lastVal"] = val.Value;
                        }
                        else
                        {
                            //Se for campo de texto normal(mesmo que seja int,estou a converter para string para fazer o includes)
                            query += @val.Key.Replace("_", ".") + ".toString().Contains(\"" + val.Value + "\")";
                        }
                        //Se não for o ultimo valor do dicionário,dá append do "And" para continuar a preencher os campos da query
                        if (!val.Equals(dictVals.Last()))
                        {
                            query += " && ";
                        }
                    }
                    else
                    {
                        //Se for data,vamos guardar que tipo de data é(Data inicial reserva,final,ou data criada)
                        orderby = val.Key;
                        //Tal como o tipo(desc,asc)
                        orderType = val.Value;
                    }

                }
                //Se a string da query acabar com os caracteres "&&",vamos removê-los
                if (query.Trim().EndsWith("&&"))
                {
                    query = query.Substring(0, query.LastIndexOf("&") - 1);

                }
                if (query == "")
                {
                    //Se query estiver vazia,quer dizer que so foi feito um filtro de datas
                    if (hasRoles)
                    {
                        //Ter em conta se foi efetuaod filtro nos anfitriões
                        Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(queryRoles, usersInRoles).OrderBy(orderby + " " + orderType).ToListAsync();

                    }
                    else
                    {
                        //Se não houver filtro de anfitriões
                        Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).OrderBy(orderby + " " + orderType).ToListAsync();
                    }


                }
                else
                {
                    if (orderby == "")
                    {
                        //Se orderBy tiver vazio,e a query não estiver vazia,filtra pelos campos e usa a order por defeito
                        if (hasRoles)
                        {
                            Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(query).Where(queryRoles, usersInRoles).ToListAsync();

                        }
                        else
                        {
                            Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(query).OrderByDescending(u => u.DataCriacao).ToListAsync();

                        }

                    }
                    else
                    {
                        if (hasRoles)
                        {
                            Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(query).Where(queryRoles, usersInRoles).OrderBy(orderby + " " + orderType).ToListAsync();

                        }
                        else
                        {
                            Utilizadores = await _context.Utilizadores.Where(u => u.Deleted != true).Where(query).OrderBy(orderby + " " + orderType).ToListAsync();

                        }


                    }


                }


            }

            //Pode acontecer o modelo dar return sem nenhum anfitrião,isto está aqui para esses casos(Idealmente,não deveria acontecer.Infelizmente não vivemos num mundo ideal)
           

            //Ussado para fazer a border do anfitrião
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            //Volta a enviar os valores do dicionário de filtros de modo a manter o estado em que estava
            TempData["dicVals"] = dictVals;
            //Mesma razão,voltar a selecionar todos os anfitriões filtrados
            TempData["Listaroles"] = ListaRoles;
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            TempData["AllRoles"] = allRoles;
            //Usado para fazer a legenda
            if (Utilizadores != null || Utilizadores.Any())
            {
                //Ir buscar os roles a que um utilizador pertence,para cada utilizador
                foreach (var u in Utilizadores)
                {


                    DictRolesUser.Add(u.Id, _userManager.GetRolesAsync(u).Result.ToHashSet());
                    //     var uVM = new UtilizadoresViewModel(u);
                    //   uVM.Roles = roles;
                    //  listaU.Add(uVM);
                }
            }

            TempData["DictRolesUser"] = DictRolesUser;
            //Usado para manter o foco no último campo em que estava a ser escrito
            TempData["Last"] = last;


            //E finalmente,dá return com o novo modelo "filtrado"

            return PartialView("_partialUtilizadoresTabela", Utilizadores);

        }

















        // POST: UtilizadoresViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel rvm)
        {
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            ViewBag.SelectionIdList = allRoles;
            if (ModelState.IsValid)
            {
                var user = _context.Utilizadores.Where(u=> u.UserName.Trim() == rvm.Username.Trim()).FirstOrDefault();
                
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
                    ViewBag.UserExiste = "Erro! \"<strong>" + rvm.Username + "</strong>\" não pode ser Cliente <strong>e</strong> Anfitrião!";
                    return View(rvm);
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
                TempData["NomeUtilizadorCriado2"] = u.UserName;
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

            var user = await _context.Utilizadores.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            
            HashSet<String> roles = _userManager.GetRolesAsync(user).Result.ToHashSet();
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();

            if (user is Anfitrioes) {
                allRoles.Remove("Cliente");
            } else {
                if (user is Clientes) {
                    allRoles.Remove("Anfitriao");
                } else {
                    allRoles.RemoveWhere(s => s is String);
                    allRoles.Add("Admin");
                }
            }

           
            
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
            var user = await _context.Utilizadores.Where(m => m.Deleted == false).FirstOrDefaultAsync(m => m.Id == userVM.Id);
            if (user == null) {
                return NotFound();
            }
            HashSet<String> allRoles = _roleManager.Roles.Select(r => r.Name).ToHashSet();
            if (user is Anfitrioes) {
                allRoles.Remove("Cliente");
            } else {
                if (user is Clientes) {
                    allRoles.Remove("Anfitriao");
                } else {
                    allRoles.RemoveWhere(s => s is String);
                    allRoles.Add("Admin");
                }
            }
            ViewBag.SelectionIdList = allRoles;

            if (ModelState.IsValid) {

                
                if (user.UserName == "admin" && userVM.Username != "admin") {
                    ViewBag.UserExiste = "Erro! Não é possivel mudar o username da conta \"<strong>" + "admin" + "</strong>\"!";
                    userVM.Username = user.UserName;
                    ViewBag.UserAntigo = user.UserName;


                    return View(userVM);
                }
                
                var user2 =await _context.Utilizadores.Where(u => u.Deleted==false).Where(u => u.UserName.Trim() == userVM.Username.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();
               
                    
                    if (user2 != null) {
                    ViewBag.UserExiste = "Utilizador \"<strong>" + user2.UserName + "</strong>\" já existe!";
                  
                    userVM.Username = user.UserName;
                    ViewBag.UserAntigo = user.UserName;


                    return View(userVM);
                } else {
                    if (userVM.Email != null && userVM.Email.Trim() != "") {
                        user2 = await _context.Utilizadores.Where(u => u.Deleted == false).Where(u => u.Email.Trim() == userVM.Email.Trim() && u.Id != userVM.Id).FirstOrDefaultAsync();
                        if (user2 != null) {
           
                            ViewBag.UserExiste = "Utilizador com email \"<strong>" + user2.Email + "</strong>\" já existe!";

                            userVM.Username = user.UserName;
                            ViewBag.UserAntigo = user.UserName;

                            return View(userVM);
                        }
                    }

                    if(user is Anfitrioes && userVM.Roles.Contains("Cliente")) {
                        ViewBag.UserExiste = "Erro! \"<strong>" + userVM.Username + "</strong>\" foi <strong> criado como Anfitrião e não pode ser Cliente!</strong>";
                        userVM.Roles.Remove("Cliente");
                       
                       
                        return View(userVM);
                    }
                    if (user is Clientes && userVM.Roles.Contains("Anfitriao")) {
                        ViewBag.UserExiste = "Erro! \"<strong>" + userVM.Username + "</strong>\" foi <strong> criado como Cliente e não pode ser Anfitrião!</strong> ";
                        userVM.Roles.Remove("Anfitriao");
                       
                        return View(userVM);
                    } else {
                        if(user is not Clientes && user is not Anfitrioes) {
                           
                        }
                       
                    }
                   
                    //Se for cliente
                    
                 

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
        //Não está a ser usado 
        /*
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
        */

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

     

        
    }
}
