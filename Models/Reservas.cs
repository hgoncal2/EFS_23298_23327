using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23306.Models
{
    public class Reservas
    {

        [Key]
        public int ReservaID { get; set; }
        
        public DateTime ReservaDate { get; set; }
        public double Preco { get; set; }
        public int NumPessoas { get; set; }
        [ForeignKey(nameof(Utilizadores))]
        public int UtilizadorID { get; set; }
        public required Utilizadores Utilizador { get; set; }
        [ForeignKey(nameof(Temas))]
        public int? TemaID { get; set; }
        public  Temas? Tema { get; set; }




    }
}
