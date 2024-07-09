using EFS_23298_23327.Models;

namespace EFS_23298_23327.ViewModel
{
    public class ReservasDashboardViewModel
    {

        public virtual ICollection<Reservas> Reservas { get; set; }
        public UserPrefsAnf UserPrefs { get; set; }

        public ReservasDashboardViewModel(UserPrefsAnf userPrefs,ICollection<Reservas> r) {
            this.Reservas = r;
            this.UserPrefs = userPrefs;
        }
    }

}
