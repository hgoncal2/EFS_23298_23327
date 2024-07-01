using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23306.Models
{
    public class Reservas : BaseEntity
    {

        [Key]
        public int ReservaId { get; set; }
        [Display(Name = "Data da reserva")]
        public DateTime ReservaDate { get; set; }
        [Display(Name = "Preço(em Euros)")]
        public double Preco { get; set; }
        [Display(Name = "Número de pessoas")]
        [RegularExpression("^\\[1 - 9]{1}")]
        public int NumPessoas { get; set; }
        [ForeignKey(nameof(Utilizadores))]
        public String? UtilizadorID { get; set; }
        public required Utilizadores? Utilizador { get; set; }
        [ForeignKey(nameof(Temas))]
        public int? TemaID { get; set; }
        public  Temas? Tema { get; set; }
      
      

    }
}
