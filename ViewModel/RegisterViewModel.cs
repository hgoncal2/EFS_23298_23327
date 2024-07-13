using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.ViewModel
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
        [EmailAddress]
        public String Email { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "A password e a password de confirmação não são iguais!")]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Primeiro Nome")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O {0} deve conter pelo menos 3 letras")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "O {0} deve começar por letra maiúscula e conter apenas letras")]
        public String PrimeiroNome { get; set; }


        [Display(Name = "Último Nome")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "O {0} deve conter pelo menos 2 letras")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "O {0} deve começar por letra maiúscula e conter apenas letras")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public String UltimoNome { get; set; }

        public  HashSet<String> Roles { get; set; } = new HashSet<String>();
    }
}
