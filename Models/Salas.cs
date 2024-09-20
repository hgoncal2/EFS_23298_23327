using EFS_23298_23327.API.DTOs;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.Models
{
    public class Salas : BaseEntity
    {
        [Key]
        public int SalaId { get; set; }

        /// <summary>
        /// Área da Sala
        /// </summary>
        [DisplayName("Área (m2)")]
        public int Area { get; set; }

        /// <summary>
        /// Número da Sala
        /// </summary>
        [DisplayName("Número da Sala")]
        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Deve conter no máximo 3 números")]
        [Required]
        public int Numero { get; set; }

        /// <summary>
        /// Lista de Anfitriões da Sala
        /// </summary>
        [DisplayName("Anfitriões")]
        public ICollection<Anfitrioes>? ListaAnfitrioes { get; set; }

        /// <summary>
        /// Lista de Reservas da Sala
        /// </summary>
        [DisplayName("Reservas")]
        public ICollection<Reservas>? ListaReservas { get; set; }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public Salas()
        {
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
            this.ListaReservas = new HashSet<Reservas>();          
        }


        public Salas(SalaDTO s) {
            this.Numero = s.Numero;
            this.Area = s.Area;
            this.ListaAnfitrioes = new HashSet<Anfitrioes>();
            this.ListaReservas = new HashSet<Reservas>();



        }
    }
}
