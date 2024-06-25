

using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models;
    

    public class Utilizadores
    {
    [Key]
        public int UtilizadorID { get; set; }
    [Display(Name = "Primeiro Nome")]
    public String? PrimeiroNome { get; set; }
    [Display(Name = "Último Nome")]
    public String? UltimoNome { get; set; }
   
    [Required(ErrorMessage ="Por favor insira uma password"),Display(Name = "Password")]
    public required String Password { get; set; }
    [Display(Name = "Email")]
    [EmailAddress]
    public String? Email { get; set; }
    [Display(Name = "Número de telemóvel",Prompt ="+351913456578")]
    [RegularExpression("^\\+[1-9]{1}[0-9]{1,2}[1-9]{1}[0-9]{3,12}$", ErrorMessage ="Por favor insira um número de telemóvel válido!")]
    public String? NumeroTelemovel { get; set; }
    [Display(Name = "Data de Criação")]
   
    public DateTime DataCriacao { get; set; }
       
        



    public Utilizadores()
        {
            this.DataCriacao = DateTime.Now;
            

    }

    }

