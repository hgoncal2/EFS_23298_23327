using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23306.Models
{
    public class Fotos : BaseEntity
    {
        [Key]
        public int FotoId { get; set; }
        public required String Nome { get; set; }
        public  DateTime? DataTirada { get; set; }
        public String? Descricao { get; set; }
        [ForeignKey(nameof(Temas))]
        public int TemaId { get; set; }
        public Temas Tema { get; set; }
        

        [SetsRequiredMembers]
        public Fotos(string nome)
        {
            this.Nome = nome; 
        }
    }

    
}
