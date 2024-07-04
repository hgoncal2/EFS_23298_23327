namespace EFS_23298_23327.Data
{
    public static class DifficultiesValue
    {
        public static int facil = 25;
        public static int normal = 50;
        public static int dificil = 75;
        public static int sherlock = 100;

        public static String GetDifficultyColor(int difficulty) {



            if (difficulty == 0) return "success";
            if (difficulty == 1) return "warning";
            if (difficulty == 2) return "danger";
            if (difficulty == 3) return "secondary";
            return "";

        }
        public static int GetDifficultyStr(int difficulty) {



            if (difficulty == 0) return facil;
            if (difficulty == 1) return normal;
            if (difficulty == 2) return dificil;
            if (difficulty == 3) return sherlock;
            return 0;

        }

    }
}
