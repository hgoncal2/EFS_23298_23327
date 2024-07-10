using Humanizer;

namespace EFS_23298_23327.Data
{
    public class AppUtils
    {
        public static string GetHumanReadableCount(string s, int c) {
            return s.ToQuantity(c);



        }
    }
}
