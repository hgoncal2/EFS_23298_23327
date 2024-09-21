using EFS_23298_23327.Data;
using EFS_23298_23327.Hubs;
using EFS_23298_23327.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);




var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddSignalR();
builder.Services.AddTransient<IEmailSender, EnviaEmail>(i =>
                new EnviaEmail(
                    builder.Configuration["EnviaEmail:Host"],
                    int.Parse(builder.Configuration["EnviaEmail:Port"]),
                  bool.Parse(builder.Configuration["EnviaEmail:EnableSSL"]),
                     builder.Configuration["EnviaEmail:Username"],
                    builder.Configuration["EnviaEmail:Password"]
                )
            );



builder.Services.AddDefaultIdentity<Utilizadores>(options => options.SignIn.RequireConfirmedAccount = bool.Parse(builder.Configuration["RequirePasswordLogin:RequirePassword"]))
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>().AddErrorDescriber<ErrosIdentityUser>().AddApiEndpoints();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();




var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
    app.UseMigrationsEndPoint();
    app.UseItToSeedSqlServer();

}
else
{
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseItToSeedSqlServer();
   
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapIdentityApi<Utilizadores>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.MapGroup("/api").MapIdentityApi<Utilizadores>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Gerir",
    pattern: "{area:exists}/{controller=Temas}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//Redireciona para paginas criadas,com o código do erro
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.MapHub<ClassHub>("/hub");


app.MapRazorPages();

app.Run();
