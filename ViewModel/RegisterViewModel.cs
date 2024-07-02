using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.ViewModel
{
    public class RegisterViewModel
    {


        
        public String? Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        [Display(Name = "Primeiro Nome")]
        public String? PrimeiroNome { get; set; }

        [Display(Name = "Último Nome")]
        public String? UltimoNome { get; set; }
        public  HashSet<String> Roles { get; set; } = new HashSet<String>();
    }
}
