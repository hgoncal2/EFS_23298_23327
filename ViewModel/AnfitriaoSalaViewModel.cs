using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EFS_23298_23327.ViewModel
{
    public class AnfitriaoSalaViewModel
    {
        /// <summary>
        /// Id da Sala
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sala
        /// </summary>
        [Display(Name = "Sala")]
        public  Salas? Sala { get; set; }

        /// <summary>
        /// Lista de Anfitriões
        /// </summary>
        [DisplayName("Anfitriões")]
        public ICollection<String>? ListaAnfitrioes { get; set; }

        /// <summary>
        /// Construtor que recebe parametros Sala e ListaAnfitriões
        /// </summary>
        /// <param name="Sala"></param>
        /// <param name="ListaAnfitrioes"></param>
        public AnfitriaoSalaViewModel(Salas Sala,ICollection<String>? ListaAnfitrioes)
        {
            this.ListaAnfitrioes = ListaAnfitrioes;
            this.Sala = Sala;
           
        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public AnfitriaoSalaViewModel() {
            this.ListaAnfitrioes = new HashSet<String>();
            

        }
    }
}
