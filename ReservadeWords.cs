namespace ProjectClasses
{
    public static class ReservadeWords
    {
        //Lista de palabras reservadas
        public static string[] ListOFReservedWords = { "card", "attribute", "effects", "description", "atype",
    "element", "race", "classe", "atk", "def", "vit", "name", "lore", "type", "affilliations", "evolname1", "evolname2",
    "job", "faith", "militar", "capital", "knowledge", "element1", "element2", "element3", "element4", "single", "multiple",
    "ally","enemy", "heal"};

        public static string[] SubjectWords = { "Ally", "Enemy" };

        public static string[] ActionWords = { "Heal", "Atk", "DefUp", "AtkUp", "DefDown", "VitDown", "AtkDown" };

        public static string[] CardNames = {"E1", "Document", "The Big Robot", "The Ascencion Is Dosen't Like You",
        "Glenn Branca", "NothingFace Is Really Happen", "The Argument", "Last Chance For a Slow Dance", "Big Stuff", "Smoke Robinson",
        "The Archer", "Magnataurus", "Bolivian Drug Store", "Guitar", "Chuck", "Almedr78", "This Plant is Amazing", "Copernicus Flower",
        "Srtotenheim", "Translate", "Compiler Error is a Joke", "No more Numbers Anymore", "I'm Exclusive in Houston", "Mask",
        "Perfect Hair"};

        public static string[] CardLore =
        {"Y besa con ira el duro", "Ya que la vida del hombre no es sin0 una acci6n a distancia",
        "i Atencibn, seiioras y seiiores! iun momento de atencibn! ", "Ustedes poseen un séptimo sentido ",
        "Me impuse que una de ellas ", "Dormir en los bancos de las plazas", "Ya he terminado mis estudios",
        "Puede dejarnos a todos en la miseria mbs espantosa", "Todo ha terminado entre nosotros", "Por aquel tiempo yo rehuia las escenas demasiado",
        "Como un globo que se desinfla mi alma perdía altura", "La otra mitad de mi ser pisionera en un hoyo", "Los delincuentes modernos",
        "Los vicios del mundo moderno"};


        //Comprueba sin una palabra es una palabra reservada de tipo subject
        public static bool IsASubjectWord(string s)
        {
            for (int i = 0; i < SubjectWords.Length; i++)
            {
                if (s == SubjectWords[i]) return true;
            }
            return false;
        }
        //Comprueba sin una palabra es una palabra reservada de tipo action
        public static bool IsAnActionWords(string s)
        {
            for (int i = 0; i < ActionWords.Length; i++)
            {
                if (s == ActionWords[i]) return true;
            }
            return false;
        }

        //Dado una palabra clave crea sus objetos correspondientes
        public static void Building(string s)
        {
            if (s == "card")
            {
                /*
                ProjectClasses.Attribute a = new ProjectClasses.Attribute();
                ProjectClasses.Effects e = new ProjectClasses.Effects();
                ProjectClasses.Description d = new ProjectClasses.Description();
                ProjectClasses.ACard ac = new ProjectClasses.ACard(a, e, d);
                ProjectClasses.ACard.PrintCard(ac);
                */
            }
        }


        //Devuelve True si es una palabra reservada
        public static bool IsAReservadeWord(string s)
        {
            for (int i = 0; i < ListOFReservedWords.Length; i++)
            {
                if (ListOFReservedWords[i] == s) return true;
            }
            return false;
        }
    }
}