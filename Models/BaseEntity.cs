using EFS_23298_23327.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class BaseEntity : BaseEntityInterface
    {

        /// <summary>
        /// Data de criação
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Boolean para Soft delete
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// OId de quem criou
        /// </summary>
        [Display(Name = "Criado Por")]
        [ForeignKey(nameof(Utilizadores))]
        public string? CriadoPorOid { get; set; }

        /// <summary>
        /// Username de quem criou
        /// </summary>
        [Display(Name = "Criado Por")]
        public string? CriadoPorUsername { get; set; }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public BaseEntity() {
        this.DataCriacao = DateTime.Now;
        }

    }
}
