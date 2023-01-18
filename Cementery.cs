using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{

    public interface IHaveACementery
    {
        public List<ACard> CardsInCementery { get; set; }
    }

    /// <summary>Clase para el cementerio de cartas.</summary>
    public class Cementery : IHaveACementery
    {
        //Propiedad que guarda las cartas en el cementerio.
        public List<ACard> CardsInCementery { get; set; }

        //Contructor por defecto vacío. Solo inicializa la lista del cartas en el cementerio.
        public Cementery()
        {
            CardsInCementery = new();
        }

        /// <summary>Método de instancia para enviar cartas al cementerio.</summary>
        public void ToCementery(List<ACard> cards)
        {
            //Cada carta la mando al cementerio una a una
            foreach (ACard card in cards)
            {
                ToCementery(card);
            }
        }

        /// <summary>Método de instancia para enviar una carta al cementerio.</summary>  
        public void ToCementery(ACard card)
        {
            //Al cementerio va con sus stats originales
            card.Reset();

            //le cambio nada más que no puede atacar ni usar efectos, y su ubicación.
            card.CanAttack = false;
            card.CanUseEffect = false;

            card.IsInCementery = true;
            card.IsInHand = false;
            card.IsInvoked = false;

            CardsInCementery.Add(card);
        }

        /// <summary>Método de instancia para comprobar si una carta está en el cementerio.</summary> 
        public bool IsInCementery(ACard card)
        {

            foreach (ACard card0 in CardsInCementery)
            {
                if (card0 != null && card0.CardName == card.CardName
                    && card0.CardLore == card.CardLore &&
                      card0.ATKOriginal == card.ATKOriginal &&
                       card0.DEFOriginal == card.DEFOriginal &&
                        card0.VITOriginal == card.VITOriginal)
                    return true;
            }

            return false;
        }

        /// <summary>Método de instancia para remover una carta del cementerio.</summary>
        public void RemoveformGraveyard(ACard card)
        {
            CardsInCementery.Remove(card);
        }

        /// <summary>Método de instancia para remover una lista de cartas del cementerio.</summary>
        public void RemoveformGraveyard(List<ACard> cards)
        {
            foreach (ACard card in cards)
            {
                RemoveformGraveyard(card);
            }
        }



    }

}