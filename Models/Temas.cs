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
        [StringLength(40)]
        [RegularExpression("^(?=.*[A-Za-z]).*$", ErrorMessage = "Deve conter pelo menos 1 letra")]
        public  String Nome { get; set; }

        /// <summary>
        /// Descrição do tema
        /// </summary>
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        public String Descricao { get; set; }


        /// <summary>
        /// Tempo estimado de duração do Tema (Em minutos)
        /// </summary>
        [Display(Name = "Tempo Estimado (min)")]
        [Required(ErrorMessage = "Por favor indique  {0}")]
        public int TempoEstimado { get; set; }

        /// <summary>
        /// Preço por Pessoa para o Tema
        /// </summary>
        [Display(Name = "Preço (Por pessoa)")]
        [Required(ErrorMessage = "Por favor indique o {0}")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Preco { get; set; }

        /// <summary>
        /// Preço por Pessoa para o Tema em formato String
        /// </summary>
        [NotMapped] 
        [Required(ErrorMessage = "Por favor indique o {0}")]      
        [StringLength(9)]
        [RegularExpression("[0-9]{1,6}([,.][0-9]{1,2})?", ErrorMessage = "Escreva um número com, no máximo 2 casa decimal, separadas por . ou ,")]
        [Display(Name = "Preço (Por pessoa)")]
        public string PrecoStr { get; set; }

        /// <summary>
        /// Ícone de representação do Tema
        /// </summary>
        [Display(Name = "Ícone")]
        public String? Icone { get; set; }

        /// <summary>
        /// Lotação Mínima do Tema
        /// </summary>
        [Display(Name = "min Pax", Description = "Lotação mínima")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Range( 0, 20)]
        public int? MinPessoas { get; set; }

        /// <summary>
        /// Lotação Máxima do Tema
        /// </summary>
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Display(Name = "max Pax")]
        [Range ( 0, 20)]
        public int? MaxPessoas { get; set; }

        /// <summary>
        /// Dificuldade do Tema
        /// </summary>
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Display(Name = "Dificuldade")]
        [EnumDataType(typeof(Dificuldade))]
        public Dificuldade Dificuldade { get; set; }

        /// <summary>
        /// Lista de Fotos do Tema
        /// </summary>
        [Display(Name = "Fotos")]
        [ForeignKey(nameof(Fotos))]
        public virtual ICollection<Fotos>? ListaFotos { get; set; }

        /// <summary>
        /// ID da Sala a que o Tema será atribuído
        /// </summary>
        [ForeignKey(nameof(Salas))]
        [Display(Name = "Sala")]
        public int? SalaID { get; set; }

        /// <summary>
        /// Sala a que o Tema será atribuído
        /// </summary>
        [DisplayName("Sala")]
        public Salas? Sala { get; set; }

        /// <summary>
        /// Anunciar a criação de novo Tema
        /// </summary>
        [NotMapped]
        [DisplayName("Anunciar Tema")]
        public bool AnunciarTema { get; set; }
        
     
        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public Temas()
        {
           
            ListaFotos = new HashSet<Fotos>();


        }


    }
}
