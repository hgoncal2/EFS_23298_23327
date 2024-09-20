using EFS_23298_23327.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EFS_23298_23327.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.API.DTOs
{
    public class TemaDTO
    {
        // Propriedades do Tema (completa)
        public int TemaId { get; set; }  // ID do tema

        /// <summary>
        /// Nome do tema
        /// </summary>
        [Display(Name = "Nome do Tema")]
        [Required(ErrorMessage = "Por favor indique o {0}")]
        [StringLength(40)]
        [RegularExpression("^(?=.*[A-Za-z]).*$", ErrorMessage = "Deve conter pelo menos 1 letra")]
        public String? Nome { get; set; }

        /// <summary>
        /// Descrição do tema
        /// </summary>
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        public String? Descricao { get; set; }


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
        /// Ícone de representação do Tema
        /// </summary>
        [Display(Name = "Ícone")]
        public String? Icone { get; set; }

        /// <summary>
        /// Lotação Mínima do Tema
        /// </summary>
        [Display(Name = "min Pax", Description = "Lotação mínima")]
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Range(0, 20)]
        public int? MinPessoas { get; set; }

        /// <summary>
        /// Lotação Máxima do Tema
        /// </summary>
        [Required(ErrorMessage = "Por favor indique a {0}")]
        [Display(Name = "max Pax")]
        [Range(0, 20)]
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

        public DateTime DataCriacao { get; set; }  // Data de criação da sala
       
        public string? CriadoPorOid { get; set; }  // ID do criador da sala
        public string? CriadoPorUsername { get; set; }  // Nome de utilizador do criador da sala

        // Listas para IDs e nomes de relações
        public List<string>? ListaDeFotos { get; set; }  // Lista de fotos

        // Construtor vazio
        public TemaDTO()
        {
            ListaDeFotos = new List<string>();
        }

        // Construtor para inicializar com valores
        public TemaDTO(Temas tema, List<string> listaDeFotos) {
            TemaId = tema.TemaId;
            Nome = tema.Nome;
            Descricao = tema.Descricao;
            TempoEstimado = tema.TempoEstimado;
            Preco = tema.Preco;
            Icone = tema.Icone;
            MinPessoas = tema.MinPessoas;
            MaxPessoas = tema.MaxPessoas;
            Dificuldade = tema.Dificuldade;

            DataCriacao = tema.DataCriacao;          
            CriadoPorOid = tema.CriadoPorOid;
            CriadoPorUsername = tema.CriadoPorUsername;
            ListaDeFotos = listaDeFotos;
        }
    }
}

