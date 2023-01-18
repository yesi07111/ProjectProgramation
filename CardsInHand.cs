using System.Runtime.Serialization;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IHaveCardsInHand
    {
        //La propiedad mano no puede faltar en forma de array de cartas.
        public ACard[] Hand { get; set; }
    }

    /// <summary>Clase para la mano de cartas de los jugadores.</summary>
    public class CardsInHand : IHaveCardsInHand
    {
        //Constructor de la mano de cartas  que recibe un array de cartas.
        public CardsInHand(ACard[] cards)
        {
            Hand = cards;
        }

        /// <summary>Mano de cartas formado por un array de cartas, las cuales son objetos de tipo ACard. Es una propiedad accesible desde cualquier objeto de instancia CardsInHand.</summary>
        public ACard[] Hand { get; set; }

        /// <summary>Dados el deck actual y la cantidad de cartas a tomar del deck a la mano, modifica la propiedad Hand en consecuencia a las cartas que se agregan y la propiedad Deck en consecuencia a las cartas tomadas.</summary>
        public void DrawingCards(DeckOfCards actualdeck, int drawnumber)
        {
            if (this.Hand.Length >= 10)
            {
                return;
            }
            //Creo una lista  
            List<ACard> cardstakenfromdeck = new();

            //Compruebo que el deck tiene igual o más cartas que las se van a robar.
            if (actualdeck.Deck.Length >= drawnumber)
            {
                //Y de ser así las guardo en la lista de cartas.
                for (int k = 0; k < drawnumber; k++)
                {
                    cardstakenfromdeck.Add(actualdeck.Deck[k]);
                }
            }

            //Si el deck no tiene cartas suficientes pero al menos tiene 1.
            else if (actualdeck.Deck.Length >= 1)
            {
                for (int k = 0; k < actualdeck.Deck.Length; k++)
                {
                    cardstakenfromdeck.Add(actualdeck.Deck[k]);
                }
            }

            //Si la lista de cartas robadas tiene al menos 1 carta la añado a la mano y la elimino del deck.
            if (cardstakenfromdeck.Count >= 1)
            {
                ACard[] csfd = cardstakenfromdeck.ToArray();
                Add(csfd);
                actualdeck.Remove(csfd);
            }

        }

        /// <summary>Método de instancia que recibe un array de cartas. Modifica la propiedad Hand y la iguala a la nueva mano con las cartas que se tenía más las del array de cartas.</summary>
        public void Add(ACard[] cards)
        {
            //Array de cartas para guardar la nueva mano
            ACard[] newhand = new ACard[Hand.Length + cards.Length];
            int count = 0;

            for (int k = 0; k < newhand.Length; k++)
            {
                if (k < Hand.Length)
                {
                    newhand[k] = Hand[k];
                }

                else
                {
                    newhand[k] = cards[count];
                    count++;
                }

            }

            Hand = newhand;
        }

        /// <summary>Método de instancia que recibe una carta. Modifica la propiedad Hand y la iguala a la nueva mano con las cartas que se tenía más la carta nueva.</summary>
        public void Add(ACard card)
        {
            List<ACard> prov = Hand.ToList();
            prov.Add(card);
            Hand = prov.ToArray();
        }

        /// <summary>Método de instancia que recibe una carta. Modifica la propiedad Hand quitándole una carta en caso de tenerla en la mano.</summary>
        public void RemoveFromHand(ACard card)
        {
            //llevo la propiedad Hand a lista
            List<ACard> hand = Hand.ToList();

            //compruebo si tiene la carta en la mano
            if (hand.Contains(card))
            {
                //la quito de ser así
                hand.Remove(card);
            }

            //y luego la igualo a Hand
            Hand = hand.ToArray();
        }

        /// <summary>Método de instancia que recibe un array de cartas. Modifica la propiedad Hand quitándole una carta en caso de tenerla en la mano.</summary>
        public void RemoveFromHand(ACard[] cards)
        {
            //por cada carta del array
            foreach (ACard card in cards)
            {
                //la elimino
                RemoveFromHand(card);
            }

        }

        public bool ContainCard(ACard card)
        {
            for (int k = 0; k < Hand.Length; k++)
            {
                if (Hand[k] != null && Hand[k].CardName == card.CardName
                   && Hand[k].CardLore == card.CardLore &&
                     Hand[k].ATKOriginal == card.ATKOriginal &&
                      Hand[k].DEFOriginal == card.DEFOriginal &&
                       Hand[k].VITOriginal == card.VITOriginal)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método de instancia que cuenta la cantidad de cartas naturales en mano.</summary>
        public int AmountOfNaturalCardsInHand()
        {
            int res = 0;

            foreach (ACard card in Hand)
            {
                if (card != null && card.CardTypeNum == 1)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia que cuenta la cantidad de cartas mágicas en mano.</summary>
        public int AmountOfMagicalCardsInHand()
        {
            int res = 0;

            foreach (ACard card in Hand)
            {
                if (card != null && card.CardTypeNum == 2)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia que devuelve las cartas naturales en mano.</summary>
        public List<ACard> GetNaturalCardsInHand()
        {
            List<ACard> cards = new();

            foreach (ACard card in Hand)
            {
                if (card != null && card.CardTypeNum == 1)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        /// <summary>Método de instancia que devuelve las cartas mágicas en mano.</summary>
        public List<ACard> GetMagicalCardsInHand()
        {
            List<ACard> cards = new();

            foreach (ACard card in Hand)
            {
                if (card != null && card.CardTypeNum == 2)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }



        /// <summary>Método de instancia que devuelve las cartas naturales en mano en orden de la de mayor ATK al ser invocada a la de menor.</summary>
        public List<ACard> GetMostPowerfulCardsInHand(Player user)
        {
            //Lista de cartas en la mano
            List<ACard> cards = GetNaturalCardsInHand();

            return user.OrderByPower(cards, user);

        }

        /// <summary>Método de instancia que devuelve las cartas naturales en mano en orden de la de mayor DEF + VIT a la de menor.</summary>
        public List<ACard> GetMostDefensibleCardsInHand(Player user)
        {
            //Lista para guardarlas en orden de mayor a menor DEF + VIT
            List<ACard> InOrder = new();

            //Lista de cartas en la mano
            List<ACard> cards = GetNaturalCardsInHand();

            //guardo el tamaño original de la lista de cartas a ordenar
            int amounttofill = cards.Count;

            //carta para ir guardando la más poderosa (la de mayor DEF + VIT) y un int para su índice
            ACard mostpowerful = cards[0];
            int ind = -1;

            //clono la lista de cartas
            List<ACard> clonedcards = user.CloneCollection(cards);

            //recorro por las cartas clonadas
            for (int n = 0; n < clonedcards.Count; n++)
            {
                //a cada carta clonada le hago el efecto de aumentar sus stats según su afiliación, como si fuera invocada
                //ya que al invocarla ese es su ATK real a tener en cuenta

                //recorro por las afiliaciones de la carta actual
                for (int k = 0; k < clonedcards[n].Affiliations.Count; k++)
                {
                    //busco la afiliación de la carta que corresponda con la disposición del reino
                    if (user.Kingdom.Disposition == clonedcards[n].Affiliations[k].TypeOfKingdomAffiliation)
                    {
                        //Cuando la encuentre aumento el ATK en dicha cantidad de afiliación por 10.
                        clonedcards[n].ATK += (clonedcards[n].Affiliations[k].amount * 10);
                    }
                }
            }

            //recorro por la cantidad de cartas a llenar
            for (int j = 0; j < amounttofill; j++)
            {
                //y por la cantidad de cartas clonadas para encontrar la más fuerte si fueran invocadas
                for (int k = 0; k < clonedcards.Count; k++)
                {
                    //si la actual es más fuerte que la que ya tenía, y no es menos que 0 su DEF + VIT
                    if ((cards[k].DEF + cards[k].VIT) > (mostpowerful.DEF + mostpowerful.VIT) && (cards[k].DEF + cards[k].VIT) > 0)
                    {
                        //entonces tomo la carta clonada como la nueva más poderosa
                        mostpowerful = clonedcards[k];

                        //y tomo su índice
                        ind = k;

                        //y le quito el DEF y VIT a la carta, pues es más fuerte que cualquier otra que busque luego
                        mostpowerful.DEF = -1;
                        mostpowerful.VIT = -1;
                    }
                }

                //añado la más fuerte original a partir del índice, no la clonada con su DEF y VIT aumentados como si estuviera invocada
                InOrder.Add(cards[ind]);

                //y reseteo el índice
                ind = -1;

            }

            return InOrder;
        }

        /// <summary>Método para comprobar si la mano está vacía.</summary>
        public bool IsEmpty()
        {
            if (Hand == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>Método para comprobar si la mano tiene cartas naturales.</summary>
        public bool AreNaturalCardsInHand()
        {
            //recorro por las cartas en la mano
            for (int k = 0; k < Hand.Length; k++)
            {
                //Y si una carta es tipo 1, natural devuelvo verdadero
                if (Hand[k].CardTypeNum == 1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método para comprobar si la mano tiene cartas mágicas.</summary>
        public bool AreMagicalCardsInHand()
        {
            //recorro por las cartas en la mano
            for (int k = 0; k < Hand.Length; k++)
            {
                //Y si una carta es tipo 2, mágica devuelvo verdadero
                if (Hand[k].CardTypeNum == 2)
                {
                    return true;
                }
            }

            return false;
        }

        public ACard FindMagicalCardWithEffect(List<ACard> cards, string effname)
        {
            foreach (ACard card in cards)
            {
                if (card.EffectTxt1.name == effname)
                {
                    return card;
                }

            }

            return null!;
        }

    }

}
