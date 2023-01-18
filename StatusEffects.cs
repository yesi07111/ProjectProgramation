namespace ProjectClasses
{
    /// <summary>Clase de los estado.</summary>
    public class StatusEffect : ElementalDamage
    {
        //Diccionario que relaciona el nombre del efecto y su método
        public Dictionary<string, Func<ACard, ACard>> StatesEffectPerTurn { get; set; }

        public Dictionary<string, string> StatesandDescription { get; set; }

        public StatusEffect()
        {
            StatesEffectPerTurn = new();
            StatesandDescription = new();

            #region Efecto por turno de estados

            StatesEffectPerTurn.Add(states[1], Burned);
            StatesEffectPerTurn.Add(states[2], Poisoned);
            StatesEffectPerTurn.Add(states[3], Petrified);
            StatesEffectPerTurn.Add(states[4], Confused);
            StatesEffectPerTurn.Add(states[5], Paralyzed);
            StatesEffectPerTurn.Add(states[6], Freezed);
            StatesEffectPerTurn.Add(states[7], Bleeding);
            StatesEffectPerTurn.Add(states[8], Infected);
            StatesEffectPerTurn.Add(states[9], Tied);
            StatesEffectPerTurn.Add(states[10], Blinded);
            StatesEffectPerTurn.Add(states[11], Cursed);
            StatesEffectPerTurn.Add(states[12], DefenseBroken);
            StatesEffectPerTurn.Add(states[13], Infected);

            StatesandDescription.Add(states[1], "Card will receive a damage of 150 ~ 600!");
            StatesandDescription.Add(states[2], "Card will receive a damage of 250 ~ 400!");
            StatesandDescription.Add(states[3], "Card will reduce is DEF in half!");
            StatesandDescription.Add(states[4], "Card can't choose targets!");
            StatesandDescription.Add(states[5], "Card will receive a damage of 100 ~ 300 and can't use effects!");
            StatesandDescription.Add(states[6], "Card will receive a damage of 100 ~ 500 and can't attack!");
            StatesandDescription.Add(states[7], "Card's DEF will be 0!");
            StatesandDescription.Add(states[8], "Card will receive a damage of 500 ~ 1500!");
            StatesandDescription.Add(states[9], "If card is infected receive damage of 300 ~ 500!");
            StatesandDescription.Add(states[10], "Card will receive a damage of 100 ~ 200 and can't attack!");
            StatesandDescription.Add(states[11], "Card can't attack or use effects!");
            StatesandDescription.Add(states[12], "Card will receive a damage of 700 ~ 2000!");
            StatesandDescription.Add(states[13], "CArd will restores VIT in half of the damage dealed to the infected card!");

            #endregion

            base.EBStatesEffectPerTurn = StatesEffectPerTurn;
        }

        #region Efectos de estado por turno:

        /// <summary>Efecto de estado: Quemado. Realiza un daño que oscila entre 150 y 600.</summary>
        public static ACard Burned(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(150, 601);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);

            return tgt;
        }

        /// <summary>Efecto de estado: Envenedado. Realiza un daño que oscila entre 250 y 400.</summary>
        public static ACard Poisoned(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(250, 400);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);
            return tgt;
        }

        /// <summary>Efecto de estado: Petrificado. La DEF se reduce un 50%.</summary>
        public static ACard Petrified(ACard tgt)
        {
            tgt.DEF -= tgt.DEF / 2;
            return tgt;
        }

        /// <summary>Efecto de estado: Confuso. Ataca a un enemigo o aliado de forma random.</summary>
        public static ACard Confused(ACard tgt)
        {
            tgt.CanSelectTarget = false;
            return tgt;
        }

        /// <summary>Efecto de estado: Paralizado. Realiza un daño que oscila entre 100 y 300. La carta no puede usar efectos.</summary>
        public static ACard Paralyzed(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(100, 300);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);

            tgt.CanUseEffect = false;
            return tgt;
        }

        /// <summary>Efecto de estado: Congelado. Realiza un daño que oscila entre 100 y 500. La carta no puede atacar.</summary>
        public static ACard Freezed(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(100, 500);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);

            tgt.CanAttack = false;
            return tgt;
        }

        /// <summary>Efecto de estado: Defensa Rota. La DEF se hace 0.</summary>
        public static ACard DefenseBroken(ACard tgt)
        {
            tgt.DEF = 0;
            return tgt;
        }

        /// <summary>Efecto de estado: Sangrado. Realiza un daño que oscila entre 500 y 1500.</summary>
        public static ACard Bleeding(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(500, 1500);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);

            return tgt;
        }

        /// <summary>Efecto de estado: Infectado. Realiza un daño que oscila entre 300 y 500 al infectado. Si es la carta que infecto entonces se cura la mitad del daño.</summary>
        public static ACard Infected(ACard tgt)
        {
            if (!tgt.InfectedOther && tgt.IsInfected)
            {
                System.Random r = new System.Random();
                double predmg = r.Next(300, 500);
                double dmg = Math.Round(predmg / 10) * 10;
                tgt = DoingEffect.DamageDealer(tgt, dmg);
                DoingEffect.DamageToTurnInHeal = dmg;
                return tgt;
            }

            else if (tgt.InfectedOther)
            {
                if (DoingEffect.DamageToTurnInHeal != 0)
                {
                    tgt.VIT += DoingEffect.DamageToTurnInHeal / 2;
                }

                DoingEffect.DamageToTurnInHeal = 0;
                return tgt;
            }

            return tgt;
        }

        /// <summary>Efecto de estado: Atado. Realiza un daño que oscila entre 100 y 200. El objetivo no puede atacar.</summary>
        public static ACard Tied(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(100, 200);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);

            tgt.CanAttack = false;

            return tgt;
        }

        /// <summary>Efecto de estado: Cegado. La carta no puede atacar ni usar efectos.</summary>
        public static ACard Blinded(ACard tgt)
        {
            tgt.CanAttack = false;
            tgt.CanUseEffect = false;
            return tgt;
        }

        /// <summary>Efecto de estado: Maldito. Realiza un daño que oscila entre 700 y 2000.</summary>
        public static ACard Cursed(ACard tgt)
        {
            System.Random r = new System.Random();
            double predmg = r.Next(700, 2000);
            double dmg = Math.Round(predmg * 10) / 10;
            tgt = DoingEffect.DamageDealer(tgt, dmg);
            return tgt;
        }

        #endregion

    }



}