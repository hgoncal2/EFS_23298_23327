using EFS_23298_23306.Models;

namespace EFS_23298_23306.ViewModel
{
    public class AnfitriaoSalaViewModel
    {
        public int Id { get; set; }

        public  Salas? Sala { get; set; }
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
