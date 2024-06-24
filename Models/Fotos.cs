using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models
{
    public class Fotos
    {
        [Key]
        public int FotoID { get; set; }
        public required String Caminho { get; set; }
        public  DateTime? DataTirada { get; set; }
        public String? Descricao { get; set; }
    }
}
