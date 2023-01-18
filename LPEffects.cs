namespace ProjectClasses
{
    public class LPEffects
    {
        public LPEffects(ProjectClasses.Player user, List<double> costs,
        ProjectClasses.ACard card, ProjectClasses.ACard[] target, ProjectClasses.Player enemy, ObjectEffect e)
        {

        }

        //Crea el efecto escrito por el usuario
        public static ProjectClasses.Player[] UseEffect(ProjectClasses.Player user, List<double> costs,
        ProjectClasses.ACard card, ProjectClasses.ACard[] target, ProjectClasses.Player enemy, ObjectEffect e)
        {
            //si la carta no está en el campo entonces no se activa su efecto
            //         if (user.AField.IsInField(card))
            //          {
            //              return new ProjectClasses.Player[] { user, enemy };
            //        }

            //en caso de existir en el campo, tomo su ubicación 
            //           (int x, int y) loc = user.AField.Find(card);

            for (int i = 0; i < target.Length; i++)
            {
                //si el objetivo actual no se encuentra en el campo enemigo se retorna
                //               if (!enemy.AField.IsInField(target[i]))
                //              {
                //                   return new ProjectClasses.Player[] { user, enemy };
                //               }

                //tomo la ubicación del objetivo actual del campo enemigo
                //               (int x, int y) loc2 = enemy.AField.Find(target[i]);

                //le aplica el efecto
                ProjectClasses.ACard acard = e.Operate(card, target[i]);

                //cobra los costos
                user.Kingdom.Capital -= e.CapitalCost;
                user.Kingdom.Faith -= e.FaithCost;
                user.Kingdom.Knowledge -= e.KnowledgeCost;
                user.Kingdom.Militarism -= e.MilitaryCost;

                //y si es aliada
                if (e.Action.SubjectType == SubjectTypes.Ally)
                {
                    //las coloco en el campo, ya modificada la carta objetivo
                    //                  user.AField.SetCard(card, loc);
                    //                 user.AField.SetCard(acard, loc2);
                    return new ProjectClasses.Player[] { user, enemy };
                }
            }

            throw new NotImplementedException("Aún en Desarrollo");
        }

        public static bool ThisCodeCorrectWriting(List<string> code)
        {
            if (ReservadeWords.IsASubjectWord(code[0]))
            {
                if (!AuxiliarMethods.IsANumber(code[1])) return false;
                if (!AuxiliarMethods.IsANumber(code[3])) return false;
                if (!ReservadeWords.IsAnActionWords(code[2])) return false;
                return true;
            }
            return false;
        }

    }

}

