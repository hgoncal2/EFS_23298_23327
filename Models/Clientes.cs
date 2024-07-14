using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace EFS_23298_23327.Models
{
    public class Clientes:Utilizadores
    {
        /// <summary>
        /// Lista de Reservas
        /// </summary>
        [DisplayName("Lista de Reservas")]
        public ICollection<Reservas>? ListaReservas { get; set; }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public Clientes() {

            ListaReservas = new HashSet<Reservas>();
        }

        /// <summary>
        /// Construtor que recebe uma instância de RegisterViewModel
        /// </summary>
        /// <param name="r"></param>
        public Clientes(RegisterViewModel r) {
            this.DataCriacao = DateTime.Now;
            var hasher = new PasswordHasher<Utilizadores>();
            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;
            this.PasswordHash = hasher.HashPassword(null, r.Password);
            this.Email = r.Email;
        }

        /// <summary>
        /// Contrutor que recebe uma instância de UtilizadoresViewModel
        /// </summary>
        /// <param name="r"></param>
        public Clientes(UtilizadoresViewModel r) {
            this.DataCriacao = DateTime.Now;
            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;
            this.Email = r.Email;
        }
    }
}
