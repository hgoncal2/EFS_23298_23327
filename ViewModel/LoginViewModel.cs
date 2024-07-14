using Azure.Identity;

namespace EFS_23298_23327.ViewModel
{
    public class LoginViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Construtor que recebe Username e Password
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public LoginViewModel(String Username,String Password) { 

            this.Username = Username;
            this.Password = Password;
        
        }

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public LoginViewModel() {


        }
    }
}
