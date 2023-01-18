namespace ProjectClasses
{
    public class SubjectWord
    {
        //Esta Clase Ejecuta las acciones comandadas mediante el objeto especial SubjectWord
        public int Cuantify { get; set; }
        public SubjectTypes SubjectType { get; set; }
        public ActionWord Action { get; set; }
        public SubjectWord(int number, string subject, ActionWord aw)
        {
            Cuantify = number;
            if (subject == "Ally")
                SubjectType = SubjectTypes.Ally;
            else if (subject == "Enemy")
                SubjectType = SubjectTypes.Enemy;
            else
                throw new Exception("Invalid Code");
            Action = aw;
        }
        public SubjectWord()
        {

        }

        //Opera mediante los comandos y dos cartas
        public ProjectClasses.ACard Operate(ProjectClasses.ACard card, ProjectClasses.ACard target)
        {
            if (Action.Value == "Heal")
            {
                target.VIT += Action.Cuantify;
            }
            if (Action.Value == "Atk")
            {
                target.VIT -= card.ATK;
            }
            if (Action.Value == "DefUp")
            {
                target.DEF += Action.Cuantify;
            }
            if (Action.Value == "AtkUp")
            {
                target.ATK += Action.Cuantify;
            }
            if (Action.Value == "DefDown")
            {
                target.DEF -= Action.Cuantify;
            }
            if (Action.Value == "VitDown")
            {
                target.VIT -= Action.Cuantify;
            }
            if (Action.Value == "AtkDown")
            {
                target.ATK -= Action.Cuantify;
            }
            return target;
        }
    }


    public enum SubjectTypes
    {
        Ally,
        Enemy
    }
}