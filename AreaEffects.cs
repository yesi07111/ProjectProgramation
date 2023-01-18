namespace ProjectClasses
{
    /// <summary>Clase de efectos de área, con varios objetivos.</summary>
    public class AreaEffect : Effects
    {
        //Lista de efectos de rango medio (afectan 3 enemigos)
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,
         (string name, string effect) label, double[] costs)> MidAreaEffects
        { get; set; }


        //Lista de efectos de rango total (afectan todos enemigos)
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,
        (string name, string effect) label, double[] costs)> HighAreaEffects
        { get; set; }

        /// <summary>Devuelve un efecto. Efectos de rango medio: (0)Cure for Three (1)Three Attack Up (2)Three Defense Up,. (3)Three Attack Down, (4)Three Defense Down, (5)Three Life Down, (6)Three Chains, (7)Three Silence, (8)Three Remove Debuff, (9)Three Remove All Debuff, (10)Three Remove Restrictions y (11)Three Attack At Once. Efectos de rango alto: (0)Chain Lightining (1)Gears's Command (2)Messiah's Sacrifice (3)Saint's Sacrifice (4)Erwin's Sacrifice (5)Imagine Breaker (6)Divine Shield (7)Saint Blessing.</summary>                      
        public AreaEffect()
        {
            MidAreaEffects = new();
            HighAreaEffects = new();

            SavingAreaEffectPerDeffect();

            base.EBMidAreaEffects = MidAreaEffects;
            base.EBHighAreaEffects = HighAreaEffects;
        }

        /// <summary>>Devuelve un efecto. Si powerofeffect = 1 entonces el efecto es de la lista de efectos de rango medio (MidAreaEffects) y si es otro número entonces es de la lista de efectos de rango alto (HighAreaEffects). >Para efectos de rango medio: (0)Cure for Three (1)Three Attack Up (2)Three Defense Up, (3)Three Attack Down, (4)Three Defense Down, (5)Three Life Down, (6)Three Chains, (7)Three Silence, (8)Three Remove Debuff, (9)Three Remove All Debuff, (10)Three Remove Restrictions y (11)Three Attack At Once. >Para efectos de rango alto: (0)Chain Lightning, (1)Gears's Command, (2)Messiah's Sacrifice, (3)Saint's Sacrifice, (4)Erwin's Sacrifice, (5)Imagine Breaker, (6)Divine Shield y (7)Saint Blessing.</summary>
        public AreaEffect(int effectnum, int powerofeffect)
        {
            MidAreaEffects = new();
            HighAreaEffects = new();

            SavingAreaEffectPerDeffect();

            base.EBMidAreaEffects = MidAreaEffects;
            base.EBHighAreaEffects = HighAreaEffects;

            //Creo un random en caso de que el número del efecto asignado no sea válido asignar un efecto al azar
            System.Random r = new System.Random();

            //Asigno el efecto en la propiedad de la base en dependencia de su nivel
            if (powerofeffect == 1)
            {
                if (effectnum > 11 || effectnum < 0)
                {
                    effectnum = r.Next(0, 11);
                }

                base.AMidAreaEffect = MidAreaEffects[effectnum];
                base.ObjectEffect3Area = this;
            }

            else
            {
                if (effectnum > 7 || effectnum < 0)
                {
                    effectnum = r.Next(0, 7);
                }

                base.AHighAreaEffect = HighAreaEffects[effectnum];
                base.ObjectEffect4 = this;
            }


        }

        /// <summary>Creador de listas con delegados que hacen referencia a efectos de área por defecto.</summary>
        void SavingAreaEffectPerDeffect()
        {

            #region Efectos de Área Media

            MidAreaEffects.Add(((HealAll), ("Cure for three", "Heals three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((IncreaseATKAll), ("Three Attack Up", "Increase ATK of three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((IncreaseDEFAll), ("Three Defense Up", "Increase DEF of three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((DecreaseATKAll), ("Three Attack Down", "Decrease ATK of three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((DecreaseDEFAll), ("Three Defense Down ", "Decrease DEF of three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((DecreaseVITAll), ("Three Life Down", "Decrease VIT of three targets for a moderate amount."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((RestrictAttackAll), ("Three Chains", "Three targets gets a chance of 70% of having attack restricted."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((RestrictEffectsAll), ("Three Silence", "Three targets gets a chance of 70% of having effects restricted."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((RemoveDebuffAll), ("Three Remove Debuff", "Three targets gets a chance of 60% of remove last debuff that affects target."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((RemoveAllDebuffAll), ("Three Remove All Debuff", "Three targets gets a chance of 60% of remove all debuff that affects the target."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((RemoveRestrictionAll), ("Three Remove Restrictions", "Three targets gets a chance of 60% of remove all restrictions."), new double[] { 0, 0, 0, 0 }));
            MidAreaEffects.Add(((AttackAll), ("Attack Three at Once", "Deals normal damage to three targets."), new double[] { 0, 0, 0, 0 }));

            #endregion

            #region Efectos de Área Total

            HighAreaEffects.Add(((LinkAttackAll), ("Chain Lightining", "Attack a moderate amount of cards while attack powers get 15% stronger for each targets that hits."), new double[] { 500, 300, 200, 500 }));
            HighAreaEffects.Add(((BrainWashed), ("Gears's Command", "Each target auto inflict damage of her own ATK multiplied by 2.5."), new double[] { 200, 500, 250, 600 }));
            HighAreaEffects.Add(((Sacrifice), ("Messiah's Sacrifice", "The user sacrifice herself in order to have a 70% chance of kill all enemies."), new double[] { 500, 200, 200, 1000 }));
            HighAreaEffects.Add(((ReviveAllRandom), ("Saint's Sacrifice", "The user sacrifice herself in order to revive random allies form graveyard."), new double[] { 400, 1000, 200, 0 }));
            HighAreaEffects.Add(((YouDontDieInVain), ("Erwin's Sacrifice", "Attacks an ally card, if die you gain half of his ATK, DEF and VIT add to the user."), new double[] { 0, 200, 1000, 200 }));
            HighAreaEffects.Add(((Inmunity), ("Imagine Breaker", "Makes the targets inmune to effects for a moderate amount of turns."), new double[] { 1000, 500, 0, 0 }));
            HighAreaEffects.Add(((AbsoluteDefense), ("Divine Shield", "Makes the targets inmune to normal attaks for a moderate amount of turns."), new double[] { 0, 0, 500, 1000 }));
            HighAreaEffects.Add(((GodBless), ("Saint Blessing", "The user reduces to half all states in order to give all allies a 50% chance of being inmune to effects."), new double[] { 0, 800, 800, 0 }));

            #endregion

        }


        #region Efectos de rango medio:

        /// <summary>Le suma a la VIT de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] HealAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.Heal(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Le suma al ATK de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] IncreaseATKAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.IncreaseATK(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Le suma a la DEF de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] IncreaseDEFAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.IncreaseDEF(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Le resta al ATK de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] DecreaseATKAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.DecreaseATK(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Le resta a la DEF de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] DecreaseDEFAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.DecreaseDEF(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Le resta a la VIT de cada carta objetivo una cantidad determinada (parámetro double amount).</summary>
        public static Player[] DecreaseVITAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.DecreaseVIT(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Impide que las cartas objetivo ataquen por unos turnos (parámetro double amount). Cada una tiene 30% posibilidad de fallar.</summary>
        public static Player[] RestrictAttackAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.RestrictAttack(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Impide que las cartas objetivo usen efectos por unos turnos (parámetro double amount). Cada una tiene 30% posibilidad de fallar.</summary>
        public static Player[] RestrictEffectsAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.RestrictEffects(pay, costs, card, tgt, amount, enemy);
            }

            return both;
        }

        /// <summary>Elimina un efecto de estado anormal de los objetivos. Cada uno tiene 40% de posibilidad de fallar.</summary>
        public static Player[] RemoveDebuffAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.RemoveDebuff(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Elimina cualquier efecto de estado anormal de los objetivos. Cada uno tiene 40% de posibilidad de fallar.</summary>
        public static Player[] RemoveAllDebuffAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.RemoveAllDebuff(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Elimina cualquier restricción de los objetivos. Cada uno tiene 40% de posibilidad de fallar.</summary>
        public static Player[] RemoveRestrictionAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            Player[] both = new Player[2];

            foreach (ACard tgt in targets)
            {
                both = SingleEffect.RemoveRestrictions(pay, costs, card, tgt, amount, unused);
            }

            return both;
        }

        /// <summary>Ataque normal a varios enemigos.</summary>
        public static Player[] AttackAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            //aquí paga el precio de usar el efecto
            pay.Kingdom.Faith -= costs[0];
            pay.Kingdom.Knowledge -= costs[1];
            pay.Kingdom.Capital -= costs[2];
            pay.Kingdom.Militarism -= costs[3];

            Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, targets);

            return both;

        }

        #endregion

        #region Efectos de rango alto:

        /// <summary>Ataque normal que rebota a una moderada cantidad de enemigos con la potencia aumentada en 15% (parámetro amount).</summary>
        public static Player[] LinkAttackAll(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un índice random para seleccionar una carta objetivo random
                System.Random r = new System.Random();
                int ind = r.Next(0, targets.Length);

                //Se realiza el primer daño
                Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { targets[ind] });

                //Luego se le hace el daño a las otras cuya cantidad determina el amount - 1
                for (int k = 1; k < amount; k++)
                {
                    //se aumenta el ataque en un 15%
                    card.ATK += card.ATK * (15 / 100);

                    //se escoje random a la próxima víctima
                    ind = r.Next(0, targets.Length);

                    //Y se ejecuta el daño
                    both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { targets[ind] });
                }

                return both;
            }

            else
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un índice random para seleccionar una carta objetivo random
                System.Random r = new System.Random();
                int ind = r.Next(0, targets.Length);

                //Se realiza el primer daño
                Player[] both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { targets[ind] });

                //Luego se le hace el daño a las otras cuya cantidad determina el amount - 1
                for (int k = 1; k < amount; k++)
                {
                    //se aumenta el ataque en un 15%
                    card.ATK += card.ATK * (15 / 100);

                    //se escoje random a la próxima víctima
                    ind = r.Next(0, targets.Length);

                    //Y se ejecuta el daño
                    both = DoingEffect.DamageDealer(pay, costs, card, enemy, new ACard[] { targets[ind] });
                }

                return both;
            }
        }

        /// <summary>Cada objetivo usa el doble de su ATK contra sí mismo, suicidio, seppuku!</summary>
        public static Player[] BrainWashed(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                Player[] both = new Player[] { pay, pay };

                //Cada carta objetivo
                foreach (ACard tgt in targets)
                {
                    //le chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(tgt, pay, pay.AField.Find(tgt));
                    pay = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se multiplica su ataque por 2.5
                        tgt.ATK = tgt.ATK * 2.5;

                        //y recibe el daño correspondiente
                        both = DoingEffect.DamageDealer(pay, costs, tgt, pay, new ACard[] { tgt });
                    }

                    //cambio both ya que aún si no se activa el efecto todavía cambio la duración de la inmunidad de la carta del
                    //enemigo
                    Player[] bt = { pay, pay };
                    both = bt;
                }

                return both;
            }

            else
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                Player[] both = new Player[] { pay, enemy };

                //Cada carta objetivo
                foreach (ACard tgt in targets)
                {
                    //le chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(tgt, enemy, enemy.AField.Find(tgt));
                    enemy = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se multiplica su ataque por 2.5
                        tgt.ATK = tgt.ATK * 2.5;

                        //y recibe el daño correspondiente
                        both = DoingEffect.DamageDealer(pay, costs, tgt, enemy, new ACard[] { tgt });
                    }

                    //cambio both ya que aún si no se activa el efecto todavía cambio la duración de la inmunidad de la carta del
                    //enemigo
                    Player[] bt = { pay, enemy };
                    both = bt;
                }

                return both;
            }

        }

        /// <summary>La carta se suicidia a cambio de un 75% de posibilidad de matar a cada carta enemiga.</summary>
        public static Player[] Sacrifice(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player enemy)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí se elimina del campo y se manda al cementerio
                pay.AField.UsedOrDestroyed(card);
                pay.Cementery.ToCementery(card);

                //Creo un random para la probabilidad de fallar
                System.Random r = new System.Random();

                //Cada carta prueba su suerte
                foreach (ACard tgt in targets)
                {
                    //Si entra aquí tuvo mala suerte
                    if (r.Next(0, 100) >= 25)
                    {
                        //pero primero se le chequea la inmunidad
                        var inmunitycheck = DoingEffect.CheckInmunityStates(tgt, pay, pay.AField.Find(tgt));
                        pay = inmunitycheck.enemy;

                        //si después de chequear hay que activar el efecto
                        if (inmunitycheck.NeedToActiveEffect)
                        {
                            //Se elimina del campo y se manda al cementerio
                            pay.AField.UsedOrDestroyed(tgt);
                            pay.Cementery.ToCementery(tgt);
                        }

                    }
                }


                return new Player[] { pay, enemy };
            }

            else
            {

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí se elimina del campo y se manda al cementerio
                pay.AField.UsedOrDestroyed(card);
                pay.Cementery.ToCementery(card);

                //Creo un random para la probabilidad de fallar
                System.Random r = new System.Random();

                //Cada carta prueba su suerte
                foreach (ACard tgt in targets)
                {
                    //Si entra aquí tuvo mala suerte
                    if (r.Next(0, 100) >= 25)
                    {
                        //pero primero se le chequea la inmunidad
                        var inmunitycheck = DoingEffect.CheckInmunityStates(tgt, enemy, enemy.AField.Find(tgt));
                        enemy = inmunitycheck.enemy;

                        //si después de chequear hay que activar el efecto
                        if (inmunitycheck.NeedToActiveEffect)
                        {
                            //Se elimina del campo y se manda al cementerio
                            enemy.AField.UsedOrDestroyed(tgt);
                            enemy.Cementery.ToCementery(tgt);
                        }

                    }
                }

                return new Player[] { pay, enemy };
            }

        }

        /// <summary>La carta se suicidia a cambio de revivir cuantas cartas se pueda de forma random del cementerio.</summary>
        public static Player[] ReviveAllRandom(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == unused)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí se elimina del campo y se manda al cementerio
                pay.AField.UsedOrDestroyed(card);
                pay.Cementery.ToCementery(card);

                //Creo un random para tomar las cartas del cementerio
                System.Random r = new System.Random();

                //Recorro por las cartas del cementerio
                for (int k = 0; k < pay.Cementery.CardsInCementery.Count; k++)
                {
                    //tomo una random
                    int ind = r.Next(0, pay.Cementery.CardsInCementery.Count);
                    ACard cardfromcementery = pay.Cementery.CardsInCementery[ind];

                    //Y en caso de que se puede invocar alguna carta
                    if (pay.AField.AreNaturalCardsInvokable())
                    {
                        //Pues invocamos esta y la quitamos del cementerio
                        pay.AField.Invocation(cardfromcementery, pay.AField.FindEmpty(1), pay);
                        pay.Cementery.RemoveformGraveyard(cardfromcementery);
                    }

                    //si ya no se puede invocar más se devuelve después de ubicarla
                    else
                    {
                        return new Player[] { pay, pay };
                    }


                }

                //O si habían menos o la misma cantidad de cartas en el cementerio que espacios en el campo, se retorna aquí
                return new Player[] { pay, pay };
            }

            else
            {

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí se elimina del campo y se manda al cementerio
                pay.AField.UsedOrDestroyed(card);
                pay.Cementery.ToCementery(card);

                //Creo un random para tomar las cartas del cementerio
                System.Random r = new System.Random();

                //Recorro por las cartas del cementerio
                for (int k = 0; k < pay.Cementery.CardsInCementery.Count; k++)
                {
                    //tomo una random
                    int ind = r.Next(0, pay.Cementery.CardsInCementery.Count);
                    ACard cardfromcementery = pay.Cementery.CardsInCementery[ind];

                    //Y en caso de que se puede invocar alguna carta
                    if (pay.AField.AreNaturalCardsInvokable())
                    {
                        //Pues invocamos esta y la quitamos del cementerio
                        pay.AField.Invocation(cardfromcementery, pay.AField.FindEmpty(1), pay);
                        pay.Cementery.RemoveformGraveyard(cardfromcementery);
                    }

                    //si ya no se puede invocar más se devuelve después de ubicarla
                    else
                    {

                        return new Player[] { pay, unused };
                    }
                }

                //O si habían menos o la misma cantidad de cartas en el cementerio que espacios en el campo, se retorna aquí
                return new Player[] { pay, unused };

            }
        }

        /// <summary>Mata a un aliado y si muere obtienes la mitad de su ATK, DEF y VIT. En el próximo turno no puede atacar ni usar otro efecto.</summary>
        public static Player[] YouDontDieInVain(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == unused)
            {
                //obtengo la localización de la carta
                (int x1, int y1) pos0 = pay.AField.Find(card);


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //le chequeo la inmunidad al aliado
                var inmunitycheck = DoingEffect.CheckInmunityStates(targets[0], pay, pay.AField.Find(targets[0]));
                pay = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //Aquí se elimina del campo el aliado y se manda al cementerio
                    pay.AField.UsedOrDestroyed(targets[0]);
                    pay.Cementery.ToCementery(targets[0]);

                    //Aquí se aumentan los stats y se prohibe atacar o usar efectos
                    card.ATK += targets[0].ATK / 2;
                    card.DEF += targets[0].DEF / 2;
                    card.VIT += targets[0].VIT / 2;
                    card.CanAttack = false;
                    card.CanUseEffect = false;
                    card.RestrictAttackTurns = 1;
                    card.RestrictEffectTurns = 1;
                }

                pay.AField.SetCard(card, pos0);

            }

            else
            {
                //obtengo la localización de la carta
                (int x1, int y1) pos0 = pay.AField.Find(card);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //le chequeo la inmunidad al aliado
                var inmunitycheck = DoingEffect.CheckInmunityStates(targets[0], pay, pay.AField.Find(targets[0]));
                pay = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //Aquí se elimina del campo el aliado y se manda al cementerio
                    pay.AField.UsedOrDestroyed(targets[0]);
                    pay.Cementery.ToCementery(targets[0]);

                    //Aquí se aumentan los stats y se prohibe atacar o usar efectos
                    card.ATK += targets[0].ATK / 2;
                    card.DEF += targets[0].DEF / 2;
                    card.VIT += targets[0].VIT / 2;
                    card.CanAttack = false;
                    card.CanUseEffect = false;
                    card.RestrictAttackTurns = 1;
                    card.RestrictEffectTurns = 1;
                }

                pay.AField.SetCard(card, pos0);

            }


            return new Player[] { pay, unused };

        }

        /// <summary>Lleva su VIT a 1 a cambio de inmunidad a efectos por x turnos (parámetro amount).</summary>
        public static Player[] Inmunity(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == unused)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(pay.AField.Find(targets[j]));
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su VIT a 1 y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.VIT = 1;
                (targets[0]).AsEffectInmunity = true;
                (targets[0]).InmunityEffectDuration = ((int)amount);


                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    pay.AField.SetCard(targets[j], listpos1[j]);
                }
            }

            else
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(unused.AField.Find(targets[j]));
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su VIT a 1 y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.VIT = 1;
                (targets[0]).AsEffectInmunity = true;
                (targets[0]).InmunityEffectDuration = ((int)amount);


                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    unused.AField.SetCard(targets[j], listpos1[j]);
                }
            }


            return new Player[] { pay, unused };
        }

        /// <summary>Lleva su ATK a 1 a cambio de volver al objetivo inafectado por ataques por 3 turnos.</summary>
        public static Player[] AbsoluteDefense(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == unused)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(pay.AField.Find(targets[j]));
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su ATK a 1 y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.ATK = 1;
                (targets[0]).AsPhysicalInmunity = true;
                (targets[0]).InmunityPhysicalDuration = ((int)amount);

                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    pay.AField.SetCard(targets[j], listpos1[j]);
                }
            }

            else
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(unused.AField.Find(targets[j]));
                }


                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su ATK a 1 y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.ATK = 1;
                (targets[0]).AsPhysicalInmunity = true;
                (targets[0]).InmunityPhysicalDuration = ((int)amount);


                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    unused.AField.SetCard(targets[j], listpos1[j]);
                }
            }

            return new Player[] { pay, unused };
        }

        /// <summary>Reduce sus stats a la mitad a cambio de dar un 50% de probabilidad sobrevivir ante efectos de estado a todos sus aliados por x turnos.</summary>
        public static Player[] GodBless(Player pay, List<double> costs, ACard card, ACard[] targets, double amount, Player unused)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == unused)
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(pay.AField.Find(targets[j]));
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su stats a la mitad y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.ATK = card.ATK / 2;
                card.DEF = card.DEF / 2;
                card.VIT = card.VIT / 2;

                foreach (ACard tgt in targets)
                {
                    (tgt).AsInmunityChance = true;
                    (tgt).InmunityChanceDuration = ((int)amount);
                }

                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    pay.AField.SetCard(targets[j], listpos1[j]);
                }
            }

            else
            {
                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);

                List<(int x2, int y2)> listpos1 = new();

                for (int j = 0; j < targets.Length; j++)
                {
                    listpos1.Add(unused.AField.Find(targets[j]));
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Aquí reduzco su stats a la mitad y concedo el estado de inmunidad a la carta objetivo por amount turnos.
                card.ATK = card.ATK / 2;
                card.DEF = card.DEF / 2;
                card.VIT = card.VIT / 2;

                foreach (ACard tgt in targets)
                {
                    (tgt).AsInmunityChance = true;
                    (tgt).InmunityChanceDuration = ((int)amount);
                }

                pay.AField.SetCard(card, pos0);

                for (int j = 0; j < targets.Length; j++)
                {
                    unused.AField.SetCard(targets[j], listpos1[j]);
                }
            }

            return new Player[] { pay, unused };
        }


        #endregion

    }


}