﻿namespace EFS_23298_23306.Data
{
    using EFS_23298_23306.Models;
    using Microsoft.AspNetCore.Identity;


    namespace Aulas.Data
    {

        internal class DbInitializer
        {

            internal static async void Initialize(ApplicationDbContext dbContext)
            {

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
                if (!dbContext.Utilizadores.Any())
                {
                    users = [
                       new Utilizadores{UserName="admin",NormalizedUserName="ADMIN",PasswordHash=hasher.HashPassword(null,"teste123")}
                    
                    ];
                    await dbContext.Utilizadores.AddRangeAsync(users);
                    haAdicao = true;
                }




                try
                {
                    if (haAdicao)
                    {
                        // tornar persistentes os dados
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }



            }
        }
    }
}