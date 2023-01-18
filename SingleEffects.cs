namespace ProjectClasses
{
    /// <summary>Clase de efectos sencillos, con un único objetivo.</summary>
    public class SingleEffect : Effects
    {
        //Lista de delegados que son los efectos sencillos o de un solo objetivo. Se les pasa el jugador que la activa, la carta que lo hace,
        //la carta objetivo, la cantidad del efecto, el jugador que tiene la carta objetivo y lista de precios en orden de Fe, Conocimiento, Capital y Militar.
        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,
        (string name, string effectdesc) label, double[] costs)> SingleMagicalEffects
        { get; set; }

        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,
       (string name, string effectdesc) label, double[] costs)> SingleNaturalEffects
        { get; set; }

        ///<summary>En el constructor se agregan los efectos sencillos por defecto para cartas mágicas: (0)Healing Light, (1)Attack Up, (2)Defense Up, (3)Attack Down, (4)Defense Down, (5)Life Down, (6)Full Power Up, (7)All Power Down, (8)Back to Life, (9)A Sword of Reveling Light, (10)Silence, (11)Berserk, (12)Turn Undead, (13)Life's Secure, (14)Mimic, (15)Cure, (16)Cure All, (17)Remove Restrictions, (18)Tower of Guns, (19)Tower of Salomon, (20)Tower of God y (21)Tower of Money. Cartas naturales: (0)A Sword of Reveling Light, (1)Silence, (2)Berserk, (3)Turn Undead, (4)Cure, (5)Cure All y (6)Remove Restrictions.</summary>
        public SingleEffect()
        {
            //Inicializo las listas de las propiedades de la clase y de la base y las relleno 
            SingleMagicalEffects = new();
            SingleNaturalEffects = new();

            SavingSingleEffectPerDeffect();

            base.EBSingleMagicalEffects = SingleMagicalEffects;
            base.EBSingleNaturalEffects = SingleNaturalEffects;

        }

        /// <summary>Si se pone typecard = 1 es para agregarle efecto a una carta natural y typecard = 2 para cartas mágicas. Cualquier otro número devuelve tipo 1. Cartas mágicas: (0)Healing Light, (1)Attack Up, (2)Defense Up, (3)Attack Down, (4)Defense Down, (5)Life Down, (6)Full Power Up, (7)All Power Down, (8)Back to Life, (9)A Sword of Reveling Light, (10)Silence, (11)Berserk, (12)Turn Undead, (13)Life's Secure, (14)Mimic, (15)Cure, (16)Cure All, (17)Remove Restrictions, (18)Tower of Guns, (19)Tower of Salomon, (20)Tower of God y (21)Tower of Money. Cartas naturales: (0)A Sword of Reveling Light, (1)Silence, (2)Berserk, (3)Turn Undead, (4)Cure, (5)Cure All y (6)Remove Restrictions.</summary>
        public SingleEffect(int effectnum, int typecard)
        {
            //Inicializo las listas de las propiedades de la clase y de la base y las relleno 
            SingleMagicalEffects = new();
            SingleNaturalEffects = new();

            SavingSingleEffectPerDeffect();

            base.EBSingleMagicalEffects = SingleMagicalEffects;
            base.EBSingleNaturalEffects = SingleNaturalEffects;


            //Creo un random en caso de que el número del efecto asignado no sea válido asignar un efecto al azar
            System.Random r = new System.Random();

            //Asigno el efecto en la propiedad de la base (Effects.ASingleMagicalEffect o Effects.ASingleNaturalEffect) 
            //en dependencia del tipo de carta
            if (typecard == 2)
            {
                if (effectnum > 21 || effectnum < 0)
                {
                    effectnum = r.Next(0, 18);
                }

                //se lo asigno como propiedad individual que tomará la carta después 
                base.ASingleMagicalEffect = SingleMagicalEffects[effectnum];

            }

            else
            {
                if (effectnum > 6 || effectnum < 0)
                {
                    effectnum = r.Next(0, 18);
                }

                //se lo asigno como propiedad individual que tomará la carta después 
                base.ASingleNaturalEffect = SingleNaturalEffects[effectnum];

            }

            base.ObjectEffect1 = this;
        }

        /// <summary>Creador de listas con delegados que hacen referencia a efectos sencillos por defecto.</summary>
        void SavingSingleEffectPerDeffect()
        {

            #region Efectos sencillos de cartas mágicas

            SingleMagicalEffects.Add(((Heal), ("Healing Light", "Increase VIT for a moderate amount."), new double[] { 200, 100, 100, 0 }));
            SingleMagicalEffects.Add(((IncreaseATK), ("Attack Up", "Increase ATK for a moderate amount."), new double[] { 0, 100, 100, 200 }));
            SingleMagicalEffects.Add(((IncreaseDEF), ("Defense Up", "Increase DEF for a moderate amount."), new double[] { 100, 100, 200, 0 }));
            SingleMagicalEffects.Add(((DecreaseATK), ("Attack Down", "Decrease ATK for a moderate amount."), new double[] { 0, 200, 100, 100 }));
            SingleMagicalEffects.Add(((DecreaseDEF), ("Defense Down", "Decrease DEF for a moderate amount."), new double[] { 0, 100, 100, 200 }));
            SingleMagicalEffects.Add(((DecreaseVIT), ("Life Down", "Decrease VIT for a moderate amount."), new double[] { 200, 100, 100, 0 }));
            SingleMagicalEffects.Add(((IncreaseStats), ("Full Power Up", "Increase ATK, DEF, VIT for a moderate amount."), new double[] { 200, 200, 200, 200 }));
            SingleMagicalEffects.Add(((DecreaseStats), ("All Power Down", "Decrease ATK, DEF and VIT for a moderate amount."), new double[] { 250, 250, 250, 250 }));
            SingleMagicalEffects.Add(((ReturnFromGraveyard), ("Back to Life", "Return a card form graveyard in a random position in the field."), new double[] { 500, 350, 200, 0 }));
            SingleMagicalEffects.Add(((RestrictAttack), ("A Sword of Reveling Light", "The target can't attack for a moderate amount of turns. As 30% chance of fail."), new double[] { 200, 200, 0, 100 }));
            SingleMagicalEffects.Add(((RestrictEffects), ("Silence", "The target can't use any effect for a moderate amount of turns. As 30% chance of fail."), new double[] { 300, 200, 0, 100 }));
            SingleMagicalEffects.Add(((SacrificeVIT), ("Berserk", "Turn half of VIT into ATK power."), new double[] { 0, 300, 150, 100 }));
            SingleMagicalEffects.Add(((VampireFang), ("Turn Undead", "Turns target in a vampire. Can use skill Vampire's Fang: Turn half of the damage dealed into VIT."), new double[] { 0, 300, 200, 100 }));
            SingleMagicalEffects.Add(((ReturnToHand), ("Life's Secure", "Get back to hand a card from graveyard."), new double[] { 300, 150, 100, 0 }));
            SingleMagicalEffects.Add(((TransformInto), ("Mimic", "Target card can transform into any other card in field."), new double[] { 0, 400, 100, 300 }));
            SingleMagicalEffects.Add(((RemoveDebuff), ("Cure", "Target card have a 60% chance of heal a debuff."), new double[] { 100, 400, 200, 0 }));
            SingleMagicalEffects.Add(((RemoveAllDebuff), ("Cure All", "Target card have a 60% chance of heal each debuff that have."), new double[] { 200, 1000, 1000, 0 }));
            SingleMagicalEffects.Add(((RemoveRestrictions), ("Remove Restrictions", "Target card can have a 70% chance of heal any restrictions."), new double[] { 100, 100, 400, 300 }));
            SingleMagicalEffects.Add(((DoingEffect.TowerofArms), ("Tower of Guns", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Militarism en 200% y el resto en 100%."), new double[] { 500, 500, 500, 1000 }));
            SingleMagicalEffects.Add(((DoingEffect.TowerofSalomon), ("Tower of Salomon", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Knowledge en 200% y el resto en 100%."), new double[] { 500, 1000, 500, 500 }));
            SingleMagicalEffects.Add(((DoingEffect.TowerofGod), ("Tower of God", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Faith en 200% y el resto en 100%."), new double[] { 1000, 500, 500, 500 }));
            SingleMagicalEffects.Add(((DoingEffect.TowerofMoney), ("Tower of Money", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Capital en 200% y el resto en 100%."), new double[] { 500, 500, 1000, 500 }));

            #endregion

            #region Efectos sencillos de cartas naturales

            SingleNaturalEffects.Add(((RestrictAttack), ("A Sword of Reveling Light", "The target can't attack for a moderate amount of turns. As 30% chance of fail."), new double[] { 200, 200, 0, 100 }));
            SingleNaturalEffects.Add(((RestrictEffects), ("Silence", "The target can't use any effect for a moderate amount of turns. As 30% chance of fail."), new double[] { 300, 200, 0, 100 }));
            SingleNaturalEffects.Add(((SacrificeVIT), ("Berserk", "Turn half of VIT into ATK power."), new double[] { 0, 300, 150, 100 }));
            SingleNaturalEffects.Add(((VampireFang), ("Turn Undead", "Turns target in a vampire. Can use skill Vampire's Fang: Turn half of the damage dealed into VIT."), new double[] { 0, 300, 200, 100 }));
            SingleNaturalEffects.Add(((RemoveDebuff), ("Cure", "Target card have a 60% chance of heal a debuff."), new double[] { 100, 400, 200, 0 }));
            SingleNaturalEffects.Add(((RemoveAllDebuff), ("Cure All", "Target card have a 60% chance of heal each debuff that have."), new double[] { 200, 1000, 1000, 0 }));
            SingleNaturalEffects.Add(((RemoveRestrictions), ("Remove Restrictions", "Target card can have a 70% chance of heal any restrictions."), new double[] { 100, 100, 400, 300 }));

            #endregion

        }


        #region Efectos sencillos:

        /// <summary>Le suma a la VIT de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] Heal(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {

            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //creo el random
            System.Random r = new System.Random();

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                target.VIT += (r.Next(1, 15) * 100);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                pay.AField.SetCard(target, pos1);

                return new Player[] { pay, pay };

            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = user.AField.Find(target);

                target.VIT += (r.Next(1, 15) * 100);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                user.AField.SetCard(target, pos1);

                return new Player[] { pay, user };

            }
        }

        /// <summary>Le suma al ATK de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] IncreaseATK(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            //creo el random
            System.Random r = new System.Random();

            if (target == null)
            {
                return new Player[] { pay, user };
            }


            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                target.ATK += (r.Next(1, 15) * 100);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                pay.AField.SetCard(target, pos1);

                return new Player[] { pay, pay };
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = user.AField.Find(target);

                target.ATK += (r.Next(1, 15) * 100);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                user.AField.SetCard(target, pos1);

                return new Player[] { pay, user };

            }
        }

        /// <summary>Le suma al DEF de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] IncreaseDEF(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            //creo el random
            System.Random r = new System.Random();

            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                target.DEF += (r.Next(1, 15) * 100);

                pay.AField.SetCard(target, pos1);

                return new Player[] { pay, pay };
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = user.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                target.DEF += (r.Next(1, 15) * 100);


                user.AField.SetCard(target, pos1);

                return new Player[] { pay, user };

            }
        }

        /// <summary>Le resta al ATK de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] DecreaseATK(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            //creo el random
            System.Random r = new System.Random();

            if (target == null)
            {
                return new Player[] { pay, tgt };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, pay, pay.AField.Find(target));
                pay = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.ATK -= (r.Next(1, 15) * 100);
                }

                pay.AField.SetCard(target, pos1);

                return new Player[] { pay, pay };
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = tgt.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));
                tgt = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.ATK -= (r.Next(1, 15) * 100);
                }

                tgt.AField.SetCard(target, pos1);

                return new Player[] { pay, tgt };

            }
        }

        /// <summary>Le resta al DEF de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] DecreaseDEF(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            //creo el random
            System.Random r = new System.Random();

            if (target == null)
            {
                return new Player[] { pay, tgt };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, pay, pay.AField.Find(target));
                pay = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.DEF -= (r.Next(1, 15) * 100);
                }

                pay.AField.SetCard(target, pos1);

                return new Player[] { pay, pay };
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = tgt.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));
                tgt = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.DEF -= (r.Next(1, 15) * 100);
                }

                tgt.AField.SetCard(target, pos1);

                return new Player[] { pay, tgt };

            }

        }

        /// <summary>Le resta a la VIT de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] DecreaseVIT(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            //creo el random
            System.Random r = new System.Random();

            if (target == null)
            {
                return new Player[] { pay, tgt };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, pay, pay.AField.Find(target));
                pay = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.VIT -= (r.Next(1, 15) * 100);
                }

                //si la carta murió la quito del campo y la mando al cementerio
                if (target.VIT <= 0)
                {
                    pay.AField.UsedOrDestroyed(target);
                    pay.Cementery.ToCementery(target);
                }

                //sino la pongo en el campo
                else
                {
                    pay.AField.SetCard(target, pos1);
                }


                return new Player[] { pay, pay };
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = tgt.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //chequeo la inmunidad
                var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));
                tgt = inmunitycheck.enemy;

                //si después de chequear hay que activar el efecto
                if (inmunitycheck.NeedToActiveEffect)
                {
                    //se hace
                    target.VIT -= (r.Next(1, 15) * 100);
                }

                //si la carta murió la quito del campo y la mando al cementerio
                if (target.VIT <= 0)
                {
                    tgt.AField.UsedOrDestroyed(target);
                    tgt.Cementery.ToCementery(target);
                }

                //sino la pongo en el campo
                else
                {
                    tgt.AField.SetCard(target, pos1);
                }


                return new Player[] { pay, tgt };

            }

        }

        /// <summary>Le suma al ATK, DEF y VIT de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] IncreaseStats(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            Player[] both = IncreaseATK(pay, costs, card, target, amount, user);
            both = IncreaseDEF(both[0], costs, card, target, amount, both[1]);
            both = Heal(both[0], costs, card, target, amount, both[1]);

            return both;
        }

        /// <summary>Le resta al ATK, DEF y VIT de la carta objetivo una cantidad determinada de forma random.</summary>
        public static Player[] DecreaseStats(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            if (target == null)
            {
                return new Player[] { pay, tgt };
            }
            //Chequeo la inmunidad
            var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));

            //Creo el resultado
            Player[] both = new Player[] { pay, inmunitycheck.enemy };

            //si después del chequeo de inmunidad hay que activar el efecto
            if (inmunitycheck.NeedToActiveEffect)
            {
                //se hace y se returna aquí
                both = DecreaseATK(pay, costs, card, target, amount, inmunitycheck.enemy);
                both = DecreaseDEF(both[0], costs, card, target, amount, both[1]);
                both = DecreaseVIT(both[0], costs, card, target, amount, both[1]);

                return new Player[] { pay, inmunitycheck.enemy };
            }

            return both;
        }

        /// <summary>Regresa del cementerio una carta.</summary>
        public static Player[] ReturnFromGraveyard(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //Si la carta está en el cementerio y hay espacio para invocarla en el campo 
                if (pay.Cementery.IsInCementery(target) && pay.AField.AreNaturalCardsInvokable())
                {
                    //entonces se invoca en un lugar random del campo
                    pay.AField.ReturnToField(card, pay);
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                return new Player[] { pay, pay };
            }

            else
            {
                //Si la carta está en el cementerio y hay espacio para invocarla en el campo 
                if (user.Cementery.IsInCementery(target) && user.AField.AreNaturalCardsInvokable())
                {
                    //entonces se invoca en un lugar random del campo
                    user.AField.ReturnToField(card, pay);
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];


                return new Player[] { pay, user };
            }

        }

        /// <summary>Regresa una carta del cementerio a la mano.</summary>
        public static Player[] ReturnToHand(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Si la carta está en el cementerio y hay espacio en la mano (menos de 10 cartas)
                if (pay.Cementery.IsInCementery(target) && pay.CardsInHand.Hand.Length <= 10)
                {
                    pay.CardsInHand.Add(target);
                    pay.Cementery.RemoveformGraveyard(target);
                }

            }

            else
            {
                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Si la carta está en el cementerio y hay espacio en la mano (menos de 10 cartas)
                if (user.Cementery.IsInCementery(target) && user.CardsInHand.Hand.Length <= 10)
                {
                    user.CardsInHand.Add(target);
                    user.Cementery.RemoveformGraveyard(target);
                }
            }

            return new Player[] { pay, user };
        }

        /// <summary>Se transforma en otra carta en el campo.</summary>
        public static Player[] TransformInto(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                pay.AField.SetCard(target, pay.AField.Find(card));
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                pay.AField.SetCard(target, pay.AField.Find(card));

            }

            return new Player[] { pay, user };
        }

        /// <summary>Impide que una carta ataque por unos turnos (parámetro double amount). Tiene 30% posibilidad de fallar.</summary>
        public static Player[] RestrictAttack(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            if (target == null)
            {
                return new Player[] { pay, tgt };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un random a ver si el porciento cumple el requisito
                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    //luego chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(target, pay, pay.AField.Find(target));
                    pay = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se hace
                        card.CanAttack = false;
                        card.RestrictAttackTurns = ((int)amount);

                        pay.AField.SetCard(target, pos1);

                    }

                    return new Player[] { pay, pay };

                }


            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = tgt.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un random a ver si el porciento cumple el requisito
                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    //luego chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));
                    tgt = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se hace
                        card.CanAttack = false;
                        card.RestrictAttackTurns = ((int)amount);
                        pay.AField.SetCard(card, pos0);
                        tgt.AField.SetCard(target, pos1);
                    }

                    return new Player[] { pay, tgt };

                }

            }


            return new Player[] { pay, tgt };
        }

        /// <summary>Impide que una carta use efectos por unos turnos (parámetro double amount). Tiene 30% posibilidades de fallar.</summary>
        public static Player[] RestrictEffects(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            if (target == null)
            {
                return new Player[] { pay, tgt };
            }
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un random a ver si el porciento cumple el requisito
                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    //luego chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(target, pay, pay.AField.Find(target));
                    pay = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se hace
                        card.CanUseEffect = false;
                        card.RestrictEffectTurns = ((int)amount);
                    }

                    pay.AField.SetCard(card, pos0);
                    pay.AField.SetCard(target, pos1);

                    return new Player[] { pay, pay };

                }

            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = tgt.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //creo un random a ver si el porciento cumple el requisito
                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    //luego chequeo la inmunidad
                    var inmunitycheck = DoingEffect.CheckInmunityStates(target, tgt, tgt.AField.Find(target));
                    tgt = inmunitycheck.enemy;

                    //si después de chequear hay que activar el efecto
                    if (inmunitycheck.NeedToActiveEffect)
                    {
                        //se hace
                        card.CanUseEffect = false;
                        card.RestrictEffectTurns = ((int)amount);
                    }

                    pay.AField.SetCard(card, pos0);
                    tgt.AField.SetCard(target, pos1);

                    return new Player[] { pay, tgt };

                }

                pay.AField.SetCard(card, pos0);
                tgt.AField.SetCard(target, pos1);

            }


            return new Player[] { pay, tgt };
        }

        /// <summary>Convierte el 50% de la vida en poder de ataque.</summary>
        public static Player[] SacrificeVIT(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                target.ATK += card.VIT / 2;
                target.VIT -= card.VIT / 2;

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = user.AField.Find(target);

                target.ATK += card.VIT / 2;
                target.VIT -= card.VIT / 2;

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                pay.AField.SetCard(card, pos0);
                user.AField.SetCard(target, pos1);

            }

            return new Player[] { pay, user };
        }

        /// <summary>Ataca y convierte 50% del daño realizado en vida.</summary>
        public static Player[] VampireFang(Player pay, List<double> costs, ACard card, ACard target, double amount, Player tgt)
        {
            if (target == null)
            {
                return new Player[] { pay, tgt };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == tgt)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Hago el daño
                Player[] both = DoingEffect.DamageDealer(pay, costs, card, pay, new ACard[] { target });

                //Y obtengo el daño contando con el aumento o decremento elemental
                ACard temp = DoingEffect.AddElementalDamage(card, target).card;

                //Y la mitad de ese daño se la agrego a la carta
                card.VIT += temp.ATK / 2;

                return both;
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!tgt.AField.IsInField(target))
                {
                    return new Player[] { pay, tgt };
                }

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //Hago el daño
                Player[] both = DoingEffect.DamageDealer(pay, costs, card, tgt, new ACard[] { target });

                //Y obtengo el daño contando con el aumento o decremento elemental
                ACard temp = DoingEffect.AddElementalDamage(card, target).card;

                //Y la mitad de ese daño se la agrego a la carta
                card.VIT += temp.ATK / 2;

                return both;

            }
        }

        /// <summary>Elimina un efecto de estado anormal. Tiene 40% de posibilidad de fallar.</summary>
        public static Player[] RemoveDebuff(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                //si el random esta entre 40 y 100
                if (porcent >= 40)
                {
                    //Y si la carta no tiene varios estados anormales
                    if (!target.AsManyAbnormalState)
                    {
                        //Le quito el que tiene y reinicio la lista
                        target.AsAnAbnormalState = false;
                        target.AbnormalStateDuration = 0;
                        target.ListOfAbnormalStateDuration = new();
                    }

                    //en caso de tener varios estados anormales
                    else
                    {
                        //tomo el el último estado anormal
                        int ind = target.ListOfAbnormalStateDuration.Count - 1;
                        var keys = target.ListOfAbnormalStateDuration.Keys.ToList();
                        string abnormalstate = keys[ind];

                        //lo elimino de la lista de estados anormales
                        target.ListOfAbnormalStateDuration.Remove(abnormalstate);

                        //si la lista tiene un solo elemento entonces ya no tiene varios efectos de estado y cambio esa propiedad
                        if (target.ListOfAbnormalStateDuration.Count == 1)
                        {
                            target.AsManyAbnormalState = false;
                        }

                        //renuevo el valor de duracion del estado anormal, que en caso de ser varios toma el mayor valor de la lista
                        //para eso obtengo los valores
                        var values = target.ListOfAbnormalStateDuration.Values.ToList();

                        //Busco el mayor
                        int max = 0;

                        foreach (int val in values)
                        {
                            max = (max < val) ? val : max;
                        }

                        //y se lo asigno a la propiedad
                        target.AbnormalStateDuration = max;

                    }

                }

            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = user.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                //si el random esta entre 40 y 100
                if (porcent >= 40)
                {
                    //Y si la carta no tiene varios estados anormales
                    if (!target.AsManyAbnormalState)
                    {
                        //Le quito el que tiene y reinicio la lista
                        target.AsAnAbnormalState = false;
                        target.AbnormalStateDuration = 0;
                        target.ListOfAbnormalStateDuration = new();
                    }

                    //en caso de tener varios estados anormales
                    else
                    {
                        //tomo el el último estado anormal
                        int ind = target.ListOfAbnormalStateDuration.Count - 1;
                        var keys = target.ListOfAbnormalStateDuration.Keys.ToList();
                        string abnormalstate = keys[ind];

                        //lo elimino de la lista de estados anormales
                        target.ListOfAbnormalStateDuration.Remove(abnormalstate);

                        //si la lista tiene un solo elemento entonces ya no tiene varios efectos de estado y cambio esa propiedad
                        if (target.ListOfAbnormalStateDuration.Count == 1)
                        {
                            target.AsManyAbnormalState = false;
                        }

                        //renuevo el valor de duracion del estado anormal, que en caso de ser varios toma el mayor valor de la lista
                        //para eso obtengo los valores
                        var values = target.ListOfAbnormalStateDuration.Values.ToList();

                        //Busco el mayor
                        int max = 0;

                        foreach (int val in values)
                        {
                            max = (max < val) ? val : max;
                        }

                        //y se lo asigno a la propiedad
                        target.AbnormalStateDuration = max;

                    }

                }

                pay.AField.SetCard(card, pos0);
                user.AField.SetCard(target, pos1);

            }


            return new Player[] { pay, user };
        }

        /// <summary>Elimina cualquier efecto de estado anormal. Tiene 40% de posibilidad de fallar cada uno.</summary>
        public static Player[] RemoveAllDebuff(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //obtengo la lista de los estados y creo un random
                List<string> keys = target.ListOfAbnormalStateDuration.Keys.ToList();
                System.Random r = new System.Random();

                for (int k = keys.Count - 1; k >= 0; k--)
                {
                    int porcent = r.Next(0, 100);

                    //si el random esta entre 40 y 100
                    if (porcent >= 40)
                    {
                        //lo elimino de la lista de estados anormales
                        target.ListOfAbnormalStateDuration.Remove(keys[k]);
                    }
                }

                //si la lista tiene un solo elemento entonces ya no tiene varios efectos de estado y cambio esa propiedad
                if (target.ListOfAbnormalStateDuration.Count == 1)
                {
                    target.AsManyAbnormalState = false;
                }

                //o si no tiene ninguno cambio ambas propiedades ya que no tiene ni uno
                else if (target.ListOfAbnormalStateDuration.Count == 0)
                {
                    target.AsAnAbnormalState = false;
                    target.AsManyAbnormalState = false;
                }

                //renuevo el valor de duracion del estado anormal, que en caso de ser varios toma el mayor valor de la lista
                //para eso obtengo los valores
                var values = target.ListOfAbnormalStateDuration.Values.ToList();

                //Variable para guardar el mayor
                int max = 0;

                //Si la lista tiene algún valor
                if (values.Count != 0)
                {
                    //tomo el mayor
                    foreach (int val in values)
                    {
                        max = (max < val) ? val : max;
                    }
                }

                //y se lo asigno a la propiedad
                target.AbnormalStateDuration = max;

            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de ambas cartas y las pongo al final donde mismo ya modificadas
                (int x1, int y1) pos0 = pay.AField.Find(card);
                (int x2, int y2) pos1 = user.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                //obtengo la lista de los estados y creo un random
                List<string> keys = target.ListOfAbnormalStateDuration.Keys.ToList();
                System.Random r = new System.Random();

                for (int k = keys.Count - 1; k >= 0; k--)
                {
                    int porcent = r.Next(0, 100);

                    //si el random esta entre 40 y 100
                    if (porcent >= 40)
                    {
                        //lo elimino de la lista de estados anormales
                        target.ListOfAbnormalStateDuration.Remove(keys[k]);
                    }
                }

                //si la lista tiene un solo elemento entonces ya no tiene varios efectos de estado y cambio esa propiedad
                if (target.ListOfAbnormalStateDuration.Count == 1)
                {
                    target.AsManyAbnormalState = false;
                }

                //o si no tiene ninguno cambio ambas propiedades ya que no tiene ni uno
                else if (target.ListOfAbnormalStateDuration.Count == 0)
                {
                    target.AsAnAbnormalState = false;
                    target.AsManyAbnormalState = false;
                }

                //renuevo el valor de duracion del estado anormal, que en caso de ser varios toma el mayor valor de la lista
                //para eso obtengo los valores
                var values = target.ListOfAbnormalStateDuration.Values.ToList();

                //Variable para guardar el mayor
                int max = 0;

                //Si la lista tiene algún valor
                if (values.Count != 0)
                {
                    //tomo el mayor
                    foreach (int val in values)
                    {
                        max = (max < val) ? val : max;
                    }
                }

                //y se lo asigno a la propiedad
                target.AbnormalStateDuration = max;

                pay.AField.SetCard(card, pos0);
                user.AField.SetCard(target, pos1);

            }


            return new Player[] { pay, user };

        }

        /// <summary>Elimina cualquier restricción. Tiene 30% de posibilidad de fallar.</summary>
        public static Player[] RemoveRestrictions(Player pay, List<double> costs, ACard card, ACard target, double amount, Player user)
        {
            if (target == null)
            {
                return new Player[] { pay, user };
            }

            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == user)
            {
                //si la carta no está en el campo no hace nada
                if (!pay.AField.IsInField(target))
                {
                    return new Player[] { pay, pay };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = pay.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    target.CanAttack = true;
                    target.CanUseEffect = true;
                    target.RestrictAttackTurns = 0;
                    target.RestrictEffectTurns = 0;
                    pay.AField.SetCard(card, pos1);
                }
            }

            else
            {
                //si la carta no está en el campo no hace nada
                if (!user.AField.IsInField(target))
                {
                    return new Player[] { pay, user };
                }

                //obtengo la ubicación de la carta objetivo
                (int x2, int y2) pos1 = user.AField.Find(target);

                //aquí paga el precio de usar el efecto
                pay.Kingdom.Faith -= costs[0];
                pay.Kingdom.Knowledge -= costs[1];
                pay.Kingdom.Capital -= costs[2];
                pay.Kingdom.Militarism -= costs[3];

                System.Random r = new System.Random();
                int porcent = r.Next(0, 100);

                if (porcent >= 30)
                {
                    target.CanAttack = true;
                    target.CanUseEffect = true;
                    target.RestrictAttackTurns = 0;
                    target.RestrictEffectTurns = 0;
                    user.AField.SetCard(target, pos1);
                }

            }


            return new Player[] { pay, user };
        }

        #endregion

    }

}