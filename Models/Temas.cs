using EFS_23298_23306.Data;
using EFS_23298_23306.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23306.Models
{
    public class Temas:BaseEntity
    {

        [Key]
        [Display(Name = "ID")]
        public int TemaID { get; set; }
        [Display(Name = "Nome")]
        public required String Nome { get; set; }
        [Display(Name = "Descrição")]
        public String? Descricao { get; set; }
        //Tempo em Minutos
        [Display(Name = "Tempo Estimado (Em minutos)")]
        public int TempoEstimado { get; set; }
        [Display(Name = "Lotação mínima")]
        [Range( 0, 10)]
        public int? MinPessoas { get; set; }
        [Display(Name = "Lotação máxima")]
        [Range ( 0, 20)]
        public int? MaxPessoas { get; set; }
        [Display(Name = "Dificuldade")]
        [EnumDataType(typeof(Dificuldade))]
        public Dificuldade Dificuldade { get; set; }
        [Display(Name = "Fotos")]
        [ForeignKey(nameof(Fotos))]
        public virtual ICollection<Fotos>? ListaFotos { get; set; }
        [ForeignKey(nameof(Salas))]
        [Display(Name = "Sala")]
        public int? SalaID { get; set; }
        public Salas? Sala { get; set; }
        
     

        public Temas()
        {
           
            ListaFotos = new HashSet<Fotos>();


        }


    }
}
