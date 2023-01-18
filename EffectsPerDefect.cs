namespace ProjectClasses
{

    /// <summary>Clase estática de los efectos por defecto que serán llamados por delegados.</summary>
    public static class DoingEffect
    {
        //Propiedades a usar
        public static double DamageToTurnInHeal { get; set; }

        #region Efectos básicos:

        /// <summary>Hace daño del ataque a la defensa y vitalidad de su/s objetivo/s.</summary>
        public static Player[] DamageDealer(Player pay, List<double> costs, ACard dealer, Player enemy, ACard[] targets)
        {
            //compruebo si los jugadores son los mismos entonces el efecto sea sobre uno solo
            if (pay == enemy)
            {
                //Si hay objetivos entonces
                if (targets[0] != null)
                {
                    //Recorro por cada uno
                    for (int k = 0; k < targets.Length; k++)
                    {
                        //Si esa carta se encuentra en el campo
                        if (pay.AField.IsInField(targets[k]))
                        {
                            //Obtengo la carta y sus coordenadas
                            ACard target = targets[k];
                            (int x, int y) location = pay.AField.Find(target);

                            //Compruebo si es inmune a daño de ataque normal
                            if (target.AsPhysicalInmunity)
                            {
                                //si lo es entonces reduzco la duración de dicha inmunidad
                                target.InmunityPhysicalDuration -= 1;

                                //si dicha inmunidad llega a 0 la elimino
                                if (target.InmunityPhysicalDuration == 0)
                                {
                                    target.AsPhysicalInmunity = false;
                                }

                                //la coloco en el campo con los cambios y listo, continuo al próximo objetivo
                                pay.AField.SetCard(target, location);
                                continue;
                            }

                            //En caso que no sea inmune obtengo el daño contando con el daño elemental y el posible aumento de 
                            //la defensa del objetivo
                            var res = AddElementalDamage(dealer, targets[k]);
                            dealer = res.card;
                            target = res.target;
                            double dmg = dealer.ATK;

                            //si la defensa es menor que el daño
                            if (target.DEF - dmg < 0)
                            {
                                //entonces el nuevo daño es igual al daño menos la DEF y esta es 0 después.
                                dmg -= target.DEF;
                                target.DEF = 0;

                                //luego si este nuevo daño es mayor que la VIT
                                if (target.VIT - dmg < 0)
                                {
                                    //Entonces es el nuevo daño es igual al daño menos la VIT, este pasa como daño directo a la vida
                                    //del jugador. La VIT es 0 y la carta es destruida y enviada la cementerio. 
                                    dmg -= target.VIT;
                                    target.VIT = 0;
                                    pay.Life -= dmg;
                                    pay.Cementery.ToCementery(target);
                                    pay.AField.UsedOrDestroyed(target);
                                }

                                //si el daño es menor que la VIT se le resta a la misma y ya.
                                else
                                {
                                    target.VIT -= dmg;
                                    pay.AField.SetCard(target, location);
                                }
                            }

                            else
                            {
                                target.DEF -= dmg;
                                pay.AField.SetCard(target, location);
                            }
                        }


                    }

                }

                //Sino
                else
                {
                    //solo obtengo el daño 
                    double dmg = dealer.ATK;

                    //si este es mayor que la vida del oponente, la reduzco a 0
                    if (pay.Life <= dmg)
                    {
                        pay.Life = 0;
                    }
                    //sino
                    else
                    {
                        //ataco directo
                        pay.Life -= dmg;
                    }

                }

                dealer.ActivateEffectOrAttackOnce = true;

            }

            else
            {
                //Si hay objetivos entonces
                if (targets[0] != null)
                {
                    //Recorro por cada uno
                    for (int k = 0; k < targets.Length; k++)
                    {
                        //Si esa carta se encuentra en el campo
                        if (enemy.AField.IsInField(targets[k]))
                        {
                            //Obtengo la carta y sus coordenadas
                            ACard target = targets[k];
                            (int x, int y) location = enemy.AField.Find(target);

                            //Compruebo si es inmune a daño de ataque normal
                            if (target.AsPhysicalInmunity)
                            {
                                //si lo es entonces reduzco la duración de dicha inmunidad
                                target.InmunityPhysicalDuration -= 1;

                                //si dicha inmunidad llega a 0 la elimino
                                if (target.InmunityPhysicalDuration == 0)
                                {
                                    target.AsPhysicalInmunity = false;
                                }

                                //la coloco en el campo con los cambios y listo, continuo al próximo objetivo
                                enemy.AField.SetCard(target, location);
                                continue;
                            }

                            //En caso que no sea inmune obtengo el daño contando con el daño elemental y el posible aumento de 
                            //la defensa del objetivo
                            var res = AddElementalDamage(dealer, targets[k]);
                            dealer = res.card;
                            target = res.target;
                            double dmg = dealer.ATK;

                            //si la defensa es menor que el daño
                            if (target.DEF - dmg < 0)
                            {
                                //entonces el nuevo daño es igual al daño menos la DEF y esta es 0 después.
                                dmg -= target.DEF;
                                target.DEF = 0;

                                //luego si este nuevo daño es mayor que la VIT
                                if (target.VIT - dmg < 0)
                                {
                                    //Entonces es el nuevo daño es igual al daño menos la VIT, este pasa como daño directo a la vida
                                    //del jugador. La VIT es 0 y la carta es destruida y enviada la cementerio. 
                                    dmg -= target.VIT;
                                    target.VIT = 0;
                                    enemy.Life -= dmg;
                                    enemy.Cementery.ToCementery(target);
                                    enemy.AField.UsedOrDestroyed(target);
                                }

                                //si el daño es menor que la VIT se le resta a la misma y ya.
                                else
                                {
                                    target.VIT -= dmg;
                                    enemy.AField.SetCard(target, location);
                                }
                            }

                            else
                            {
                                target.DEF -= dmg;
                                enemy.AField.SetCard(target, location);
                            }
                        }


                    }

                }

                //Sino
                else
                {
                    //solo obtengo el daño 
                    double dmg = dealer.ATK;

                    //si este es mayor que la vida del oponente, la reduzco a 0
                    if (enemy.Life <= dmg)
                    {
                        enemy.Life = 0;
                    }
                    //sino
                    else
                    {
                        //ataco directo
                        enemy.Life -= dmg;
                    }

                }

                dealer.ActivateEffectOrAttackOnce = true;

            }

            return new Player[] { pay, enemy };

        }

        /// <summary>Sobrecarga que solo necesita de la carta y el daño que sufrirá.</summary>
        public static ACard DamageDealer(ACard target, double dmg)
        {
            //si la defensa es menor que el daño
            if (target.DEF - dmg < 0)
            {
                //entonces el nuevo daño es igual al daño menos la DEF y esta es 0 después.
                dmg -= target.DEF;
                target.DEF = 0;

                //luego si este nuevo daño es mayor que la VIT
                if (dmg > target.VIT)
                {
                    //Entonces es el nuevo daño es igual al daño menos la VIT, este pasa como daño directo a la vida
                    //del jugador. La VIT es 0. 
                    dmg -= target.VIT;
                    target.VIT = 0;
                }

                //si el daño es menor que la VIT se le resta a la misma y ya.
                else
                {
                    target.VIT -= dmg;
                }
            }

            //En caso que la defensa sea mayor que el daño, se le resta y ya
            else
            {
                target.DEF -= dmg;
            }

            return target;
        }

        /// <summary>Método estático para añadir los efectos de los elementos al ataque.</summary>
        public static (ACard card, ACard target) AddElementalDamage(ACard card, ACard target)
        {
            List<string> list = card.ActualFourElements;

            List<List<string>> strongto = card.ElementsObject.AllElementsRelationOfACard(list, 1);
            List<List<string>> resistto = target.ElementsObject.AllElementsRelationOfACard(list, 2);
            List<List<string>> inmunityto = target.ElementsObject.AllElementsRelationOfACard(list, 3);

            //Si hay algún elemento que sea débil a algún elemento de la carta que ataca
            foreach (List<string> listofelem in strongto)
            {
                foreach (string elem in listofelem)
                {
                    //entonces su ataque aumenta en un 25%
                    if (target.ContainElement(elem))
                    {
                        card.ATK += Math.Round(card.ATK * (1 / 4));
                    }
                }
            }

            //Si hay algún elemento que sea resistente a algún elemento de la carta que ataca
            foreach (List<string> listofelem in resistto)
            {
                foreach (string elem in listofelem)
                {
                    //entonces su defensa aumenta en un 25%
                    if (target.ContainElement(elem))
                    {
                        target.DEF += Math.Round(target.DEF * (1 / 4));
                    }
                }
            }

            //Si hay algún elemento que sea inmune a algún elemento de la carta que ataca
            foreach (List<string> listofelem in inmunityto)
            {
                foreach (string elem in listofelem)
                {
                    //entonces su ataque se reduce a la mitad
                    if (target.ContainElement(elem))
                    {
                        card.ATK -= Math.Round(card.ATK * (1 / 2));
                    }
                }
            }

            return (card, target);


        }

        /// <summary>Método estático para checar si una carta tiene algún estado de inmunidad parcial o total.</summary>
        public static (Player enemy, bool NeedToActiveEffect) CheckInmunityStates(ACard card, Player pay, (int x, int y) location)
        {
            //si la carta es inmune a efectos
            if (card.AsEffectInmunity)
            {
                //le quito uno a la duración de la inmunidad
                card.InmunityEffectDuration -= 1;

                //Y si es 0 le quito la inmunidad
                if (card.InmunityEffectDuration == 0)
                {
                    card.AsEffectInmunity = false;
                }

                //la coloco en el campo y la devuelvo
                pay.AField.SetCard(card, location);
                return (pay, false);
            }

            //Si tiene chance de inmunidad
            if (card.AsInmunityChance)
            {
                //se comprueba si saca más de 50 con un random
                System.Random r = new System.Random();

                if (r.Next(0, 100) >= 50)
                {
                    //le quito uno a la duración de la inmunidad
                    card.InmunityChanceDuration -= 1;

                    //Y si es 0 le quito la inmunidad
                    if (card.InmunityChanceDuration == 0)
                    {
                        card.AsInmunityChance = false;
                    }

                    //la coloco en el campo y la devuelvo
                    pay.AField.SetCard(card, location);
                    return (pay, false);
                }

                else
                {
                    //le quito uno a la duración de la inmunidad
                    card.InmunityChanceDuration -= 1;

                    //Y si es 0 le quito la inmunidad
                    if (card.InmunityChanceDuration == 0)
                    {
                        card.AsInmunityChance = false;
                    }

                    //la coloco en el campo y la devuelvo
                    pay.AField.SetCard(card, location);
                    return (pay, true);
                }
            }

            //la coloco en el campo y la devuelvo
            pay.AField.SetCard(card, location);
            return (pay, true);


        }

        /// <summary>Método para comprobar si un efecto es negativo, o sea reduce stats, prohibe acciones y así, toma además los que tienen efectos positivos y negaticos a la vez. Solo devuelve false si el efecto es positivo.</summary>
        public static bool GiveNegativeEffects(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> effect, string efctname)
        {
            //creo una carta y un jugador para probar el efecto
            ACard card = new(new Attribute(1000, 1000, 1000, new Element()), new Effects(), new Description("Test", "Test", new AType(1), new Race(1), 1000, 1000, 1000, 1000));
            Player player = new(new Kingdoms("Faith", 1000, 1000, 1000, 1000), new CardsInHand(new ACard[] { }), new DeckOfCards(new ACard[] { }), new Cementery(), new AField(), new Rules(), 1000);

            //le quito el primer elemento ya que en dependencia de cual sea la carta puede ser inmune al efecto y ser tomado como bueno cuando
            //no lo es
            card.CardElementName1 = "None";

            //tomo el nombre del efecto palabra a palabra
            List<string> efctnameparted = new();
            string pre = "";

            for (int j = 0; j < efctname.Length; j++)
            {
                if (efctname[j] == ' ')
                {
                    efctnameparted.Add(pre);
                    pre = "";
                }

                else
                {
                    pre += efctname[j];
                }
            }

            player.AField.InvocationSpecial(card, player.AField.FindEmpty(1), player);

            for (int j = 0; j < 500; j++)
            {
                effect.Invoke(player, (new double[] { 100, 100, 100, 100 }).ToList(), card, card, 3, player);

                //chequeo si hay algún cambio negativo, ignorando algunos efectos existentes que tienen parte negativa pero son benignos porque si
                if (card.ATK < 1000 || card.DEF < 1000 || card.VIT < 1000 || !card.CanAttack || !card.CanUseEffect || card.AsAnAbnormalState
                || card.CapitalCost > 1000 || card.FaithCost > 1000 || card.MilitarCost > 1000 || card.KnowledgeCost > 1000 ||
                !card.CanSelectTarget)
                {
                    //efectos benignos a ignorar
                    if ((efctname == "Berserk" || efctname == "Turn Undead" || efctnameparted.Contains("Berserk") || efctnameparted.Contains("Turn")))
                    {
                        return false;
                    }

                    return true;
                }

            }

            return false;
        }

        /// <summary>Sobrecarga para efectos de varios objetivos.</summary>
        public static bool GiveNegativeEffects(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> effect, string efctname)
        {
            //con ver que le hace a uno basta

            //creo una carta y un jugador para probar el efecto
            ACard card = new(new Attribute(1000, 1000, 1000, new Element()), new Effects(), new Description("Test", "Test", new AType(1), new Race(1), 1000, 1000, 1000, 1000));
            Player player = new(new Kingdoms("Faith", 1000, 1000, 1000, 1000), new CardsInHand(new ACard[] { }), new DeckOfCards(new ACard[] { }), new Cementery(), new AField(), new Rules(), 1000);

            //le quito el primer elemento ya que en dependencia de cual sea la carta puede ser inmune al efecto y ser tomado como bueno cuando
            //no lo es
            card.CardElementName1 = "None";

            //tomo el nombre del efecto palabra a palabra
            List<string> efctnameparted = new();
            string pre = "";

            for (int j = 0; j < efctname.Length; j++)
            {
                if (efctname[j] == ' ')
                {
                    efctnameparted.Add(pre);
                    pre = "";
                }

                else
                {
                    pre += efctname[j];
                }
            }

            player.AField.InvocationSpecial(card, player.AField.FindEmpty(1), player);

            for (int j = 0; j < 500; j++)
            {
                effect.Invoke(player, (new double[] { 100, 100, 100, 100 }).ToList(), card, new ACard[] { card }, 3, player);

                //chequeo si hay algún cambio negativo, ignorando algunos efectos existentes que tienen parte negativa pero son benignos porque si
                if (card.ATK < 1000 || card.DEF < 1000 || card.VIT < 1000 || !card.CanAttack || !card.CanUseEffect || card.AsAnAbnormalState
                || card.CapitalCost > 1000 || card.FaithCost > 1000 || card.MilitarCost > 1000 || card.KnowledgeCost > 1000 ||
                !card.CanSelectTarget)
                {
                    //efectos benignos a ignorar
                    if ((efctname == "Berserk" || efctname == "Turn Undead" || efctnameparted.Contains("Berserk") || efctnameparted.Contains("Turn")))
                    {
                        return false;
                    }

                    return true;
                }

            }

            return false;


        }

        public static bool IsAMagicTowerCard(string name)
        {
            if (name == "Tower of Guns" || name == "Tower of Money" || name == "Tower of Salomon" || name == "Tower of God")
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Efectos únicos de cartas mágicas:

        /// <summary>Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Knowledge en 200% y el resto en 100%.</summary>
        public static Player[] TowerofSalomon(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //Si antes de este no se ha usado un efecto de torre
            if (!pay.UsedOneTowerEffect)
            {
                //Aplico los cambios de 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.RecuperationPerTurnKnowledge * 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }

            //en caso de que si
            else
            {
                //reseteo los valores de recuperación a los originales 
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.OriginalRecuperationKnowledge;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.OriginalRecuperationCapital;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.OriginalRecuperationFaith;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.OriginaRecuperationMilitar;

                //y después los modifico 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.RecuperationPerTurnKnowledge * 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }


            //Cambio los valores booleanos para comprobar que se usó este efecto
            pay.UsedOneTowerEffect = true;
            pay.UsedKnowledgeUpEffect = true;

            return new Player[] { pay, enemy };
        }

        /// <summary>Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Faith en 200% y el resto en 100%.</summary>
        public static Player[] TowerofGod(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //Si antes de este no se ha usado un efecto de torre
            if (!pay.UsedOneTowerEffect)
            {
                //Aplico los cambios de 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.RecuperationPerTurnFaith * 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }

            //en caso de que si
            else
            {
                //reseteo los valores de recuperación a los originales 
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.OriginalRecuperationKnowledge;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.OriginalRecuperationCapital;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.OriginalRecuperationFaith;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.OriginaRecuperationMilitar;

                //y después los modifico 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.RecuperationPerTurnFaith * 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }


            //Cambio los valores booleanos para comprobar que se usó este efecto
            pay.UsedOneTowerEffect = true;
            pay.UsedFaithUpEffect = true;

            return new Player[] { pay, enemy };
        }

        /// <summary>Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Militarism en 200% y el resto en 100%.</summary>
        public static Player[] TowerofArms(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {

            //Si antes de este no se ha usado un efecto de torre
            if (!pay.UsedOneTowerEffect)
            {
                //Aplico los cambios de 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.RecuperationPerTurnMilitarism * 2;
            }

            //en caso de que si
            else
            {
                //reseteo los valores de recuperación a los originales 
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.OriginalRecuperationKnowledge;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.OriginalRecuperationCapital;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.OriginalRecuperationFaith;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.OriginaRecuperationMilitar;

                //y después los modifico 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital += pay.Kingdom.RecuperationPerTurnCapital / 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.RecuperationPerTurnMilitarism * 2;
            }


            //Cambio los valores booleanos para comprobar que se usó este efecto
            pay.UsedOneTowerEffect = true;
            pay.UsedMilitarismUpEffect = true;

            return new Player[] { pay, enemy };
        }

        /// <summary>Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Capital en 200% y el resto en 100%.</summary>
        public static Player[] TowerofMoney(Player pay, List<double> costs, ACard card, ACard target, double amount, Player enemy)
        {
            //Si antes de este no se ha usado un efecto de torre
            if (!pay.UsedOneTowerEffect)
            {
                //Aplico los cambios de 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.RecuperationPerTurnCapital * 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }

            //en caso de que si
            else
            {
                //reseteo los valores de recuperación a los originales 
                pay.Kingdom.RecuperationPerTurnKnowledge = pay.Kingdom.OriginalRecuperationKnowledge;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.OriginalRecuperationCapital;
                pay.Kingdom.RecuperationPerTurnFaith = pay.Kingdom.OriginalRecuperationFaith;
                pay.Kingdom.RecuperationPerTurnMilitarism = pay.Kingdom.OriginaRecuperationMilitar;

                //y después los modifico 200% en uno y 100% en el resto
                pay.Kingdom.RecuperationPerTurnKnowledge += pay.Kingdom.RecuperationPerTurnKnowledge / 2;
                pay.Kingdom.RecuperationPerTurnCapital = pay.Kingdom.RecuperationPerTurnCapital * 2;
                pay.Kingdom.RecuperationPerTurnFaith += pay.Kingdom.RecuperationPerTurnFaith / 2;
                pay.Kingdom.RecuperationPerTurnMilitarism += pay.Kingdom.RecuperationPerTurnMilitarism / 2;
            }


            //Cambio los valores booleanos para comprobar que se usó este efecto
            pay.UsedOneTowerEffect = true;
            pay.UsedCapitalUpEffect = true;

            return new Player[] { pay, enemy };
        }


        #endregion

    }


}