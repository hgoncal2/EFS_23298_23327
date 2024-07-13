using System.ComponentModel;
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
        [DisplayName("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DisplayName("Chave")]
        public int Key { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DisplayName("Valor")]
        public string Value { get; set; }

        /// <summary>
        /// Id das preferencias do user
        /// </summary>
        [DisplayName("Id das Preferências do Utilizador")]
        public int UserPrefId { get; set; }

        /// <summary>
        /// Preferências do Anfitrião
        /// </summary>
        [DisplayName("Preferências do Anfitrião")] 
        public UserPrefsAnf UserPrefsAnf { get; set; }
    }
}
