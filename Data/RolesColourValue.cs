using System.Data;

namespace EFS_23298_23327.Data
{
    public static class RolesColourValue
    {
        public static String Admin = "danger";
        public static String Anfitriao = "warning";
        public static String Cliente = "success";



        public static String GetRoleColour(String role) {

            if (role.Equals("Admin")) {
                return Admin;

            }
            if (role.Equals("Anfitriao")) {
                return Anfitriao;

            }
            if (role.Equals("Cliente")) {
                return Cliente;

            }


            return "";

        }
    }
}
