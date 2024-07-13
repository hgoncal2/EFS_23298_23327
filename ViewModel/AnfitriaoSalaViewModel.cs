using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EFS_23298_23327.ViewModel
{
    public class AnfitriaoSalaViewModel
    {

        public int Id { get; set; }

        public  Salas? Sala { get; set; }
        [DisplayName("Anfitriões")]
        public ICollection<String>? ListaAnfitrioes { get; set; }


        public AnfitriaoSalaViewModel(Salas Sala,ICollection<String>? ListaAnfitrioes)
        {
            this.ListaAnfitrioes = ListaAnfitrioes;
            this.Sala = Sala;
           
        }
        public AnfitriaoSalaViewModel() {
            this.ListaAnfitrioes = new HashSet<String>();
            

        }
    }
}
