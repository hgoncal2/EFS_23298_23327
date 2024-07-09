using EFS_23298_23327.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23327.Models
{
    public class Reservas : BaseEntity
    {

        [Key]
        public int ReservaId { get; set; }
        [Display(Name = "Data de início reserva")]
        public DateTime ReservaDate { get; set; }
        [Display(Name = "Data de fim reserva")]
        public DateTime ReservaEndDate { get; set; }
        [Display(Name = "Número de pessoas")]
        [RegularExpression("^\\[1 - 9]{1}")]
        public int NumPessoas { get; set; }
        [Display(Name = "Preço Total")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public decimal TotalPreco { get; set; }
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }
        [ForeignKey(nameof(Clientes))]
        public String? ClienteID { get; set; }
        public  Clientes? Cliente { get; set; }
        [ForeignKey(nameof(Salas))]
        public int? SalaId { get; set; }
        public  Salas? Sala { get; set; }
        public bool Cancelada { get; set; }
        public String? TemaNome { get; set; }
        [EnumDataType(typeof(Dificuldade))]

        public Dificuldade? TemaDif { get; set; }

        public DateTime DataCancel {  get; set; }
       
        public Reservas(Clientes u) {
            this.ClienteID = u.Id;
            this.Cliente = u;
        }
        public Reservas() {
           
        }


    }
}
