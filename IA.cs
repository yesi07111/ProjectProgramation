using System.Reflection;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;


namespace ProjectClasses
{
    /// <summary>Clase de las inteligencias artificiales.</summary>
    public class IA : Player
    {
        //Guarda las acciones a realizar por turno, varía según la IA
        public Dictionary<string, Func<Player, ACard[], ACard[], Player[]>> TurnActions { get; set; }

        //Lista de string de acciones llevadas a cabo
        public List<string> performedactions { get; set; }

        /// <summary>Inteligencias artificiales. Dificultad sencilla: (1) jugador random. Dificultad difícil: (2) jugador agresivo.</summary>
        public IA(int difficult, Kingdoms kingdom, CardsInHand cardsinhand, DeckOfCards deck, Cementery cementery, AField field, Rules rules, double life) : base(kingdom, cardsinhand, deck, cementery, field, rules, life)
        {
            //Propiedades propias del jugador IA:
            Kingdom = kingdom;
            CardsInHand = cardsinhand;
            DeckOfCards = deck;
            Cementery = cementery;
            AField = field;
            Life = life;
            CanDrawCard = true;
            TurnActions = new();
            performedactions = new();

            //Agregar turn action según el tipo de IA:

            //Dificultad 1: Fácil. IA: Random but inteligent.
            if (difficult == 1)
            {
                TurnActions.Add("MP1", OneInvocationPhase1);
                TurnActions.Add("BP", OneBattleAndEffectPhase);
                TurnActions.Add("MP2", OneInvocationPhase2);
            }

            //Dificultad 2: Difícil. IA: Attack & Powerful Cards Centred
            if (difficult == 2)
            {
                TurnActions.Add("MP1", TwoInvocationPhase1);
                TurnActions.Add("BP", TwoBattleandEffectsPhase);
                TurnActions.Add("MP2", TwoInvocationPhase2);
            }


        }

        #region IA: Random

        /// <summary>IA: Random. Primera parte de un turno para robar cartas e invocar.</summary>
        public Player[] OneInvocationPhase1(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //Aquí se ganan los puntos al inicio del turno.
            this.Kingdom.Faith += this.Kingdom.RecuperationPerTurnFaith;
            this.Kingdom.Knowledge += this.Kingdom.RecuperationPerTurnKnowledge;
            this.Kingdom.Capital += this.Kingdom.RecuperationPerTurnCapital;
            this.Kingdom.Militarism += this.Kingdom.RecuperationPerTurnMilitarism;

            //Aquí resetea los permisos de las cartas para usar un efecto o atacar
            List<ACard> cardsinfield = AField.GetNaturalCardsInField();

            foreach (ACard card in cardsinfield)
            {
                card.ActivateEffectOrAttackOnce = false;
            }

            //Aquí roba cartas, en caso de quedar y poder
            if (CanDrawCard)
            {
                CardsInHand.DrawingCards(DeckOfCards, Rules.CardsDrawPerTurn);
            }

            //si la mano no es vacía
            if (!this.CardsInHand.IsEmpty())
            {
                //intenta invocar lo que pueda de forma random
                RandomInvocation(enemy);
            }


            return new Player[] { this, enemy };

        }

        /// <summary>IA: Random. Última parte de un turno para invocar cartas.</summary>
        public Player[] OneInvocationPhase2(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //si la mano no es vacía
            if (!this.CardsInHand.IsEmpty())
            {
                //intenta invocar lo que pueda de forma random
                RandomInvocation(enemy);
            }
            return new Player[] { this, enemy };
        }

