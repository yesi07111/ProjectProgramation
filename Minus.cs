namespace ProjectClasses
{
    public class Minus : BinaryOperator, IOperator
    {
        public Minus(Token Left, Token Right)
        {
            this.Symbol = "-";
            this.Right = Right;
            this.Left = Left;
            if (IsPosibleOperate())
            {
                Operate();
            }
            else
            {
                throw new Exception("Two Values Can't Be Operated");
            }
        }

        //Devuelve true si es posible operar
        public bool IsPosibleOperate()
        {
            return AuxiliarMethods.IsPosibleOperateTwoNumbersValueToArithmeticianOperate(Left, Right);
        }

        //Opera y crea un token con el resultado
        public void Operate()
        {
            string value = (int.Parse(Right.Value) - int.Parse(Left.Value)).ToString();
            Token t = new Token(TokenType.Number, value);
            this.Value = t;
        }
    }
}