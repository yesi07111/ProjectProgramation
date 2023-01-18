namespace ProjectClasses
{
    /// <summary>Clase de efectos elementales, sencillos y con varios objetivos.</summary>
    public class ElementalDamage : Effects
    {
        //lista de los efectos elementales que afectan un solo objetivo
        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,
      (string name, string effectdesc) label, double[] costs)> SingleElementalEffect
        { get; set; }

        //lista de los efectos elemetales qye afectan varios objetivos
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,
        (string name, string effect) label, double[] costs)> MidAreaElementalEffects
        { get; set; }

        /// <summary>Clase de efectos elementales, sencillos y con varios objetivos. >Para efectos elementales sencillos: (0)FireBolt (1)Poison Shot (2)Meduza's Gaze (3)Whirlind (4)ThunderBolt (5)Freeze (6)Fractals (7)Seed (8)Shadow Chain (9)Holy Light (10)Darkness (11)Mechanic Shocker. >Para efectos elementales de rango medio: (0)FireStorm (1)Maelstrom (2)Earthquake (3)Typhonn (4)ThunderStorm (5)Absolute Zero (6)Fragmentation (7)Natural Disaster (8)Shadow Jail (9)DivineBlow (10)AbsoluteDarkness (11)Ultra M. Shocker..</summary>
        public ElementalDamage()
        {
            SingleElementalEffect = new();
            MidAreaElementalEffects = new();

            SavingElementalDamagePerDeffect();

            base.EBSingleElementalEffect = SingleElementalEffect;
            base.EBMidAreaElementalEffects = MidAreaElementalEffects;


        }

        /// <summary>Devuelve un efecto. Si singleormidarea = 1 se toma efecto elemental sencillo (SingleElementalEffect) y cualquier otro número se toma como efecto elemental de rango medio (MidAreaElementalEffects). >Para efectos elementales sencillos: (0)FireBolt (1)Poison Shot (2)Meduza's Gaze (3)Whirlind (4)ThunderBolt (5)Freeze (6)Fractals (7)Seed (8)Shadow Chain (9)Holy Light (10)Darkness (11)Mechanic Shocker. >Para efectos elementales de rango medio: (0)FireStorm (1)Maelstrom (2)Earthquake (3)Typhonn (4)ThunderStorm (5)Absolute Zero (6)Fragmentation (7)Natural Disaster (8)Shadow Jail (9)DivineBlow (10)AbsoluteDarkness (11)Ultra M. Shocker.</summary>
        public ElementalDamage(int effectnum, int singleormidarea)
        {
            SingleElementalEffect = new();
            MidAreaElementalEffects = new();

            SavingElementalDamagePerDeffect();

            base.EBSingleElementalEffect = SingleElementalEffect;
            base.EBMidAreaElementalEffects = MidAreaElementalEffects;


            //Creo un random en caso de que el número del efecto asignado no sea válido asignar un efecto al azar
            System.Random r = new System.Random();

            //Asigno el efecto en la propiedad de la base en dependencia de su nivel
            if (singleormidarea == 1)
            {
                if (effectnum > 11 || effectnum < 0)
                {
                    effectnum = r.Next(0, 11);
                }

                base.ASingleElementalEffect = SingleElementalEffect[effectnum];
                base.ObjectEffect2 = this;
            }

            else
            {
                if (effectnum > 11 || effectnum < 0)
                {
                    effectnum = r.Next(0, 11);
                }

                base.AMidAreaElementalEffects = MidAreaElementalEffects[effectnum];
                base.ObjectEffect3Elemental = this;
            }

        }

        /// <summary>Creador de listas con delegados que hacen referencia a efectos de daño elemental por defecto.</summary>
        void SavingElementalDamagePerDeffect()
        {

            #region Efectos que pueden provocar estados a un objetivo por defecto

            SingleElementalEffect.Add(((FireBolt), ("FireBolt", "Deals fire damage to an enemy and has 60% of chance of give the state " + states[1] + "."), new double[] { 200, 200, 200, 300 }));
            SingleElementalEffect.Add(((PoisonShoot), ("Poison Shot", "Deals aqua damage to an enemy and has 70% of chance of give the state " + states[2] + "."), new double[] { 200, 300, 200, 200 }));
            SingleElementalEffect.Add(((MedusaGaze), ("Meduza's Gaze", "Deals earth damage to an enemy and has 40% of chance of give the state " + states[3] + "."), new double[] { 300, 200, 200, 200 }));
            SingleElementalEffect.Add(((Torbellino), ("Whirlind", "Deals wind damage to an enemy and has 45% of chance of give the state " + states[4] + "."), new double[] { 200, 200, 200, 200 }));
            SingleElementalEffect.Add(((ThunderBolt), ("ThunderBolt", "Deals lightning damage to an enemy and has 40% of chance of give the state " + states[5] + "."), new double[] { 300, 300, 150, 150 }));
            SingleElementalEffect.Add(((Freeze), ("Freeze", "Deals ice damage to an enemy and has 45% of chance of give the state " + states[6] + "."), new double[] { 150, 150, 250, 300 }));
            SingleElementalEffect.Add(((Fractals), ("Fractals", "Deals crystal damage to an enemy and has 35% of chance of give the state " + states[8] + "."), new double[] { 100, 300, 100, 300 }));
            SingleElementalEffect.Add(((Seed), ("Seed", "Deals plant damage to an enemy and has 60% of chance of give the state " + states[9] + "."), new double[] { 200, 300, 200, 300 }));
            SingleElementalEffect.Add(((ShadowChain), ("Shadow Chain", "Deals shadow damage to an enemy and has 50% of chance of give the state " + states[10] + "."), new double[] { 100, 300, 0, 300 }));
            SingleElementalEffect.Add(((HolyLight), ("Holy Light", "Deals light damage to an enemy and has 65% of chance of give the state " + states[11] + "."), new double[] { 350, 350, 0, 0 }));
            SingleElementalEffect.Add(((Darkness), ("Darkness", "Deals darkness damage to an enemy and has 25% of chance of give the state " + states[12] + "."), new double[] { 0, 300, 300, 500 }));
            SingleElementalEffect.Add(((Shocker), ("Mechanic Shocker", "Deals electric damage to an enemy and has 80% of chance of give the state " + states[7] + "."), new double[] { 0, 300, 300, 200 }));

            #endregion

            #region Efectos que pueden provocar estados a varios objetivos por defecto

            MidAreaElementalEffects.Add(((FireStorm), ("FireStorm", "Deals fire damage to some enemy and has 60% of chance of give the state " + states[1] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((Maelstrom), ("Malestrom", "Deals aqua damage to some enemy and has 70% of chance of give the state " + states[2] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((Earthquake), ("Earthquake", "Deals earth damage to some enemy and has 40% of chance of give the state " + states[3] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((Typhonn), ("Typhonn", "Deals wind damage to some enemy and has 45% of chance of give the state " + states[4] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((ThunderStorm), ("ThunderStorm", "Deals lightining damage to some enemy and has 40% of chance of give the state " + states[5] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((AbsoluteZero), ("Absolute Zero", "Deals ice damage to some enemy and has 45% of chance of give the state " + states[6] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((Fragmentation), ("Fragmentation", "Deals crystal damage to some enemy and has 35% of chance of give the state " + states[8] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((NaturalDisaster), ("Natural Disaster", "Deals plant damage to some enemy and has 60% of chance of give the state " + states[9] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((ShadowJail), ("Shadow Jail", "Deals shadow damage to some enemy and has 50% of chance of give the state " + states[10] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((DivineBlow), ("Divine Blow", "Deals light damage to some enemy and has 65% of chance of give the state " + states[11] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((AbsoluteDarkness), ("World of Darkness", "Deals darkness damage to some enemy and has 25% of chance of give the state " + states[12] + "."), new double[] { 0, 0, 0, 0 }));
            MidAreaElementalEffects.Add(((UltraShocker), ("Ultra M. Shocker", "Deals electric damage to some enemy and has 80% of chance of give the state " + states[7] + "."), new double[] { 0, 0, 0, 0 }));
            #endregion
        }


        #region Efectos con estados sencillos:

        /// <summary>Realiza daño de fuego a un enemigo y tiene un 60% de posibilidad de provocar el estado "Burned" por amount turnos.</summary>
        public static Player[] FireBolt(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Burned"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -5 y 25 %.
                    card = ChangePureElementDamage(card, -5, 25);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Burned");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Burned"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -5 y 25 %.
                    card = ChangePureElementDamage(card, -5, 25);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Burned");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }

            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de agua a un enemigo y tiene un 70% de posibilidad de provocar el estado "Poisoned" por amount turnos.</summary>
        public static Player[] PoisonShoot(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Poisoned"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -7 y 28 %.
                    card = ChangePureElementDamage(card, -7, 28);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 30 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Poisoned");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Poisoned"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -7 y 28 %.
                    card = ChangePureElementDamage(card, -7, 28);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 30 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Poisoned");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }



            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de  tierra a un enemigo y tiene un 80% de posibilidad de provocar el estado "Petrified" por amount turnos.</summary>
        public static Player[] MedusaGaze(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Petrified"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -10 y 30 %.
                    card = ChangePureElementDamage(card, -10, 30);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 20 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Petrified");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Petrified"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -10 y 30 %.
                    card = ChangePureElementDamage(card, -10, 30);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 20 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Petrified");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }

            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de viento a un enemigo y tiene un 45% de posibilidad de provocar el estado "Confused" por amount turnos.</summary>
        public static Player[] Torbellino(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Confused"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -3 y 15 %.
                    card = ChangePureElementDamage(card, -3, 15);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Confused");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Confused"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -3 y 15 %.
                    card = ChangePureElementDamage(card, -3, 15);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Confused");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }




            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de relámpago a un enemigo y tiene un 40% de posibilidad de provocar el estado "Paralyzed" por amount turnos.</summary>
        public static Player[] ThunderBolt(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {


            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Paralyzed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -15 y 45 %.
                    card = ChangePureElementDamage(card, -15, 45);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 60 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Paralyzed");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Paralyzed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -15 y 45 %.
                    card = ChangePureElementDamage(card, -15, 45);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 60 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Paralyzed");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }



            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de hielo a un enemigo y tiene un 45% de posibilidad de provocar el estado "Freeze" por amount turnos.</summary>
        public static Player[] Freeze(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Freezed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -15 y 45 %.
                    card = ChangePureElementDamage(card, -15, 45);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Freezed");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Freezed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -15 y 45 %.
                    card = ChangePureElementDamage(card, -15, 45);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Freezed");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }



            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de rayo a un enemigo y tiene un 60% de posibilidad de provocar el estado "DefenseBroken" por amount turnos.</summary>
        public static Player[] Shocker(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["DefenseBroken"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -1 y 11 %.
                    card = ChangePureElementDamage(card, -1, 11);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "DefenseBroken");

                    }

                    pay = both[0];
                }


            }

            else
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];



                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["DefenseBroken"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -1 y 11 %.
                    card = ChangePureElementDamage(card, -1, 11);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "DefenseBroken");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }

            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de cristal a un enemigo y tiene un 35% de posibilidad de provocar el estado "Bleeding" por amount turnos.</summary>
        public static Player[] Fractals(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Bleeding"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -30 y 60 %.
                    card = ChangePureElementDamage(card, -30, 60);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 65 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Bleeding");

                    }

                    pay = both[0];
                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Bleeding"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -30 y 60 %.
                    card = ChangePureElementDamage(card, -30, 60);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 65 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Bleeding");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }



            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de planta a un enemigo y tiene un 60% de posibilidad de provocar el estado "Infected" por amount turnos.</summary>
        public static Player[] Seed(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento y el objetivo no es tipo planta (ya que las plantas no se infectan)
                if (!IsInmuneToElement(target, efc.statesandelements["IsInfected"]) && !target.ContainElement("Plant"))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -2 y 14 %.
                    card = ChangePureElementDamage(card, -2, 14);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && pay.AField.IsInField(target))
                    {
                        //entonces como es seguro que infectará cambia el estado del que infecta y del infectado
                        card = PuttingAbnormalState(card, amount, "AsInfected");
                        card.InfectedOther = true;
                        target = PuttingAbnormalState(target, amount, "IsInfected");
                        target.IsInfected = true;

                        pay.AField.SetCard(card, pay.AField.Find(card));
                        pay.AField.SetCard(target, pay.AField.Find(target));
                    }

                    pay = both[0];
                }

            }

            else
            {

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento y el objetivo no es tipo planta (ya que las plantas no se infectan)
                if (!IsInmuneToElement(target, efc.statesandelements["IsInfected"]) && !target.ContainElement("Plant"))
                {

                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -2 y 14 %.
                    card = ChangePureElementDamage(card, -2, 14);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 40 && enemy.AField.IsInField(target))
                    {
                        //entonces como es seguro que infectará cambia el estado del que infecta y del infectado
                        card = PuttingAbnormalState(card, amount, "AsInfected");
                        card.InfectedOther = true;
                        target = PuttingAbnormalState(target, amount, "IsInfected");
                        target.IsInfected = true;

                        pay.AField.SetCard(card, pay.AField.Find(card));
                        enemy.AField.SetCard(target, enemy.AField.Find(target));
                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }


            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de sombra a un enemigo y tiene un 65% de posibilidad de provocar el estado "Tied" por amount turnos.</summary>
        public static Player[] ShadowChain(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Tied"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -17 y 46 %.
                    card = ChangePureElementDamage(card, -17, 46);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 35 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Tied");
                    }

                    pay = both[0];


                }

            }

            else
            {


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Tied"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -17 y 46 %.
                    card = ChangePureElementDamage(card, -17, 46);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 35 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Tied");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }



            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de luz a un enemigo y tiene un 45% de posibilidad de provocar el estado "Blinded" por amount turnos.</summary>
        public static Player[] HolyLight(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Blinded"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -30 y 65 %.
                    card = ChangePureElementDamage(card, -30, 65);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Blinded");

                    }

                    pay = both[0];

                }

            }

            else
            {

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Blinded"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -30 y 65 %.
                    card = ChangePureElementDamage(card, -30, 65);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 55 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Blinded");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }


            return new Player[] { pay, enemy };
        }

        /// <summary>Realiza daño de oscuridad a un enemigo y tiene un 25% de posibilidad de provocar el estado "Cursed" por amount turnos.</summary>
        public static Player[] Darkness(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Cursed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -35 y 70 %.
                    card = ChangePureElementDamage(card, -30, 65);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random, en caso que la carta siga viva
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 75 && pay.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Cursed");

                    }

                    pay = both[0];
                }

            }

            else
            {
                //obtengo la ubicación de la carta
                (int x1, int y1) pos0 = pay.AField.Find(card);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un objeto efecto para acceder al diccionario de estado con elemento que lo provoca
                Effects efc = new Effects();

                //Si no es inmune al elemento
                if (!IsInmuneToElement(target, efc.statesandelements["Cursed"]))
                {
                    //entonces cambia la cantidad de daño a realizar y el posible aumento entre -35 y 70 %.
                    card = ChangePureElementDamage(card, -30, 65);
                    Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { target });
                    card.ResetATK(pay);

                    //luego comprobamos si se le pondrá el efecto de estado con un random, en caso que la carta siga viva
                    System.Random r = new System.Random();

                    if (r.Next(0, 100) > 75 && enemy.AField.IsInField(target))
                    {
                        target = PuttingAbnormalState(target, amount, "Cursed");

                    }

                    pay = both[0];
                    enemy = both[1];
                }

            }


            return new Player[] { pay, enemy };
        }

        /// <summary>Método para comprobar si una carta es inmune al elemento puro del ataque.</summary>
        static bool IsInmuneToElement(ACard tgt, string elm)
        {
            if (tgt != null)
            {
                var inmunityto = tgt.ElementsObject.AllElementsRelationOfACard(tgt.ActualFourElements, 3);

                foreach (List<string> list in inmunityto)
                {
                    foreach (string element in list)
                    {
                        if (element == elm)
                        {
                            return true;
                        }
                    }
                }
            }


            return false;
        }

        /// <summary>Método para cambiar el daño del ataque elemental aumenta en un porciento random entre el limitA y el limitB, de ser negativos, dismiuye en ese porciento en vez de aumentar.</summary>
        static ACard ChangePureElementDamage(ACard card, int limitA, int limitB)
        {
            System.Random r = new System.Random();
            int porcentofchange = r.Next(limitA, limitB + 1);

            if (porcentofchange >= 0)
            {
                card.ATK += card.ATK * porcentofchange / 100;
            }

            else
            {
                card.ATK -= card.ATK * ((-1) * porcentofchange / 100);
            }

            return card;
        }

        // <summary>Método para colocar a una carta un estado anormal determinado y la cantidad de turnos que durara.</summary>
        static ACard PuttingAbnormalState(ACard target, double amount, string state)
        {
            //Si no tiene ningún efecto de estado aún
            if (!target.AsAnAbnormalState)
            {
                //actualizo el estado y lo agrego a la lista
                target.CardState = state;
                target.CardStates.Add(state);

                //cambio el bool, la duración del estado y la agrega a la lista de los efectos de estado
                target.AsAnAbnormalState = true;
                target.AbnormalStateDuration = ((int)amount);
                target.ListOfAbnormalStateDuration.Add(state, ((int)amount));
            }

            //en caso que ya tuviera algún efecto de estado
            else
            {
                //agrego el nuevo estado al string de estado y a la lista
                target.CardState += " " + state;
                target.CardStates.Add(state);

                //entonces la duración del estado anormal si no es mayor que el actual se cambia
                if (target.AbnormalStateDuration < ((int)amount))
                {
                    target.AbnormalStateDuration = ((int)amount);
                }

                //Luego se comprueba si ya se encuentra en la lista de estados anormales
                if (target.ListOfAbnormalStateDuration.ContainsKey(state))
                {
                    //se le aumentan los turnos 
                    target.ListOfAbnormalStateDuration[state] += ((int)amount);

                    //y se comprueba nuevamente si es el máximo valor de duración de estado
                    if (target.AbnormalStateDuration < target.ListOfAbnormalStateDuration[state])
                    {
                        target.AbnormalStateDuration = target.ListOfAbnormalStateDuration[state];
                    }
                }

                //sino
                else
                {
                    //se agrega a la lista de los efectos de estado
                    target.ListOfAbnormalStateDuration.Add(state, ((int)amount));
                }

            }

            return target;
        }

        #endregion

        #region Efectos con estados múltiples:

        /// <summary>Realiza daño de fuego a varios enemigos y tiene un 60% de posibilidad de provocar el estado "Burned" por amount turnos.</summary>
        public static Player[] FireStorm(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = FireBolt(pay, costs, card, tgt, amount, enemy);
            }

            return both;

        }

        /// <summary>Realiza daño de agua a varios enemigos y tiene un 70% de posibilidad de provocar el estado "Poisoned" por amount turnos.</summary>
        public static Player[] Maelstrom(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = PoisonShoot(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de  tierra a varios enemigos y tiene un 40% de posibilidad de provocar el estado "Petrified" por amount turnos.</summary>
        public static Player[] Earthquake(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = MedusaGaze(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de viento a varios enemigos y tiene un 45% de posibilidad de provocar el estado "Confused" por amount turnos.</summary>
        public static Player[] Typhonn(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Torbellino(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de relámpago a varios enemigos y tiene un 40% de posibilidad de provocar el estado "Paralyzed" por amount turnos.</summary>
        public static Player[] ThunderStorm(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = ThunderBolt(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de hielo a varios enemigos y tiene un 45% de posibilidad de provocar el estado "Freeze" por amount turnos.</summary>
        public static Player[] AbsoluteZero(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Freeze(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de rayo a varios enemigos y tiene un 80% de posibilidad de provocar el estado "DefenseBroken" por amount turnos.</summary>
        public static Player[] UltraShocker(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Shocker(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de cristal a varios enemigos y tiene un 35% de posibilidad de provocar el estado "Bleeding" por amount turnos.</summary>
        public static Player[] Fragmentation(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Fractals(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de planta a varios enemigos y tiene un 60% de posibilidad de provocar el estado "Infected" por amount turnos.</summary>
        public static Player[] NaturalDisaster(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Seed(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de sombra a varios enemigos y tiene un 50% de posibilidad de provocar el estado "Tied" por amount turnos.</summary>
        public static Player[] ShadowJail(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = ShadowChain(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de luz a varios enemigos y tiene un 65% de posibilidad de provocar el estado "Blinded" por amount turnos.</summary>
        public static Player[] DivineBlow(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = HolyLight(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Realiza daño de oscuridad a varios enemigos y tiene un 25% de posibilidad de provocar el estado "Cursed" por amount turnos.</summary>
        public static Player[] AbsoluteDarkness(Player pay, List<double> costs, ACard card, ACard[] target, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in target)
            {
                both = Darkness(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }


        #endregion

    }
}