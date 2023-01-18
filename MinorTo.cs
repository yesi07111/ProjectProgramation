namespace ProjectClasses
{
    public class MinorTo : BinaryOperator, IOperator //<
    {
        public MinorTo(Token Left, Token Right)
        {
            this.Symbol = "<";
            this.Left = Left;
            this.Right = Right;
            if (IsPosibleOperate())
            {
                Operate();
            }
            else
            {
                throw new Exception("Two Values Can't Be Operated");
            }
        }

        public bool IsPosibleOperate()
        {
            return AuxiliarMethods.IsPosibleOperateTwoNumbersValueToArithmeticianOperate(Left, Right);
        }

        public void Operate()
        {
            if (int.Parse(Left.Value) < int.Parse(Right.Value))
            {
                Token t = new Token(TokenType.BooleanValues, "True");
                this.Value = t;
            }
            else
            {
                Token t = new Token(TokenType.BooleanValues, "False");
                this.Value = t;
            }
        }
    }
}