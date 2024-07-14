using EFS_23298_23327.Models;

namespace EFS_23298_23327.ViewModel
{
    public class ReservasDashboardViewModel
    {
        /// <summary>
        /// Lista de Reservas
        /// </summary>
        public virtual ICollection<Reservas> Reservas { get; set; }

        /// <summary>
        /// Preferências do Anfitrião
        /// </summary>
        public UserPrefsAnf UserPrefs { get; set; }


        /// <summary>
        /// Construtor que recebe parametros UserPrefsAnf e lista de Reservas
        /// </summary>
        /// <param name="userPrefs"></param>
        /// <param name="r"></param>
        public ReservasDashboardViewModel(UserPrefsAnf userPrefs,ICollection<Reservas> r) {
            this.Reservas = r;
            this.UserPrefs = userPrefs;
        }
    }

}
