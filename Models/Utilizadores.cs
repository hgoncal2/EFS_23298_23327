
using EFS_23298_23327.ViewModel;
using EFS_23298_23327.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFS_23298_23327.Models;


public class Utilizadores:IdentityUser,BaseEntityInterface
    {

    /// <summary>
    /// Primeiro Nome do Utilizador
    /// </summary>
    [Required]
    [Display(Name = "Primeiro Nome")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Deve conter pelo menos 3 letras")]
    [RegularExpression("^[A-Z][a-zA-Z]*$")]
    public String? PrimeiroNome { get; set; }

    /// <summary>
    /// Último Nome do Utilizador
    /// </summary>
    [Required]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Deve conter pelo menos 2 letras")]
    [RegularExpression("^[A-Z][a-zA-Z]*$")]
    [Display(Name = "Último Nome")]
    public String? UltimoNome { get; set; }
    
    /// <summary>
    /// Data em que foi criado o Utilizador
    /// </summary>
    [Display(Name = "Data de Criação")]
    public DateTime DataCriacao { get; set; }

    /// <summary>
    /// Soft Delete do Utilizador
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// OId Criador do Utilizador
    /// </summary>
    [ForeignKey(nameof(Utilizadores))]
    [Display(Name = "Criado Por")]
    public string? CriadoPorOid { get; set; }

    /// <summary>
    /// Username do Criador do Utilizador
    /// </summary>
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

    public String getUserNameString() {
        return this.PrimeiroNome + " " + this.UltimoNome;
    }

}

