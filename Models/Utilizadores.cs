

using EFS_23298_23306.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23306.Models;


public class Utilizadores:IdentityUser,BaseEntityInterface
    {
   
    [Display(Name = "Primeiro Nome")]
    public String? PrimeiroNome { get; set; }
   
    [Display(Name = "Último Nome")]
    public String? UltimoNome { get; set; }
   
    [Display(Name = "Data de Criação")]
   
    public DateTime DataCriacao { get; set; }
    public bool Deleted { get; set; }
    [ForeignKey(nameof(Utilizadores))]
    [Display(Name = "Criado Por")]
    public string? CriadoPorOid { get; set; }
    [Display(Name = "Criado Por")]
    public string? CriadoPorUsername { get; set; }

    public Utilizadores()
        {
            this.DataCriacao = DateTime.Now;
            

    }

    }

