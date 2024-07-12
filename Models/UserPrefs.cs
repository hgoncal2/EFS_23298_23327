using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class UserPrefs
    {
        [Key]
        public int Id { get; set; }
        public String UtilizadorId {  get; set; }
        public Boolean alertas {  get; set; }
        public Boolean reservas {  get; set; }
        public UserPrefs(Utilizadores u)
        {
            this.UtilizadorId = u.Id;
        }
    }
}
