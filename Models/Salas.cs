using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models
{
    public class Salas
    {
        [Key]
        public int SalaID { get; set; }
        [DisplayName("Área (m2)")]
        public int Area { get; set; }
        [DisplayName("Número da Sala")]
        [Required]
        public int Numero { get; set; }
        [DisplayName("Anfitriões")]
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }
        public DateTime DataCriacao { get; set; }

        public Salas()
        {
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
            this.DataCriacao = DateTime.Now;
        }
    }
}
