using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EFS_23298_23327.ViewModel
{
    public class EditReservaViewModel
    {
        /// <summary>
        /// Tema
        /// </summary>
       public Temas tema { get; set; }

        /// <summary>
        /// Reserva
        /// </summary>
        public Reservas Reserva { get; set; }
        
        /// <summary>
        /// Lista de Reservas
        /// </summary>
        public ICollection<Reservas> ListaReservas { get; set; }

        /// <summary>
        /// Data de Reserva
        /// </summary>
        public DateTime ReservaDate { get; set; }

        /// <summary>
        /// Data de Fim de Reserva
        /// </summary>
        public DateTime ReservaEndDate { get; set; }

        /// <summary>
        /// Lista de Anfitriões da Reserva
        /// </summary>
        public ICollection<String> RlistaAnfitrioes { get; set; }

        /// <summary>
        /// Notifica o cliente de alteração da reserva
        /// </summary>
        [DisplayName("Notificar Cliente de alteração da reserva")]
        public bool NotificarCliente { get; set; }

        /// <summary>
        /// Construtor que recebe parametros reserva, listaReservas e tema
        /// </summary>
        /// <param name="reserva"></param>
        /// <param name="listaReservas"></param>
        /// <param name="tema"></param>
        public EditReservaViewModel(Reservas reserva,ICollection<Reservas> listaReservas,Temas tema)
        {
            this.tema = tema;
        this.Reserva = reserva;
            this.ListaReservas = listaReservas;
            this.RlistaAnfitrioes = reserva.ListaAnfitrioes.Select(r=>r.Id).ToList();
           
        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public EditReservaViewModel() {
          
        }

    }
}
