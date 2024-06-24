using EFS_23298_23306.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23306.Models
{
    public class Temas
    {

        [Key]
        public int TemaID { get; set; }
        public required String Nome { get; set; }
        public String? Descricao { get; set; }
        //Tempo em Minutos
        public int TempoEstimado { get; set; }
        public int? MinPessoas { get; set; }
        public int? MaxPessoas { get; set; }
        public Dificuldade Dificuldade { get; set; }
        [ForeignKey(nameof(Fotos))]
        public int FotoID { get; set; }
        public Fotos? Foto { get; set; }
        [ForeignKey(nameof(Salas))]
        public int? SalaID { get; set; }
        public Salas? Sala { get; set; }





    }
}
