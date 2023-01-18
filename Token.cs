namespace ProjectClasses
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType Type, string Value)
        {
            this.Type = Type;
            this.Value = Value;
        }

        public static void PrintToken(Token t)
        {
            Console.Write("(");
            System.Console.Write(t.Type.ToString());
            System.Console.Write(" , ");
            System.Console.Write(t.Value);
            System.Console.Write(")");
        }

        //Enumera todos los tipos de Tokens que soporta el lenguaje
    }
    public enum TokenType
    {
        Unknown, //Ç º
        Number, // 1,2,...
        Text, // abjksbask
        BooleanValues, //true, false
        Symbol, // ; ( ) { }
        Operator, // + * - / if
        Keyword, // Card
        Identifier, // Destroyer (Card Destroyer)

        #region Words From LP
        MasterWord, //Lenguaje Reservade Word
        SubjectWord, //Ally and Enemy
        ActionWord, //Heal, Atk, etc
        #endregion
    }
}