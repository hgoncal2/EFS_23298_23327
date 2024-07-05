using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EFS_23298_23327.ViewModel
{
    public class SalaTemaViewModel
    {
        public int Id { get; set; }

        public  Salas? Sala { get; set; }
        [DisplayName("Anfitriões")]
        public ICollection<String>? Temas { get; set; }


        public SalaTemaViewModel(Salas Sala,ICollection<String>? Temas)
        {
            this.Temas = Temas;
            this.Sala = Sala;
           
        }
        public SalaTemaViewModel() {
            this.Temas = new HashSet<String>();
            

        }
    }
}
