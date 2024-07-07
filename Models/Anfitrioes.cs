using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace EFS_23298_23327.Models
{
    public class Anfitrioes:Utilizadores
    {
        public ICollection<Salas>? ListaSalas { get; set; }
        public ICollection<Reservas>? ListaReservas { get; set; }
        public Anfitrioes()
        {
            this.ListaSalas = new HashSet<Salas>();
        }
        public Anfitrioes(RegisterViewModel r) {
            this.DataCriacao = DateTime.Now;
            var hasher = new PasswordHasher<Utilizadores>();
            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;
            this.PasswordHash = hasher.HashPassword(null, r.Password);
            this.Email = r.Email;
        }
        public Anfitrioes(UtilizadoresViewModel r) {
            this.DataCriacao = DateTime.Now;

            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;

            this.Email = r.Email;
        }


    }
}
