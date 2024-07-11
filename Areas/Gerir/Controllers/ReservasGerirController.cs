using EFS_23298_23327.Areas.Gerir.Controllers;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    [CustomAuthorize(Roles ="Admin,Anfitriao")]
    [Area("Gerir")]
    public class ReservasGerirController : Controller
    {
        private readonly UserManager<Utilizadores> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public ReservasGerirController(ApplicationDbContext context, UserManager<Utilizadores> userManager,
     RoleManager<IdentityRole> roleManager) {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            //todos os utilizadores não apagados
            
            var reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a=>a.ListaAnfitrioes).Include(c=>c.Cliente).Include(s=>s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();
            var ud = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            var u = await _context.Anfitrioes.Where(u => u.Id == ud).Include(u => u.userPrefsAn).ThenInclude(u => u.Cores).FirstOrDefaultAsync();
            if (u != null) {
                if (u.userPrefsAn != null) {
                    Dictionary<int, string> coresSalas = new Dictionary<int, string>();

                    foreach (var i in u.userPrefsAn.Cores) {
                        var sNum=reservas.Where(s => s.SalaId == i.Key).Select(s => s.Sala.Numero).First();


                        coresSalas.Add(sNum, i.Value);
                    }
                    TempData["SalasCor"] = coresSalas;


                }
            }
          
            

            return View(reservas);
        }

        /// <summary>
        /// Protótipo de filtro por datas,antes de decidir que ia meter filtros nos campos todos(infelizmente)
        /// </summary>
        /// <param name="dictVals"></param>
        /// <param name="dic"></param>
        /// <param name="last"></param>
        /// <param name="anfs"></param>
        /// <returns>Partial view tabela reservas</returns>
        /*
        [HttpGet]
        public async Task<IActionResult> OrdenaDataDesc(string vari,string type, Dictionary<int, string> dic) {



            var reservas = new List<Reservas>();
            if (vari == "dataI") {

                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.ReservaDate).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.ReservaDate).ToListAsync();

                }
            }
            if(vari == "dataF") {
                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.ReservaEndDate).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.ReservaEndDate).ToListAsync();

                }
            }
            if (vari == "dataC") {
                if (type == "asc") {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(u => u.DataCriacao).ToListAsync();

                } else {
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();

                }
            }



            return PartialView("_partialReservasTabela",reservas);

        }
        */


        /// <summary>
        /// Função responsável por aplicar filtros à tabela reservas,do index.
        /// </summary>
        /// <param name="dictVals">Dicionário que contém campos a filtrar e respetivos valores</param>
        /// <param name="dic">Dicionário com as cores de preferência do utilizador</param>
        /// <param name="last">ID do ultimo campo "filtrado".Usado para voltar a aplicar foco no input.Não está a ser usado nas dropdowns nem nas datas(o valor está a ser passado,mas não está a ser aplicado foco)</param>
        /// <param name="anfs">Lista de anfitriões do filtro."Lista" em String,que depois está a ser separada por vírgulas(",")</param>
        /// <returns>Partial view tabela reservas</returns>

        [HttpGet]
        public async Task<IActionResult> Filter(Dictionary<string, string> dictVals, Dictionary<int, string> dic,String last, String anfs) {

            //Se tem filtro de Anfitriões
            var hasAnfs = false;
            var reservas = new List<Reservas>();
            //Inicializa query varia Anfitriões
            var queryAnf = "";
            var ListaAnfs = new List<string>();
            if (anfs != "" && anfs!=null) {

                hasAnfs = true;
                ListaAnfs = anfs.Split(",").ToList();
                //Interseção da lista de anfitriões da reserva com a lista de Ids do filtro.Se o tamanho da interseção das duas listas for igual ao tamanho da lista do filtro,devolve true(todos os elementos do filtro pertencem à lista de anfitriões)
                 queryAnf = "ListaAnfitrioes.Select(a => a.Id).Intersect(@0).Count() == @0.Count()";
               // reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where("ListaAnfitrioes.Any(a => @0.Contains(a.Id))", ListaAnfs).ToListAsync();




               
            }
            //O dicionário contém "dic" se não houver valores filtrados(excepto anfitriões,não arranjei forma de passar o array no dicionário).Se não houver filtros de texto e não houver filtro de Anfitriões,devolve lista normal

            if (dictVals.ContainsKey("dic") && !dictVals.ContainsKey("anfs")) {
                reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();

                //Se só houver filtro de anfitriões
            }if (dictVals.ContainsKey("dic") && dictVals.ContainsKey("anfs")) {
                //Pode acontecer ter a key "anfs" mas o Value estar vazio.Vamos certificar-nos que apanhamos esses casos
                if (hasAnfs) {
                    //Devolve lista com filtro Anfitriões
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(queryAnf, ListaAnfs).ToListAsync();

                } else {
                    //Devolve lista normal,pois não caiu no primeiro "if"
                    reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderByDescending(u => u.DataCriacao).ToListAsync();


                }
                //Se houver "mais" filtros na tabela(qualquer um excepto anfitrião
            } else {
                //Inicializa variáveis a string vazia.Vão ser necessárias para saber que tipo de filtro precisamos(importante para os OrderBys)
                var query = "";
                var orderby = "";
                var orderType = "";
                //Por cada valor no dicionário(Campo,Valor a filtrar)
                foreach (var val in dictVals) {
                    //Se o campo for data/anfs,vamos ignorar por agora
                    if (!val.Key.ToLower().Contains("dat") && !val.Key.ToLower().Contains("anfs")) {
                        //Se for um campo boolean,a expressão vai ser diferente,temos que ter isso em conta
                            if (val.Value.ToLower().Contains("true") || val.Value.ToLower().Contains("false")) {
                                query += @val.Key.Replace("_", ".") + "==" + val.Value;
                            //Guarda id do campo que foi submetido em ultimo.Esta linha de código é extremamente importante devido ao listener que está na dropdown(on change)que ative cada vez que se dá render da partial view
                            //Se este tempdata não tiver nulo,significa que já foi filtrado,e não preciisamos de chamar esta função outra vez.Esta verificação está a ser feita no lado do cliente
                            //Sem esta linha de código,vai entra em loop infinito!
                                TempData["lastVal"] = val.Value;
                            } else {
                            //Se for campo de texto normal(mesmo que seja int,estou a converter para string para fazer o includes)
                                query += @val.Key.Replace("_", ".") + ".toString().Contains(\"" + val.Value + "\")";
                            }
                            //Se não for o ultimo valor do dicionário,dá append do "And" para continuar a preencher os campos da query
                        if (!val.Equals(dictVals.Last())) {
                            query += " && ";
                        }
                    } else {
                        //Se for data,vamos guardar que tipo de data é(Data inicial reserva,final,ou data criada)
                        orderby = val.Key;
                        //Tal como o tipo(desc,asc)
                        orderType = val.Value;
                    }

                }
                //Se a string da query acabar com os caracteres "&&",vamos removê-los
                if (query.Trim().EndsWith("&&")) {
                    query = query.Substring(0, query.LastIndexOf("&") - 1);

                }
                if (query == "") {
                    //Se query estiver vazia,quer dizer que so foi feito um filtro de datas
                    if (hasAnfs) {
                        //Ter em conta se foi efetuaod filtro nos anfitriões
                        reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(queryAnf, ListaAnfs).OrderBy(orderby + " " + orderType).ToListAsync();

                    } else {
                        //Se não houver filtro de anfitriões
                        reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).OrderBy(orderby + " " + orderType).ToListAsync();
                    }


                } else {
                    if (orderby == "") {
                        //Se orderBy tiver vazio,e a query não estiver vazia,filtra pelos campos e usa a order por defeito
                        if (hasAnfs) {
                            reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).Where(queryAnf, ListaAnfs).ToListAsync();

                        } else {
                            reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).OrderByDescending(u=>u.DataCriacao).ToListAsync();

                        }

                    } else {
                        if (hasAnfs) {
                            reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).Where(queryAnf, ListaAnfs).OrderBy(orderby + " " + orderType).ToListAsync();

                        } else {
                            reservas = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala).Where(query).OrderBy(orderby + " " + orderType).ToListAsync();

                        }


                    }


                }


            }

            //Pode acontecer o modelo dar return sem nenhum anfitrião,isto está aqui para esses casos(Idealmente,não deveria acontecer.Infelizmente não vivemos num mundo ideal)
            if (!reservas.Select(a => a.ListaAnfitrioes).Any()) {
                TempData["Anfs"] = await _context.Anfitrioes.Where(u=> !u.Deleted).Include(a=>a.ListaReservas).Where(a=>a.ListaReservas.Any()).ToListAsync();
            }

            //Ussado para fazer a border do anfitrião
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            //Volta a enviar os valores do dicionário de filtros de modo a manter o estado em que estava
            TempData["dicVals"] = dictVals;
            //Mesma razão,voltar a selecionar todos os anfitriões filtrados
            TempData["ListaAnfs"] = ListaAnfs;
            //Usado para fazer a legenda
            TempData["SalasCor"] =dic;
            //Usado para manter o foco no último campo em que estava a ser escrito
            TempData["Last"] = last;
           

            //E finalmente,dá return com o novo modelo "filtrado"

            return PartialView("_partialReservasTabela", reservas);

        }
       

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            TempData["UserLogado"] = User.FindFirstValue(ClaimTypes.Name);

            var reserva = await _context.Reservas.Where(u => u.Deleted != true).Include(a => a.ListaAnfitrioes).Include(c => c.Cliente).Include(s => s.Sala)
.FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reserva == null) {
                return NotFound();
            }
            TempData["Cancelavel"] = DateTime.Now.AddHours(48) < reserva.ReservaDate;

            return View(reserva);
        }
    }
}


