namespace ProjectClasses
{
    public class ObjectEffect
    {
        public static string[] RandomNamesForObjectEffect = { "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8", "e9", "e10", "e11", "e12", "e13", "e14", "e15" };
        public SubjectWord Action { get; set; }
        public double FaithCost { get; set; }
        public double MilitaryCost { get; set; }
        public double KnowledgeCost { get; set; }
        public double CapitalCost { get; set; }
        public string Description { get; set; }
        public (string description, string name) Tuple { get; set; }
        public int ID { get; set; }

        public Func<ProjectClasses.Player, List<double>,
            ProjectClasses.ACard, ProjectClasses.ACard[], ProjectClasses.Player, string, ProjectClasses.Player[]> action
        { get; set; }

        public ObjectEffect(SubjectWord Action, double FaithCost, double MilitaryCost, double KnowledgeCost, double CapitalCost)
        {
            ID = MakeRandomID();
            this.Action = Action;
            this.MilitaryCost = MilitaryCost;
            this.FaithCost = FaithCost;
            this.KnowledgeCost = KnowledgeCost;
            this.CapitalCost = CapitalCost;
            string s = "";
            if (Action.SubjectType == SubjectTypes.Ally)
            {
                s = "Allies";
            }
            else
            {
                s = "Enemies";
            }
            Description = "The " + Action.Action.Value + " effect with a value of " + Action.Action.Cuantify + " points will be applied to " + Action.Cuantify + " " + s;
            Random r = new Random();
            this.Tuple = (Description, RandomNamesForObjectEffect[r.Next(0, RandomNamesForObjectEffect.Length - 1)]);
        }
        public ObjectEffect(SubjectWord Action, double FaithCost, double MilitaryCost, double KnowledgeCost, double CapitalCost, Func<ProjectClasses.Player, List<double>,
           ProjectClasses.ACard, ProjectClasses.ACard[], ProjectClasses.Player, string, ProjectClasses.Player[]> action)
        {
            ID = MakeRandomID();
            this.Action = Action;
            this.MilitaryCost = MilitaryCost;
            this.FaithCost = FaithCost;
            this.KnowledgeCost = KnowledgeCost;
            this.CapitalCost = CapitalCost;
            string s = "";
            if (Action.SubjectType == SubjectTypes.Ally)
            {
                s = "Allies";
            }
            else
            {
                s = "Enemies";
            }
            Description = "The " + Action.Action.Value + " effect with a value of " + Action.Action.Cuantify + " points will be applied to " + Action.Cuantify + " " + s;
            Random r = new Random();
            this.Tuple = (Description, RandomNamesForObjectEffect[r.Next(0, RandomNamesForObjectEffect.Length - 1)]);
            this.action = action;
        }

        //Este Constructor Asume que el código introducido es correcto, para serciorarse de ello debe usar el método EffectIsCorrectWriting de la clase LexicalAnalyzer
        public ObjectEffect(List<string> code)
        {
            ActionWord aw = new ActionWord();
            for (int i = 0; i < code.Count; i++)
            {
                if (code[i] == "Action")
                {
                    List<string> temp = LexicalAnalyzer.CodeTransformForAnalysis(code[i + 1]);
                    aw = new ActionWord(temp[0], int.Parse(temp[1]));
                }
            }
            SubjectWord sw = new SubjectWord();
            int FCost = 0;
            int MCost = 0;
            int CCost = 0;
            int KCost = 0;
            for (int i = 0; i < code.Count; i++)
            {
                if (code[i] == "Target")
                {
                    List<string> temp = LexicalAnalyzer.CodeTransformForAnalysis(code[i + 1]);
                    sw = new SubjectWord(int.Parse(temp[1]), temp[0], aw);
                }
                if (code[i] == "FaithCost")
                    FCost = int.Parse(code[i + 1]);
                if (code[i] == "MilitaryCost")
                    MCost = int.Parse(code[i + 1]);
                if (code[i] == "CapitalCost")
                    CCost = int.Parse(code[i + 1]);
                if (code[i] == "KnowledgeCost")
                    KCost = int.Parse(code[i + 1]);
            }
            this.Action = sw;
            this.CapitalCost = CCost;
            this.FaithCost = FCost;
            this.KnowledgeCost = KCost;
            this.MilitaryCost = MCost;
            string s = "";
            if (Action.SubjectType == SubjectTypes.Ally)
            {
                s = "Allies";
            }
            else
            {
                s = "Enemies";
            }
            Description = "The " + Action.Action.Value + " effect with a value of " + Action.Action.Cuantify + " points will be applied to " + Action.Cuantify + " " + s;
            Random r = new Random();
            this.Tuple = (Description, RandomNamesForObjectEffect[r.Next(0, RandomNamesForObjectEffect.Length - 1)]);
        }
        //Constructor que crea un ObjectEffect solo introduciendo el código
        public ObjectEffect(string code)
        {
            ObjectEffect oe = new ObjectEffect((LexicalAnalyzer.TransformCodeForEffect(LexicalAnalyzer.ContentOfExpression(code, '{', '}'))));
            this.Action = oe.Action;
            this.CapitalCost = oe.CapitalCost;
            this.Description = oe.Description;
            this.FaithCost = oe.FaithCost;
            this.KnowledgeCost = oe.KnowledgeCost;
            this.MilitaryCost = oe.MilitaryCost;
            this.Tuple = oe.Tuple;
        }

        public static void PrintEffect(ObjectEffect e)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Effect ");
            System.Console.WriteLine("{ ");
            System.Console.WriteLine("Action = " + e.Description);
            System.Console.WriteLine("FaithCost = " + e.FaithCost);
            System.Console.WriteLine("MilitaryCost = " + e.MilitaryCost);
            System.Console.WriteLine("CapitalCost = " + e.CapitalCost);
            System.Console.WriteLine("KnowledgeCost =" + e.KnowledgeCost);
            System.Console.WriteLine("}");
        }

        public ProjectClasses.ACard Operate(ProjectClasses.ACard card, ProjectClasses.ACard target)
        {
            return Action.Operate(card, target);
        }

        public int MakeRandomID()
        {
            Random r = new();
            return r.Next() * 10 + r.Next() * 100 + r.Next() * 1000;
        }
    }
}