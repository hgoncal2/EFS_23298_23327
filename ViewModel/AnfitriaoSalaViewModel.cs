using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
        [NotMapped]
        public ICollection<int>? ListaReservas { get; set; }

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
        public AnfitriaoSalaViewModel(Salas Sala, ICollection<String>? ListaAnfitrioes, ICollection<int>? res)
        {
            this.ListaAnfitrioes = ListaAnfitrioes;
            this.Sala = Sala;
            this.ListaReservas = res;

        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public AnfitriaoSalaViewModel()
        {
            this.ListaAnfitrioes = new HashSet<String>();
            this.ListaReservas = new HashSet<int>();



        }

     
    }
}
