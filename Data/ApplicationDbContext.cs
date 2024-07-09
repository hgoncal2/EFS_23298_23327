using EFS_23298_23327.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using EFS_23298_23327.ViewModel;
namespace EFS_23298_23327.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizadores>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fotos>()
                .HasOne(e => e.Tema).
                WithMany(c => c.ListaFotos)
                .HasForeignKey(e => e.TemaId);

            modelBuilder.Entity<Reservas>()
               .HasOne(e => e.Sala).
               WithMany(c => c.ListaReservas)
               .HasForeignKey(e => e.SalaId);
            modelBuilder.Entity<Reservas>()
              .HasOne(e => e.Cliente).
              WithMany(c => c.ListaReservas)
              .HasForeignKey(e => e.ClienteID);
            modelBuilder.Entity<UserPrefsAnf>()
              .HasMany(p => p.Cores)
              .WithOne(c => c.UserPrefsAnf)
              .HasForeignKey(c => c.UserPrefId);




            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "anf", Name = "Anfitriao", NormalizedName = "ANFITRIAO" },
             new IdentityRole { Id = "adm", Name = "Admin", NormalizedName = "ADMIN" },
             new IdentityRole { Id = "cl", Name = "Cliente", NormalizedName = "CLIENTE" }
             );
        }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Anfitrioes> Anfitrioes { get; set; }
        public DbSet<Temas> Temas { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Salas> Salas { get; set; }
        public DbSet<Fotos> Fotos { get; set; }
        public DbSet<EFS_23298_23327.ViewModel.LoginViewModel> LoginViewModel { get; set; } = default!;
        public DbSet<EFS_23298_23327.ViewModel.UtilizadoresViewModel> UtilizadoresViewModel { get; set; } = default!;

    }


}
