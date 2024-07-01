using Microsoft.AspNetCore.Identity;

namespace EFS_23298_23327.Models
{
    public class Anfitrioes:Utilizadores
    {
        public ICollection<Salas>? ListaSalas { get; set; }

        public Anfitrioes()
        {
            this.ListaSalas = new HashSet<Salas>();
        }

    }
}
