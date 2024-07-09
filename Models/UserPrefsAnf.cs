using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class UserPrefsAnf
    {
        [Key]
        public int UserPrefId { get; set; }
        [ForeignKey(nameof(Anfitrioes))]
        public string AnfId { get; set; }
        public Anfitrioes? Anfitriao { get; set; }
        public ICollection<UserPrefAnfCores> Cores { get; set; }
        public bool mostrarCanceladas { get; set; }


       
        public UserPrefsAnf() {
            this.Cores = new HashSet<UserPrefAnfCores>();
        }
        
    }
}