        /// <summary>IA: Random. Segunda parte de un turno para atacar o activar efectos.</summary>
        public Player[] OneBattleAndEffectPhase(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //***Activa efectos de forma random y ataca después

            //creo un random para varios usos
            System.Random r = new System.Random();

            //creo una lista para guardar las cartas que logren activar su efecto y por tanto no atacarán
            List<ACard> cardsthatusedeffect = new();

            //si hay cartas naturales invocadas
            if (AField.AreNaturalCardInvoked())
            {
                //entonces las obtengo 
                List<ACard> naturalcardsinvoked = AField.GetNaturalCardsInField();

                //obtengo además una lista de los objetivos ya que al ser seleccionados random, si uno muere debe eliminarse
                //de la lista de objetivos para no tener posibilidad de ser escogido otra vez
                List<ACard> targets = tgt.ToList();

                //y creo una lista de listas de bool que dicen cuales de los 4 efectos de cada carta se pueden pagar.
                List<List<bool>> EffectsThatCanBeUsedOrNotPerCard = new();

                //a cada carta en la lista de cartas invocadas.
                foreach (ACard card in naturalcardsinvoked)
                {
                    //le busco su lista de bool que dice cual de sus 4 efectos son pagables y la agrega a la lista de las listas
                    EffectsThatCanBeUsedOrNotPerCard.Add(CanPayEffect(card, this));
                }

                //quito de la lista las cartas que no pueden activar ningún efecto.
                List<ACard> CardsWithEffects = CardsThatCanUseEffect(naturalcardsinvoked, EffectsThatCanBeUsedOrNotPerCard);

                //uso el random para ver cuantas cartas va a activar efecto, de 1 a la cantidad de cartas que podrían
                int amount = r.Next(0, CardsWithEffects.Count);

                //trato de usar tantos efectos como se quiere
                for (int k = 0; k < amount; k++)
                {
                    //tomo un índice random, la carta de dicho índice, su lista de bool, así como su ubicación
                    int index = r.Next(0, CardsWithEffects.Count);
                    ACard card = CardsWithEffects[index];
                    List<bool> canpay = EffectsThatCanBeUsedOrNotPerCard[index];
                    (int x, int y) location = this.AField.Find(card);

                    //tomo el primer efecto usable de dicha carta y el número de dicho efecto 
                    var efct = FirstUsableEffect(card, canpay);

                    //una lista para guardar los costos 
                    List<double> costs = new();

                    //un random para seleccionar el objetivo enemigo random del efecto, en caso de ser efecto negativo
                    int tgtindex = r.Next(0, targets.Count);

                    //y otro para seleccionar el objetivo aliado random del efecto, en caso de ser efecto positivo
                    int allyindex = r.Next(0, allies.Length);

                    //y el amount a pasar a los efectos, que en general es la duración de algún efecto, por defecto será 3
                    int amount0 = 3;

                    //Hay que sacar el costo de dicho efecto, dependiendo de que número sea (si es Effect1, Effects2, etc)

                    //si entra aquí es Efecto 1 o 2
                    if (efct.effectnum <= 2)
                    {
                        //Si es 1, entonces es el Efecto 1 que es efecto sencillo (con un solo objetivo y en la mayoría de casos se usa en aliados)
                        if (efct.effectnum == 1)
                        {
                            //tomo los costos del Efecto 1
                            costs = card.ListCostsFKCMEffect1.ToList();

                            //compruebo si lo puede pagar (solo la primera iteración tiene garantizado que lo puede pagar)
                            if (CanPayEffect(this, costs))
                            {
                                //si es un efecto negativo
                                if (DoingEffect.GiveNegativeEffects(efct.single, card.EffectTxt1.name))
                                {
                                    //lo activo contra el enemigo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = efct.single.Invoke(this, costs, card, tgt[tgtindex], amount0, enemy);

                                    //guardo la acción
                                    performedactions.Add("Blinded Man's card " + card.CardName + " used " + card.EffectTxt1.name + " on " + tgt[tgtindex].CardName + ".");

                                    //compruebo si la carta sobrevivió o murió después del efecto
                                    if (!enemy.AField.IsInField(tgt[tgtindex]))
                                    {
                                        //guardo la acción
                                        performedactions.Add(tgt[tgtindex].CardName + " died!");
                                    }

                                    else
                                    {
                                        //guardo la acción
                                        performedactions.Add(tgt[tgtindex].CardName + " is still alive!");
                                    }

                                    //cambio el estado del enemy y el propio
                                    enemy = userandenemy[1];
                                    ActualizeState(userandenemy[0]);

                                    //cambio la propiedad bool de efecto/ataque activado
                                    card.ActivateEffectOrAttackOnce = true;

                                    //guardo la carta en el campo
                                    this.AField.SetCard(card, location);

                                    //Y la guardo en la lista que usaron un efecto con éxito
                                    cardsthatusedeffect.Add(card);

                                }

                                //si es un efecto positivo
                                else
                                {
                                    //lo activo en una carta aliada obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = efct.single.Invoke(this, costs, card, allies[allyindex], amount0, enemy);

                                    //guardo la acción
                                    performedactions.Add("Blinded Man's card " + card.CardName + " used " + card.EffectTxt1.name + " on " + allies[allyindex].CardName + ".");

                                    //y su pseudo-resultado
                                    performedactions.Add(allies[allyindex].CardName + " was positively affected by the effect!");

                                    //cambio el estado propio
                                    ActualizeState(userandenemy[0]);

                                    //cambio la propiedad bool de efecto/ataque activado
                                    card.ActivateEffectOrAttackOnce = true;

                                    //guardo la carta en el campo
                                    this.AField.SetCard(card, location);

                                    //Y la guardo en la lista que usaron un efecto con éxito
                                    cardsthatusedeffect.Add(card);
                                }

                            }

                        }

                        //Si es 2, entonces es el Efecto 2 que es efecto sencillo (con un solo objetivo) 
                        else
                        {
                            //tomo los costos del Efecto 2
                            costs = card.ListCostsFKCMEffect2.ToList();

                            //compruebo si lo puede pagar
                            if (CanPayEffect(this, costs))
                            {

                                //guardo su estado antes de usar el efecto
                                string originalstate = tgt[tgtindex].CardState;

                                //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                Player[] userandenemy = efct.single.Invoke(this, costs, card, tgt[tgtindex], amount0, enemy);

                                //guardo la acción
                                performedactions.Add("Blinded Man's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + tgt[tgtindex].CardName + ".");

                                //compruebo si la carta sobrevivió o murió después del efecto
                                if (!enemy.AField.IsInField(tgt[tgtindex]))
                                {
                                    //guardo la acción
                                    performedactions.Add(tgt[tgtindex].CardName + " died!");
                                }

                                else
                                {
                                    //guardo la acción
                                    performedactions.Add(tgt[tgtindex].CardName + " is still alive!");

                                    if (originalstate != tgt[tgtindex].CardState)
                                    {
                                        performedactions.Add(tgt[tgtindex].CardName + " is " + tgt[tgtindex].CardStates[tgt[tgtindex].CardStates.Count - 1] + "!");
                                    }
                                }

                                //cambio el estado del enemy y el propio
                                enemy = userandenemy[1];
                                ActualizeState(userandenemy[0]);

                                //cambio la propiedad bool de efecto/ataque activado
                                card.ActivateEffectOrAttackOnce = true;

                                //guardo la carta en el campo
                                this.AField.SetCard(card, location);

                                //Y la guardo en la lista que usaron un efecto con éxito
                                cardsthatusedeffect.Add(card);

                                //cambio el estado del enemy y el propio
                                enemy = userandenemy[1];
                                ActualizeState(userandenemy[0]);
                            }

                        }
                    }

                    //si entra aquí es Efecto 3 o 4
                    else
                    {
                        //si es 3, entonces es el Efecto 3 que es efecto múltiple (con varios objetivos, mín 1 y máx 3)
                        if (efct.effectnum == 3)
                        {
                            //tomo los costos del Efecto 3
                            costs = card.ListCostsFKCMEffect3.ToList();

                            //compruebo si lo puede pagar
                            if (CanPayEffect(this, costs))
                            {
                                //creo el array de los objetivos con su máxima cantidad posible: 3
                                ACard[] newtgt = new ACard[3];

                                //si el efecto es negativo va contra los enemigos
                                if (DoingEffect.GiveNegativeEffects(card.Effect3, card.EffectTxt3.name))
                                {
                                    //Compruebo si la cantidad de objetivos no es correcta. Si hay más de 3 es porque son 4 o 5 objetivos.
                                    if (targets.Count > 3)
                                    {
                                        //si son 4 objetivos
                                        if (targets.Count == 4)
                                        {
                                            //elimino uno de forma random 
                                            newtgt = RandomRemoveAmount(tgt, 1);
                                        }

                                        //sino son 4, entonces son 5 objetivos
                                        else
                                        {
                                            //elimino 2 de forma random
                                            newtgt = RandomRemoveAmount(tgt, 2);
                                        }
                                    }

                                    //si hay tres o menos pues es correcto activar el efecto en ellos
                                    else
                                    {
                                        newtgt = CloneCollection(tgt);
                                    }


                                    //creo una lista para guardar los estados de la carta antes de usar el efecto
                                    List<string> originalstates = new();

                                    //guardo el estado de cada carta antes de activar el efecto
                                    for (int j = 0; j < newtgt.Length; j++)
                                    {
                                        originalstates.Add(newtgt[j].CardState);
                                    }


                                    //y luego activo el efecto obteniendo su resultado, 
                                    //ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = efct.multiple.Invoke(this, costs, card, newtgt, amount0, enemy);

                                    //guardo la acción para cada carta
                                    for (int j = 0; j < newtgt.Length; j++)
                                    {
                                        performedactions.Add("Blinded Man's's card " + card.CardName + " used " + card.EffectTxt3.name + " on " + newtgt[j].CardName + ".");

                                        //compruebo si la cartas sobrevivió o murió después del efecto
                                        if (!enemy.AField.IsInField(newtgt[j]))
                                        {
                                            //guardo la acción
                                            performedactions.Add(newtgt[j].CardName + " died!");
                                        }

                                        else
                                        {
                                            //guardo la acción
                                            performedactions.Add(newtgt[j].CardName + " is still alive!");

                                            if (originalstates[j] != newtgt[j].CardState)
                                            {
                                                //guardo la acción
                                                performedactions.Add(newtgt[j].CardName + " is " + newtgt[j].CardStates[newtgt[j].CardStates.Count - 1] + "!");
                                            }

                                            if (this.CheckingRestrictedStates(newtgt[j]) == "")
                                            {
                                                performedactions.Add(newtgt[j].CardName + " don't have any restriction!");
                                            }

                                            else if (CheckingRestrictedStates(newtgt[j]) != "")
                                            {
                                                performedactions.Add(CheckingRestrictedStates(newtgt[j]));
                                            }
                                        }

                                    }

                                    //cambio el estado del enemy y el propio
                                    enemy = userandenemy[1];
                                    ActualizeState(userandenemy[0]);

                                    //cambio la propiedad bool de efecto/ataque activado
                                    card.ActivateEffectOrAttackOnce = true;

                                    //guardo la carta en el campo
                                    this.AField.SetCard(card, location);

                                    //Y la guardo en la lista que usaron un efecto con éxito
                                    cardsthatusedeffect.Add(card);
                                }

                                //si el efecto es bueno los objetivos son aliados
                                else
                                {
                                    //Compruebo si la cantidad de objetivos no es correcta. Si hay más de 3 es porque son 4 o 5 objetivos.
                                    if (allies.Length > 3)
                                    {
                                        //si son 4 objetivos
                                        if (allies.Length == 4)
                                        {
                                            //elimino uno de forma random 
                                            newtgt = RandomRemoveAmount(allies, 1);
                                        }

                                        //sino son 4, entonces son 5 objetivos
                                        else
                                        {
                                            //elimino 2 de forma random
                                            newtgt = RandomRemoveAmount(allies, 2);
                                        }

                                        //creo una lista para guardar los estados de la carta antes de usar el efecto
                                        List<string> originalstates = new();

                                        //guardo el estado de cada carta antes de activar el efecto
                                        for (int j = 0; j < newtgt.Length; j++)
                                        {
                                            originalstates.Add(newtgt[j].CardState);
                                        }


                                        //y luego activo el efecto obteniendo su resultado, 
                                        //ambos jugadores {user , enemy} en un array
                                        Player[] userandenemy = efct.multiple.Invoke(this, costs, card, newtgt, amount0, enemy);

                                        //guardo la acción para cada carta
                                        for (int j = 0; j < newtgt.Length; j++)
                                        {
                                            performedactions.Add("Blinded Man's's card " + card.CardName + " used " + card.EffectTxt3.name + " on " + newtgt[j].CardName + ".");

                                            if (originalstates[j] != newtgt[j].CardState)
                                            {
                                                //guardo la acción
                                                performedactions.Add(newtgt[j].CardName + " is " + newtgt[j].CardStates[newtgt[j].CardStates.Count - 1] + "!");
                                            }

                                            if (this.CheckingRestrictedStates(newtgt[j]) == "")
                                            {
                                                performedactions.Add(newtgt[j].CardName + " don't have any restriction!");
                                            }

                                            else if (CheckingRestrictedStates(newtgt[j]) != "")
                                            {
                                                performedactions.Add(CheckingRestrictedStates(newtgt[j]));
                                            }

                                        }

                                        //cambio el estado del enemy y el propio
                                        enemy = userandenemy[1];
                                        ActualizeState(userandenemy[0]);

                                        //cambio la propiedad bool de efecto/ataque activado
                                        card.ActivateEffectOrAttackOnce = true;

                                        //guardo la carta en el campo
                                        this.AField.SetCard(card, location);

                                        //Y la guardo en la lista que usaron un efecto con éxito
                                        cardsthatusedeffect.Add(card);
                                    }

                                }


                            }

                        }

                        //si es 4, entonces es el Efecto 4 que es efecto múltiple (con varios objetivos, cualesquiera)
                        else
                        {
                            //tomo los costos del Efecto 4
                            costs = card.ListCostsFKCMEffect4.ToList();

                            //compruebo si lo puede pagar
                            if (CanPayEffect(this, costs))
                            {
                                //si es un efecto negativo los objetivos son los enemigos
                                if (DoingEffect.GiveNegativeEffects(card.Effect4, card.EffectTxt4.name))
                                {
                                    //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = efct.multiple.Invoke(this, costs, card, tgt, amount0, enemy);

                                    //cambio el estado del enemy y el propio
                                    enemy = userandenemy[1];
                                    ActualizeState(userandenemy[0]);

                                    //guardo la acción para cada carta
                                    for (int j = 0; j < tgt.Length; j++)
                                    {
                                        performedactions.Add("Blinded Man's's card " + card.CardName + " used " + card.EffectTxt4.name + " on " + tgt[j].CardName + ".");

                                        //compruebo si la cartas sobrevivió o murió después del efecto
                                        if (!enemy.AField.IsInField(tgt[j]))
                                        {
                                            //guardo la acción
                                            performedactions.Add(tgt[j].CardName + " died!");
                                        }

                                        else
                                        {
                                            //guardo la acción
                                            performedactions.Add(tgt[j].CardName + " is still alive!");
                                        }
                                    }
                                }

                                //si es un efecto positivo los aliados
                                else
                                {
                                    //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = efct.multiple.Invoke(this, costs, card, allies, amount0, this);

                                    ActualizeState(userandenemy[0]);

                                    if (!this.AField.IsInField(card))
                                    {
                                        //guardo la acción
                                        performedactions.Add(card.CardName + " died!");
                                    }

                                    //guardo la acción para cada carta
                                    for (int j = 0; j < allies.Length; j++)
                                    {
                                        performedactions.Add("Blinded Man's's card " + card.CardName + " used " + card.EffectTxt4.name + " on " + allies[j].CardName + ".");

                                        //compruebo si la cartas sobrevivió o murió después del efecto
                                        if (!this.AField.IsInField(allies[j]))
                                        {
                                            //guardo la acción
                                            performedactions.Add(allies[j].CardName + " died!");
                                        }

                                        else
                                        {
                                            //guardo la acción
                                            performedactions.Add(allies[j].CardName + " is still alive!");
                                        }

                                        performedactions.Add(InmunityCheck(allies[j]));
                                    }
                                }

                                //cambio la propiedad bool de efecto/ataque activado
                                card.ActivateEffectOrAttackOnce = true;

                                //guardo la carta en el campo
                                this.AField.SetCard(card, location);

                                //Y la guardo en la lista que usaron un efecto con éxito
                                cardsthatusedeffect.Add(card);
                            }

                        }
                    }


                    //Finalmente tras la activación de un efecto renuevo los objetivos en caso que alguno muriera
                    targets = RemoveDeadTargets(targets, enemy);
                    allies = RemoveDeadTargets(allies.ToList(), this).ToArray();
                    tgt = targets.ToArray();
                }

                //si quedan objetivos y no todas las cartas usaron efectos
                if (tgt.Length != 0 && naturalcardsinvoked.Count != cardsthatusedeffect.Count)
                {
                    //entonces el resto de cartas usa ataque normal a un objetivo random
                    foreach (ACard card in naturalcardsinvoked)
                    {
                        //si la carta no ha usado un efecto
                        if (!cardsthatusedeffect.Contains(card))
                        {
                            //tomo la ubicación de la carta
                            (int x, int y) location0 = this.AField.Find(card);

                            //busco un objetivo random, lo tomo y lo guardo en un array de un solo elemento
                            int rantgt = r.Next(0, targets.Count);
                            if (targets.Count == 0 || rantgt >= targets.Count || targets[rantgt] == null)
                            {
                                break;
                            }
                            ACard thetgt0 = targets[rantgt];
                            ACard[] thetgt = { thetgt0 };

                            //creo una lista de precios vacíos
                            List<double> nocost = (new double[] { 0 }).ToList();

                            //y activo el ataque obteniendo su resultado, ambos jugadores {user , enemy} en un array
                            Player[] userandenemy = card.EffectsObject.NormalAttack.Invoke(this, nocost, card, enemy, thetgt);

                            //guardo la acción
                            performedactions.Add("Blinded Man's's card " + card.CardName + " attacked " + thetgt0.CardName + ".");

                            //guardo la acción
                            performedactions.Add(card.CardName + " dealed  " + card.ATK + " of base damage to " + thetgt0.CardName + "!");

                            //compruebo si la cartas sobrevivió o murió después del ataque
                            if (!enemy.AField.IsInField(thetgt0))
                            {
                                //guardo la acción
                                performedactions.Add(thetgt0.CardName + " died!");
                            }

                            else
                            {
                                //guardo la acción
                                performedactions.Add(thetgt0.CardName + " is still alive!");
                            }

                            //cambio el estado del enemy y el propio
                            enemy = userandenemy[1];
                            ActualizeState(userandenemy[0]);

                            //cambio la propiedad bool de efecto/ataque activado
                            card.ActivateEffectOrAttackOnce = true;

                            //guardo la carta en el campo
                            this.AField.SetCard(card, location0);

                            //Y la guardo en la lista que usaron un efecto con éxito
                            cardsthatusedeffect.Add(card);

                            //Finalmente tras la activación del ataque renuevo los objetivos en caso que alguno muriera
                            targets = RemoveDeadTargets(targets, enemy);
                            tgt = targets.ToArray();

                        }
                    }

                }

            }

            return new Player[] { this, enemy };
        }

        /// <summary>Invoca una cantidad de cartas random de forma random en el campo.</summary>
        void RandomInvocation(Player enemy)
        {
            //Creo un random de varios usos y una lista para random usados a descartar. 
            //Apartir de aquí todo número al final de una variable se refiere a que trabaja con cartas: (1)Natural (2)Magical
            System.Random r = new System.Random();
            List<int> randoms1 = new();
            List<int> randoms2 = new();

            //Si hay espacio para invocar cartas naturales y al menos existe una carta natural por la que se puede pagar entonces
            if (AField.AreNaturalCardsInvokable() && this.CanPayAtLeastOneNatural())
            {
                //Busco la cantidad máxima de cartas que se pueden invocar, o sea, la cantidad de espacios libres en el campo
                int maxinvocable1 = AField.AmountOfInvokableNaturalCards();

                //Busco la cantidad máxima de cartas que se pueden pagar si son escogidas de forma random
                int maxcosteable1 = HowManyRandomCanPay(this, 1);

                //el máximo de cartas random a intentar invocar es el menor entre los espacios disponibles y las que se pueden
                //pagar de ser escogidas de forma random
                int max1 = Math.Min(maxinvocable1, maxcosteable1);

                //Luego la cantidad de cartas a invocar es un random entre 1 y esa máxima cantidad 
                int amount1 = r.Next(0, max1);

                //Por cada carta natural a invocar trato de invocar una
                for (int k = 0; k < amount1; k++)
                {
                    int index = 0;

                    //cuento las cartas naturales en mano y las guardo en una lista
                    int amountnc = CardsInHand.AmountOfNaturalCardsInHand();
                    List<ACard> naturalcards = CardsInHand.GetNaturalCardsInHand();

                    //si todos los índices ya fueron tomados pues los randoms tiene tantos números como cartas naturales en mano
                    if (randoms1.Count == naturalcards.Count)
                    {
                        //rompo el ciclo
                        k = amount1 + 1;
                        break;
                    }

                    while (true)
                    {
                        //tomo un índice de las cartas naturales en la mano al azar 
                        index = r.Next(0, naturalcards.Count);

                        //Compruebo que el random no se usó antes y resulto ser una carta que no se puede pagar
                        if (!randoms1.Contains(index))
                        {
                            break;
                        }
                    }

                    //tomo la carta de ese índice
                    ACard card = naturalcards[index];

                    //si la IA tiene suficiente para pagar la invocación de esa carta, lo hace y agrega el índice a los que ya probó
                    if (this.CanPay(card))
                    {
                        //compruebo si la carta está en el campo
                        if (AField.IsInField(card))
                        {
                            //tomo su posición y su evollv antes de que evolucione
                            (int x, int y) pos0 = AField.Find(card);
                            int evollevel = card.EvolutionLevel;

                            //y si lo está la invoca ahí para que la carta evolucione
                            AField.Invocation(card, pos0, this);

                            //agrego la acción en dependencia del evol level.
                            if (evollevel == 1)
                            {
                                performedactions.Add(("Blinded Man evolved card " + card.CardName + " to " + card.EvolvedNames.evolname1 + " " + card.CardName + " in his field at the position (" + pos0.x + "," + pos0.y + ")."
                           + "\nHis race evolved to " + card.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + card.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else
                            {
                                performedactions.Add(("Blinded Man mega evolved card " + card.CardName + " to " + card.EvolvedNames.evolname2 + " " + card.CardName + " in his field at the position (" + pos0.x + "," + pos0.y + ")."
                                                          + "\nHis race evolved to " + card.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + card.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                        }

                        //sino la invoca en un lugar vacío y la agrego a los randoms usados (representa las cartas ya usadas o 
                        //índices de cartas que no se podían pagar)
                        (int x, int y) pos = AField.FindEmpty(1);
                        AField.Invocation(card, pos, this);
                        randoms1.Add(index);

                        //agrego la acción
                        performedactions.Add(("Blinded Man invoked natural card " + card.CardName + " in his field at the position (" + pos.x + "," + pos.y + ")."));

                    }

                    //sino puede pagar la carta, como ya hay garantía de que se puede pagar una al menos, 
                    //trata con la siguiente guardando el random al azar en una lista
                    else
                    {
                        randoms1.Add(index);
                        k -= 1;

                        //si todos los índices ya fueron tomados pues los randoms tiene tantos números como cartas naturales en mano
                        if (randoms1.Count == naturalcards.Count)
                        {
                            //rompo el ciclo
                            k = amount1 + 1;
                            break;
                        }
                    }

                }

            }

            //Si hay espacio para invocar cartas mágicas y al menos existe una carta mágica por la que se puede pagar entonces
            if (AField.AreMagicalCardsInvokable() && this.CanPayAtLeastOneMagical())
            {
                //Busco la cantidad máxima de cartas que se pueden invocar, o sea, la cantidad de espacios libres en el campo
                int maxinvocable2 = AField.AmountOfInvokableMagicalCards();

                //Busco la cantidad máxima de cartas que se pueden pagar si son escogidas de forma random
                int maxcosteable2 = HowManyRandomCanPay(this, 2);

                //el máximo de cartas random a intentar invocar es el menor entre los espacios disponibles y las que se pueden
                //pagar de ser escogidas de forma random
                int max2 = Math.Min(maxinvocable2, maxcosteable2);

                //Luego la cantidad de cartas a invocar es un random entre 1 y esa máxima cantidad 
                int amount2 = r.Next(0, max2);

                //Por cada carta mágica a invocar trato de invocar una
                for (int k = 0; k < amount2; k++)
                {
                    //Creo int para el index
                    int index = 0;

                    //cuanto las cartas naturales en mano y las guardo en una lista
                    List<ACard> magicalcards = CardsInHand.GetMagicalCardsInHand();

                    //si todos los índices ya fueron tomados pues los randoms tiene tantos números como cartas mágicas en mano
                    if (randoms2.Count == magicalcards.Count)
                    {
                        //rompo el ciclo
                        k = amount2 + 1;
                        break;
                    }

                    while (true)
                    {
                        //tomo un índice de las cartas en la mano al azar 
                        index = r.Next(0, magicalcards.Count);

                        //Compruebo que el random no se usó antes y resulto ser una carta que no se puede pagar
                        if (!randoms2.Contains(index))
                        {
                            break;
                        }
                    }

                    //tomo la carta de ese índice
                    ACard card = magicalcards[index];

                    //si la IA tiene suficiente para pagar la invocación de esa carta, revisa si su uso es correcto y agrega el índice a los que ya probó
                    if (this.CanPay(card))
                    {
                        //si el efecto es una torre
                        if (DoingEffect.IsAMagicTowerCard(card.EffectTxt1.name))
                        {
                            //entonces le hago una invocación especial en el campo
                            this.AField.InvocationSpecial(card, this.AField.FindEmpty(2), this);

                            performedactions.Add("Blinded Man invoked " + card.EffectTxt1.name + " in her magic field!");
                            performedactions.Add("All points recuperation gets a plus!");
                        }

                        else
                        {
                            //si el efecto es negativo
                            if (DoingEffect.GiveNegativeEffects(card.Effect1, card.EffectTxt1.name))
                            {
                                //si hay cartas enemigas invocadas
                                if (enemy.AField.AreNaturalCardInvoked())
                                {
                                    //las tomo
                                    List<ACard> enemycards = enemy.AField.GetNaturalCardsInField();

                                    //y uso el efecto en una random
                                    int ind = r.Next(0, enemycards.Count);
                                    card.Effect1.Invoke(this, (new double[] { 0, 0, 0, 0 }).ToList(), card, enemycards[ind], 3, enemy);


                                    performedactions.Add(("Blinded Man used magic card " + card.CardName + " on your card " + enemycards[ind].CardName + "!"));
                                    performedactions.Add("Effect description: " + card.EffectTxt1.description);

                                }
                            }

                            //si el efecto es positivo
                            else
                            {
                                //si hay cartas aliadas invocadas
                                if (this.AField.AreNaturalCardInvoked())
                                {
                                    //las tomo
                                    List<ACard> alliescards = this.AField.GetNaturalCardsInField();

                                    //y uso el efecto en una random
                                    int ind = r.Next(0, alliescards.Count);
                                    card.Effect1.Invoke(this, (new double[] { 0, 0, 0, 0 }).ToList(), card, alliescards[ind], 3, this);

                                    performedactions.Add(("Blinded Man used magic card " + card.CardName + " on his card " + alliescards[ind].CardName + "!"));
                                    performedactions.Add("Effect description: " + card.EffectTxt1.description);
                                }
                            }
                        }

                        randoms2.Add(index);
                    }

                    //sino puede pagar la carta, como ya hay garantía de que se puede pagar una al menos, 
                    //trata con la siguiente guardando el random al azar en una lista
                    else
                    {
                        randoms2.Add(index);
                        k -= 1;

                        //si todos los índices ya fueron tomados pues los randoms tiene tantos números como cartas mágicas en mano
                        if (randoms2.Count == magicalcards.Count)
                        {
                            //rompo el ciclo
                            k = amount2 + 1;
                            break;
                        }
                    }

                }

            }


        }

        #endregion

        #region IA: Attacker

        /// <summary>IA: Attacker. Primera parte de un turno para robar cartas e invocar.</summary>
        public Player[] TwoInvocationPhase1(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //Aquí se ganan los puntos al inicio del turno.
            this.Kingdom.Faith += this.Kingdom.RecuperationPerTurnFaith;
            this.Kingdom.Knowledge += this.Kingdom.RecuperationPerTurnKnowledge;
            this.Kingdom.Capital += this.Kingdom.RecuperationPerTurnCapital;
            this.Kingdom.Militarism += this.Kingdom.RecuperationPerTurnMilitarism;

            //Aquí resetea los permisos de las cartas para usar un efecto o atacar
            List<ACard> cardsinfield = AField.GetNaturalCardsInField();

            foreach (ACard card in cardsinfield)
            {
                card.ActivateEffectOrAttackOnce = false;
            }

            //Aquí roba cartas, en caso de quedar y poder
            if (CanDrawCard)
            {
                CardsInHand.DrawingCards(DeckOfCards, Rules.CardsDrawPerTurn);
            }

            //Trata de invocar:

            //Compruebo si hay cartas en la mano
            if (!CardsInHand.IsEmpty())
            {
                //actualizo el estado tras invocar cartas naturales, en caso de tener
                this.ActualizeState(this.AttackInvocation(enemy));
            }


            return new Player[] { this, enemy };
        }

        /// <summary>IA: Attacker. Última parte de un turno para invocar cartas.</summary>
        public Player[] TwoInvocationPhase2(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //Trata de invocar:

            //Compruebo si hay cartas en la mano
            if (!CardsInHand.IsEmpty() && AField.AreNaturalCardsInvokable())
            {
                //actualizo el estado tras invocar cartas naturales, en caso de tener
                this.ActualizeState(this.AttackInvocation(enemy));

            }

            return new Player[] { this, enemy };
        }

        /// <summary>IA: Attacker. Segunda parte de un turno para atacar o activar efectos.</summary>
        public Player[] TwoBattleandEffectsPhase(Player enemy, ACard[] tgt, ACard[] allies)
        {
            //si el enemigo y la IA tienen cartas invocadas
            if (enemy.AField.AreNaturalCardInvoked() && this.AField.AreNaturalCardInvoked())
            {
                //tomo las cartas de ambos campos
                List<ACard> cardsinfield = this.AField.GetNaturalCardsInField();
                List<ACard> enemies = enemy.AField.GetNaturalCardsInField();

                //clono la lista de enemigos en el campo, para irla reduciendo según mueran cartas
                List<ACard> targets = CloneCollection(enemies);

                //finalmente una lista para llevar las cartas aliadas que ya usaron un efecto
                List<ACard> cardsthatusedeffect = new();

                //recorre por las cartas en el campo de la IA
                for (int j = 0; j < cardsinfield.Count; j++)
                {
                    if (enemies.Count == 0)
                    {
                        break;
                    }

                    //obtengo la localización de la carta en el campo
                    (int x, int y) location = AField.Find(cardsinfield[j]);

                    //guardo la carta en una variable más corta de nombre
                    ACard card = cardsinfield[j];

                    //y otra para la carta enemiga
                    ACard etgt = enemies[0];

                    //tomo el nivel de evolución de la carta
                    int evolnum = card.EvolutionLevel;


                    //si la carta no ha activado su efecto entonces
                    if (cardsthatusedeffect.Count == 0 || !cardsthatusedeffect.Contains(card))
                    {
                        //si la carta no ha evolucionado tiene dos efectos como máximo, de objetivos sencillos
                        if (evolnum == 1)
                        {
                            //si la carta tiene efecto elemental (que siempre son los efecto 2), si lo puede pagar y no ha activado efecto
                            if (card.Effect2 != null && this.CanPayEffect(this, card.ListCostsFKCMEffect2.ToList()) && !cardsthatusedeffect.Contains(card))
                            {
                                //obtengo la lista de elementos inmunes al ataque elemental (efecto 2)
                                List<string> inmunitytoattack = ElementsThatAreInmuneToGivedElementalAttack(card, 2);

                                //bool para saber si es un objetivo a atacar o no
                                bool istarget = false;

                                //obtengo la lista de costo del efecto 2
                                List<double> costs = card.ListCostsFKCMEffect2.ToList();

                                //recorro por las cartas enemigas 
                                for (int k = 0; k < enemies.Count; k++)
                                {
                                    //guardo el enemigo actual y comprueba si es inmune o no
                                    etgt = enemies[k];
                                    bool isinmune = TheCardISInmuneToElement(enemies[k], inmunitytoattack);

                                    if (!isinmune)
                                    {
                                        istarget = true;
                                        break;
                                    }
                                }

                                //si no tiene ningún elemento inmune al elemento del ataque
                                if (istarget)
                                {
                                    string originalstate = etgt.CardState;

                                    //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                    Player[] userandenemy = cardsinfield[j].Effect2.Invoke(this, costs, card, etgt, 3, enemy);

                                    //guardo la acción
                                    performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + etgt.CardName + ".");

                                    //compruebo si la carta sobrevivió o murió después del efecto
                                    if (!enemy.AField.IsInField(etgt))
                                    {
                                        //guardo la acción
                                        performedactions.Add(etgt.CardName + " died!");
                                    }

                                    else
                                    {
                                        //guardo la acción
                                        performedactions.Add(etgt.CardName + " is still alive!");

                                        if (originalstate != etgt.CardState)
                                        {
                                            performedactions.Add(etgt.CardName + " is " + etgt.CardStates[etgt.CardStates.Count - 1] + "!");
                                        }
                                    }


                                    //cambio el estado del enemy y el propio
                                    enemy = userandenemy[1];
                                    ActualizeState(userandenemy[0]);

                                    //cambio la propiedad bool de efecto/ataque activado
                                    cardsinfield[j].ActivateEffectOrAttackOnce = true;

                                    //guardo la carta en el campo
                                    this.AField.SetCard(card, location);

                                    //Y la guardo en la lista que usaron un efecto con éxito
                                    cardsthatusedeffect.Add(card);
                                }


                            }

                            //si no tiene efecto elemental, no lo puede pagar o este no es efectivo compruebo si tiene efecto 1, lo puede pagar y no ha activado efecto      
                            if (card.Effect1 != null && this.CanPayEffect(this, card.ListCostsFKCMEffect1.ToList()) && !cardsthatusedeffect.Contains(card))
                            {
                                //luego compruebo si es efecto de daño recorriendo por la lista de nombres de efectos de daño sencillos
                                for (int k = 0; k < card.EffectsObject.NaturalEffectToDealDamageSingle.Count; k++)
                                {
                                    //si el nombre del efecto está en la lista de nombres de efectos que hacen daño
                                    if (card.EffectTxt1.name == card.EffectsObject.NaturalEffectToDealDamageSingle[k])
                                    {
                                        //guardo el costo del efecto
                                        List<double> costs = card.ListCostsFKCMEffect1.ToList();

                                        //recorro por la lista de enemigos
                                        for (int n = 0; n < enemies.Count; n++)
                                        {
                                            //guardo el enemigo que no tenga inmunidad a efectos
                                            if (!enemies[n].AsEffectInmunity)
                                            {
                                                etgt = enemies[n];
                                            }

                                        }

                                        string originalstate = etgt.CardState;

                                        //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                        Player[] userandenemy = card.Effect1.Invoke(this, costs, card, etgt, 3, enemy);

                                        //guardo la acción
                                        performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + etgt.CardName + ".");

                                        //compruebo si la carta sobrevivió o murió después del efecto
                                        if (!enemy.AField.IsInField(etgt))
                                        {
                                            //guardo la acción
                                            performedactions.Add(etgt.CardName + " died!");
                                        }

                                        else
                                        {
                                            //guardo la acción
                                            performedactions.Add(etgt.CardName + " is still alive!");

                                            if (originalstate != etgt.CardState)
                                            {
                                                performedactions.Add(etgt.CardName + " is " + etgt.CardStates[etgt.CardStates.Count - 1] + "!");
                                            }
                                        }


                                        //cambio el estado del enemy y el propio
                                        enemy = userandenemy[1];
                                        ActualizeState(userandenemy[0]);

                                        //cambio la propiedad bool de efecto/ataque activado
                                        card.ActivateEffectOrAttackOnce = true;

                                        //guardo la carta en el campo
                                        this.AField.SetCard(card, location);

                                        //Y la guardo en la lista que usaron un efecto con éxito
                                        cardsthatusedeffect.Add(card);
                                    }
                                }
                            }

                            //si no había de daño o no era pagable ataco normal
                            else
                            {
                                //solo si la carta no ha usado un efecto
                                if (!cardsthatusedeffect.Contains(card))
                                {
                                    if (enemies.Count == 0)
                                    {
                                        break;
                                    }

                                    //tomo la ubicación de la carta
                                    (int x, int y) location0 = this.AField.Find(card);

                                    //y un bool para ver si encontró un objetivo que muera al ser atacado
                                    bool attackanddead = false;

                                    //recorro por la lista de enemigos
                                    for (int n = 0; n < enemies.Count; n++)
                                    {
                                        //busco un enemigo que pueda matar al instante y no sea inmune a ataques
                                        if (card.ATK > (enemies[n].DEF + enemies[n].VIT) && !enemies[n].AsPhysicalInmunity)
                                        {
                                            etgt = enemies[n];
                                            attackanddead = true;
                                        }

                                    }

                                    //si el enemigo no tenía ninguna carta que muera al instante busco una que al menos no sea inmune
                                    if (!attackanddead)
                                    {
                                        //recorro por la lista de enemigos again
                                        for (int n = 0; n < enemies.Count; n++)
                                        {
                                            //busco un enemigo que no sea inmune a ataques
                                            if (!enemies[n].AsPhysicalInmunity)
                                            {
                                                //guardo el enemigo
                                                etgt = enemies[n];

                                                //y uso este bool ahora para indicar que hay un enemigo al que se le puede hacer daño
                                                attackanddead = true;
                                            }

                                        }
                                    }

                                    //si hay un enemigo que muera al instante o que al menos no tenga inmunidad activo el ataque
                                    if (attackanddead)
                                    {
                                        //creo una lista de precios vacíos
                                        List<double> nocost = (new double[] { 0 }).ToList();

                                        //y activo el ataque obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                        Player[] userandenemy = card.EffectsObject.NormalAttack.Invoke(this, nocost, card, enemy, new ACard[] { etgt });

                                        //guardo la acción
                                        performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + etgt.CardName + ".");

                                        //compruebo si la carta sobrevivió o murió después del efecto
                                        if (!enemy.AField.IsInField(etgt))
                                        {
                                            //guardo la acción
                                            performedactions.Add(etgt.CardName + " died!");
                                        }

                                        else
                                        {
                                            //guardo la acción
                                            performedactions.Add(etgt.CardName + " is still alive!");

                                        }

                                        //cambio el estado del enemy y el propio
                                        enemy = userandenemy[1];
                                        ActualizeState(userandenemy[0]);

                                        //cambio la propiedad bool de efecto/ataque activado
                                        card.ActivateEffectOrAttackOnce = true;

                                        //guardo la carta en el campo
                                        this.AField.SetCard(card, location0);

                                        //Y la guardo en la lista que usaron un efecto con éxito
                                        cardsthatusedeffect.Add(card);
                                    }




                                }
                            }

                            //Finalmente tras la activación del efecto/ataque renuevo los objetivos en caso que alguno muriera
                            targets = RemoveDeadTargets(targets, enemy);
                            tgt = targets.ToArray();
                        }

                        //si evolucionó una vez tiene un efecto múltiple o un efecto elemental múltiple
                        else if (evolnum == 2)
                        {
                            //compruebo que la carta puede pagar el efecto 3 y no haya activado efecto antes
                            if (CanPayEffect(this, card.ListCostsFKCMEffect3.ToList()) && !cardsthatusedeffect.Contains(card))
                            {
                                //comprueba si es un efecto de daño elemental
                                for (int k = 0; k < card.EffectsObject.NaturalEffectToDealElementalDamage.Count; k++)
                                {
                                    //si el efecto 3 es un ataque de daño elemental
                                    if (card.EffectTxt3.name == card.EffectsObject.NaturalEffectToDealElementalDamage[k])
                                    {
                                        if (enemies.Count == 0)
                                        {
                                            break;
                                        }
                                        //obtengo la lista de elementos inmunes al ataque elemental (efecto 3)
                                        List<string> inmunitytoattack = ElementsThatAreInmuneToGivedElementalAttack(card, 3);

                                        //obtengo la lista de costo del efecto 3
                                        List<double> costs = card.ListCostsFKCMEffect3.ToList();

                                        //creo la lista de objetivos, máximo 3 mínimo 1
                                        List<ACard> tgts = new();

                                        //recorro por las cartas enemigas 
                                        for (int x = 0; x < enemies.Count; x++)
                                        {
                                            //si la carta enemigo actual no es inmune al ataque
                                            if (!TheCardISInmuneToElement(enemies[x], inmunitytoattack))
                                            {
                                                //y la lista de objetivos tiene 2 o menos la agrego
                                                if (tgts.Count <= 2)
                                                {
                                                    tgts.Add(enemies[x]);
                                                }
                                            }
                                        }

                                        //si algún objetivo no es inmune activo el ataque elemental
                                        if (tgts.Count != 0)
                                        {
                                            //guardo los estados originales
                                            List<string> originalstates = new();
                                            for (int l = 0; l < tgts.Count; l++)
                                            {
                                                originalstates.Add(tgts[l].CardState);
                                            }

                                            //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                            Player[] userandenemy = card.Effect3.Invoke(this, costs, card, tgts.ToArray(), 3, enemy);

                                            //guardo cada ataque
                                            for (int l = 0; l < tgts.Count; l++)
                                            {
                                                //guardo la acción
                                                performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + tgts[l].CardName + ".");

                                                //compruebo si la carta sobrevivió o murió después del efecto
                                                if (!enemy.AField.IsInField(tgts[l]))
                                                {
                                                    //guardo la acción
                                                    performedactions.Add(tgts[l].CardName + " died!");
                                                }

                                                else
                                                {
                                                    //guardo la acción
                                                    performedactions.Add(tgts[l].CardName + " is still alive!");

                                                    if (originalstates[l] != tgts[l].CardState)
                                                    {
                                                        performedactions.Add(tgts[l].CardName + " is " + tgts[l].CardStates[tgts[l].CardStates.Count - 1] + "!");
                                                    }
                                                }

                                                //cambio el estado del enemy y el propio
                                                enemy = userandenemy[1];
                                                ActualizeState(userandenemy[0]);

                                                //cambio la propiedad bool de efecto/ataque activado
                                                card.ActivateEffectOrAttackOnce = true;

                                                //guardo la carta en el campo
                                                this.AField.SetCard(card, location);

                                                //Y la guardo en la lista que usaron un efecto con éxito
                                                cardsthatusedeffect.Add(card);

                                                //Finalmente tras la activación del efecto/ataque renuevo los objetivos en caso que alguno muriera
                                                targets = RemoveDeadTargets(targets, enemy);
                                                tgt = targets.ToArray();
                                            }

                                        }


                                    }

                                    //sino al menos un efecto de daño múltiple, en caso de que no se activara el de arriba
                                    if (!cardsthatusedeffect.Contains(card))
                                    {
                                        if (enemies.Count == 0)
                                        {
                                            break;
                                        }

                                        //recorro por los efectos para hacer daño múltiple
                                        for (int g = 0; g < card.EffectsObject.NaturalEffectToDealDamageMultiple.Count; g++)
                                        {
                                            //si la carta es un efecto para hacer daño múltiple
                                            if (card.EffectTxt3.name == card.EffectsObject.NaturalEffectToDealDamageMultiple[g])
                                            {
                                                //guardo el costo del efecto
                                                List<double> costs = card.ListCostsFKCMEffect3.ToList();

                                                //creo la lista de objetivos, máximo 5 mínimo 1
                                                List<ACard> tgts = new();

                                                //recorro por la lista de enemigos
                                                for (int n = 0; n < enemies.Count; n++)
                                                {
                                                    //guardo el enemigo que no tenga inmunidad a efectos siempre que la lista tengo menos de 5
                                                    //objetivos
                                                    if (!enemies[n].AsEffectInmunity && tgts.Count <= 4)
                                                    {
                                                        tgts.Add(enemies[n]);
                                                    }
                                                }

                                                //si existe algún objetivo 
                                                if (tgts.Count != 0)
                                                {
                                                    //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                                    Player[] userandenemy = card.Effect3.Invoke(this, costs, card, tgts.ToArray(), 3, enemy);

                                                    //guardo los estados originales
                                                    List<string> originalstates = new();
                                                    for (int l = 0; l < tgts.Count; l++)
                                                    {
                                                        originalstates.Add(tgts[l].CardState);
                                                    }

                                                    //guardo cada ataque
                                                    for (int l = 0; l < tgts.Count; l++)
                                                    {
                                                        //guardo la acción
                                                        performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + tgts[l].CardName + ".");

                                                        //compruebo si la carta sobrevivió o murió después del efecto
                                                        if (!enemy.AField.IsInField(tgts[l]))
                                                        {
                                                            //guardo la acción
                                                            performedactions.Add(tgts[l].CardName + " died!");
                                                        }

                                                        else
                                                        {
                                                            //guardo la acción
                                                            performedactions.Add(tgts[l].CardName + " is still alive!");

                                                            if (originalstates[l] != tgts[l].CardState)
                                                            {
                                                                performedactions.Add(tgts[l].CardName + " is " + tgts[l].CardStates[tgts[l].CardStates.Count - 1] + "!");
                                                            }
                                                        }


                                                        //cambio el estado del enemy y el propio
                                                        enemy = userandenemy[1];
                                                        ActualizeState(userandenemy[0]);

                                                        //cambio la propiedad bool de efecto/ataque activado
                                                        card.ActivateEffectOrAttackOnce = true;

                                                        //guardo la carta en el campo
                                                        this.AField.SetCard(card, location);

                                                        //Y la guardo en la lista que usaron un efecto con éxito
                                                        cardsthatusedeffect.Add(card);

                                                        //Finalmente tras la activación del efecto/ataque renuevo los objetivos en caso que alguno muriera
                                                        targets = RemoveDeadTargets(targets, enemy);
                                                        tgt = targets.ToArray();
                                                    }
                                                }
                                            }
                                        }

                                        //si llega aquí y no ha activado ningún efecto pues la trata como carta de evol1
                                        if (!cardsthatusedeffect.Contains(card))
                                        {
                                            //que actue como si su evolnum fuera 1, para eso le quito uno al índice de la carta actual
                                            //así en la próxima iteración toma la misma carta pero como si fuera evol1
                                            evolnum = 1;
                                            j -= 1;
                                        }
                                    }

                                    //sino 
                                    else
                                    {
                                        //que actue como si su evolnum fuera 1, para eso le quito uno al índice de la carta actual
                                        //así en la próxima iteración toma la misma carta pero como si fuera evol1
                                        evolnum = 1;
                                        j -= 1;
                                    }
                                }
                            }

                            //si evoluciono dos veces tiene el efecto definitivo
                            else
                            {
                                //compruebo que la carta puede pagar el efecto 4 y no ha activado efecto
                                if (CanPayEffect(this, card.ListCostsFKCMEffect4.ToList()) && !cardsthatusedeffect.Contains(card))
                                {
                                    //recorro por los efectos para hacer daño múltiple
                                    for (int k = 0; k < card.EffectsObject.NaturalEffectToDealDamageMultiple.Count; k++)
                                    {
                                        //si la carta es un efecto para hacer daño múltiple
                                        if (card.EffectTxt4.name == card.EffectsObject.NaturalEffectToDealDamageMultiple[k])
                                        {
                                            //guardo el costo del efecto
                                            List<double> costs = card.ListCostsFKCMEffect4.ToList();

                                            //creo la lista de objetivos, máximo 5 mínimo 1
                                            List<ACard> tgts = new();

                                            //recorro por la lista de enemigos
                                            for (int n = 0; n < enemies.Count; n++)
                                            {
                                                //guardo el enemigo que no tenga inmunidad a efectos siempre que la lista tengo menos de 5
                                                //objetivos
                                                if (!enemies[n].AsEffectInmunity && tgts.Count <= 4)
                                                {
                                                    tgts.Add(enemies[n]);
                                                }
                                            }

                                            //si existe algún objetivo 
                                            if (tgts.Count != 0)
                                            {
                                                //lo activo obteniendo su resultado, ambos jugadores {user , enemy} en un array
                                                Player[] userandenemy = card.Effect4.Invoke(this, costs, card, tgts.ToArray(), 3, enemy);

                                                //guardo cada ataque
                                                for (int l = 0; l < tgts.Count; l++)
                                                {
                                                    //guardo la acción
                                                    performedactions.Add("The Berserker's card " + card.CardName + " used " + card.EffectTxt2.name + " on " + tgts[l].CardName + ".");

                                                    //compruebo si la carta sobrevivió o murió después del efecto
                                                    if (!enemy.AField.IsInField(tgts[l]))
                                                    {
                                                        //guardo la acción
                                                        performedactions.Add(tgts[l].CardName + " died!");
                                                    }

                                                    else
                                                    {
                                                        //guardo la acción
                                                        performedactions.Add(tgts[l].CardName + " is still alive!");

                                                    }
                                                }

                                                //cambio el estado del enemy y el propio
                                                enemy = userandenemy[1];
                                                ActualizeState(userandenemy[0]);

                                                //cambio la propiedad bool de efecto/ataque activado
                                                card.ActivateEffectOrAttackOnce = true;

                                                //guardo la carta en el campo
                                                this.AField.SetCard(card, location);

                                                //Y la guardo en la lista que usaron un efecto con éxito
                                                cardsthatusedeffect.Add(card);

                                                //Finalmente tras la activación del efecto/ataque renuevo los objetivos en caso que alguno muriera
                                                targets = RemoveDeadTargets(targets, enemy);
                                                tgt = targets.ToArray();
                                            }
                                        }
                                    }

                                    //si llega aquí y no ha activado ningún efecto pues la trata como carta de evol1
                                    if (!cardsthatusedeffect.Contains(card))
                                    {
                                        //que actue como si su evolnum fuera 1, para eso le quito uno al índice de la carta actual
                                        //así en la próxima iteración toma la misma carta pero como si fuera evol1
                                        evolnum = 1;
                                        j -= 1;
                                    }

                                }

                                //sino 
                                else
                                {
                                    //que actue como si su evolnum fuera 2, para eso le quito uno al índice de la carta actual
                                    //así en la próxima iteración toma la misma carta pero como si fuera evol2
                                    evolnum = 2;
                                    j -= 1;
                                }
                            }

                        }
                    }
                }




            }

            return new Player[] { this, enemy };
        }

        /// <summary>Las cartas naturales prioriza invocar las más fuertes (mayor ATK) y las mágicas de forma random.</summary>
        IA AttackInvocation(Player enemy)
        {
            //compruebo que tenga cartas naturales en la mano y puedo pagar al menos una
            if (CardsInHand.AreNaturalCardsInHand() && this.CanPayAtLeastOneNatural() && AField.AreNaturalCardsInvokable())
            {
                //obtengo las cartas naturales en orden de la más fuerte a la menos, es al menos 1 carta
                List<ACard> InOrderOfPower = CardsInHand.GetMostPowerfulCardsInHand(this);

                //Luego compruebo si ambos campos están vacíos
                if (this.AField.IsEmpty() && enemy.AField.IsEmpty())
                {
                    //***Prioridad llenar el campo.

                    //cantidad de cartas a invocar igual a la cantidad de cartas en mano
                    int amountofcards = InOrderOfPower.Count;

                    //si hay más de 5 cartas en mano entonces la cantidad a invocar es 5
                    if (amountofcards >= 5)
                    {
                        amountofcards = 5;
                    }

                    //invoco  la máxima cantidad de cartas en orden de las más fuertes que el presupuesto permita
                    Player ia = this.InvokeXAmountOfMostPowerfulPayableCards(InOrderOfPower, amountofcards, this);

                    //actualizo el estado de la IA
                    this.ActualizeState(ia);
                }

                //sino compruebo si el campo enemigo está vacío
                else if (enemy.AField.IsEmpty())
                {
                    //***Prioridad evolucionar cartas en campo y rellenar el campo.

                    //intento evolucionar cartas en el campo y actualizo el estado de la IA
                    this.ActualizeState(TryToEvolveCardsInField());

                    //luego intento invocar normal

                    //cantidad de cartas a invocar igual al mínimo de la cantidad de cartas en mano y los espacios en el campo
                    int invocationspaces = AField.AmountOfInvokableNaturalCards();
                    int amountofcards = InOrderOfPower.Count;

                    int amount = Math.Min(invocationspaces, amountofcards);

                    //invoco la máxima cantidad de cartas en orden de las más fuertes que el presupuesto permita
                    Player ia2 = this.InvokeXAmountOfMostPowerfulPayableCards(InOrderOfPower, amountofcards, this);

                    //actualizo el estado de la IA
                    this.ActualizeState(ia2);

                }

                //sino si el campo aliado está vacío
                else if (this.AField.IsEmpty())
                {
                    //***Prioridad invocar cartas que puedan eliminar enemigos

                    //cantidad de cartas a invocar igual a la cantidad de cartas en mano
                    int amountofcards = InOrderOfPower.Count;

                    //si hay más de 5 cartas en mano entonces la cantidad a invocar es 5
                    if (amountofcards >= 5)
                    {
                        amountofcards = 5;
                    }

                    //invoco la más la máxima cantidad de cartas en orden de las más fuertes que el presupuesto permita
                    Player ia = this.InvokeXAmountOfMostPowerfulPayableCards(InOrderOfPower, amountofcards, this);

                    //actualizo el estado de la IA
                    this.ActualizeState(ia);
                }

                //sino
                else
                {
                    ///***Prioridad evolucionar e invocar cartas que pueda eliminar enemigos

                    //intento evolucionar cartas en el campo y actualizo el estado de la IA
                    this.ActualizeState(TryToEvolveCardsInField());

                    //luego intento invocar normal

                    //obtengo la cantidad de cartas que podría invocar (espacios a rellenar en el campo)
                    int amounttoinvoke = this.AField.AmountOfInvokableNaturalCards();

                    //si quiero invocar más de las cartas que tengo en mano
                    if (amounttoinvoke > InOrderOfPower.Count)
                    {
                        //cambio la cantidad a invocar por la cantidad de cartas en la mano
                        amounttoinvoke = InOrderOfPower.Count;
                    }

                    //invoco la más la máxima cantidad de cartas en orden de las más fuertes que el presupuesto permita
                    Player ia = this.InvokeXAmountOfMostPowerfulPayableCards(InOrderOfPower, amounttoinvoke, this);

                    //actualizo el estado de la IA
                    this.ActualizeState(ia);

                }


            }

            //luego compruebo si hay espacio para invocar cartas mágicas, al menos existe una carta mágica por la que se puede pagar entonces
            if (AField.AreMagicalCardsInvokable() && this.CanPayAtLeastOneMagical())
            {
                //Busco la cantidad máxima de cartas mágicas que se pueden pagar si son escogidas de forma random
                int maxcosteable = HowManyRandomCanPay(this, 2);

                //tomo las cartas naturales en mano y las guardo en una lista
                List<ACard> magicalcards = CardsInHand.GetMagicalCardsInHand();

                //busco los objetivos enemigos en orden de los de mayor ataque en caso que sea un efecto negativo
                List<ACard> enemies = new();

                if (enemy.AField.AreNaturalCardInvoked())
                {
                    enemies = enemy.AField.GetNaturalCardsInField();
                    enemies = OrderByPower(enemies, enemy);
                }

                //busco los objetivos amigos en orden de los más balanceados en caso que sea un efecto positivo
                List<ACard> allies = new();

                if (this.AField.AreNaturalCardInvoked())
                {
                    allies = this.AField.GetNaturalCardsInField();
                    allies = OrderByBalancedStats(allies);
                }

                //creo dos int para llevar las veces que fueron usados efectos en las cartas
                //para no usar un mismo objetivo 2 veces nada más
                int usedally = 0;
                int usedenemy = 0;

                //Si puedo invocar una primero intento que sea una que aumenta el ataque
                if (maxcosteable == 1)
                {
                    ACard useful = CardsInHand.FindMagicalCardWithEffect(magicalcards, "Attack Up");

                    //si no es null y se puede pagar
                    if (useful != null && CanPay(useful))
                    {
                        //la invoco en el campo hacia la carta más balanceada (si existe) y la elimino de la mano
                        if (allies.Count != 0)
                        {
                            Player[] both = useful.Effect1.Invoke(this, useful.ListCostsFKCMEffect1.ToList(), useful, allies[0], 3, this);
                            performedactions.Add(("The Berserker used magic card " + useful.CardName + " on his card " + allies[0].CardName + "!"));
                            performedactions.Add("Effect description: " + useful.EffectTxt1.description);

                            ActualizeState(both[0]);

                            CardsInHand.RemoveFromHand(useful);


                        }

                    }
                }

                //Si puedo invocar dos o más primero intento que sean las 2 que aumentan el ataque
                else if (maxcosteable >= 2)
                {
                    ACard useful = CardsInHand.FindMagicalCardWithEffect(magicalcards, "Attack Up");
                    ACard useful2 = CardsInHand.FindMagicalCardWithEffect(magicalcards, "Berserk");

                    //si useful no es null y se puede pagar
                    if (useful != null && CanPay(useful))
                    {
                        //la invoco en el campo hacia la carta más balanceada (si existe), la elimino de la mano y reduzco el amount en 1
                        if (allies.Count != 0)
                        {
                            Player[] both = useful.Effect1.Invoke(this, useful.ListCostsFKCMEffect1.ToList(), useful, allies[0], 3, this);
                            performedactions.Add(("The Berserker used magic card " + useful.CardName + " on his card " + allies[0].CardName + "!"));
                            performedactions.Add("Effect description: " + useful.EffectTxt1.description);

                            ActualizeState(both[0]);

                            CardsInHand.RemoveFromHand(useful);
                            maxcosteable -= 1;


                        }


                    }

                    //si useful2 no es null y se puede pagar
                    if (useful2 != null && CanPay(useful2))
                    {
                        //la invoco en el campo hacia la carta más balanceada (si existe), la elimino de la mano y reduzco el amount en 1
                        if (allies.Count != 0)
                        {
                            Player[] both = useful2.Effect1.Invoke(this, useful2.ListCostsFKCMEffect1.ToList(), useful2, allies[0], 3, this);
                            performedactions.Add(("The Berserker used magic card " + useful2.CardName + " on his card " + allies[0].CardName + "!"));
                            performedactions.Add("Effect description: " + useful2.EffectTxt1.description);

                            ActualizeState(both[0]);

                            CardsInHand.RemoveFromHand(useful2);
                            maxcosteable -= 1;


                        }

                    }
                }

                //ind para el objetivo enemigo
                int ind = 0;

                //ind para el objetivo aliado
                int ind2 = 0;

                //si quedan cartas cartas mágicas en la mano
                if (this.CardsInHand.AreMagicalCardsInHand())
                {
                    //Por cada carta mágica a invocar trato de invocar una
                    for (int k = 0; k < maxcosteable; k++)
                    {
                        //Creo int para el index
                        int index = 0;

                        //actualizo la lista de cartas en la mano
                        magicalcards = CardsInHand.GetMagicalCardsInHand();

                        //tomo la carta de ese índice
                        ACard card = magicalcards[index];

                        //si la IA tiene suficiente para pagar la invocación de esa carta, lo hace y aumenta el índice
                        if (this.CanPay(card))
                        {
                            //si el efecto es una torre
                            if (DoingEffect.IsAMagicTowerCard(card.EffectTxt1.name))
                            {
                                //entonces le hago una invocación especial en el campo y aumento el índice
                                Player lonly = this.AField.InvocationSpecial(card, this.AField.FindEmpty(2), this);

                                performedactions.Add("The Berserker's invoked " + card.EffectTxt1.name + " in her magic field!");
                                performedactions.Add("All points recuperation gets a plus!");

                                ActualizeState(lonly);

                                index++;
                            }

                            else
                            {
                                //si es un efecto negativo 
                                if (DoingEffect.GiveNegativeEffects(card.Effect1, card.EffectTxt1.name))
                                {
                                    //lo activo solo si hay enemigos 
                                    if (enemies.Count != 0)
                                    {
                                        //aumento los usos de este objetivo
                                        usedenemy++;

                                        //si no ha sido objetivo 2 veces ya el enemy en ese ind
                                        if (usedenemy < 3)
                                        {
                                            //lo activo en él
                                            Player[] both = card.Effect1.Invoke(this, card.ListCostsFKCMEffect1.ToList(), card, enemies[ind], 3, enemy);

                                            performedactions.Add(("The Berserker used magic card " + card.CardName + " on enemy card " + enemies[ind].CardName + "!"));
                                            performedactions.Add("Effect description: " + card.EffectTxt1.description);

                                            if (this.CheckingRestrictedStates(enemies[ind]) == "")
                                            {
                                                performedactions.Add(enemies[ind].CardName + " don't have any restriction!");
                                            }

                                            else if (CheckingRestrictedStates(enemies[ind]) != "")
                                            {
                                                performedactions.Add(CheckingRestrictedStates(enemies[ind]));
                                            }

                                            if (enemy.AField.IsInField(enemies[ind]))
                                            {
                                                performedactions.Add(enemies[ind].CardName + " is dead!");
                                            }

                                            else
                                            {
                                                performedactions.Add(enemies[ind].CardName + " is still alive!");
                                            }

                                            ActualizeState(both[0]);
                                            enemy = both[1];

                                            //quito ese enemigo de los objetivos de cartas mágicas en caso de que muriera, la carta de la mano
                                            //y aumento el index para que siga a la próxima
                                            enemies = enemy.RemoveDeadTargets(enemies, enemy);
                                            CardsInHand.RemoveFromHand(card);
                                            index++;
                                        }

                                        else
                                        {
                                            //sino cambio  el ind al próximo enemigo y lo activo si existe
                                            ind++;

                                            if (ind < enemies.Count)
                                            {
                                                usedenemy = 1;
                                                Player[] both = card.Effect1.Invoke(this, card.ListCostsFKCMEffect1.ToList(), card, enemies[ind], 3, enemy);

                                                performedactions.Add(("The Berserker used magic card " + card.CardName + " on enemy card " + enemies[ind].CardName + "!"));
                                                performedactions.Add("Effect description: " + card.EffectTxt1.description);

                                                if (this.CheckingRestrictedStates(enemies[ind]) == "")
                                                {
                                                    performedactions.Add(enemies[ind].CardName + " don't have any restriction!");
                                                }

                                                else if (CheckingRestrictedStates(enemies[ind]) != "")
                                                {
                                                    performedactions.Add(CheckingRestrictedStates(enemies[ind]));
                                                }

                                                if (enemy.AField.IsInField(enemies[ind]))
                                                {
                                                    performedactions.Add(enemies[ind].CardName + " is dead!");
                                                }

                                                else
                                                {
                                                    performedactions.Add(enemies[ind].CardName + " is still alive!");
                                                }

                                                ActualizeState(both[0]);
                                                enemy = both[1];


                                                //quito ese enemigo de los objetivos de cartas mágicas en caso de que muriera y la carta de la mano
                                                enemies = enemy.RemoveDeadTargets(enemies, enemy);
                                                CardsInHand.RemoveFromHand(card);
                                                index++;
                                            }
                                        }


                                    }

                                    //sino aumento el índice sino se usó la carta por falta de objetivos
                                    else
                                    {
                                        index++;
                                    }

                                }

                                //si es un efecto positivo
                                else
                                {


                                    // lo activo solo si hay aliados  
                                    if (allies.Count != 0)
                                    {
                                        //aumento los usos de este objetivo
                                        usedally++;

                                        //si no ha sido objetivo 2 veces ya el ally en ese ind2
                                        if (usedally < 3)
                                        {
                                            //lo activo, quito la carta de la mano y aumento el index
                                            Player[] both = card.Effect1.Invoke(this, card.ListCostsFKCMEffect1.ToList(), card, allies[ind2], 3, this);
                                            CardsInHand.RemoveFromHand(card);
                                            index++;

                                            performedactions.Add(("The Berserker used magic card " + card.CardName + " on his card " + allies[ind2].CardName + "!"));
                                            performedactions.Add("Effect description: " + card.EffectTxt1.description);

                                            if (this.CheckingRestrictedStates(allies[ind2]) == "")
                                            {
                                                performedactions.Add(allies[ind2].CardName + " don't have any restriction!");
                                            }

                                            else if (CheckingRestrictedStates(allies[ind2]) != "")
                                            {
                                                performedactions.Add(CheckingRestrictedStates(allies[ind2]));
                                            }

                                            ActualizeState(both[0]);

                                        }

                                        else
                                        {
                                            //sino cambio el ind2 al próximo ally y lo activo si existe 
                                            ind2++;

                                            if (ind2 < allies.Count)
                                            {
                                                //lo activo, quito la carta de la mano y sigo a la próxima
                                                usedally = 1;
                                                Player[] both = card.Effect1.Invoke(this, card.ListCostsFKCMEffect1.ToList(), card, allies[ind2], 3, this);
                                                CardsInHand.RemoveFromHand(card);
                                                index++;

                                                performedactions.Add(("The Berserker used magic card " + card.CardName + " on his card " + allies[ind2].CardName + "!"));
                                                performedactions.Add("Effect description: " + card.EffectTxt1.description);

                                                if (this.CheckingRestrictedStates(allies[ind2]) == "")
                                                {
                                                    performedactions.Add(allies[ind2].CardName + " don't have any restriction!");
                                                }

                                                else if (CheckingRestrictedStates(allies[ind2]) != "")
                                                {
                                                    performedactions.Add(CheckingRestrictedStates(allies[ind2]));
                                                }

                                                ActualizeState(both[0]);

                                                //quito la carta de la mano
                                                CardsInHand.RemoveFromHand(card);

                                            }
                                        }

                                    }

                                    //sino aumento el índice
                                    else
                                    {
                                        index++;
                                    }

                                }

                            }

                        }

                        //sino puede pagar la carta sigue
                        else
                        {
                            index++;
                        }

                    }

                }

            }

            return this;

        }

        IA TryToEvolveCardsInField()
        {
            //intento evolucionar cartas en el campo

            //tomo las cartas en el campo y en la mano
            List<ACard> cardsinfield = AField.GetNaturalCardsInField();
            List<ACard> cardsinhand = CardsInHand.GetNaturalCardsInHand();

            //variable para guardar la ubicación de una carta que puede evolucionar
            (int x, int y) location = (-1, -1);

            //Lista de solo las cartas
            List<ACard> cardsevol = new();

            //Lista de su ubicación
            List<(int x, int y)> cardsloc = new();

            //recorro por las cartas en la mano
            for (int j = 0; j < cardsinhand.Count; j++)
            {
                //si la carta en el campo actual se encuentra en la mano 
                if (AField.IsInField(cardsinhand[j]))
                {
                    //tomo su posición en el campo
                    location = AField.Find(cardsinhand[j]);

                    //la carta en cuestión a invocar la agrego junto con su localización a la lista y a otra lista solo con
                    //las cartas
                    cardsevol.Add(cardsinhand[j]);
                    cardsloc.Add(location);
                }
            }


            //las ordeno según las más fuertes
            var res = OrderByPower(cardsevol, this, cardsloc);

            cardsevol = res.cards;
            cardsloc = res.location;

            // si se encontraron resultados 
            if (location.x != -1 && location.y != -1 && cardsevol != null && cardsloc != null)
            {
                //las evoluciono según el presupuesto deje y en orden de las más fuertes
                IA ia1 = InvokeXAmountOfMostPowerfulPayableCards(cardsevol, cardsevol.Count, this, cardsloc);
                return ia1;
            }

            return this;

        }

        #endregion

        /// <summary>Actualiza el estado de la IA a partir de ella misma (obtenida después de hacer alguna acción.)</summary>
        void ActualizeState(Player newme)
        {
            //Actualizo todas las propiedades de la IA
            this.AField = newme.AField;
            this.AlreadyDrawCard = newme.AlreadyDrawCard;
            this.CardsInHand = newme.CardsInHand;
            this.Cementery = newme.Cementery;
            this.DeckOfCards = newme.DeckOfCards;
            this.GainedPoints = newme.GainedPoints;
            this.IsHisTurn = newme.IsHisTurn;
            this.Kingdom = newme.Kingdom;
            this.Life = newme.Life;
            this.Rules = newme.Rules;
            this.UsedCapitalUpEffect = newme.UsedCapitalUpEffect;
            this.UsedFaithUpEffect = newme.UsedFaithUpEffect;
            this.UsedKnowledgeUpEffect = newme.UsedKnowledgeUpEffect;
            this.UsedMilitarismUpEffect = newme.UsedMilitarismUpEffect;
            this.UsedOneTowerEffect = newme.UsedOneTowerEffect;

        }

    }

}