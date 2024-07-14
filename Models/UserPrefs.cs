using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace EFS_23298_23327.Models
{
    public class UserPrefs
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id do Utilizador a que serão atribuídas as Preferências
        /// </summary>
        [DisplayName("Id do Utilizador")]
        public String UtilizadorId {  get; set; }

        /// <summary>
        /// Ativar/Desativar Alertas de Reservas
        /// </summary>
        [DisplayName("Ativar Alerta de Reservas")]
        public Boolean reservas {  get; set; }

        /// <summary>
        /// Preferências do Anfitrião
        /// </summary>
        [DisplayName("Preferências do Anfitrião")]
        public int? UserPrefsAnf { get; set; }

        /// <summary>
        /// Construtor que recebe uma instância do Anfitrião
        /// </summary>
        /// <param name="u"></param>
        public UserPrefs(Anfitrioes u)
        {
            this.UtilizadorId = u.Id;
            this.reservas = true;
            this.UserPrefsAnf = u.userPrefsAnfId;
        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public UserPrefs()
        {
            this.reservas = true;
        }
    }
}
