using EFS_23298_23327.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Data.Interfaces
{
    public interface BaseEntityInterface
    {
        public DateTime DataCriacao { get; set; }

        public bool Deleted { get; set; }
       
        public string? CriadoPorOid { get; set; }
       
        public string? CriadoPorUsername { get; set; }



    }
}
