using EFS_23298_23306.Models;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.ViewModel
{
    public class UtilizadoresViewModel
    {
        public int Id { get; set; }
        public String Username { get; set; }
        [Display(Name = "Primeiro Nome")]
        public String? PrimeiroNome { get; set; }
        public String? Email { get; set; }

        [Display(Name = "Último Nome")]
        public String? UltimoNome { get; set; }

        [Display(Name = "Roles")]

        public HashSet<String> Roles { get; set; } = new HashSet<String>();
        [Display(Name = "Data de criação")]
        public DateTime DataCriacao { get; set; }

        public UtilizadoresViewModel() { 
        
        }

        public UtilizadoresViewModel(Utilizadores u) { 
            this.Username = u.UserName;
            this.PrimeiroNome = u.PrimeiroNome;
            this.UltimoNome = u.UltimoNome;
            this.DataCriacao = u.DataCriacao;
            this.Email= u.Email;
        }
    }
}
