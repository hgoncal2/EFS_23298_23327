using EFS_23298_23327.Data.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23327.Models
{
    public class Reservas : BaseEntity
    {
        /// <summary>
        /// Id da Reserva
        /// </summary>
        [Key]
        [DisplayName("Id")]
        public int ReservaId { get; set; }

        /// <summary>
        /// Data de inicio da Reserva
        /// </summary>
        [Display(Name = "Data de início reserva")]
        public DateTime ReservaDate { get; set; }

        /// <summary>
        /// Data do fim da Reserva
        /// </summary>
        [Display(Name = "Data de fim reserva")]
        public DateTime ReservaEndDate { get; set; }

        /// <summary>
        /// Número de pessoas da Reserva
        /// </summary>
        [Display(Name = "Número de pessoas")]
        [RegularExpression("^[1-9]{0,3}$",ErrorMessage ="Por favor insira um caracter de 1-9,opcionalmente seguiod de,no máximo 3 caracteres de 1-9 ")]
        public int NumPessoas { get; set; }

        /// <summary>
        /// Preço Total da Reserva (preço do Tema * num Pessoas)
        /// </summary>
        [Display(Name = "Preço Total")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public decimal TotalPreco { get; set; }

        /// <summary>
        /// Lista de Anfitriões do Tema Reservado
        /// </summary>
        [DisplayName("Lista de Anfitriões")]
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }

        /// <summary>
        /// Id de quem faz a reserva (Cliente)
        /// </summary>
        [ForeignKey(nameof(Clientes))]
        [DisplayName("Id do Cliente")]
        public String? ClienteID { get; set; }

        /// <summary>
        /// Cliente que fez a reserva
        /// </summary>
        [DisplayName("Cliente")]
        public  Clientes? Cliente { get; set; }

        /// <summary>
        /// Id da Sala a que pertence o Tema Reservado
        /// </summary>
        [ForeignKey(nameof(Salas))]
        [DisplayName("Id da Sala")]
        public int SalaId { get; set; }

        /// <summary>
        /// Sala a que pertence o Tema Reservado
        /// </summary>
        [DisplayName("Sala")]
        public Salas Sala { get; set; }

        /// <summary>
        /// Boolean que define se a reserva está cancelada
        /// </summary>
        [DisplayName("Cancelada")]
        public bool Cancelada { get; set; }

        /// <summary>
        /// Nome do Tema reservado
        /// </summary>
        [DisplayName("Nome do Tema")]
        public String? TemaNome { get; set; }

        /// <summary>
        /// Dificuldade do Tema reservado
        /// </summary>
        [DisplayName("Dificuldade do Tema")]
        public Dificuldade? TemaDif { get; set; }

        /// <summary>
        /// Data em que a reserva foi cancelada
        /// </summary>
        [DisplayName("Data de Cancelamento")]
        public DateTime DataCancel {  get; set; }

        /// <summary>
        /// Construtor que recebe uma instância do Cliente que está a fazer a reserva
        /// </summary>
        /// <param name="u"></param>
        
        public Reservas(Clientes u) {
            this.ClienteID = u.Id;
            this.Cliente = u;
        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public Reservas() {
           
        }


    }
}
