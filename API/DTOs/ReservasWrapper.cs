using EFS_23298_23327.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.API.DTOs
{
    public class ReservasWrapper
    {

        public int ReservaId { get; set; }
        public DateTime ReservaDate { get; set; }

        /// <summary>
        /// Data do fim da Reserva
        /// </summary>
        [Display(Name = "Data de fim reserva")]
        public DateTime ReservaEndDate { get; set; }
        [Display(Name = "Preço Total")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public decimal TotalPreco { get; set; }
        public int NumPessoas { get; set; }

        public String? ClienteID { get; set; }
        public String? ClienteUsername { get; set; }
        public String? ClientePrimeiroNome { get; set; }
        public String? ClienteUltimoNome { get; set; }
        public ICollection<AnfsWrapper>? Anfitrioes { get; set; }
        public bool cancelada { get; set; }


        public ReservasWrapper(Reservas res) { 

            this.Anfitrioes = new HashSet<AnfsWrapper>();
            this.ReservaId = res.ReservaId;
            this.ReservaDate = res.ReservaDate;
            this.ReservaEndDate = res.ReservaEndDate;
            this.TotalPreco = res.TotalPreco;
            this.ClienteID = res.ClienteID;
            this.ClienteUsername = res.Cliente.UserName;
            this.ClientePrimeiroNome = res.Cliente.PrimeiroNome;
            this.ClienteUltimoNome=res.Cliente.UltimoNome;
            this.NumPessoas= res.NumPessoas;
            this.cancelada = res.Cancelada;
        
        }
    }

    public class AnfsWrapper {

        public string userId { get; set; }
        public string primeiroNome { get; set; }
        public  string ultimoNome { get; set; }
        public string username { get; set; }

        public AnfsWrapper(Anfitrioes a) {
            this.userId = a.Id;
            this.primeiroNome = a.PrimeiroNome;
            this.ultimoNome = a.UltimoNome;
            this.username    = a.UserName;
        }
    }
}
