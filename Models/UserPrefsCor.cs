using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFS_23298_23327.Models
{
    public class UserPrefAnfCores
    {

        /// <summary>
        /// Id da UserPrefAnfCores
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Id das preferencias do user
        /// </summary>
        public int UserPrefId { get; set; }

        /// <summary>
        /// Preferências do Anfitrião
        /// </summary>
        public UserPrefsAnf UserPrefsAnf { get; set; }
    }
}
