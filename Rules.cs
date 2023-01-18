namespace ProjectClasses
{
    public interface IRules
    {
        public int MaxCardsInDeck { get; set; }
        public int CardsInHandAtStart { get; set; }
        public int CardsDrawPerTurn { get; set; }
        public bool WinBecauseLifeReachZero { get; set; }


    }

    /// <summary>Clase de las reglas del juego.</summary>
    public class Rules : IRules
    {
        //Propiedades de las reglas
        public int MaxCardsInDeck { get; set; }
        public int CardsInHandAtStart { get; set; }
        public int CardsDrawPerTurn { get; set; }
        public bool WinBecauseLifeReachZero { get; set; }

        /// <summary>Reglas para ganar: Gana si lleva la vida del oponente a 0. Se pueden establecer las cartas máximas del deck entre 15 y 40, las cartas en mano al inicio entre 1 y 6, las cartas que robas por turno entre 1 y 3. De no poner nada va por defecto: reglas clásicas (1), 40 cartas en el deck, 5 en la mano al empezar y robar 1 por turno. Si se introducen valores inválidos se ponen los por defecto igual.</summary>
        public Rules()
        {
            MaxCardsInDeck = 40;
            CardsInHandAtStart = 5;
            CardsDrawPerTurn = 1;
            WinBecauseLifeReachZero = true;
        }

        /// <summary>Sobrecarga para personalizar todas las reglas: Se pueden establecer las cartas máximas del deck entre 15 y 40, las cartas en mano al inicio entre 1 y 6, las cartas que robas por turno entre 1 y 3. Si se introducen valores inválidos se ponen los por defecto.</summary>
        public Rules(int MaxCardsInDeck, int CardsInHandAtStart, int CardsDrawPerTurn)
        {

            //Propiedades:

            if (MaxCardsInDeck < 15 || MaxCardsInDeck >= 40)
            {
                this.MaxCardsInDeck = 40;
            }

            else
            {
                this.MaxCardsInDeck = MaxCardsInDeck;
            }

            if (CardsInHandAtStart < 1 || CardsInHandAtStart > 6)
            {
                this.CardsInHandAtStart = 5;
            }

            else
            {
                this.CardsInHandAtStart = CardsInHandAtStart;
            }

            if (CardsDrawPerTurn < 1 || CardsDrawPerTurn > 3)
            {
                this.CardsDrawPerTurn = 1;
            }

            else
            {
                this.CardsDrawPerTurn = CardsDrawPerTurn;
            }


        }

        /// <summary>Método estático para comprobar si un reino cumple con la regla de vida.</summary>
        public static bool CheckLife(Player k)
        {
            if (k.Life <= 0) return false;
            return true;
        }
    }

}

