using EFS_23298_23306.Models;

namespace EFS_23298_23306.ViewModel
{
    public class TemasFotoViewModel
    {
        public int Id { get; set; }

        public String Tema { get; set; }
        public String Foto { get; set; }

        public TemasFotoViewModel(String Tema,String Foto)
        {

            this.Tema = Tema;
            this.Foto = Foto;
        }
    }
}
