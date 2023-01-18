namespace ProjectClasses
{

public class Conditionalif
{
    public List<Token> Condition { get; set; }
    public List<Token> Action { get; set; }

    public Conditionalif(List<string> Condition, List<string> Action)
    {
        this.Condition = LexicalAnalyzer.ClasifyWords(Condition);
        this.Action = LexicalAnalyzer.ClasifyWords(Action);
    }





    //Comprueba si el usuario introdujo correctamente la estructura de un if 
    public static bool CheckConditionalIfIsCorrectlyWritting(List<string> WordList)
    {
        string[] array = { "if", "(", ")", "then", "(", ")" };
        bool[] mask = { false, false, false, false, false, false };
        string s = "";
        int index = 0;
        for (int i = 0; i < WordList.Count; i++)
        {
            if (index == mask.Length) return true;
            if (WordList[i] == array[index])
            {
                mask[index] = true;
                index++;
            }
        }
        throw new Exception("Conditional Expression Incorrect");
        //return false;
        /*for (int i = 0; i < mask.Length; i++)
         {
             if (mask[i] == false)
             {
                 throw new Exception("Conditional Expression Incorrect");
                 //return false;
             }
         }
         return true;*/
    }
}
}