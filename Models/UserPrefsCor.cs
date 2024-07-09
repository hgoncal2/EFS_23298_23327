using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class UserPrefAnfCores
    {
        [Key]
        public int Id { get; set; }

        public int Key { get; set; }
        public string Value { get; set; }

        public int UserPrefId { get; set; }
        public UserPrefsAnf UserPrefsAnf { get; set; }
    }
}
