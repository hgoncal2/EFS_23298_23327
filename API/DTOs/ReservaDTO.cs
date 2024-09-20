using EFS_23298_23327.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EFS_23298_23327.Data.Enum;
using EFS_23298_23327.Data;

namespace EFS_23298_23327.API.DTOs
{
    public class ReservaDTO
    {

        public int TemaId { get; set; }
        public int SalaId { get; set; }
        public int SalaNumero { get; set; }
        public int SalaArea { get; set; }
        public String TemaNome { get; set; }
        public String TemaDesc { get; set; }
        public int TempoEstimado { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal TemaPreco { get; set; }
        public String? TemaIcone { get; set; }

        
        public int? TemaMinPessoas { get; set; }

       
        public int? TemaMaxPessoas { get; set; }
        //Vamos mandar logo a cor da dificuldade,para evitar requests adicionais
        public Dictionary<Dificuldade,String> TemaDificuldade { get; set; }

        public ICollection<String>? TemaListaFotosNome { get; set; }

        public ICollection<ReservasWrapper>? ListaReservas { get; set; }


        public ReservaDTO(Salas sala,Temas tema) {
            this.ListaReservas = new List<ReservasWrapper>();
            this.TemaId = tema.TemaId;
            this.SalaId = sala.SalaId;
            this.SalaNumero = sala.Numero;
            this.SalaArea = sala.Area;
            this.TemaNome = tema.Nome;
            this.TemaDesc=tema.Descricao;
            this.TempoEstimado = tema.TempoEstimado;
            this.TemaPreco = tema.Preco;
            this.TemaIcone = tema.Icone;
            this.TemaMaxPessoas = tema.MaxPessoas;
            this.TemaMinPessoas = tema.MinPessoas;
            this.TemaDificuldade = new Dictionary<Dificuldade, string>();
            this.TemaDificuldade.Add(tema.Dificuldade, DifficultiesValue.GetDifficultyColor((int)tema.Dificuldade));
            this.TemaListaFotosNome = new HashSet<string>();
            this.TemaListaFotosNome = tema.ListaFotos.Select(x => x.Nome).ToList();






        }





    }
}

