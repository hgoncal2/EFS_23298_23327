
using EFS_23298_23327.ViewModel;
using EFS_23298_23327.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFS_23298_23327.Models;


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

    public Utilizadores(RegisterViewModel r) {
        this.DataCriacao = DateTime.Now;
        var hasher = new PasswordHasher<Utilizadores>();
        this.PrimeiroNome = r.PrimeiroNome;
        this.UltimoNome = r.UltimoNome;
        this.UserName = r.Username;
        this.PasswordHash=hasher.HashPassword(null,r.Password);
        this.Email = r.Email;
    }
    public Utilizadores(UtilizadoresViewModel r) {
        this.DataCriacao = DateTime.Now;
      
        this.PrimeiroNome = r.PrimeiroNome;
        this.UltimoNome = r.UltimoNome;
        this.UserName = r.Username;

        this.Email = r.Email;
    }

}

