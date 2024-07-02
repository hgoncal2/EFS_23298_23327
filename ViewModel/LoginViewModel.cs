using Azure.Identity;

namespace EFS_23298_23327.ViewModel
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        public String Username { get; set; }
        public String Password { get; set; }

        public LoginViewModel(String Username,String Password) { 

            this.Username = Username;
            this.Password = Password;
        
        }
        public LoginViewModel() {


        }
    }
}
