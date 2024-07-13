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
        public String UtilizadorId {  get; set; }

        /// <summary>
        /// Ativar/Desativar Alertas de Reservas
        /// </summary>
        public Boolean reservas {  get; set; }


        public int? UserPrefsAnf { get; set; }
        public UserPrefs(Anfitrioes u)
        {
            this.UtilizadorId = u.Id;
            this.reservas = true;
            this.UserPrefsAnf = u.userPrefsAnfId;
        }
        public UserPrefs()
        {
            this.reservas = true;
        }
    }
}
