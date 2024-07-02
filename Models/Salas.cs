using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.Models
{
    public class Salas : BaseEntity
    {
        [Key]
        public int SalaId { get; set; }
        [DisplayName("Área (m2)")]
        public int Area { get; set; }
        [DisplayName("Número da Sala")]
        [Required]
        public int Numero { get; set; }
        [DisplayName("Anfitriões")]
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }
        public ICollection<Reservas>? ListaReservas { get; set; }

        public Salas()
        {
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
           
        }
    }
}
