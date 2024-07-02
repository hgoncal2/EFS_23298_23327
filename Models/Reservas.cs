﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFS_23298_23327.Models
{
    public class Reservas : BaseEntity
    {

        [Key]
        public int ReservaId { get; set; }
        [Display(Name = "Data da reserva")]
        public DateTime ReservaDate { get; set; }
        [Display(Name = "Preço(em Euros)")]
        public double Preco { get; set; }
        [Display(Name = "Número de pessoas")]
        [RegularExpression("^\\[1 - 9]{1}")]
        public int NumPessoas { get; set; }
        [ForeignKey(nameof(Clientes))]
        
        public String? ClienteID { get; set; }
        public  Clientes? Cliente { get; set; }
        [ForeignKey(nameof(Salas))]
        public int? SalaId { get; set; }
        public  Salas? Sala { get; set; }

      
        public Reservas(Clientes u) {
            this.ClienteID = u.Id;
            this.Cliente = u;
        }
        public Reservas() {
           
        }


    }
}
