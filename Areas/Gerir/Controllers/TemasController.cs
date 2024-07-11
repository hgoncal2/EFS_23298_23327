﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFS_23298_23327.Data;
using EFS_23298_23327.Models;
using System.Numerics;
using System.Security.Claims;
using EFS_23298_23327.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace EFS_23298_23327.Areas.Gerir.Controllers
{
    /// <summary>
    /// Apenas Utilizadores com a role Admin OU Anfitriao podem aceder
    /// </summary>
    [CustomAuthorize(Roles = "Admin,Anfitriao")]

    //Especificar que está dentro da área "Gerir"
    [Area("Gerir")]
    public class TemasController : Controller
    {
        /// <summary>
        /// Contexto da Base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Instância do ambiente da aplicação web
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// Provém da biblioteca SignalR,usado para mandar mensagens websocket
        /// </summary>
        private readonly IHubContext<ClassHub> _progressHubContext;


        public TemasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHubContext<ClassHub> hubContext) {
            _progressHubContext = hubContext;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// // GET: /Gerir/Temas
        /// </summary>
        /// <returns>View com lista de temas</returns>
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala).Where(s => s.Deleted != true).Where(m => m.Deleted != true).OrderByDescending(m => m.DataCriacao);

            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// // GET: /GerirTemas/Details/5
        /// </summary>
        /// <param name="id">ID do tema</param>
        /// <returns>View com tema correspondente ao ID,ou 404 se não for encontrado nenhum.</returns>
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala)
.FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null) {
                return NotFound();
            }

            return View(temas);
        }

        /// <summary>
        /// GET: Gerir/Temas/Create
        /// </summary>
        /// <returns>View("Create")</returns>
        public async Task<ActionResult> Create() {
            //Remove da lista salas que estão a ser usadas por outro tema
            List<Salas> a = await _context.Temas.Where(s => !s.Deleted).Select(s => s.Sala).ToListAsync();
            List<Salas> s = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s.Except(a).ToList();
            return View();
        }

        /// <summary>
        /// POST: Gerior/Temas/Create
        /// </summary>
        /// <param name="temas"></param>
        /// <returns>View("Create",Objeto Tema)</returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID,PrecoStr,Icone,AnunciarTema")] Temas temas) {
            //Lista de ficheiros(imagens,idealmente) que foram carregadas/uploaded no form
            var Imagens = HttpContext.Request.Form.Files;

            //Ir buscar já a lista de salas e atribuí-la à viewbag correspondente, caso aconteça algum erro posteriormente,ou se o modelo não for válido
            //De momento,apesar da relação Modelo-Sala não ser 1-1,estamos a "GUIAR" o utilizador a apenas atribuir uma sala a um tema
            //Lista de salas que têm uma tema associado
            List<Salas> a = await _context.Temas.Where(s => !s.Deleted).Select(s => s.Sala).Where(s => !s.Deleted).ToListAsync();
            //Lista de todas as salas
            List<Salas> s = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            //De todas as salas,vamos remover aquelas que já têm tema associado
            ViewBag.s = s.Except(a).ToList();
            //Se o modelo for válido
            if (ModelState.IsValid) {
                //Converte String preço para decimal
                temas.Preco = Convert.ToDecimal(temas.PrecoStr.Replace('.', ','));
                //Inicializar variáveis
                var msgErro = "";
                var erro = false;
                //Ver se existe tema com o mesmo nome
                var tema = _context.Temas.FirstOrDefault(m => (m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower()) && !m.Deleted);
                //Se existir,vamos avisar o utilizador que já existe um tema com esse nome
                if (tema != null) {
                    ViewBag.TemaExistente = tema.Nome;

                    return View(temas);
                }

                //Temos também que verificar se maxPessoas>minPessoas,adicionar mensagem de erro ao modelstate,que irá notificar o utilizador do seu erro
                if (temas.MaxPessoas <= temas.MinPessoas) {
                    msgErro = "Erro!Máximo de pessoas tem que ser maior que o mínimo de pessoas";
                    ModelState.AddModelError("MaxPessoas", msgErro);
                    erro = true;

                }
                if (temas.MinPessoas >= temas.MaxPessoas) {
                    msgErro = "Erro!Mínimo de pessoas tem que ser menor que o máximo de pessoas";

                    ModelState.AddModelError("MinPessoas", msgErro);
                    erro = true;

                }
                //Inicializa a variável que irá ditar se foram efetivamente imagens carregadas
                var hasImagem = false;
                string nomeImagem = "";
                //Este  map vai dar jeito mais à frente,quando estivermos efetivamente a guardar o ficheiro no servidor
                Dictionary<Fotos, IFormFile> mapFotos = new Dictionary<Fotos, IFormFile>();

                //Se houver ficheiros(imagens,idealmente)
                if (Imagens != null) {
                    //Por cada Imagem na lista de imagens
                    foreach (var Imagem in Imagens) {
                        //Em principio isto nunca deve acontecer,mas null checks nunca são demais
                        if (Imagem != null) {
                            //Se não for imagem
                            if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg")) {
                                msgErro = "Erro!Ficheiro de imagem tem que ser png ou jpeg!";
                                ModelState.AddModelError("Foto", msgErro);
                                erro = true;
                            } else { //Se houver erro,vamos dar return no proximo bloco,excusamos de fazer isto tudo
                                if (!erro) {
                                    //Cria um novo global unique ID,irá ser usado para dar nome ao ficheiro
                                    Guid g = Guid.NewGuid();
                                    nomeImagem = g.ToString();
                                    //Adiciona extensão da imagem ao guid
                                    string extensaoImagem = Path.GetExtension(Imagem.FileName).ToLowerInvariant();
                                    nomeImagem += extensaoImagem;
                                    //Cria novo objeto Fotos,inicializando-o com o nome criado em cima
                                    Fotos f = new Fotos(nomeImagem);
                                    //Fazer a ligação entre foto e tema
                                    f.TemaId = temas.TemaId;
                                    f.Tema = temas;
                                    //Definir quem criou a foto(username e Oid)
                                    //Achei por bem estar a guardar os dois valores(isto é feito para praticamente todas as classes na BD)
                                    //Pois nem sempre precisamos do Objeto Utilizador em si,apenas o username basta para mostrar na camada de aplicação
                                    //Evitamos uma query adicional à base de dados.O Oid serve para se for preciso o objeto utilizador,usarmos uma coluna que está indexada
                                    f.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    f.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                                    //Adiciona foto à lista de fotos do tema
                                    temas.ListaFotos.Add(f);
                                    //Adiciona nome de ficheiro e ficheiro ao map criado previamente
                                    mapFotos.Add(f, Imagem);
                                    hasImagem = true;
                                }


                            }
                        }
                    }

                }


                //Se houver algum erro,devolve a view
                if (erro) {
                    return View(temas);
                }
                //https://stackoverflow.com/a/71882405
                temas.CriadoPorOid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                temas.CriadoPorUsername = User.FindFirstValue(ClaimTypes.Name);
                //Adiciona tema à base de dados,e guarda o contexto desta
                _context.Add(temas);
                await _context.SaveChangesAsync();
                //Se tem Imagem
                if (hasImagem) {
                    //Path root da aplicação web(ex "http://localhost.com/")
                    string localizacaoImagem = _webHostEnvironment.WebRootPath;
                    //Adiciona "Imagens" ao Path root(ex "http://localhost.com/Imagens")
                    localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                    //Se o diretório "Imagens" não existir,cria-o
                    if (!Directory.Exists(localizacaoImagem)) {
                        Directory.CreateDirectory(localizacaoImagem);
                    }
                    //Por cada Imagem(Nome e ficheiro)
                    foreach (KeyValuePair<Fotos, IFormFile> i in mapFotos) {
                        //Guarda a imagem fisicamente no servidor,no diretório definido em cima
                        localizacaoImagem = Path.Combine(localizacaoImagem, i.Key.Nome);
                        using var stream = new FileStream(localizacaoImagem, FileMode.Create);
                        await i.Value.CopyToAsync(stream);
                        //Volta a dar reset às variáveis para serem usadas na proxima iteração
                        localizacaoImagem = _webHostEnvironment.WebRootPath;
                        localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                    }
                }
                //Se o tema tiver sala associado e tiver sido marcado a checkbox "Anunciar Tema",é mandado uma mensagem por WS a todos os utilizadores(anonymous incluidos)
                if (temas.SalaID == null && temas.AnunciarTema) {
                    TempData["MensagemErro"] = "Não foi possível anunciar tema porque <strong class=''>nenhuma sala foi associada</strong>";
                } else {
                    if (temas.SalaID != null && temas.AnunciarTema) {
                        await _progressHubContext.Clients.Group("Clientes").SendAsync("tema", "system", temas.SalaID + "," + DifficultiesValue.GetDifficultyColor((int)temas.Dificuldade) + ","+ "Novo tema disponível!");

                    }
                }
                
                //Guarda nome do tema criado no dicionário TempData,usado pela view para mostrar uma mensagem ao utilizador a dizer que o tema "x" foi criado com sucesso
                TempData["NomeTemaCriado"] = temas.Nome;
                //Redireciona para o Index,daí não podermos usar o objeto ViewBag na instrução anterior
                return RedirectToAction(nameof(Index));
            }
            //Se o modelo não for válido
            @ViewBag.ErroTema = "Erro ao criar tema!";
            return View(temas);
        }

        /// <summary>
        /// GET: /Gerir/Temas/Edit/{id}
        /// </summary>
        /// <param name="id">ID do tema a ser editado</param>
        /// <returns>View com o tema correspondente,ou 404 se não for encontrado</returns>
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos.Where(f => f.Deleted != true)).Include(s => s.Sala).Where(s => !s.Deleted).FirstOrDefaultAsync(m => m.TemaId == id);

            if (temas == null) {
                return NotFound();
            }
            List<Salas> a = await _context.Temas.Where(s => !s.Deleted && s.TemaId != id).Select(s => s.Sala).ToListAsync();
            List<Salas> s = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s.Except(a).ToList();
            ViewBag.TemaAntigo = temas.Nome;

            temas.PrecoStr = temas.Preco.ToString();

            return View(temas);
        }

        // POST: /Gerir/Temas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaId,Nome,Descricao,TempoEstimado,MinPessoas,MaxPessoas,Dificuldade,SalaID,Icone,PrecoStr,AnunciarTema")] Temas temas, string nomeAntigo) {

            //É improvável que aconteça,mas o temaID pode ter sido adulterado
            if (id != temas.TemaId) {
                return NotFound();
            }
            //Exclui tema atual da lista de salas a usadas por temas
            List<Salas> a = await _context.Temas.Where(s => !s.Deleted && s.TemaId != temas.TemaId).Select(s => s.Sala).Where(s => !s.Deleted && s.SalaId != temas.SalaID).ToListAsync();
            List<Salas> s = await _context.Salas.Where(s => s.Deleted == false).ToListAsync();
            ViewBag.s = s.Except(a).ToList();
            //Remove Nome antigo(do tema) do modelstate.Isto foi adicionado pois por vezes o modelstate não era válido devido ao nomeAntigo
            //Visto que não faz parte do modelo "Temas"
            if (ModelState.ContainsKey("nomeAntigo")) {
                ModelState.Remove("nomeAntigo");
            }

            var Imagens = HttpContext.Request.Form.Files;
            if (ModelState.IsValid) {
                temas.Preco = Convert.ToDecimal(temas.PrecoStr.Replace('.', ','));

                //Procurar por Nomes iguais,excluir tema atual(obviamente) e temas apagados
                var tema = _context.Temas.FirstOrDefault(m => m.Nome.Trim().ToLower() == temas.Nome.Trim().ToLower() && m.TemaId != temas.TemaId && !m.Deleted);
                if (tema != null) {
                    //Avisa o utilizador que tema "x" já existe
                    ViewBag.TemaExistente = tema.Nome;
                    //Volta a definir o nome do tema como sendo o nome antigo,caso contrário,apesar de o novo nome "não ser válido",teríamos que fazer
                    //uma query à base de dados para ir buscar o estado original do tema
                    temas.Nome = nomeAntigo;
                    ViewBag.TemaAntigo = temas.Nome;
                    return View(temas);

                }
                var msgErro = "";
                var erro = false;

                if (temas.MaxPessoas <= temas.MinPessoas) {
                    msgErro = "Erro!Máximo de pessoas tem que ser maior que o mínimo de pessoas";
                    ModelState.AddModelError("MaxPessoas", msgErro);
                    erro = true;

                }
                if (temas.MinPessoas >= temas.MaxPessoas) {
                    msgErro = "Erro!Mínimo de pessoas tem que ser menor que o máximo de pessoas";

                    ModelState.AddModelError("MinPessoas", msgErro);
                    erro = true;

                }

                var hasImagem = false;
                string nomeImagem = "";
                Dictionary<Fotos, IFormFile> mapFotos = new Dictionary<Fotos, IFormFile>();

                if (Imagens != null) {
                    foreach (var Imagem in Imagens) {
                        if (Imagem != null) {
                            if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg")) {
                                msgErro = "Erro!Ficheiro de imagem tem que ser png ou jpeg!";
                                ModelState.AddModelError("Foto", msgErro);
                                erro = true;
                            } else {
                                if (!erro) {
                                    Guid g = Guid.NewGuid();
                                    nomeImagem = g.ToString();
                                    string extensaoImagem = Path.GetExtension(Imagem.FileName).ToLowerInvariant();
                                    nomeImagem += extensaoImagem;
                                    Fotos f = new Fotos(nomeImagem);

                                    f.TemaId = temas.TemaId;
                                    f.Tema = temas;

                                    temas.ListaFotos.Add(f);
                                    mapFotos.Add(f, Imagem);
                                    hasImagem = true;
                                }



                            }
                        }
                    }

                }



                if (erro) {

                    return View(temas);
                }
                try {

                    _context.Update(temas);
                    //Exclui estas propriedades de serem modificadas
                    //O que estava a acontecer é que a data em que o tema foi editado esta a dar overwrite à data em que foi criado
                    _context.Entry(temas).Property(t => t.DataCriacao).IsModified = false;
                    _context.Entry(temas).Property(t => t.CriadoPorOid).IsModified = false;
                    _context.Entry(temas).Property(t => t.CriadoPorUsername).IsModified = false;

                    await _context.SaveChangesAsync();
                    if (hasImagem) {
                        string localizacaoImagem = _webHostEnvironment.WebRootPath;
                        localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                        if (!Directory.Exists(localizacaoImagem)) {
                            Directory.CreateDirectory(localizacaoImagem);
                        }
                        foreach (KeyValuePair<Fotos, IFormFile> i in mapFotos) {

                            localizacaoImagem = Path.Combine(localizacaoImagem, i.Key.Nome);
                            using var stream = new FileStream(
                          localizacaoImagem, FileMode.Create
                          );
                            await i.Value.CopyToAsync(stream);
                            localizacaoImagem = _webHostEnvironment.WebRootPath;
                            localizacaoImagem = Path.Combine(localizacaoImagem, "Imagens");
                        }



                    }
                } catch (DbUpdateConcurrencyException) {
                    if (!TemasExists(temas.TemaId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                ViewBag.TemaAntigo = temas.Nome;
                ViewBag.ShowAlert = true;
                if (temas.SalaID == null && temas.AnunciarTema) {
                    ViewBag.MensagemErro = "Não foi possível anunciar tema porque <strong class=''>nenhuma sala foi associada</strong>";
                } else {
                    if (temas.SalaID != null && temas.AnunciarTema) {
                        await _progressHubContext.Clients.Group("Clientes").SendAsync("tema", "system", temas.SalaID + "," + DifficultiesValue.GetDifficultyColor((int)temas.Dificuldade) + "," + "Tema Atualizado!");

                    }
                }

                tema = await _context.Temas.Include(f => f.ListaFotos.Where(f => !f.Deleted)).Where(t => t.TemaId == temas.TemaId).FirstOrDefaultAsync();



                return View(tema);
            }

            return View(temas);
        }
        //função criada por defeito,não está a ser usada
        // GET: Temas/Delete/5
        /*
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temas = await _context.Temas.Include(m => m.ListaFotos).FirstOrDefaultAsync(m => m.TemaId == id);
            if (temas == null)
            {
                return NotFound();
            }

            return View(temas);
        }
        */

        /// <summary>
        /// POST: /Gerir/Temas/Delete/{id}
        /// </summary>
        /// <param name="id">ID do tema a apagar</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            string? tema = null;

            var temas = await _context.Temas.FindAsync(id);

            if (temas != null) {

                tema = temas.Nome;
                temas.Deleted = true;
                temas.SalaID = null;
                var fotos = await _context.Fotos.Where(f => f.TemaId == temas.TemaId).ToListAsync();
                foreach (var item in fotos) {
                    item.Deleted = true;

                }

            } else {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            if (tema != null) {
                TempData["TemaApagado"] = tema;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Temas/EliminaFoto/5
        [HttpPost, ActionName("EliminaFoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminaFoto(int id) {


            var foto = await _context.Fotos.FindAsync(id);
            Temas tema;
            if (foto != null) {


                foto.Deleted = true;
                tema = await _context.Temas.FirstOrDefaultAsync(f => f.TemaId == foto.TemaId);
                if (tema != null) {
                    tema.ListaFotos.FirstOrDefault(f => f.FotoId == id).Deleted = true;
                    await _context.SaveChangesAsync();

                    TempData["FotoEliminada"] = true;
                    return RedirectToAction(nameof(Edit), new { id = tema.TemaId });
                }

            } else {
                return NotFound();
            }

            return NotFound();
        }

        [HttpPost, ActionName("EliminaFotos")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminaFotos(int id) {


            var tema = await _context.Temas.Include(t => t.ListaFotos).Where(t => t.TemaId == id).FirstOrDefaultAsync();
           
            if (tema != null && tema.ListaFotos.Any()) {

                foreach(var f in tema.ListaFotos) {
                    f.Deleted = true;
                }

                _context.UpdateRange(tema.ListaFotos);
                await _context.SaveChangesAsync();

                TempData["FotoEliminada"] = true;
                return RedirectToAction(nameof(Edit), new { id = tema.TemaId });

            } else {
                return NotFound();
            }

            return NotFound();
        }



        private bool TemasExists(int id) {
            return _context.Temas.Any(e => e.TemaId == id);
        }
    }
}
