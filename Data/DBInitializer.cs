namespace EFS_23298_23327.Data
{
    using EFS_23298_23327.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using System.ComponentModel;

    namespace Aulas.Data
    {

        internal class DbInitializer
        {

            internal static async void Initialize(ApplicationDbContext dbContext) {

                /*
                 * https://stackoverflow.com/questions/70581816/how-to-seed-data-in-net-core-6-with-entity-framework
                 * 
                 * https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0#initialize-db-with-test-data
                 * https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/data/ef-mvc/intro/samples/5cu/Program.cs
                 * https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0300
                 */


                ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
                dbContext.Database.EnsureCreated();

                // var auxiliar
                bool haAdicao = false;



                // Se não houver Admins, cria-os
                var users = Array.Empty<Utilizadores>();
                var hasher = new PasswordHasher<Utilizadores>();
                if (!dbContext.Utilizadores.Any()) {
                    users = [
                       new Utilizadores{UserName="admin",NormalizedUserName="ADMIN",Email="teste@teste.com",PasswordHash=hasher.HashPassword(null,"teste123")}

                    ];
                    await dbContext.Utilizadores.AddRangeAsync(users);


                    haAdicao = true;
                }
                var anfs = Array.Empty<Anfitrioes>();
                var hasherAnf = new PasswordHasher<Anfitrioes>();
                if (!dbContext.Anfitrioes.Any()) {
                    anfs = [
                       new Anfitrioes{UserName="jonSilva",NormalizedUserName="JONSILVA",Email="testejon@teste.com",PrimeiroNome="Jon",UltimoNome="Silva",PasswordHash=hasherAnf.HashPassword(null,"teste123")},
                       new Anfitrioes{UserName="jorgeMiguel",NormalizedUserName="JORGEMIGUEL",Email="testeJorge@teste.com",PrimeiroNome="Jorge",UltimoNome="Miguel",PasswordHash=hasherAnf.HashPassword(null,"teste123")},
                       new Anfitrioes{UserName="jorginaMiguelina",NormalizedUserName="JORGINAMIGUELINA",Email="testejorgina@teste.com",PrimeiroNome="Jorgina",UltimoNome="Miguelina",PasswordHash=hasherAnf.HashPassword(null,"teste123")}


                    ];
                    await dbContext.Anfitrioes.AddRangeAsync(anfs);
                    haAdicao = true;
                }
                var temas = Array.Empty<Temas>();
                if (!dbContext.Temas.Any()) {
                    temas = [
                       new Temas{Nome="Laboratório Secreto",Descricao="Você e seu time são um grupo de cientistas presos em um laboratório secreto. Uma experiência deu errado, e vocês têm uma hora para encontrar a cura para um vírus mortal que está prestes a se espalhar pelo mundo. Vocês precisam decifrar códigos, resolver enigmas científicos e acessar áreas restritas do laboratório para salvar a humanidade",
                           MinPessoas=10,MaxPessoas=15,TempoEstimado=60,DataCriacao=DateTime.Now,CriadoPorUsername="System",Dificuldade=Enum.Dificuldade.Sherlock,Preco=Convert.ToDecimal("2,5")},
                       new Temas{Nome="Mansão Assombrada",Descricao="Vocês estão presos em uma antiga mansão que dizem ser assombrada por espíritos vingativos. Para escapar, vocês devem descobrir o segredo da família que morava lá e libertar suas almas. A mansão está cheia de enigmas misteriosos, passagens secretas e fenômenos paranormais que testarão sua coragem e inteligência"
                       ,MinPessoas=5,MaxPessoas=17,TempoEstimado=120,DataCriacao=DateTime.Now,CriadoPorUsername="System",Dificuldade=Enum.Dificuldade.Díficil,Preco=Convert.ToDecimal("10,2")},

                        new Temas{Nome="Missão Espacial",Descricao="Durante uma missão espacial, sua nave sofre uma falha crítica e vocês são lançados em uma área desconhecida do espaço. Vocês têm uma hora para consertar a nave e escapar antes que o oxigênio acabe. Resolver quebra-cabeças tecnológicos, decifrar sinais alienígenas e reconfigurar sistemas são alguns dos desafios que enfrentarão"
                        ,MinPessoas=10,MaxPessoas=20,TempoEstimado=90,DataCriacao=DateTime.Now,CriadoPorUsername="System",Dificuldade=Enum.Dificuldade.Normal,Preco=Convert.ToDecimal("18,7")}


                    ];
                    await dbContext.Temas.AddRangeAsync(temas);
                    haAdicao = true;
                }




                    try {
                        if (haAdicao) {
                            // tornar persistentes os dados
                            dbContext.SaveChanges();
                        }
                    } catch (Exception ex) {

                        throw;
                    }



                }
            }
        }
    }

