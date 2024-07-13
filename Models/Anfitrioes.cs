using EFS_23298_23327.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class Anfitrioes:Utilizadores
    {
        /// <summary>
        /// Lista de Salas
        /// </summary>
        public ICollection<Salas>? ListaSalas { get; set; }

        /// <summary>
        /// Lista de Reservas
        /// </summary>
        public ICollection<Reservas>? ListaReservas { get; set; }

        /// <summary>
        /// Id das preferências do Anfitrião
        /// </summary>
        [ForeignKey(nameof(UserPrefsAnf))]     
        public int? userPrefsAnfId { get; set; }

        /// <summary>
        /// Preferências do Anfitrião
        /// </summary>
        public UserPrefsAnf? userPrefsAn { get; set; }

        /// <summary>
        /// Construtor por defeito 
        /// </summary>
        public Anfitrioes()
        {
            this.ListaSalas = new HashSet<Salas>();
        }

        /// <summary>
        /// Construtor que recebe uma instância de RegisterViewModel
        /// </summary>
        public Anfitrioes(RegisterViewModel r) {
            this.DataCriacao = DateTime.Now;
            var hasher = new PasswordHasher<Utilizadores>();
            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;
            this.PasswordHash = hasher.HashPassword(null, r.Password);
            this.Email = r.Email;
        }

        /// <summary>
        /// Construtor que recebe uma instância de UtilizadoresViewModel
        /// </summary>
        /// <param name="r"></param>
        public Anfitrioes(UtilizadoresViewModel r) {
            this.DataCriacao = DateTime.Now;
            this.PrimeiroNome = r.PrimeiroNome;
            this.UltimoNome = r.UltimoNome;
            this.UserName = r.Username;
            this.Email = r.Email;
        }



    }
}
