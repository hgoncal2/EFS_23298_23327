using EFS_23298_23327.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EFS_23298_23327.API.DTOs
{
    public class FazReservaDTO
    {
        public int SalaId { get; set; }
        [Display(Name = "Data de início reserva")]
        public DateTime ReservaDate { get; set; }

        

        public int NumPessoas { get; set; }

        

       
    }
}
