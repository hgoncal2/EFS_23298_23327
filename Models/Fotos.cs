using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23306.Models
{
    public class Fotos
    {
        [Key]
        public int FotoID { get; set; }
        public required String Nome { get; set; }
        public  DateTime? DataTirada { get; set; }
        public String? Descricao { get; set; }
        [SetsRequiredMembers]
        public Fotos(string nome)
        {
            this.Nome = nome; 
        }
    }

    
}
