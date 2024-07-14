using System.ComponentModel;
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
        [DisplayName("Id das Preferências do Utilizador")]
        public int UserPrefId { get; set; }

        /// <summary>
        /// Id do Anfitrião
        /// </summary>
        [ForeignKey(nameof(Anfitrioes))]
        [DisplayName("Id do Anfitrião")]
        public string AnfId { get; set; }

        /// <summary>
        /// Anfitrião
        /// </summary>
        [DisplayName("Anfitrião")]
        public Anfitrioes? Anfitriao { get; set; }

        /// <summary>
        /// Lista de UserPrefAnfCores
        /// </summary>
        [DisplayName("Lista das Preferências de Cor")]
        public ICollection<UserPrefAnfCores> Cores { get; set; }

        /// <summary>
        /// Mostrar Reservas Canceladas
        /// </summary>
        [DisplayName("Mostrar Canceladas")]
        public bool mostrarCanceladas { get; set; }


        /// <summary>
        /// construtor por defeito
        /// </summary>
        public UserPrefsAnf() {
            this.Cores = new HashSet<UserPrefAnfCores>();
        }
        
    }
}
