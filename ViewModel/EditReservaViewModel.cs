using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EFS_23298_23327.ViewModel
{
    public class EditReservaViewModel
    {
       public Temas tema { get; set; }

        public Reservas Reserva { get; set; }
        
        public ICollection<Reservas> ListaReservas { get; set; }
        public DateTime ReservaDate { get; set; }
        public DateTime ReservaEndDate { get; set; }

        public ICollection<String> RlistaAnfitrioes { get; set; }
        [DisplayName("Notificar Cliente de alteração da reserva")]
        public bool NotificarCliente { get; set; }


        public EditReservaViewModel(Reservas reserva,ICollection<Reservas> listaReservas,Temas tema)
        {
            this.tema = tema;
        this.Reserva = reserva;
            this.ListaReservas = listaReservas;
            this.RlistaAnfitrioes = reserva.ListaAnfitrioes.Select(r=>r.Id).ToList();
           
        }
        public EditReservaViewModel() {
          
        }

    }
}
