namespace ProjectClasses
{
    public static class AuxiliarMethods
    {
        /*
        public static int GiveNumberOfThisRespectiveRace(string race)
        {
            for (int i = 0; i < ProjectClasses.Description.racesnames.Length; i++)
            {
                if (ProjectClasses.Description.racesnames[i] == race) return i + 1;
            }
            throw new Exception("Race Dosen't Exist");
        }
        */
        //Comprueba si cierta posicion escapa del array

        public static ProjectClasses.ACard CreateRandomCard()
        {
            Random r = new Random();


            ProjectClasses.Attribute attribute = new ProjectClasses.Attribute(r.NextDouble() * 10000,
            r.NextDouble() * 10000, r.NextDouble() * 10000, new ProjectClasses.Element(r.Next(1, 12)));

            int typeOfCard = r.Next(1, 2);
            int cantEffects = 82;
            if (typeOfCard == 1)
            {
                cantEffects = 6;
            }
            else
            {
                cantEffects = 21;
            }
            int singleormidarea = r.Next(1, 2);

            ProjectClasses.Effects eff = new ProjectClasses.Effects(new ProjectClasses.SingleEffect(cantEffects, typeOfCard),
            new ProjectClasses.ElementalDamage(singleormidarea, r.Next(0, 11)));

            ProjectClasses.Description descr = new ProjectClasses.Description
            (ReservadeWords.CardNames[r.Next(0, ReservadeWords.CardNames.Length - 1)],
            ReservadeWords.CardLore[r.Next(0, ReservadeWords.CardLore.Length - 1)],
            new ProjectClasses.AType(typeOfCard), new ProjectClasses.Race(r.Next(1, 9)),
            r.NextDouble() * 1000, r.NextDouble() * 1000, r.NextDouble() * 1000, r.NextDouble() * 1000);

            ProjectClasses.ACard ac = new ProjectClasses.ACard(attribute, eff, descr);
            return ac;
        }
        public static void PrintCard(ProjectClasses.ACard ac)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("name = " + ac.CardName);
            System.Console.WriteLine("{");
            System.Console.WriteLine("lore = " + ac.CardLore);
            System.Console.WriteLine("type = " + ac.CardTypeName);
            System.Console.WriteLine("race = " + ac.RaceName);
            System.Console.WriteLine("faith cost = " + ac.FaithCost);
            System.Console.WriteLine("military cost = " + ac.MilitarCost);
            System.Console.WriteLine("capital cost = " + ac.CapitalCost);
            System.Console.WriteLine("knowledge cost = " + ac.KnowledgeCost);
            System.Console.WriteLine("ATK = " + ac.ATK);
            System.Console.WriteLine("DEF = " + ac.DEF);
            System.Console.WriteLine("VIT = " + ac.VIT);
            System.Console.WriteLine("element1 = " + ac.CardElementName1);
            System.Console.WriteLine("element2 = " + ac.CardElementName2);
            System.Console.WriteLine("element3 = " + ac.CardElementName3);
            System.Console.WriteLine("element4 = " + ac.CardElementName4);
            System.Console.WriteLine("}");

        }
        public static bool IsNotOutArray(int pos, int size)
        {
            return ((pos >= 0) && (pos < size)) ? true : false;
        }

        //Imprime una lista
        public static void PrintList<T>(List<T> list)
        {
            System.Console.WriteLine();
            System.Console.Write("{ ");
            for (int i = 0; i < list.Count; i++)
            {
                System.Console.Write(list[i]);
                if (i < list.Count - 1) System.Console.Write(" , ");
            }
            System.Console.Write(" }");
        }

        //Comprueba si un string s es un número
        public static bool IsANumber(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i])) return false;
            }
            return true;
        }
        //Imprime una lista de Tokens     
        public static void PrintListToken(List<Token> TList)
        {
            for (int i = 0; i < TList.Count; i++)
            {
                Token.PrintToken(TList[i]);
            }
        }
        //Comprueba si un string es un símbolo de operación
        public static bool IsAOperatorSymbol(string s)
        {
            switch (s)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case ">":
                case "<":
                case "==":
                case ">=":
                case "<=":
                    return true;
                default: return false;
            }
        }

        //Imprime un Binary Operator
        public static void PrintBinaryOperator(BinaryOperator bo)
        {
            System.Console.WriteLine();
            Token.PrintToken(bo.Left);
            System.Console.Write(" " + bo.Symbol + " ");
            Token.PrintToken(bo.Right);
            System.Console.Write(" = ");
            Token.PrintToken(bo.Value);
            System.Console.WriteLine();
        }

        //Comprueba si es posible operar dos valores numericos en un operador aritmético binario
        public static bool IsPosibleOperateTwoNumbersValueToArithmeticianOperate(Token Left, Token Right)
        {
            return ((Left.Type == TokenType.Number) && (Right.Type == TokenType.Number)) ? true : false;
        }

        //Comprueba si existe un if en la lista de palabras
        public static bool ComprobateExistenceIf(List<string> ListOfWords)
        {
            for (int i = 0; i < ListOfWords.Count; i++)
            {
                if (ListOfWords[i] == "if") return true;
            }
            return false;
        }
        //Si t es un operador booleano devuelve true
        public static bool IsABooleanOperator(Token t)
        {
            string[] list = { "<", "<=", "==", ">", ">=" };
            for (int i = 0; i < list.Length; i++)
            {
                if (t.Value == list[i]) return true;
            }
            return false;
        }
        //Comprueba si una expresión tiene correctamente escritos los símbolos de aislamiento operacional
        public static bool CorrectOperacionalOrdererSymbols(string s, char caracteropen, char caracterclosed)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (count < 0) return false;
                if (s[i] == caracteropen) count++;
                if (s[i] == caracterclosed) count--;
            }
            if (count != 0) return false;
            return true;
        }

    }
}