

using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23306.Models;
    

    public class Utilizadores
    {
    [Key]
        public int UtilizadorID { get; set; }
        public String? PrimeiroNome { get; set; }
        public String? UltimoNome { get; set; }

        public required String Password { get; set; }
        public String? Email { get; set; }
        public String? NumeroTelemovel { get; set; }
        public DateTime DataCriacao { get; set; }
       
        



    public Utilizadores()
        {
            this.DataCriacao = DateTime.Now;
            

    }

    }

