using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles = "Admin,Anfitriao")]
    [Area("Gerir")]
    public class SalasController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;
       
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public SalasController(ApplicationDbContext context, UserManager<Utilizadores> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            
            _userManager = userManager;
        }


        /// <summary>
        /// GET /GERIR/SALAS
        /// </summary>
        /// <returns>Lista de salas</returns>
        public async Task<IActionResult> Index()

            
        {
            //Mapa que irá conter sala e número de reservas pendentes da semana
            Dictionary<int, int> mapSalaRes = new Dictionary<int,int>();
            //Todas as reservas pendentes
            var r = await _context.Reservas.Include(s=>s.Sala).Where(r => !r.Deleted).Where(r => r.ReservaEndDate > DateTime.Now).ToListAsync();
            //Todas as salas
            var applicationDbContext = _context.Salas.Include(m => m.ListaAnfitrioes.Where(f => f.Deleted != true)).Where(sa=>!sa.Deleted).OrderByDescending(m => m.DataCriacao);
            var s = await applicationDbContext.ToListAsync();
            //Por cada sala,adiciona número de reservas pendentes da semana ao map
            foreach (var item in s)
            {  

                mapSalaRes.Add(item.SalaId, r.Where(r => !r.Deleted && r.SalaId == item.SalaId && ISOWeek.GetWeekOfYear(r.ReservaDate) == ISOWeek.GetWeekOfYear(DateTime.Now)).Count());

            }
            //User logado. Poderia se usar,também, o User.Identity.Name,quis experimentar outras abordagens
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            //Lista de anfitriões,irá ser preciso para fazer os filtros.Está a ser incluido as preferencias,de modo a mostrar as cores das salas de acordo come stas
            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u => u.Cores).FirstOrDefaultAsync();
            if (u != null)
            {
                if (u.userPrefsAn != null)
                    //Criado mapa que irá conter a sala e a respetiva cor,de acordo com as preferências do utilizador
                {
                    Dictionary<int, string> coresSalas = new Dictionary<int, string>();

                    foreach (var i in u.userPrefsAn.Cores)
                    {
                        var sNum = s.Where(s => s.SalaId == i.Key).Select(a=>a.Numero).First();


                        coresSalas.Add(sNum, i.Value);
                    }
                    TempData["SalasCor"] = coresSalas;


                }
            }

            ViewBag.SalasCount = mapSalaRes;
            return View(s);
        }


        /// <summary>
        /// Filtro das salas. Ver comentários efetuados no ReservasGerirController,no mesmo método,onde cada linha é explicada com mais detalhe
        /// </summary>
        /// <param name="dictVals"></param>
        /// <param name="dic"></param>
        /// <param name="last"></param>
        /// <param name="anfs"></param>
        /// <param name="dicSalasCount"></param>
        /// <returns>Partial view com tabela filtrada</returns>
        [HttpGet]
        public async Task<IActionResult> Filter(Dictionary<string, string> dictVals, Dictionary<int, string> dic, string last, string anfs,Dictionary<int, int> dicSalasCount)
        {

            //Se tem filtro de Anfitriões
            var hasAnfs = false;
            var salas = new List<Salas>();
            //Inicializa query varia Anfitriões
            var queryAnf = "";
            var isNumReservas = dictVals.ContainsKey("NumReservas");
            List<int> num = null;
            if (isNumReservas)
            {
                
                num = await _context.Salas.Include(s=>s.ListaReservas).Where(s => (s.ListaReservas.Where(r => r.ReservaEndDate > DateTime.Now)).Count() == int.Parse(dictVals.GetValueOrDefault("NumReservas"))).Select(r => r.SalaId).ToListAsync();
                
            }
            var ListaAnfs = new List<string>();
            if (anfs != "" && anfs != null)
            {

                hasAnfs = true;
                ListaAnfs = anfs.Split(",").ToList();
                //Interseção da lista de anfitriões da reserva com a lista de Ids do filtro.Se o tamanho da interseção das duas listas for igual ao tamanho da lista do filtro,devolve true(todos os elementos do filtro pertencem à lista de anfitriões)
                queryAnf = "ListaAnfitrioes.Select(a => a.Id).Intersect(@0).Count() == @0.Count()";





            }
            //O dicionário contém "dic" se não houver valores filtrados(excepto anfitriões,não arranjei forma de passar o array no dicionário).Se não houver filtros de texto e não houver filtro de Anfitriões,devolve lista normal

            if (dictVals.ContainsKey("dic") && !dictVals.ContainsKey("anfs"))
            {
                salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).OrderByDescending(u => u.DataCriacao).ToListAsync();

                //Se só houver filtro de anfitriões
            }
            if (dictVals.ContainsKey("dic") && dictVals.ContainsKey("anfs"))
            {
                //Pode acontecer ter a key "anfs" mas o Value estar vazio.Vamos certificar-nos que apanhamos esses casos
                if (hasAnfs)
                {
                    //Devolve lista com filtro Anfitriões
                    salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(queryAnf, ListaAnfs).ToListAsync();

                }
                else
                {
                    //Devolve lista normal,pois não caiu no primeiro "if"
                    salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).OrderByDescending(u => u.DataCriacao).ToListAsync();


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
                    //Se o campo for data/anfs,vamos ignorar por agora
                    if (!val.Key.ToLower().Contains("dat") && !val.Key.ToLower().Contains("anfs"))
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
                        if (val.Key.ToLower().Contains("reserva"))
                        {
                            query +="(\""+ string.Join(",", num) + "\").Contains(SalaId.toString())";
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
                    if (hasAnfs)
                    {
                      
                            salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(queryAnf, ListaAnfs).OrderBy(orderby + " " + orderType).ToListAsync();

                        

                    }
                    else
                    {
                       
                        salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).OrderBy(orderby + " " + orderType).ToListAsync();
                            
                      }
                      


                }
                else
                {
                    if (orderby == "")
                    {
                        //Se orderBy tiver vazio,e a query não estiver vazia,filtra pelos campos e usa a order por defeito
                        if (hasAnfs)
                        {
                           
                            
                            salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(query).Where(queryAnf, ListaAnfs).ToListAsync();

                            


                        }
                        else
                        {
                          

                                salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(query).OrderByDescending(u => u.DataCriacao).ToListAsync();

                            

                        }

                    }
                    else
                    {
                        if (hasAnfs)
                        {
                           
                                salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(query).Where(queryAnf, ListaAnfs).OrderBy(orderby + " " + orderType).ToListAsync();

                            

                        }
                        else
                        {
                                                           salas = await _context.Salas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Where(query).OrderBy(orderby + " " + orderType).ToListAsync();

                            

                        }


                    }


                }


            }

            //Pode acontecer o modelo dar return sem nenhum anfitrião,isto está aqui para esses casos(Idealmente,não deveria acontecer.Infelizmente não vivemos num mundo ideal)
            if (!salas.Select(a => a.ListaAnfitrioes).Any())
            {
                TempData["Anfs"] = await _context.Anfitrioes.Where(u => !u.Deleted).Include(a => a.ListaReservas).Where(a => a.ListaReservas.Any()).ToListAsync();
            }

            //Ussado para fazer a border do anfitrião
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            //Volta a enviar os valores do dicionário de filtros de modo a manter o estado em que estava
            TempData["dicVals"] = dictVals;
            //Mesma razão,voltar a selecionar todos os anfitriões filtrados
            TempData["ListaAnfs"] = ListaAnfs;
            //Usado para fazer a legenda
            TempData["SalasCor"] = dic;
            //Usado para manter o foco no último campo em que estava a ser escrito
            TempData["Last"] = last;

            ViewBag.SalasCount = dicSalasCount;
            //E finalmente,dá return com o novo modelo "filtrado"

            return PartialView("_partialSalasTabela", salas);

        }






        /// <summary>
        /// GET GERIR/SALAS/DETAILS/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View detlhes com modelo sala</returns>
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas.Include(s=> s.ListaAnfitrioes)
                .FirstOrDefaultAsync(m => m.SalaId == id);
            if (salas == null)
            {
                return NotFound();
            }

            return View(salas);
        }

        /// <summary>
        /// GET GERIR/SALAS/CREATE - Cria nova sala
        /// </summary>
        /// 
        /// <returns>Retorna View criação de sala</returns>
        public async Task<IActionResult> Create()
        {

            var ViewModel = new AnfitriaoSalaViewModel(new Salas(), new HashSet<string>());

            var userList = await _userManager.GetUsersInRoleAsync("Anfitriao");
            HashSet<Utilizadores> anfitrioes = userList.ToList().Where(a => a.Deleted == false).ToHashSet();
            ViewBag.SelectionIdList = anfitrioes;
            return View(ViewModel);
        }

        /// <summary>
        /// POST GERIR/SALAS/CREATE - Cria nova sala
        /// </summary>
        /// <param name="salasAnf">Viewmodel com sala e lista de anfitriões</param>
        /// <returns>Retorna para o index salas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sala,ListaAnfitrioes")] AnfitriaoSalaViewModel salasAnf)
        {
            if (ModelState.IsValid)
            {
                var userList = await _userManager.GetUsersInRoleAsync("Anfitriao");
                HashSet<Utilizadores> anfitrioes = userList.ToList().Where(a => a.Deleted == false).ToHashSet();
                ViewBag.SelectionIdList = anfitrioes;
                var salaExiste = _context.Salas.FirstOrDefault(m => m.Numero == salasAnf.Sala.Numero);
                if (salaExiste != null)
                {
                    ViewBag.SalaExistente = salaExiste.Numero;

                    return View(salasAnf);
                }
                Salas sala = salasAnf.Sala;
                var anfs = new HashSet<Anfitrioes>();
                if (salasAnf.ListaAnfitrioes.Any())
                {

                    foreach (var item in salasAnf.ListaAnfitrioes)
                    {
                        var anf = await _context.Anfitrioes.FirstOrDefaultAsync(m => m.Id == item);
                        if (anf != null)
                        {
                            anfs.Add(anf);
                        }
                    }



                }
                sala.ListaAnfitrioes = anfs;

                sala.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sala.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(sala);
                await _context.SaveChangesAsync();
                TempData["NumeroSalaCriada"] = sala.Numero;
                return RedirectToAction(nameof(Index));
            }
            return View(salasAnf);
        }

        /// <summary>
        /// GET GERIR/SALAS/EDIT/{id}
        /// </summary>
        /// <param name="id">ID da sala para editar</param>
        /// <returns>View edit</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salas = await _context.Salas.Include(s => s.ListaAnfitrioes).Where(s => s.SalaId == id).FirstOrDefaultAsync();
            if (salas == null)
            {
                return NotFound();
            }

            List<string>anfs = salas.ListaAnfitrioes.Select(a => a.Id).ToList();
            var ViewModel = new AnfitriaoSalaViewModel(salas, anfs);

            var userList = await _userManager.GetUsersInRoleAsync("Anfitriao");
            HashSet<Utilizadores> anfitrioes = userList.ToList().Where(a => a.Deleted == false).ToHashSet();
            ViewBag.SelectionIdList = anfitrioes;
            return View(ViewModel);
          
        }
        /// <summary>
        /// POST GERIR/SALAS/EDIT/{id} ?=
        /// </summary>
        /// <param name="id">ID da sala que está a ser editada</param>
        /// <param name="salaVM">Viewmodel sala com anfitrioões</param>
        /// <param name="numeroAntigo"> número antigo da sala,para ser possível repô-lo caso dê erro</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sala,ListaAnfitrioes")] AnfitriaoSalaViewModel salaVM,int numeroAntigo)
        {
            if (id != salaVM.Sala.SalaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userList = await _userManager.GetUsersInRoleAsync("Anfitriao");
                HashSet<Utilizadores> anfitrioes = userList.ToList().Where(a => a.Deleted == false).ToHashSet();
                ViewBag.SelectionIdList = anfitrioes;
                Salas? s = null;
                var sala = _context.Salas.FirstOrDefault(m => m.Numero == salaVM.Sala.Numero && m.SalaId != salaVM.Sala.SalaId);
                if (sala != null) {
                    //Se número da sala já existir
                    ViewBag.SalaExistente = sala.Numero;
                    salaVM.Sala.Numero = numeroAntigo;
                    ViewBag.SalaAntiga = salaVM.Sala.Numero;
                    return View(salaVM);

                }
                try
                {
                     s =await _context.Salas.Include(s=> s.ListaAnfitrioes).Where(s=> s.SalaId == salaVM.Sala.SalaId).FirstOrDefaultAsync();
                    s.Numero = salaVM.Sala.Numero;
                    s.Area = salaVM.Sala.Area;
                    List<Anfitrioes> listaUsers= await _context.Anfitrioes.Where(m => salaVM.ListaAnfitrioes.Contains(m.Id)).ToListAsync();
                    s.ListaAnfitrioes = listaUsers;
                    _context.Update(s);
                    _context.Entry(s).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(s).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(s).Property(t => t.CriadoPorUsername).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalasExists(salaVM.Sala.SalaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.TemaAntigo =s.Numero ;
                ViewBag.ShowAlert = true;

                return View(salaVM);
            }
            return View(salaVM);
        }

  
       /// <summary>
       /// POST /GERIR/SALAS/DELETE
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salas = await _context.Salas.FindAsync(id);
            if (salas != null)
            {


                var tema = await _context.Temas.Where(t => t.SalaID == salas.SalaId).FirstOrDefaultAsync();
                if (tema != null) {
                    TempData["ErroEliminar"] = tema.Nome;
                    return RedirectToAction(nameof(Index));
                }

                salas.Deleted = true;

                 _context.Update(salas);
                await _context.SaveChangesAsync();

                TempData["SalaApagada"] = salas.Numero;

                return RedirectToAction(nameof(Index));

           
        }
            return NotFound();
        }

        private bool SalasExists(int id)
        {
            return _context.Salas.Any(e => e.SalaId == id);
        }
    }
}
