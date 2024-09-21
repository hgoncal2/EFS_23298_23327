using Azure.Identity;
using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.ViewModel
{
    public class ReservaViewModel
    {
        /// <summary>
        /// Tema
        /// </summary>
        public Temas? Tema { get; set; }

        /// <summary>
        /// Sala
        /// </summary>
        public Salas? Sala { get; set; }

        public int? SalaId { get; set; }

        /// <summary>
        /// Número de Pessoas da Reserva
        /// </summary>
        [DisplayName("Número de pessoas")]
        public int nPessoas { get; set; }

        /// <summary>
        /// Data Inicial
        /// </summary>
        public DateTime dataI { get; set; }
        
        public DateTime? viewStart { get; set; }
     
        public DateTime? viewEnd { get; set; }
       
        public String? viewType { get; set; }
        

        public ReservaViewModel(Salas s,Temas t) {
            this.Sala = s;
            this.Tema = t;
           

        }
        public ReservaViewModel() {
           

        }

    }
}
