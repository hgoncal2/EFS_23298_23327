using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.Models
{

    public class Email
    {
        /// <summary>
        /// Atributo correspondente ao endereço electrónico do Destinatário
        /// </summary>

        [Required(ErrorMessage = "O {0} é de prenchimento obrigatório")]
        public string Destinatario { get; set; }

        /// <summary>
        ///  Assunto do Email
        /// </summary>
        [Required(ErrorMessage = "O {0} é de prenchimento obrigatório")]
        public string Assunto { get; set; }

        /// <summary>
        ///  Corpo do Email
        /// </summary>
        [Required(ErrorMessage = "O {0} é de prenchimento obrigatório")]
        public string Corpo { get; set; }
    }
}
