namespace EFS_23298_23306.ViewModel
{
    public class RegisterViewModel
    {


        public String? PrimeiroNome { get; set; }
        public  String?  UltimoNome { get; set; }
        public String? Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public  HashSet<String> Roles { get; set; } = new HashSet<String>();
    }
}
