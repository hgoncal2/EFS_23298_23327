using EFS_23298_23327.Models;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.ViewModel
{
    public class UtilizadoresViewModel
    {
        public String Id { get; set; }
        public String? Username { get; set; }
        [Display(Name = "Primeiro Nome")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Deve conter pelo menos 3 letras")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Deve começar por letra maiúscula e conter apenas letras")]
        public String? PrimeiroNome { get; set; }
        public String? Email { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Deve conter pelo menos 2 letras")]

        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Deve começar por letra maiúscula e conter apenas letras")]
        [Display(Name = "Último Nome")]

        public String? UltimoNome { get; set; }

        [Display(Name = "Roles")]

        public HashSet<String>? Roles { get; set; } = new HashSet<String>();
        [Display(Name = "Data de criação")]
        public DateTime? DataCriacao { get; set; }
        [Display(Name = "Criado Por")]
        public String? CriadoPor { get; set; }
       
        [Display(Name = "Password")]
        public String? Password { get; set; }

      
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "A password e a password de confirmação não são iguais!")]
        public String? ConfirmPassword { get; set; }
        public UtilizadoresViewModel() { 
        
        }

        public UtilizadoresViewModel(Utilizadores u) { 
            this.Username = u.UserName;
            this.PrimeiroNome = u.PrimeiroNome;
            this.UltimoNome = u.UltimoNome;
            this.DataCriacao = u.DataCriacao;
            this.Email= u.Email;
            this.CriadoPor=u.CriadoPorUsername;
            this.Id = u.Id;
        }
    }
}
