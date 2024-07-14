using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23327.Models
{
    public class Fotos : BaseEntity
    {

        /// <summary>
        /// Id da Foto
        /// </summary>
        [Key]
        [DisplayName("Id da Foto")]
        public int FotoId { get; set; }

        /// <summary>
        /// Nome da Foto
        /// </summary>
        [DisplayName("Nome da Foto")]
        public required String Nome { get; set; }

        /// <summary>
        /// Data de quando a foto foi tirada
        /// </summary>
        [DisplayName("Data Tirada")]
        public  DateTime? DataTirada { get; set; }

        /// <summary>
        /// Descrição da foto
        /// </summary>
        [DisplayName("Descrição")]
        public String? Descricao { get; set; }

        /// <summary>
        /// Id do Tema em que a foto foi inserida
        /// </summary>
        [ForeignKey(nameof(Temas))]
        [DisplayName("Id do Tema")]
        public int TemaId { get; set; }

        /// <summary>
        /// Tema em que a foto foi inserida
        /// </summary>
        [DisplayName("Tema")]
        public Temas Tema { get; set; }
        
        /// <summary>
        /// Construtor que recebe uma instância (String) do nome da foto
        /// </summary>
        /// <param name="nome"></param>
        [SetsRequiredMembers]
        public Fotos(string nome)
        {
            this.Nome = nome; 
        }
    }

    
}
