namespace ProjectClasses
{
    public static class LexicalAnalyzer
    {
        //Toma lo devuelto por el parser y lo analiza lexicográficamente (clasifica las palabras en tokens, creando así:
        //una lista de tokens del programa)
        public static List<Token> ClasifyWords(List<string> wordlist)
        {
            List<Token> ClasifyW = new List<Token>();
            for (int i = 0; i < wordlist.Count; i++)
            {

                if (AuxiliarMethods.IsANumber(wordlist[i]))
                {
                    Token t = new Token(TokenType.Number, wordlist[i]);
                    ClasifyW.Add(t);
                    continue;
                }
                if (AuxiliarMethods.IsAOperatorSymbol(wordlist[i]))
                {
                    Token t = new Token(TokenType.Operator, wordlist[i]);
                    ClasifyW.Add(t);
                    continue;
                }
                if (ReservadeWords.IsAReservadeWord(wordlist[i]))
                {
                    if (wordlist[i] == "card")
                        ReservadeWords.Building(wordlist[i]);
                }


            }
            return ClasifyW;
        }


        //recibe un token de tipo operador y crea su respectivo objeto de operación
        public static BinaryOperator CreateBinaryOperatorToken(Token left, Token t, Token right)
        {
            if (t.Type != TokenType.Operator) throw new Exception("Create Operator Token Can Only Recieves a Operator Token");
            if (t.Value == "<")
            {
                MinorTo mt = new MinorTo(left, right);
                return mt;
            }
            if (t.Value == "-")
            {
                Minus m = new Minus(left, right);
                return m;
            }
            if (t.Value == "+")
            {
                Add a = new Add(left, right);
                return a;
            }
            return new Minus(right, left);
            throw new Exception("Operator Not Defined");
        }

        //Devuelve el fragmento de texto tomado a partir de una posición del texto original
        public static string RestOfText(int pos, string text)
        {
            string fragment = "";
            if (pos >= text.Length) return fragment;
            for (int i = pos; i < text.Length; i++)
            {
                fragment += text[i];
            }
            return fragment;
        }
        //Devuelve el contenido de una expresión (una expresión es todo aquel texto definido entre ( ))
        public static string ContentOfExpression(string text, char opencaracter, char closedcaracter)
        {
            string content = "";
            int niv = 0;
            if (!AuxiliarMethods.CorrectOperacionalOrdererSymbols(text, opencaracter, closedcaracter)) throw new Exception("Invalid Code");
            bool start = false;
            for (int i = 0; i < text.Length; i++)
            {
                if ((!start))
                {
                    if (text[i] == opencaracter)
                    {
                        start = true;
                        niv++;
                        continue;
                    }
                    else
                        continue;
                }
                if (text[i] == ' ') continue;
                if (text[i] == opencaracter) niv++;
                if (text[i] == closedcaracter) niv--;
                if (niv == 0) break;
                if (i != 0)
                    content += text[i];
            }
            return content;
        }

        //Toma el string de un código y devuelve el código en una lista para su mejor interpretación
        public static List<string> CodeTransformForAnalysis(string code)
        {
            if (!AuxiliarMethods.CorrectOperacionalOrdererSymbols(code, '(', ')')) throw new Exception("Invalid Code, Code Don't Respect Correct Parenthesis");
            List<string> CodeForAnalisis = new List<string>();
            string s = "";
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == ')')
                {
                    s = "";
                    continue;
                }
                if (code[i] == '(')
                {
                    CodeForAnalisis.Add(s);
                    s = ContentOfExpression(RestOfText(i, code), '(', ')');
                    CodeForAnalisis.Add(s);
                    s = "";
                    continue;
                }
                if (code[i] != ' ')
                    s += code[i];
            }
            return CodeForAnalisis;
        }

        //Transforma la escritura de un efecto en una lista de string
        public static List<string> TransformCodeForEffect(string code)
        {
            List<string> CodeForAnalisis = new List<string>();
            string s = "";
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == ' ') continue;
                if ((code[i] == '=') || (code[i] == ';'))
                {
                    CodeForAnalisis.Add(s);
                    s = "";
                    continue;
                }
                s += code[i];
            }
            return CodeForAnalisis;
        }

        //Comprueba si un efecto está escrito correctamente
        public static bool EffectIsCorrectWriting(List<string> effect)
        {
            if (effect.Count != 12) throw new Exception("Invalid Code: Excess or lack of properties");
            bool[] mask = new bool[12];
            for (int i = 0; i < effect.Count; i++)
            {
                if (effect[i] == "Target")
                {
                    if (AuxiliarMethods.IsNotOutArray(i + 1, 12))
                    {
                        List<string> temp = LexicalAnalyzer.CodeTransformForAnalysis(effect[i + 1]);
                        if (temp.Count != 2) throw new Exception("Invalid Code");
                        if ((ReservadeWords.IsASubjectWord(temp[0])) && (AuxiliarMethods.IsANumber(temp[1])))
                        {
                            mask[i] = true;
                            mask[i + 1] = true;
                        }
                        else
                        {
                            throw new Exception("Invalid Code");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Code");
                    }
                }
                if (effect[i] == "Action")
                {
                    if (AuxiliarMethods.IsNotOutArray(i + 1, 12))
                    {
                        List<string> temp = LexicalAnalyzer.CodeTransformForAnalysis(effect[i + 1]);
                        if (temp.Count != 2) throw new Exception("Invalid Code");
                        if (ReservadeWords.IsAnActionWords(temp[0]) && AuxiliarMethods.IsANumber(temp[1]))
                        {
                            mask[i] = true;
                            mask[i + 1] = true;
                        }
                        else
                        {
                            throw new Exception("Invalid Code");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Code");
                    }
                }
                if ((effect[i] == "FaithCost") || (effect[i] == "MilitaryCost")
                || (effect[i] == "KnowledgeCost") || (effect[i] == "CapitalCost"))
                {
                    if (AuxiliarMethods.IsNotOutArray(i + 1, 12))
                    {
                        if (AuxiliarMethods.IsANumber(effect[i + 1]))
                        {
                            mask[i] = true;
                            mask[i + 1] = true;
                        }
                        else
                        {
                            throw new Exception("Invalid Code");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Code");
                    }
                }
            }
            for (int i = 0; i < mask.Length; i++)
            {
                if (!mask[i]) return false;
            }
            return true;
        }
    }
}