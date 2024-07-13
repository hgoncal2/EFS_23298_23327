using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class UserPrefsAnf
    {
        /// <summary>
        /// Id da UserPrefsAnf
        /// </summary>
        [Key]
        public int UserPrefId { get; set; }

        /// <summary>
        /// Id do Anfitrião
        /// </summary>
        [ForeignKey(nameof(Anfitrioes))]
        public string AnfId { get; set; }

        /// <summary>
        /// Anfitrião
        /// </summary>
        public Anfitrioes? Anfitriao { get; set; }

        /// <summary>
        /// Lista de UserPrefAnfCores
        /// </summary>
        public ICollection<UserPrefAnfCores> Cores { get; set; }

        /// <summary>
        /// Mostrar Reservas Canceladas
        /// </summary>
        public bool mostrarCanceladas { get; set; }


        
        public UserPrefsAnf() {
            this.Cores = new HashSet<UserPrefAnfCores>();
        }
        
    }
}
