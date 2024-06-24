using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models
{
    public class Salas
    {
        [Key]
        public int SalaID { get; set; }
        public int Area { get; set; }
        public int Numero { get; set; }
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }

        public Salas()
        {
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
        }
    }
}
