using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.Models
{
    public class Salas : BaseEntity
    {
        [Key]
        public int SalaId { get; set; }

        /// <summary>
        /// Área da Sala
        /// </summary>
        [DisplayName("Área (m2)")]
        public int Area { get; set; }

        /// <summary>
        /// Número da Sala
        /// </summary>
        [DisplayName("Número da Sala")]
        [StringLength(3)]
        [Required]
        public int Numero { get; set; }

        /// <summary>
        /// Lista de Anfitriões da Sala
        /// </summary>
        [DisplayName("Anfitriões")]
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }

        /// <summary>
        /// Lista de Reservas da Sala
        /// </summary>
        [DisplayName("Reservas")]
        public ICollection<Reservas>? ListaReservas { get; set; }

        public Salas()
        {
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
            this.ListaReservas = new HashSet<Reservas>();
           
        }
    }
}
