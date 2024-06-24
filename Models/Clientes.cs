namespace EFS_23298_23306.Models
{
    public class Clientes:Utilizadores
    {
        public ICollection<Reservas>? ListaReservas { get; set; }

        public Clientes() {

            ListaReservas = new HashSet<Reservas>();
        }
    }
}
