using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.ViewModel
{
    public class UtilizadoresViewModel
    {
        public int Id { get; set; }
        public String Username { get; set; }
        [Display(Name = "Primeiro Nome")]
        public String? PrimeiroNome { get; set; }

        [Display(Name = "Último Nome")]
        public String? UltimoNome { get; set; }

        [Display(Name = "Data de Criação")]

        public DateTime DataCriacao { get; set; }

        public UtilizadoresViewModel(String Username, String PrimeiroNome, String UltimoNome, DateTime DataCriacao) { 
            this.Username = Username;
            this.PrimeiroNome = PrimeiroNome;
            this.UltimoNome = UltimoNome;
            this.DataCriacao = DataCriacao;
        }
    }
}
