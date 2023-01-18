using System;
using System.Threading;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IHaveADeck
    {
        //La propiedad deck no puede faltar en forma de array de cartas.
        public ACard[] Deck { get; set; }
    }

    /// <summary>Clase para el uso del deck de cartas.</summary>
    public class DeckOfCards : IHaveADeck
    {
        //Contructor del deck que recibe un array de cartas.
        public DeckOfCards(ACard[] cards)
        {
            Deck = cards;
        }


        /// <summary>Deck de cartas formado por un array de cartas, las cuales son objetos de tipo ACard. Es una propiedad accesible desde cualquier objeto de instancia DeckOfCards.</summary>
        public ACard[] Deck { get; set; }

        /// <summary>Método de instancia que recibe  el array de cartas a remover de él. Modifica la propiedad Deck al remover las cartas del array dado de cartas.</summary>
        public void Remove(ACard[] cardstoremove)
        {
            //Convierto el array de cartas del deck en una lista.
            var listdeck = Deck.ToList();

            //Recorro el array de cartas a remover para quitarlas todas.
            for (int k = 0; k < cardstoremove.Length; k++)
            {
                listdeck.Remove(cardstoremove[k]);
            }

            //Modifico la propiedad deck igualándola a la lista con las cartas removidas vuelta array de nuevo.
            Deck = listdeck.ToArray();
        }

        /// <summary>Método de instancia que elimina una carta específica modificando la propiedad Deck.</summary>
        public void Remove(ACard cardtoremove)
        {
            //Lo convierto a lista, le quito la carta y modifico la propiedad igualándola a la lista llevada a array.
            var listdeck = Deck.ToList();
            listdeck.Remove(cardtoremove);
            Deck = listdeck.ToArray();
        }

        /// <summary>Método de instancia para añadir una carta al deck en la última posición.</summary>  
        public void Add(ACard card)
        {
            Deck.ToList().Add(card);
        }

        /// <summary>Método de instancia que recibe las cartas en el deck y te cambia la propiedad Deck con las cartas barajeadas, es decir, en índices random diferentes a su índice original.</summary>  
        public void Shuffle(ACard[] cardsindeck)
        {
            //Crea un objeto de instancia random, una lista para comprobar que índices ya usé y el array de cartas.
            Random r = new Random();
            List<double> used = new List<double>();
            ACard[] newcards = new ACard[cardsindeck.Length];

            //Recorro por el array de cartas a rellenar.
            for (int k = 0; k < newcards.Length; k++)
            {

                //Índice de las cartas a colocar en la posicion k del deck (array de cartas).
                double ind = ((int)r.Next(0, cardsindeck.Length));

                //Si ese índice no se ha usado lo agrego a la lista de usados y el deck en la poscion k es igual a la
                //carta en dicho índice.
                if (!used.Contains(ind))
                {
                    used.Add(ind);
                    newcards[k] = cardsindeck[((int)ind)];
                }

                //sino regresa a la posición anterior de k y continua el for, para que busque otro índice válido para esa
                //k.
                else
                {
                    k -= 1;
                    continue;
                }
            }

            Deck = newcards;
        }


    }

}
