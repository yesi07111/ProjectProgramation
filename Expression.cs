namespace ProjectClasses
{
//Las expresiones se definen como una lista de Tokens y opcionalmente una lista de Expresiones
public class Expresion
{
    public List<Token> ExpressionTokens { get; set; }
    public List<Expresion> Childs { get; set; }
    public Expresion(List<Token> ExpressionTokens, List<Expresion> Childs)
    {
        this.ExpressionTokens = ExpressionTokens;
        this.Childs = Childs;
    }
}
}