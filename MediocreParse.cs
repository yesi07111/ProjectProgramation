namespace ProjectClasses
{
    public static class Parse
    {
        //Parse provisional
        public static List<string> GenerateWordsList(string text)
        {
            List<string> WordsList = new List<string>();
            string s = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (s == "end") break;
                if (text[i] == ';')
                {
                    WordsList.Add(s);
                    break;
                }
                if (text[i] != ' ')
                {
                    s += text[i];
                }
                else
                {
                    WordsList.Add(s);
                    s = "";
                }

            }
            return WordsList;
        }
    }
}