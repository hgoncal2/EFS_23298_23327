using EFS_23298_23327.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class BaseEntity : BaseEntityInterface
    {
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }
        public bool Deleted { get; set; }
        [Display(Name = "Criado Por")]
        [ForeignKey(nameof(Utilizadores))]
        public string? CriadoPorOid { get; set; }
        [Display(Name = "Criado Por")]
        public string? CriadoPorUsername { get; set; }
        public BaseEntity() {
        this.DataCriacao = DateTime.Now;
        }

    }
}
