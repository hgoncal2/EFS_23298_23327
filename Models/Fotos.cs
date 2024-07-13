using Microsoft.EntityFrameworkCore;
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
        public int FotoId { get; set; }

        /// <summary>
        /// Nome da Foto
        /// </summary>
        public required String Nome { get; set; }

        /// <summary>
        /// Data de quando a foto foi tirada
        /// </summary>
        public  DateTime? DataTirada { get; set; }

        /// <summary>
        /// Descrição da foto
        /// </summary>
        public String? Descricao { get; set; }

        /// <summary>
        /// Id do Tema em que a foto foi inserida
        /// </summary>
        [ForeignKey(nameof(Temas))]
        public int TemaId { get; set; }

        /// <summary>
        /// Tema em que a foto foi inserida
        /// </summary>
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
