

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models;
    

    public class Utilizadores:IdentityUser
    {
   
    [Display(Name = "Primeiro Nome")]
    public String? PrimeiroNome { get; set; }
   
    [Display(Name = "Último Nome")]
    public String? UltimoNome { get; set; }
   
    [Display(Name = "Data de Criação")]
   
    public DateTime DataCriacao { get; set; }
    public bool Deleted { get; set; }





    public Utilizadores()
        {
            this.DataCriacao = DateTime.Now;
            

    }

    }

