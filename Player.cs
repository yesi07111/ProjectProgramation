using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface ImPlayer
    {
        public Kingdoms Kingdom { get; set; }
        public CardsInHand CardsInHand { get; set; }
        public DeckOfCards DeckOfCards { get; set; }
        public Cementery Cementery { get; set; }
        public AField AField { get; set; }
        public Rules Rules { get; set; }
        public double Life { get; set; }
        public bool IsHisTurn { get; set; }
        public bool CanDrawCard { get; set; }
        public bool AlreadyDrawCard { get; set; }
        public bool GainedPoints { get; set; }
        public bool UsedOneTowerEffect { get; set; }
        public bool UsedFaithUpEffect { get; set; }
        public bool UsedKnowledgeUpEffect { get; set; }
        public bool UsedMilitarismUpEffect { get; set; }
        public bool UsedCapitalUpEffect { get; set; }
        public static Player ActivateStateEffects(Player user)
        {
            return user;
        }

    }

    /// <summary>Clase que contiene todas las propiedades actuales de un jugador.</summary>
    public class Player : ImPlayer
    {
        //Contructor que asigna todas las propiedades de todo lo que se le pasa como parámetro.
        public Player(Kingdoms kingdom, CardsInHand cardsinhand, DeckOfCards deck, Cementery cementery, AField field, Rules rules, double life)
        {
            //Propiedades propias del jugador:
            Kingdom = kingdom;
            CardsInHand = cardsinhand;
            DeckOfCards = deck;
            Cementery = cementery;
            AField = field;
            Rules = rules;
            Life = life;
            CanDrawCard = true;
        }


        //Propiedades asociadas al jugador:
        public Kingdoms Kingdom { get; set; }
        public CardsInHand CardsInHand { get; set; }
        public DeckOfCards DeckOfCards { get; set; }
        public Cementery Cementery { get; set; }
        public AField AField { get; set; }
        public Rules Rules { get; set; }
        public double Life { get; set; }
        public bool IsHisTurn { get; set; }
        public bool CanDrawCard { get; set; }
        public bool AlreadyDrawCard { get; set; }
        public bool GainedPoints { get; set; }
        public bool UsedOneTowerEffect { get; set; }
        public bool UsedFaithUpEffect { get; set; }
        public bool UsedKnowledgeUpEffect { get; set; }
        public bool UsedMilitarismUpEffect { get; set; }
        public bool UsedCapitalUpEffect { get; set; }

        //Propiedades para métodos internos
        List<ACard[]> solutions = new();

        /// <summary>Comprueba que el jugador tenga suficientes puntos para pagar la invocación.</summary>
        public bool CanPay(ACard card)
        {
            if (card == null)
            {
                return false;
            }

            List<double> cost = card.ListOfCostFKCM;

            if (cost[0] > Kingdom.Faith || cost[1] > Kingdom.Knowledge ||
                cost[2] > Kingdom.Capital || cost[3] > Kingdom.Militarism)
            {
                return false;
            }

            return true;
        }

        /// <summary>Comprueba que el jugador tenga suficientes puntos para pagar cada efecto.</summary>
        public List<bool> CanPayEffect(ACard card, Player user)
        {
            List<bool> canpay = new();

            //Obtengo una lista de array de costos de efectos en orden FKCM y una de puntos del reino en orden FKCM
            List<double[]> listofcosts = new();
            listofcosts.Add(card.ListCostsFKCMEffect1);
            listofcosts.Add(card.ListCostsFKCMEffect2);
            listofcosts.Add(card.ListCostsFKCMEffect3);
            listofcosts.Add(card.ListCostsFKCMEffect4);

            List<double> listofpoints = new();
            listofpoints.Add(user.Kingdom.Faith);       //F - Faith
            listofpoints.Add(user.Kingdom.Knowledge);   //K - Knowledge
            listofpoints.Add(user.Kingdom.Capital);     //C - Capital
            listofpoints.Add(user.Kingdom.Militarism);  //M - Militarism

            //uso este int para checar, si cambia es que el efecto no se puede pagar
            int check = 0;

            //recorro por la lista de los costos de cada efecto
            for (int j = 0; j < listofcosts.Count; j++)
            {
                //si el efecto actual no tiene costo se agrega un false y continua al siguiente
                if (listofcosts[j] == null || listofcosts[j].Length == 0)
                {
                    canpay.Add(false);
                    continue;
                }

                //si el efecto actual tiene costos
                for (int k = 0; k < listofcosts[j].Length; k++)
                {
                    //se revisa si cada costo es mayor que cada puntuación en orden de FKCM
                    if (listofpoints != null && listofcosts[j][k] > listofpoints[k])
                    {
                        //en caso de que el costo sea mayor se devuelve falso y se cambia el check a -1
                        canpay.Add(false);
                        check = -1;
                    }
                }

                //si el check es 0 es porque ningún costo fue mayor que su puntuación correspondiente
                if (check == 0)
                {
                    canpay.Add(true);
                }

                //lo llevo a 0 en caso de que sea -1 para la próxima iteración
                check = 0;

            }

            return canpay;

        }

        /// <summary>Comprueba que el jugador tenga suficientes puntos para pagar un efecto dado su lista de costos en orden FKCM.</summary>
        public bool CanPayEffect(Player user, List<double> costs)
        {
            if (costs[0] > Kingdom.Faith || costs[1] > Kingdom.Knowledge ||
                 costs[2] > Kingdom.Capital || costs[3] > Kingdom.Militarism)
            {
                return false;
            }

            return true;
        }

        /// <summary>Devuelve una lista de cartas mágicas que el jugador puede pagar. Se le pasa el jugador, la lista de cartas y la lista de costo de cada una.</summary>
        public List<ACard> ListOfMCThatCanPayEffect(List<ACard> cards, List<double[]> costs, Player user)
        {
            //Recorro por la lista de cartas
            for (int k = cards.Count - 1; k >= 0; k--)
            {
                //si esa carta no puede pagar el efecto
                if (!CanPayEffect(user, costs[k].ToList()))
                {
                    //la quito
                    cards.Remove(cards[k]);
                }
            }

            //devuelvo solo las que pueden pagar el precio de su efecto
            return cards;
        }

        /// <summary>Comprueba que el jugador tenga suficientes puntos para pagar al menos la invocación de una carta natural en su mano.</summary>
        public bool CanPayAtLeastOneNatural()
        {
            foreach (ACard card in this.CardsInHand.GetNaturalCardsInHand())
            {
                if (CanPay(card))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>Comprueba que el jugador tenga suficientes puntos para pagar al menos la invocación de una carta mágica en su mano.</summary>
        public bool CanPayAtLeastOneMagical()
        {
            foreach (ACard card in this.CardsInHand.GetMagicalCardsInHand())
            {
                if (CanPay(card))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>Activa los efectos de estado de cada carta que tenga el jugador en el campo.</summary>
        public static Player ActivateStateEffects(Player user)
        {
            //Por cada espacio de cartas en el campo
            foreach (ACard card in user.AField.Field)
            {
                //Si ese espacio es una carta natural
                if (card != null && card.CardTypeNum == 1)
                {
                    //primero tomo la lista de estados que tiene (que puede ser vacía)
                    List<string> statenames = card.ListOfAbnormalStateDuration.Keys.ToList();

                    //Y si ese carta tiene varios o un efecto de estado
                    if (card.AsManyAbnormalState || card.AsAnAbnormalState)
                    {
                        //Por cada estado que tiene, recorriendo desde el último al primero
                        for (int k = statenames.Count - 1; k >= 0; k--)
                        {
                            //creo una instancia de efecto de estado
                            StatusEffect se = new StatusEffect();

                            //y la carta resultante al aplicarle el efecto es igual al diccionario de estados (keys) y
                            //delegados (values) que ejecutan dicho efecto al ser invocado en la carta actual. 
                            ACard cardres = se.StatesEffectPerTurn[statenames[k]].Invoke(card);

                            //ahora reduzco la cantidad de turnos restantes al estado
                            cardres.ListOfAbnormalStateDuration[statenames[k]] -= 1;

                            //compruebo si se hicieron 0 la cantidad de turnos que le faltan al estado para acabar su efecto
                            if (cardres.ListOfAbnormalStateDuration[statenames[k]] == 0)
                            {
                                //en caso de que si lo elimino
                                cardres.ListOfAbnormalStateDuration.Remove(statenames[k]);

                                //y en caso de que el efecto sea de infectar o ser infectado lo cambio
                                cardres.IsInfected = false;
                                cardres.InfectedOther = false;
                            }

                            //Finalmente coloco la carta resultante donde estaba la original
                            user.AField.SetCard(cardres, user.AField.Find(card));

                            //Compruebo si la carta llego su VIT a 0
                            if (cardres.VIT == 0)
                            {
                                //entonces la elimino
                                user.Cementery.ToCementery(cardres);
                                user.AField.UsedOrDestroyed(cardres);
                            }

                        }

                    }
                }
            }

            return user;
        }

        /// <summary>Cuenta la cantidad de cartas en la mano que tomadas de forma random podría pagar el user. (1)Para cartas naturales y (2) Para cartas mágicas.</summary>
        public int HowManyRandomCanPay(Player user, int type)
        {
            //obtengo las cartas en mano 
            List<ACard> listnatural = user.CardsInHand.GetNaturalCardsInHand();
            List<ACard> listmagical = user.CardsInHand.GetMagicalCardsInHand();

            //Son o bien tipo 2, o sea mágicas
            if (type == 2)
            {
                //Devuelve la mínima combinación de cartas mágicas que se pueden tomar de forma random
                int res = MinRandomCombinationThatOvercomePoints(listmagical, user) - 1;

                //Si la mínima combinación que sobrepasa el presupuesto es una
                if (res == 0) return 1;
                return res;
            }

            //O tipo 1, naturales
            else
            {
                //Devuelve la mínima combinación de cartas naturales que se pueden tomar de forma random
                return MinRandomCombinationThatOvercomePoints(listnatural, user);
            }

        }

        /// <summary>Cuenta la cantidad de cartas en la mano que podría pagar el user. (1)Para cartas naturales y (2) Para cartas mágicas.</summary>
        public List<ACard> HowManyCardsCanPay(Player user, int type)
        {
            //obtengo las cartas en mano 
            List<ACard> listnatural = user.CardsInHand.GetNaturalCardsInHand();
            List<ACard> listmagical = user.CardsInHand.GetMagicalCardsInHand();

            //Son o bien tipo 2, o sea mágicas
            if (type == 2)
            {
                //devuelvo la máxima combinación de cartas que no sobrepasa el presupuesto
                return MaxCombinationThatDontOvercomePoints(listmagical, user);
            }

            //O tipo 1, naturales
            else
            {
                //devuelvo la máxima combinación de cartas que no sobrepasa el presupuesto
                return MaxCombinationThatDontOvercomePoints(listnatural, user);
            }

        }

        /// <summary>Devuelve la mínima combinación de cartas que se pueden tomar de forma random tal que su precio no sea mayor que el presupuesto.</summary>
        public int MinRandomCombinationThatOvercomePoints(List<ACard> list, Player user)
        {
            //llevo la list a array y creo una lista de listas de array de carta de las posibles soluciones 
            //O sea, es para guardar el conjunto potencia de cartas de la lista.
            ACard[] arr = list.ToArray();
            List<List<ACard[]>> possiblesolutions = new();

            //primero calculamos el costo total de todas las cartas (suma de todos los costos de Faith, de todos los 
            //de Knowledge, etc que vienen en un array de costos en ese orden Faith, Knowledge, Capital, Militarism.)
            double[] costsall = SumCost(arr);

            //comprobamos si todas las cartas se pueden pagar y así nos ahorramos el resto de código
            //O sea si todos los costos son menores o igual que su respectivo presupuesto
            if (costsall[0] <= user.Kingdom.Capital && costsall[1] <= user.Kingdom.Faith &&
                    costsall[2] <= user.Kingdom.Knowledge && costsall[3] <= user.Kingdom.Militarism)
            {
                //entonces la cantidad máxima de cartas que se puede invocar tomadas de forma random son todas
                return list.Count;
            }

            //Sino se pueden todas alguna se podrá (ya que eso se chequea antes con otro método, cosa que el compilador no puede saber)
            else
            {
                //entonces recorro por todas las longitudes posibles
                for (int k = 1; k < arr.Length; k++)
                {
                    //y por cada longitud encuentro todas las combinaciones
                    Combinations(arr, k);

                    //las agrego (estas se guardan en la variable global solutions y de ahí la clono a una variable)
                    var sol = CloneCollection(solutions);
                    possiblesolutions.Add(sol);

                    //limpio el solutions para la próxima iteración
                    CleanSolution();
                }

                //Por cada conjunto de cartas del conjunto potencia de las posibles soluciones
                foreach (List<ACard[]> groupofcards in possiblesolutions)
                {
                    //recorro por el conjunto de cartas
                    for (int k = 0; k < groupofcards.Count; k++)
                    {
                        //obtengo la suma de los costes de la/s carta/s dentro de el conjunto actual
                        double[] costs = SumCost(groupofcards[k]);

                        //si los costos fueran mayores que el presupuesto
                        if (costs[0] > user.Kingdom.Capital && costs[1] > user.Kingdom.Faith &&
                        costs[2] > user.Kingdom.Knowledge && costs[3] > user.Kingdom.Militarism)
                        {
                            //entonces la cantidad máxima de cartas que se puede invocar tomadas de forma random es 
                            //la cantidad del grupo menos 1
                            return groupofcards.Count - 1;
                        }
                    }
                }
            }

            //retorno la cantidad de la lista, pero el código nunca debería llegar aquí
            return list.Count;

        }

        /// <summary>Devuelve la máxima combinación de cartas que se pueden tomar tal que su precio no sea mayor que el presupuesto.</summary>
        public List<ACard> MaxCombinationThatDontOvercomePoints(List<ACard> list, Player user)
        {
            //llevo la list a array y creo una lista de listas de array de carta de las posibles soluciones 
            //O sea, es para guardar el conjunto potencia de cartas de la lista.
            ACard[] arr = list.ToArray();
            List<List<ACard[]>> possiblesolutions = new();

            //primero calculamos el costo total de todas las cartas (suma de todos los costos de Faith, de todos los 
            //de Knowledge, etc que vienen en un array de costos en ese orden Faith, Knowledge, Capital, Militarism.)
            double[] costsall = SumCost(arr);

            //comprobamos si todas las cartas se pueden pagar y así nos ahorramos el resto de código
            //O sea si todos los costos son menores o igual que su respectivo presupuesto
            if (costsall[0] <= user.Kingdom.Capital && costsall[1] <= user.Kingdom.Faith &&
                    costsall[2] <= user.Kingdom.Knowledge && costsall[3] <= user.Kingdom.Militarism)
            {
                //entonces la cantidad máxima de cartas que se puede invocar son todas
                return list;
            }

            //Sino se pueden todas alguna se podrá (ya que eso se chequea antes con otro método, cosa que el compilador no puede saber)
            else
            {
                //entonces recorro por todas las longitudes posibles
                for (int k = 1; k < arr.Length; k++)
                {
                    //y por cada longitud encuentro todas las combinaciones
                    Combinations(arr, k);

                    //las agrego (estas se guardan en la variable global solutions y de ahí la clono a una variable)
                    var sol = CloneCollection(solutions);
                    possiblesolutions.Add(sol);

                    //limpio el solutions para la próxima iteración
                    CleanSolution();
                }

                //Recorro desde atrás por el conjunto potencia de las posibles soluciones
                for (int j = possiblesolutions.Count - 1; j >= 0; j--)
                {
                    //recorro por el conjunto de cartas desde atrás igual ya que los mayores conjuntos están atrás
                    for (int k = possiblesolutions[j].Count - 1; k >= 0; k--)
                    {
                        //obtengo la suma de los costes de la/s carta/s dentro de el conjunto actual
                        double[] costs = SumCost(possiblesolutions[j][k]);

                        //si los costos fueran menores o iguales que el presupuesto
                        if (costs[0] <= user.Kingdom.Capital && costs[1] <= user.Kingdom.Faith &&
                        costs[2] <= user.Kingdom.Knowledge && costs[3] <= user.Kingdom.Militarism)
                        {
                            //entonces esa es la cantidad máxima de cartas que se puede invocar
                            return possiblesolutions[j][k].ToList();
                        }
                    }
                }


            }

            //como el compilador no sabe que en este punto ya hubo solución, devuelvo la lista original
            return list;

        }

        /// <summary>Crea todas las posibles combinaciones de cartas a partir de un array de cartas.</summary>
        void Combinations(ACard[] arr, int M)
        {
            //array de combinaciones de tamaño M
            ACard[] comb1 = new ACard[M];

            //contador para la condición de parada
            int count = 0;

            //array booleano para marcar cuales se han usado
            bool[] used = new bool[arr.Length];

            //mínimo valor a usar después
            int minvalue = 0;

            //busqueda recursiva
            RecursiveComb(arr, count, used, comb1, minvalue);

        }

        /// <summary>Método recursivo para buscar todos las posibles combinaciones de cartas a partir de un array de cartas. Usa variable global.</summary>
        void RecursiveComb(ACard[] arr, int count, bool[] used, ACard[] comb1, int minvalue)
        {
            //si el contador es del Lenght del array de combinación entonces ya tenemos uno 
            if (count == comb1.Length)
            {
                //creamos un array que lo copie y se lo agregamos a la solución
                ACard[] arr2 = new ACard[comb1.Length];
                Array.Copy(comb1, arr2, comb1.Length);
                solutions.Add(arr2);
            }

            //sino
            else
            {
                //en el for funciona mejor k <= N - (M - Count) que solo k < N, donde N es el total y M la longitud del array combinatorio.
                for (int k = minvalue; k <= arr.Length - (comb1.Length - count); k++)
                {
                    //si no se ha usado
                    if (!used[k])
                    {
                        //se usa, se guarda y se llama recursivo
                        used[k] = true;
                        comb1[count] = arr[k];
                        RecursiveComb(arr, count + 1, used, comb1, minvalue + 1);

                        //cuando regresa se desmarca
                        used[k] = false;
                    }
                }
            }
        }

        /// <summary>Método para limpiar la variable global del método recursivo.</summary>
        void CleanSolution()
        {
            List<ACard[]> sol = new();
            solutions = sol;
        }

        /// <summary>Método para sumar los costos de cada tipo de un conjunto de cartas.</summary>
        double[] SumCost(ACard[] list)
        {
            //creo doubles para llevar las cantidades de costo
            double sumfaith = 0;
            double summilitar = 0;
            double sumknowledge = 0;
            double sumcapital = 0;


            //Por cada carta en la lista
            foreach (ACard card in list)
            {
                //aumento el valor de los costos, en este caso tendría al final los costos totales
                sumfaith += card.ListOfCostFKCM[0];
                sumknowledge += card.ListOfCostFKCM[1];
                sumcapital += card.ListOfCostFKCM[2];
                summilitar += card.ListOfCostFKCM[3];
            }


            return new double[] { sumcapital, sumfaith, sumknowledge, summilitar };
        }

        /// <summary>Elimina las cartas que no pueden activar ningún efecto de una lista de cartas dadas y la lista de listas que dicen si se puede pagar.</summary>
        public List<ACard> CardsThatCanUseEffect(List<ACard> cards, List<List<bool>> canpay)
        {
            int falseamount = 0;

            //recorro la lista de listas desde atrás
            for (int k = canpay.Count - 1; k >= 0; k--)
            {
                //por cada bool false aumento el contador
                foreach (bool b in canpay[k])
                {
                    if (!b)
                    {
                        falseamount++;
                    }
                }

                //si el contador es 4 es porque no se puede pagar ningún efecto
                if (falseamount == 4)
                {
                    //y entonces quito esa carta
                    cards.Remove(cards[k]);
                }

                //luego reseteo el contador
                falseamount = 0;
            }

            return cards;
        }

        /// <summary>Devuelve un null, el primer efecto usable y el número del 1 al 4 de dicho efecto. Se hizo así ya que solo puede tener un tipo de retorno y los efectos son 2 de un tipo y 2 de otro.</summary>
        public (Func<Player, List<double>, ACard, ACard, double, Player, Player[]> single,
                Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> multiple,
                int effectnum)
                FirstUsableEffect(ACard card, List<bool> canpay)
        {
            //Creo la lista de efectos sencillos y los múltiples y les añado los efectos, 2 para cada uno
            List<Func<Player, List<double>, ACard, ACard, double, Player, Player[]>> ListSingleEffects = new();
            List<Func<Player, List<double>, ACard, ACard[], double, Player, Player[]>> ListMultipleEffects = new();
            ListSingleEffects.Add(card.Effect1);
            ListSingleEffects.Add(card.Effect2);
            ListMultipleEffects.Add(card.Effect3);
            ListMultipleEffects.Add(card.Effect4);

            //recorro por la lista de bool
            for (int k = 0; k < canpay.Count; k++)
            {
                //si es true
                if (canpay[k])
                {
                    //entonces si el k es 0 o 1
                    if (k < 2)
                    {
                        //devuelvo la lista de sencillos en 0 o 1, el otro en null y el número del efecto es k + 1
                        return (ListSingleEffects[k], null, k + 1)!;
                    }

                    //sino entonces es 2 o 3
                    else
                    {
                        //devuelvo null, la lista de múltiples en k - 2 (o sea en 0 o 1) y el número del efecto es k + 1
                        return (null, ListMultipleEffects[k - 2], k + 1)!;
                    }
                }
            }

            //sino había ningún true devuelve (null, null, -1) 
            return (null, null, -1)!;
        }

        /// <summary>Remueve una determinada cantidad de cartas de forma random de un array.</summary>
        public ACard[] RandomRemoveAmount(ACard[] cards, int amount)
        {
            //llevo el array a lista y se lo paso a la sobrecarga que elimina una determinada cantidad de una lista
            List<ACard> listcards = cards.ToList();
            return RandomRemoveAmount(listcards, amount).ToArray();
        }

        /// <summary>Remueve una determinada cantidad de cartas de forma random de una lista.</summary>
        public List<ACard> RandomRemoveAmount(List<ACard> cards, int amount)
        {
            System.Random r = new System.Random();

            //recorro por la cantidad a eliminar
            for (int k = 0; k < amount; k++)
            {
                //tomo un índice random
                int cardtoremove = r.Next(0, cards.Count - 1);

                //y lo elimino de la lista
                cards.Remove(cards[cardtoremove]);
            }

            //devuelvo la lista con los objetivos eliminados
            return cards;
        }

        /// <summary>Método para librarse de las referencias. Dado un array devuelve otro idéntico.</summary>
        public ACard[] CloneCollection(ACard[] cards)
        {
            return cards;
        }

        /// <summary>Método para librarse de las referencias. Dado una lista de array devuelve otra idéntica.</summary>
        public List<ACard[]> CloneCollection(List<ACard[]> cards)
        {
            return cards;
        }

        /// <summary>Método para librarse de las referencias. Dado una lista devuelve otra idéntica.</summary>
        public List<ACard> CloneCollection(List<ACard> cards)
        {
            return cards;
        }

        /// <summary>Remueve las cartas muertas tras un efecto u ataque de una lista de objetivos.</summary>
        public List<ACard> RemoveDeadTargets(List<ACard> tgts, Player enemy)
        {
            //tomo las cartas naturales en el campo
            List<ACard> cardsinfield = enemy.AField.GetNaturalCardsInField();

            //si tienen la misma cantidad de cartas, entonces ninguna murió y se devuelven esos objetivos
            //se comprueba por cantidad pues la carta afectada por el efecto o ataque si sobrevivió ya no es igual
            //que su versión antes del ataque que se encuentra en la lista de objetivos
            if (cardsinfield.Count == tgts.Count)
            {
                //por eso los objetivos a devolver son las cartas en el campo con el objetivo anterior modificado
                return cardsinfield;
            }

            //sino
            else
            {
                //recorro las cartas objetivo
                for (int k = tgts.Count - 1; k >= 0; k--)
                {
                    //quito las que falten en el campo
                    if (!cardsinfield.Contains(tgts[k]))
                    {
                        tgts.Remove(tgts[k]);
                    }
                }

                //y se devuelven esos objetivos
                return tgts;
            }


        }

        /// <summary>Invoca x cantidad de las cartas más poderosas en la lista que le pasen ya ordenadas (que deben ser cartas en la mano), según se pueda dado el presupuesto. Prioriza la invocación de dos cartas, las más poderosa a invocar. Y si hay que elegir entre ambas escoge la más poderosa y la próxima más poderosa que se pueda pagar junto a esa.</summary>
        public IA InvokeXAmountOfMostPowerfulPayableCards(List<ACard> orderofpower, int amount, IA user)
        {
            //si es una carta invocar
            if (amount == 1)
            {
                //recorro por las cartas
                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la primera y más poderosa carta
                    ACard firstmostpowerfulcard = orderofpower[k];

                    //y si puedo pagarla 
                    if (user.CanPay(firstmostpowerfulcard))
                    {
                        //tomo su ind
                        (int x, int y) ind = user.AField.FindEmpty(1);

                        //la invoco, la quito de la mano y rompo el ciclo
                        user.AField.Invocation(firstmostpowerfulcard, ind, user);
                        user.performedactions.Add("The Berserker invoked " + firstmostpowerfulcard.CardName + " in his field in the position (" + ind.x + "," + ind.y + ")!");
                        user.CardsInHand.RemoveFromHand(firstmostpowerfulcard);
                        break;
                    }

                    //sino sigue la próxima carta más poderosa

                }


            }

            //si son 2 cartas a invocar
            else if (amount == 2)
            {
                //bool para comprobar si ya se encontro la combinación de 2 cartas
                bool found = false;

                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la carta más poderosa
                    ACard firstmostpowerfulcard = orderofpower[k];

                    for (int j = k + 1; j < orderofpower.Count; j++)
                    {
                        //tomo la segunda carta más poderosa
                        ACard secondmostpowerfulcard = orderofpower[j];

                        //si puede pagar esas dos
                        if (user.CanPay(firstmostpowerfulcard) && user.CanPay(secondmostpowerfulcard))
                        {
                            //obtengo sus ind
                            (int x, int y) ind = user.AField.FindEmpty(1);

                            //las invoco, las quito de la mano y rompo el ciclo
                            user.AField.Invocation(firstmostpowerfulcard, ind, user);

                            (int k, int j) ind2 = user.AField.FindEmpty(1);

                            user.AField.Invocation(secondmostpowerfulcard, ind2, user);
                            user.performedactions.Add("The Berserker invoked " + firstmostpowerfulcard.CardName + " in his field in the position (" + ind.x + "," + ind.y + ")!");
                            user.performedactions.Add("The Berserker invoked " + secondmostpowerfulcard.CardName + " in his field in the position (" + ind2.k + "," + ind2.j + ")!");
                            user.CardsInHand.RemoveFromHand(new ACard[] { firstmostpowerfulcard, secondmostpowerfulcard });

                            //aquí indico que el ciclo de afuera se puede romper
                            found = true;
                        }

                        //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                        if (found)
                        {
                            break;
                        }

                        //sino continua y prueba la 1era carta poderosa y la 3era, la 1era y la 4rta... 
                        //y si ninguna combinación con la 1era se puede, el for de afuera cambia a la 2da
                        //hasta hallar la combinación de dos cartas a invocar donde al menos 1 es poderosa 
                    }

                    //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                    if (found)
                    {
                        amount -= 2;
                        break;
                    }
                }

            }

            //o si quiero invocar 3 cartas o más 
            else
            {
                //bool para comprobar si ya se encontro la combinación de 2 cartas
                bool found = false;

                //variables para guardar las dos que serán invocadas
                ACard firstmostpowerfulcard = orderofpower[0];
                ACard secondmostpowerfulcard = orderofpower[1];

                //recorro por las cartas en order de más fuerte a más débil
                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la carta más poderosa
                    firstmostpowerfulcard = orderofpower[k];

                    for (int j = k + 1; j < orderofpower.Count; j++)
                    {
                        //tomo la segunda carta más poderosa
                        secondmostpowerfulcard = orderofpower[j];

                        //si puede pagar esas dos
                        if (user.CanPay(firstmostpowerfulcard) && user.CanPay(secondmostpowerfulcard))
                        {
                            //obtengo sus ind
                            (int x, int y) ind = user.AField.FindEmpty(1);

                            //las invoco, las quito de la mano y rompo el ciclo
                            user.AField.Invocation(firstmostpowerfulcard, ind, user);

                            (int k, int j) ind2 = user.AField.FindEmpty(1);

                            user.AField.Invocation(secondmostpowerfulcard, ind2, user);

                            user.performedactions.Add("The Berserker invoked " + firstmostpowerfulcard.CardName + " in his field in the position (" + ind.x + "," + ind.y + ")!");
                            user.performedactions.Add("The Berserker invoked " + secondmostpowerfulcard.CardName + " in his field in the position (" + ind2.k + "," + ind2.j + ")!");
                            user.CardsInHand.RemoveFromHand(new ACard[] { firstmostpowerfulcard, secondmostpowerfulcard });

                            //aquí indico que el ciclo de afuera se puede romper
                            found = true;
                        }

                        //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                        if (found)
                        {
                            break;
                        }

                        //sino continua y prueba la 1era carta poderosa y la 3era, la 1era y la 4rta... 
                        //y si ninguna combinación con la 1era se puede, el for de afuera cambia a la 2da
                        //hasta hallar la combinación de dos cartas a invocar donde al menos 1 es poderosa 
                    }

                    //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                    if (found)
                    {
                        amount -= 2;
                        break;
                    }
                }

                //Aquí quito de la lista de cartas invocables a las dos que ya invoque
                orderofpower.Remove(orderofpower[orderofpower.IndexOf(firstmostpowerfulcard)]);
                orderofpower.Remove(orderofpower[orderofpower.IndexOf(secondmostpowerfulcard)]);

                //luego recorro por las que faltan por invocar
                for (int j = 0; j < amount; j++)
                {
                    //y recorro por la lista de cartas menos las dos poderosas ya invocadas
                    for (int k = 0; k < orderofpower.Count; k++)
                    {
                        //e invoco las primeras que pueda pagar
                        if (user.CanPay(orderofpower[k]))
                        {
                            //obtengo sus ind
                            (int x, int y) ind = user.AField.FindEmpty(1);

                            //la invoco, la quito de la mano y rompo el ciclo
                            user.AField.Invocation(orderofpower[k], ind, user);
                            user.performedactions.Add("The Berserker invoked " + orderofpower[k].CardName + " in his field in the position (" + ind.x + "," + ind.y + ")!");
                            user.CardsInHand.RemoveFromHand(orderofpower[k]);
                            orderofpower.Remove(orderofpower[k]);
                            found = true;
                        }

                        if (found)
                        {
                            break;
                        }

                        //sino la próxima y sino ninguna y no se puede pagar las que se quiere invocar
                    }

                }

            }


            //devuelvo la IA
            return user;
        }

        /// <summary>Sobrecarga para invocar x cantidad de las cartas más poderosas en la lista que le pasen ya ordenadas (que deben ser cartas en la mano), según se pueda dado el presupuesto y en una ubicación específica cada una. Prioriza la invocación de dos cartas, las más poderosa a invocar. Y si hay que elegir entre ambas escoge la más poderosa y la próxima más poderosa que se pueda pagar junto a esa.</summary>
        public IA InvokeXAmountOfMostPowerfulPayableCards(List<ACard> orderofpower, int amount, IA user, List<(int x, int y)> locations)
        {
            //si no hay simplemente retorna
            if (amount == 0)
            {
                return user;
            }

            if (locations.Count == 0 || orderofpower.Count == 0)
            {
                return user;
            }

            if (locations[0].x < 0 || locations[0].y < 0)
            {
                return user;
            }

            //si es una carta invocar
            if (amount == 1)
            {
                //recorro por las cartas
                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la primera y más poderosa carta
                    ACard firstmostpowerfulcard = orderofpower[k];

                    //y si puedo pagarla 
                    if (user.CanPay(firstmostpowerfulcard))
                    {
                        //la invoco, la quito de la mano y rompo el ciclo
                        user.AField.Invocation(firstmostpowerfulcard, locations[k], user);

                        //agrego la acción en dependencia del evol level.
                        if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 2)
                        {
                            user.performedactions.Add(("The Berserkerer evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname1 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                       + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                        }

                        else if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 3)
                        {
                            user.performedactions.Add(("The Berserkerer mega evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname2 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                                                      + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                        }

                        user.CardsInHand.RemoveFromHand(firstmostpowerfulcard);
                        break;
                    }

                    //sino sigue la próxima carta más poderosa

                }


            }

            //si son 2 cartas a invocar
            else if (amount == 2)
            {
                //bool para comprobar si ya se encontro la combinación de 2 cartas
                bool found = false;

                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la carta más poderosa
                    ACard firstmostpowerfulcard = orderofpower[k];

                    for (int j = k + 1; j < orderofpower.Count; j++)
                    {
                        //tomo la segunda carta más poderosa
                        ACard secondmostpowerfulcard = orderofpower[j];

                        //si puede pagar esas dos
                        if (user.CanPay(firstmostpowerfulcard) && user.CanPay(secondmostpowerfulcard))
                        {
                            //las invoco, las quito de la mano y rompo el ciclo
                            user.AField.Invocation(firstmostpowerfulcard, locations[k], user);
                            user.AField.Invocation(secondmostpowerfulcard, locations[j], user);
                            user.CardsInHand.RemoveFromHand(new ACard[] { firstmostpowerfulcard, secondmostpowerfulcard });

                            //agrego la acción en dependencia del evol level.
                            if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 2)
                            {
                                user.performedactions.Add(("The Berserkerer evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname1 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                           + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 3)
                            {
                                user.performedactions.Add(("The Berserkerer mega evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname2 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                                                          + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }


                            if (user.AField.IsInField(secondmostpowerfulcard) && secondmostpowerfulcard.EvolutionLevel == 2)
                            {
                                user.performedactions.Add(("The Berserkerer evolved card " + secondmostpowerfulcard.CardName + " to " + secondmostpowerfulcard.EvolvedNames.evolname1 + " " + secondmostpowerfulcard.CardName + " in his field at the position (" + locations[j].x + "," + locations[j].y + ")."
                           + "\nHis race evolved to " + secondmostpowerfulcard.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + secondmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else if (user.AField.IsInField(secondmostpowerfulcard) && secondmostpowerfulcard.EvolutionLevel == 3)
                            {
                                user.performedactions.Add(("The Berserkerer mega evolved card " + secondmostpowerfulcard.CardName + " to " + secondmostpowerfulcard.EvolvedNames.evolname2 + " " + secondmostpowerfulcard.CardName + " in his field at the position (" + locations[j].x + "," + locations[j].y + ")."
                                                          + "\nHis race evolved to " + secondmostpowerfulcard.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + secondmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            //aquí indico que el ciclo de afuera se puede romper
                            found = true;
                        }

                        //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                        if (found)
                        {
                            break;
                        }

                        //sino continua y prueba la 1era carta poderosa y la 3era, la 1era y la 4rta... 
                        //y si ninguna combinación con la 1era se puede, el for de afuera cambia a la 2da
                        //hasta hallar la combinación de dos cartas a invocar donde al menos 1 es poderosa 
                    }

                    //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                    if (found)
                    {
                        break;
                    }
                }

            }

            //o si quiero invocar 3 cartas o más 
            else
            {
                //bool para comprobar si ya se encontro la combinación de 2 cartas
                bool found = false;

                //variables para guardar las dos que serán invocadas
                ACard firstmostpowerfulcard = orderofpower[0];
                ACard secondmostpowerfulcard = orderofpower[1];

                //recorro por las cartas en order de más fuerte a más débil
                for (int k = 0; k < orderofpower.Count; k++)
                {
                    //tomo la carta más poderosa
                    firstmostpowerfulcard = orderofpower[k];

                    for (int j = k + 1; j < orderofpower.Count; j++)
                    {
                        //tomo la segunda carta más poderosa
                        secondmostpowerfulcard = orderofpower[j];

                        //si puede pagar esas dos
                        if (user.CanPay(firstmostpowerfulcard) && user.CanPay(secondmostpowerfulcard))
                        {
                            //las invoco, las quito de la mano y rompo el ciclo
                            user.AField.Invocation(firstmostpowerfulcard, locations[k], user);
                            user.AField.Invocation(secondmostpowerfulcard, locations[j], user);
                            user.CardsInHand.RemoveFromHand(new ACard[] { firstmostpowerfulcard, secondmostpowerfulcard });


                            //agrego la acción en dependencia del evol level.
                            if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 2)
                            {
                                user.performedactions.Add(("The Berserkerer evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname1 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                           + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else if (user.AField.IsInField(firstmostpowerfulcard) && firstmostpowerfulcard.EvolutionLevel == 3)
                            {
                                user.performedactions.Add(("The Berserkerer mega evolved card " + firstmostpowerfulcard.CardName + " to " + firstmostpowerfulcard.EvolvedNames.evolname2 + " " + firstmostpowerfulcard.CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                                                          + "\nHis race evolved to " + firstmostpowerfulcard.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + firstmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }


                            if (user.AField.IsInField(secondmostpowerfulcard) && secondmostpowerfulcard.EvolutionLevel == 2)
                            {
                                user.performedactions.Add(("The Berserkerer evolved card " + secondmostpowerfulcard.CardName + " to " + secondmostpowerfulcard.EvolvedNames.evolname1 + " " + secondmostpowerfulcard.CardName + " in his field at the position (" + locations[j].x + "," + locations[j].y + ")."
                           + "\nHis race evolved to " + secondmostpowerfulcard.RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + secondmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else if (user.AField.IsInField(secondmostpowerfulcard) && secondmostpowerfulcard.EvolutionLevel == 3)
                            {
                                user.performedactions.Add(("The Berserkerer mega evolved card " + secondmostpowerfulcard.CardName + " to " + secondmostpowerfulcard.EvolvedNames.evolname2 + " " + secondmostpowerfulcard.CardName + " in his field at the position (" + locations[j].x + "," + locations[j].y + ")."
                                                          + "\nHis race evolved to " + secondmostpowerfulcard.RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + secondmostpowerfulcard.EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            //aquí indico que el ciclo de afuera se puede romper
                            found = true;
                        }

                        //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                        if (found)
                        {
                            break;
                        }

                        //sino continua y prueba la 1era carta poderosa y la 3era, la 1era y la 4rta... 
                        //y si ninguna combinación con la 1era se puede, el for de afuera cambia a la 2da
                        //hasta hallar la combinación de dos cartas a invocar donde al menos 1 es poderosa 
                    }

                    //si se encontró una segunda carta que se puede pagar junto a esta, se rompe el ciclo
                    if (found)
                    {
                        amount -= 2;
                        break;
                    }
                }

                //Aquí quito de la lista de cartas invocables a las dos que ya invoque y también sus localizaciones
                int indtoremove1 = orderofpower.IndexOf(firstmostpowerfulcard);
                orderofpower.Remove(orderofpower[indtoremove1]);
                locations.Remove(locations[indtoremove1]);

                int indtoremove2 = orderofpower.IndexOf(secondmostpowerfulcard);
                orderofpower.Remove(orderofpower[indtoremove2]);
                locations.Remove(locations[indtoremove2]);


                //luego recorro por las que faltan por invocar
                for (int j = 0; j < amount; j++)
                {
                    //y recorro por la lista de cartas menos las dos poderosas ya invocadas
                    for (int k = 0; k < orderofpower.Count; k++)
                    {
                        //e invoco las primeras que pueda pagar
                        if (user.CanPay(orderofpower[k]))
                        {
                            //la invoco, la quito de la mano y rompo el ciclo
                            user.AField.Invocation(orderofpower[k], locations[k], user);

                            //agrego la acción en dependencia del evol level.
                            if (user.AField.IsInField(orderofpower[k]) && orderofpower[k].EvolutionLevel == 2)
                            {
                                user.performedactions.Add(("The Berserker evolved card " + orderofpower[k].CardName + " to " + orderofpower[k].EvolvedNames.evolname1 + " " + orderofpower[k].CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                           + "\nHis race evolved to " + orderofpower[k].RaceEvolvedNames.evolname1 + " and gained a powerful new effect! \nOne " + orderofpower[k].EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            else if (user.AField.IsInField(orderofpower[k]) && orderofpower[k].EvolutionLevel == 3)
                            {
                                user.performedactions.Add(("The Berserker mega evolved card " + orderofpower[k].CardName + " to " + orderofpower[k].EvolvedNames.evolname2 + " " + orderofpower[k].CardName + " in his field at the position (" + locations[k].x + "," + locations[k].y + ")."
                                                          + "\nHis race evolved to " + orderofpower[k].RaceEvolvedNames.evolname2 + " and gained a powerful new effect! \nOne " + orderofpower[k].EvolvedNames.evolname1 + " was a added to deck!"));
                            }

                            if (user.AField.IsInField(orderofpower[k]))
                            {
                                user.CardsInHand.RemoveFromHand(orderofpower[k]);
                            }
                            orderofpower.Remove(orderofpower[k]);
                            found = true;
                        }

                        if (found)
                        {
                            break;
                        }

                        //sino la próxima y sino ninguna y no se puede pagar las que se quiere invocar
                    }

                }

            }

            //devuelvo el Player/IA
            return user;
        }

        /// <summary>Devuelvo una lista de los elementos que son inmunes al ataque elemental de la carta, que puede ser el efecto 2 o el efecto 3. Se le pasa (2) para efecto 2 y cualquiero otro número para efecto 3.</summary>
        public List<string> ElementsThatAreInmuneToGivedElementalAttack(ACard card, int effect2oreffect3)
        {
            //obtengo la lista de elementos inmunes a cada elemento de la carta
            List<List<string>> inmunityto = card.ElementsObject.AllElementsRelationOfACard(card.ActualFourElements, 3);

            string state = "";

            if (effect2oreffect3 == 2)
            {
                //obtengo el estado que provoca el efecto 2, pasándole el nombre del ataque
                state = card.EffectsObject.attackandelements[card.EffectTxt2.name];
            }

            else
            {
                //obtengo el estado que provoca el efecto 3, pasándole el nombre del ataque
                state = card.EffectsObject.attackandelements[card.EffectTxt3.name];
            }


            //obtengo el elemento del efecto pasándole el estado que provoca el ataque
            string elem = card.EffectsObject.statesandelements[state];

            //creo entonces la lista de elementos inmunes al ataque elemental (efecto 2) 
            List<string> inmunitytoattack = new();


            // busco cual de los 4 elementos de la carta coincide con el elemento del efecto
            for (int k = 0; k < card.ActualFourElements.Count; k++)
            {
                //cuando encuentre el elemento del efecto en los 4 elementos de la carta
                if (elem == card.ActualFourElements[k])
                {
                    //entonces en el mismo índice que está el elemento en la lista de los 4 está la lista de 
                    //cartas inmune a dicho elemento, la tomo y rompo el ciclo
                    inmunitytoattack = inmunityto[k];
                    break;
                }
            }

            return inmunitytoattack;
        }

        /// <summary>Dice una carta es inmune a un elemento, dado la lista de inmunidad y la carta (que viene con sus 4 elementos) compruebo si alguno de sus 4 elementos está en la lista de elementos inmunes.</summary>
        public bool TheCardISInmuneToElement(ACard card, List<string> elementsinmune)
        {
            //bool para guardar el resultado
            bool isinmune = false;

            //recorro por los elementos inmunes 
            for (int l = 0; l < elementsinmune.Count; l++)
            {
                //compruebo si alguno de los 4 elementos se encuentra en la lista de elementos inmunes al ataque

                //si el elemento 1 no es inmune a ese elemento de la lista
                if (card.CardElementName1 != elementsinmune[l])
                {
                    //ni el elemento 2
                    if (card.CardElementName2 != elementsinmune[l])
                    {
                        //ni el elemento 3
                        if (card.CardElementName3 != elementsinmune[l])
                        {
                            //ni el 4
                            if (card.CardElementName4 != elementsinmune[l])
                            {
                                //entonces es un posible objetivo
                                isinmune = true;
                            }

                            //pero si es inmune lo pongo en false, ya que puede no ser inmune a la iteración k y estar en true 
                            //pero ser inmune a la iteración k+1, por lo que debe ser devuelto a false
                            else
                            {
                                isinmune = false;
                            }
                        }

                        //pero si es inmune lo pongo en false, ya que puede no ser inmune a la iteración k y estar en true 
                        //pero ser inmune a la iteración k+1, por lo que debe ser devuelto a false
                        else
                        {
                            isinmune = false;
                        }
                    }

                    //pero si es inmune lo pongo en false, ya que puede no ser inmune a la iteración k y estar en true 
                    //pero ser inmune a la iteración k+1, por lo que debe ser devuelto a false
                    else
                    {
                        isinmune = false;
                    }
                }

                //pero si es inmune lo pongo en false, ya que puede no ser inmune a la iteración k y estar en true 
                //pero ser inmune a la iteración k+1, por lo que debe ser devuelto a false
                else
                {
                    isinmune = false;
                }


            }

            return isinmune;
        }

        /// <summary>Ordena una lista de cartas por las de mayor ATK.</summary>
        public List<ACard> OrderByPower(List<ACard> cards, Player user)
        {
            if (cards.Count == 0)
            {
                return cards;
            }

            //creo la lista para guardarlas y el ind 
            List<ACard> InOrder = new();
            int ind = -1;

            //hago un ciclo hasta que la lista se quede sin cartas
            while (cards.Count >= 1)
            {
                //recorro por las cartas
                for (int k = 0; k < cards.Count; k++)
                {
                    //comparo la actual con todas y me voy quedando con el ind de la de mayor atk
                    ind = (cards[k].ATK <= cards[0].ATK) ? cards.IndexOf(cards[0]) : cards.IndexOf(cards[k]);
                }

                //guardo la carta de ese ind y la quito de la lista
                InOrder.Add(cards[ind]);
                cards.Remove(cards[ind]);
            }

            //devuelvo la lista ordenada
            return InOrder;
        }

        /// <summary>Ordena una lista de cartas y otra con sus localizaciones respectivas por las de mayor ATK.</summary>
        public (List<ACard> cards, List<(int x, int y)> location) OrderByPower(List<ACard> cards, Player user, List<(int x, int y)> loc)
        {
            if (cards.Count == 0)
            {
                return (cards, loc);
            }

            //creo las listas para guardarlas y el ind 
            List<ACard> InOrder = new();
            List<(int x, int y)> orderloc = new();
            int ind = -1;

            //hago un ciclo hasta que la lista se quede sin cartas
            while (cards.Count >= 1)
            {
                //recorro por las cartas
                for (int k = 0; k < cards.Count; k++)
                {
                    //comparo la actual con todas y me voy quedando con el ind de la de mayor atk
                    ind = (cards[k].ATK <= cards[0].ATK) ? cards.IndexOf(cards[0]) : cards.IndexOf(cards[k]);
                }

                //guardo la carta de ese ind y la quito de la lista
                InOrder.Add(cards[ind]);
                orderloc.Add(loc[ind]);
                cards.Remove(cards[ind]);
                loc.Remove(loc[ind]);
            }


            return (InOrder, orderloc);
        }

        /// <summary>Ordena una lista de cartas por las de menor DEF + VIT.</summary>
        public List<ACard> OrderByLessDefenseAndVit(List<ACard> cards)
        {
            //creo la lista para guardarlas y el ind 
            List<ACard> InOrder = new();
            int ind = -1;

            //hago un ciclo hasta que la lista se quede sin cartas
            while (cards.Count >= 1)
            {
                //recorro por las cartas
                for (int k = 0; k < cards.Count; k++)
                {
                    //comparo la actual con todas y me  voy quedando con el ind de la de menor def + vit
                    ind = ((cards[0].DEF + cards[0].VIT) <= (cards[k].DEF + cards[k].VIT)) ? cards.IndexOf(cards[0]) : cards.IndexOf(cards[k]);
                }

                //guardo la carta de ese ind y la quito de la lista
                InOrder.Add(cards[ind]);
                cards.Remove(cards[ind]);
            }

            //devuelvo la lista ordenada
            return InOrder;
        }

        /// <summary>Ordena una lista de cartas por las más balanceadas. El balance en este caso es las de mayor ATK/(DEF + VIT).</summary>
        public List<ACard> OrderByBalancedStats(List<ACard> cards)
        {
            //creo la lista para guardarlas y el ind 
            List<ACard> InOrder = new();
            int ind = -1;

            //hago un ciclo hasta que la lista se quede sin cartas
            while (cards.Count >= 1)
            {
                //recorro por las cartas
                for (int k = 0; k < cards.Count; k++)
                {
                    //comparo la actual con todas y me  voy quedando con el ind de la de mayor atk/(def + vit)
                    ind = (cards[k].ATK / (cards[k].DEF + cards[k].VIT) <= cards[0].ATK / (cards[0].DEF + cards[0].VIT)) ? cards.IndexOf(cards[0]) : cards.IndexOf(cards[k]);
                }

                //guardo la carta de ese ind y la quito de la lista
                InOrder.Add(cards[ind]);
                cards.Remove(cards[ind]);
            }

            //devuelvo la lista ordenada
            return InOrder;
        }

        public string CheckingRestrictedStates(ACard card)
        {
            if (!card.CanAttack && !card.CanUseEffect && !card.CanSelectTarget && card.ListOfAbnormalStateDuration != null && card.ListOfAbnormalStateDuration.ContainsKey("Confusion"))
            {
                return card.CardName + " as restricted attack for " + card.RestrictAttackTurns + " more turns! \n" +
                card.CardName + " as restricted effects for " + card.RestrictEffectTurns + " more turns! \n" +
                card.CardName + " due to his state Confusion can't select attack for " + card.ListOfAbnormalStateDuration["Confusion"] + " turns!";
            }

            else if (!card.CanUseEffect && !card.CanAttack)
            {
                return card.CardName + " as restricted attack for " + card.RestrictAttackTurns + " more turns! \n" +
                card.CardName + " as restricted effects for " + card.RestrictEffectTurns + " more turns!";
            }

            else if (!card.CanUseEffect && !card.CanSelectTarget && card.ListOfAbnormalStateDuration != null && card.ListOfAbnormalStateDuration.ContainsKey("Confusion"))
            {
                return card.CardName + " as restricted effects for " + card.RestrictEffectTurns + " more turns! \n" +
                card.CardName + " due to his state Confusion can't select attack for " + card.ListOfAbnormalStateDuration["Confusion"] + " turns!";
            }

            else if (!card.CanAttack && !card.CanSelectTarget && card.ListOfAbnormalStateDuration != null && card.ListOfAbnormalStateDuration.ContainsKey("Confusion"))
            {
                return card.CardName + " as restricted attack for " + card.RestrictAttackTurns + " more turns! \n" +
                card.CardName + " due to his state Confusion can't select attack for " + card.ListOfAbnormalStateDuration["Confusion"] + " turns!";
            }

            else if (!card.CanSelectTarget && card.ListOfAbnormalStateDuration != null && card.ListOfAbnormalStateDuration.ContainsKey("Confusion"))
            {
                return card.CardName + " due to his state Confusion can't select attack for " + card.ListOfAbnormalStateDuration["Confusion"] + " turns!";
            }

            else if (!card.CanAttack)
            {
                return card.CardName + " as restricted attack for " + card.RestrictAttackTurns + " more turns!";
            }

            else if (!card.CanUseEffect)
            {
                return card.CardName + " as restricted effects for " + card.RestrictEffectTurns + " more turns!";
            }

            else
            {
                return "";
            }
        }

        public string InmunityCheck(ACard card)
        {
            string res = "";

            if (card.AsEffectInmunity)
            {
                res += (card.CardName + " as effect inmunity for " + card.InmunityEffectDuration + " turns! \n");
            }

            if (card.AsPhysicalInmunity)
            {
                res += (card.CardName + " as attack inmunity for " + card.InmunityPhysicalDuration + " turns! \n");
            }

            if (card.AsInmunityChance)
            {
                res += (card.CardName + " as a 50% chance of being inmunity to effects for " + card.InmunityChanceDuration + " turns! \n");
            }

            return res;
        }
    }
}




