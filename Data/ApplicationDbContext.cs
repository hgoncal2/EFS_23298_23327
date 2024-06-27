using EFS_23298_23306.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace EFS_23298_23306.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
                .HasForeignKey(e => e.TemaID);


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Anfitrioes> Anfitrioes { get; set; }
        public DbSet<Temas> Temas { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Salas> Salas { get; set; }
        public DbSet<Fotos> Fotos { get; set; }

    }


}
