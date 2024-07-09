using EFS_23298_23327.Data;
using EFS_23298_23327.Data.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    /// <summary>
    /// Modelo Temas
    /// </summary>
    public class Temas:BaseEntity
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int TemaId { get; set; }
        /// <summary>
        /// Nome do tema
        /// </summary>
        [Display(Name = "Nome do Tema")]
        [Required(ErrorMessage ="Por favor indique o {0}")]
        public  String Nome { get; set; }
        /// <summary>
        /// Descrição do tema
        /// </summary>
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        public String Descricao { get; set; }


        //Tempo em Minutos
        [Display(Name = "Tempo Estimado (Em minutos)")]
        [Required(ErrorMessage = "Por favor indique  {0}")]
        public int TempoEstimado { get; set; }


        [Display(Name = "Preço (Por pessoa)")]
        [Required(ErrorMessage = "Por favor indique o {0}")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Preco { get; set; }

        [NotMapped] 
        [Required(ErrorMessage = "Por favor indique o {0}")]
       
        [StringLength(9)]
        [RegularExpression("[0-9]{1,6}([,.][0-9]{1,2})?", ErrorMessage = "Escreva um número com, no máximo 2 casa decimal, separadas por . ou ,")]
        [Display(Name = "Preço  (Por pessoa)")]
        public string PrecoStr { get; set; }


        [Display(Name = "Ícone")]
        public String? Icone { get; set; }

        
        [Display(Name = "Lotação mínima")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Range( 0, 20)]
        public int? MinPessoas { get; set; }
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Display(Name = "Lotação máxima")]
        [Range ( 0, 20)]
        public int? MaxPessoas { get; set; }
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Display(Name = "Dificuldade")]
        [EnumDataType(typeof(Dificuldade))]
        public Dificuldade Dificuldade { get; set; }

        [Display(Name = "Fotos")]
        [ForeignKey(nameof(Fotos))]
        public virtual ICollection<Fotos>? ListaFotos { get; set; }
        [ForeignKey(nameof(Salas))]
        [Display(Name = "Sala")]
        public int? SalaID { get; set; }
        public Salas? Sala { get; set; }
        [NotMapped]
        [DisplayName("Anunciar Tema")]
        public bool AnunciarTema { get; set; }
        
     

        public Temas()
        {
           
            ListaFotos = new HashSet<Fotos>();


        }


    }
}
