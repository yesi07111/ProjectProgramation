using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System;
using ProjectClasses;
using System.IO;

namespace TheConsoleGame
{
    public static class Game
    {
        public static List<ACard> CreatedCards = new();
        public static bool PlayWithCreatedCards = false;
        public static string playername = "Alex";
        public static string[] dispositionss = { "Faith", "Knowledge", "Capital", "Militar" };
        public static string disposition = "Faith";
        public static double Life = 10000;
        public static double F = 1000;
        public static double M = 1000;
        public static double K = 1000;
        public static double C = 1000;
        public static int MaxCD = 40;
        public static int MinCD = 15;
        public static int MCH = 6;
        public static int MDT = 1;
        public static int ianum = 1;
        public static List<ACard> database = CardLoader.LoadFirstDataBase();
        public static CardsInHand ch = new(new ACard[] { });
        public static DeckOfCards dc = MakeRandomDeck();
        public static Kingdoms kg = new(disposition, F, M, K, C);
        public static CardsInHand ch0 = new(new ACard[] { });
        public static DeckOfCards dc0 = MakeRandomDeck();
        public static Kingdoms kg0 = new(disposition, F, M, K, C);
        public static Rules rl = new(MaxCD, MCH, MDT);
        public static Player p1 = new(kg, ch, dc, new Cementery(), new AField(), rl, Life);
        public static IA ia = new(ianum, kg0, ch0, dc0, new Cementery(), new AField(), rl, Life);

        static double lif = CopyValue(Life);
        static double fa = CopyValue(F);
        static double ca = CopyValue(C);
        static double kw = CopyValue(K);
        static double mi = CopyValue(M);


        public static void Main()
        {

            Console.Title = "Emperor's Game v1.0 ☠️ ";

            while (true)
            {
                Console.Clear();
                PrintMenu();

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.N:
                    case ConsoleKey.Enter:
                        {
                            NewGame();
                            break;
                        }

                    case ConsoleKey.S:
                        {
                            Settings();
                            break;
                        }

                    case ConsoleKey.C:
                        {
                            CardCreator();
                            break;
                        }

                    case ConsoleKey.E:
                    case ConsoleKey.Escape:
                        {
                            return;
                        }

                    default:
                        break;
                }
            }
        }

        #region Methods to play

        static void NewGame()
        {
            Console.Clear();
            System.Console.WriteLine("IA Drawing Start Cards...");
            //Thread.Sleep(3000);
            Console.Clear();
            Intialize();
            PrintIAState();

            System.Console.WriteLine("Setting fields...");
            //Thread.Sleep(3000);
            System.Console.WriteLine("\nIA Field: \n");
            PrintField(ia.AField.Field);
            //Thread.Sleep(3500);
            System.Console.WriteLine();

            System.Console.WriteLine("\nPlayer Field: \n");
            PrintField(ia.AField.Field);
            //Thread.Sleep(3500);
            System.Console.WriteLine();

            System.Console.WriteLine("Player Drawing Start Cards...");
            //Thread.Sleep(3000);
            Console.Clear();

            PrintIAState();
            System.Console.WriteLine("\nIA Field: \n");
            PrintField(ia.AField.Field);
            System.Console.WriteLine();

            System.Console.WriteLine("\nPlayer Field: \n");
            PrintField(ia.AField.Field);
            System.Console.WriteLine();

            System.Console.WriteLine("Count deck " + dc.Deck.Length);

            System.Console.WriteLine("" + playername + "'s cards in hand: ");
            DrawCards(MCH);
            System.Console.WriteLine();
            PrintUserState();
            TurnActions();

        }

        static void DrawCards(int amount)
        {
            if (p1.CardsInHand.Hand.Length >= 10)
            {
                return;
            }

            for (int j = 0; j < amount; j++)
            {
                if (p1.DeckOfCards != null && p1.DeckOfCards.Deck != null)
                {
                    PrintACard(p1.DeckOfCards.Deck[0]);
                    p1.DeckOfCards.Deck[0].IsInHand = true;
                    p1.CardsInHand.Add(p1.DeckOfCards.Deck[0].Clone(p1.DeckOfCards.Deck[0]));
                    p1.DeckOfCards.Remove(p1.DeckOfCards.Deck[0]);
                    //Thread.Sleep(2000);
                }

            }
        }

        static void Intialize()
        {
            System.Random r = new();
            dc = MakeRandomDeck();
            dc0 = MakeRandomDeck();

            if (PlayWithCreatedCards)
            {
                LoadSavedCardsToDeck();
            }

            dc.Shuffle(dc.Deck);
            dc0.Shuffle(dc0.Deck);

            kg0 = new(dispositionss[r.Next(0, dispositionss.Length)], F, M, K, C);
            ia = new(ianum, kg0, ch0, dc0, new Cementery(), new AField(), rl, Life);
            kg = new(disposition, F, M, K, C);
            p1 = new(kg, ch, dc, new Cementery(), new AField(), rl, Life);

            ia.CardsInHand.DrawingCards(ia.DeckOfCards, rl.CardsInHandAtStart);
        }

        public static DeckOfCards MakeRandomDeck()
        {
            List<int> indexesofdeck = new();

            System.Random r = new();

            float lim = (MaxCD / 2);
            lim -= 2;
            int res = (int)(Math.Round(lim));

            for (int k = 0; k < res; k++)
            {
                int ind = r.Next(0, database.Count);

                indexesofdeck.Add(ind);
                indexesofdeck.Add(ind);
            }

            ACard[] cardstodeck = new ACard[((int)MaxCD)];

            for (int k = 0; k < MaxCD - 4; k++)
            {
                if (k >= indexesofdeck.Count)
                {
                    break;
                }

                ACard card = database[indexesofdeck[k]];

                if (card.CardTypeNum == 1)
                {
                    card.EvolutionLevel = 1;
                }

                cardstodeck[k] = database[indexesofdeck[k]];
            }

            cardstodeck[cardstodeck.Length - 4] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(19, 2)),
            new Description("Tower of Guns", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Militarism en 200% y el resto en 100%.",
            new AType(2), new Race(1), 500, 500, 1000, 500)));

            cardstodeck[cardstodeck.Length - 3] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(20, 2)),
           new Description("Tower of Salomon", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Knowledge en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            cardstodeck[cardstodeck.Length - 2] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(21, 2)),
           new Description("Tower of God", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Faith en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            cardstodeck[cardstodeck.Length - 1] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(22, 2)),
           new Description("Tower of Money", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Capital en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            if (cardstodeck.ToList().Count < MaxCD)
            {
                for (int j = cardstodeck.ToList().Count; j < MaxCD; j++)
                {
                    cardstodeck[j] = database[50];
                }
            }


            DeckOfCards deck = new(cardstodeck);
            return deck;
        }

        static void TurnInvoke()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Checking empty spaces in field...");
            //Thread.Sleep(2000);

            bool natural = p1.AField.AreNaturalCardsInvokable();
            bool magic = p1.AField.AreMagicalCardsInvokable();

            if (!natural && !magic)
            {
                System.Console.WriteLine("You can't invoke any type of card!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

            else if (!natural)
            {
                System.Console.WriteLine("Keep in mind that you can only invoke magical cards...");
                System.Console.WriteLine();
                //Thread.Sleep(2000);
            }

            else if (!magic)
            {
                System.Console.WriteLine("Keep in mind that you can only invoke natural cards...");
                System.Console.WriteLine();
                //Thread.Sleep(2000);
            }

            System.Console.WriteLine("Searching cards to invoke... ");
            //Thread.Sleep(2000);
            System.Console.WriteLine();

            int avalibleoptions = p1.CardsInHand.Hand.Length;

            for (int j = 0; j < avalibleoptions; j++)
            {
                System.Console.WriteLine("({0}){1}", j, p1.CardsInHand.Hand[j].CardName);
            }

            System.Console.WriteLine("(B)ack");

            if (avalibleoptions == 0)
            {
                System.Console.WriteLine("You don't have cards in hand!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

            ConsoleKey kii3 = Console.ReadKey(true).Key;

            switch (kii3)
            {
                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                        break;
                    }

                case ConsoleKey.D0:
                    {
                        ACard card = p1.CardsInHand.Hand[0];

                        if (card.CardTypeNum == 1 && !natural)
                        {
                            System.Console.WriteLine("You don't have any free natural slots to do that!");
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();
                        }

                        else if (card.CardTypeNum == 2 && !magic)
                        {
                            System.Console.WriteLine("You don't have any free magic slots to do that!");
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();
                        }

                        InvokeACard(card);
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                        break;
                    }

                case ConsoleKey.D1:
                    {
                        if (avalibleoptions >= 2)
                        {
                            ACard card = p1.CardsInHand.Hand[1];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }



                        break;
                    }

                case ConsoleKey.D2:
                    {
                        if (avalibleoptions >= 3)
                        {
                            ACard card = p1.CardsInHand.Hand[2];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }


                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        else
                        {
                            System.Console.WriteLine("The int of avalible options doesn't work");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();

                        }

                        break;
                    }

                case ConsoleKey.D3:
                    {
                        if (avalibleoptions >= 4)
                        {
                            ACard card = p1.CardsInHand.Hand[3];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        else
                        {
                            System.Console.WriteLine("The int of avalible options doesn't work");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();

                        }

                        break;
                    }

                case ConsoleKey.D4:
                    {
                        if (avalibleoptions >= 5)
                        {
                            ACard card = p1.CardsInHand.Hand[4];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }



                        break;
                    }

                case ConsoleKey.D5:
                    {
                        if (avalibleoptions >= 6)
                        {
                            ACard card = p1.CardsInHand.Hand[5];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }


                        break;
                    }

                case ConsoleKey.D6:
                    {
                        if (avalibleoptions >= 7)
                        {
                            ACard card = p1.CardsInHand.Hand[6];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }



                        break;
                    }

                case ConsoleKey.D7:
                    {
                        if (avalibleoptions >= 8)
                        {
                            ACard card = p1.CardsInHand.Hand[7];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        else
                        {
                            System.Console.WriteLine("The int of avalible options doesn't work");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();

                        }

                        break;
                    }

                case ConsoleKey.D8:
                    {
                        if (avalibleoptions > +9)
                        {
                            ACard card = p1.CardsInHand.Hand[8];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }


                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        else
                        {
                            System.Console.WriteLine("The int of avalible options doesn't work");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();

                        }

                        break;
                    }

                case ConsoleKey.D9:
                    {
                        if (avalibleoptions >= 10)
                        {
                            ACard card = p1.CardsInHand.Hand[9];

                            if (card.CardTypeNum == 1 && !natural)
                            {
                                System.Console.WriteLine("You don't have any free natural slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }

                            else if (card.CardTypeNum == 2 && !magic)
                            {
                                System.Console.WriteLine("You don't have any free magic slots to do that!");
                                //Thread.Sleep(2000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnInvoke();
                            }


                            InvokeACard(card);
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        else
                        {
                            System.Console.WriteLine("The int of avalible options doesn't work");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnInvoke();

                        }

                        break;
                    }
            }
        }

        static void InvokeACard(ACard card)
        {
            System.Console.WriteLine("You choosed " + card.CardName + "." + "\nLet's check if you can invoke it...");
            //Thread.Sleep(2000);

            if (p1.CanPay(card))
            {
                if (p1.AField.IsInField(card) && card.CardTypeNum == 1)
                {
                    System.Console.WriteLine("You already as the same card in field. Want to evolutione it? \n(1)Yes     (2)No");

                    ConsoleKey kii2 = Console.ReadKey(true).Key;
                    switch (kii2)
                    {
                        case ConsoleKey.D1:
                            {
                                System.Console.WriteLine("Evolutioning " + card.CardName + "...");
                                //Thread.Sleep(2000);
                                p1 = p1.AField.Invocation(card, p1.AField.Find(card), p1);
                                System.Console.WriteLine("Suceed! \n" + card.CardName + " evolved to " + card.EvolvedNames.evolname1 + " " + card.CardName + "!");
                                //Thread.Sleep(1500);
                                System.Console.WriteLine("\nHis race evolved to " + card.RaceEvolvedNames.evolname1 + " and gained a powerful new effect!");
                                //Thread.Sleep(3500);
                                System.Console.WriteLine("\nA copy of the evolved card " + card.EvolvedNames.evolname1 + " was added to the deck!");
                                //Thread.Sleep(3500);
                                break;
                            }

                        case ConsoleKey.D2:
                            {
                                System.Console.WriteLine("Invoking " + card.CardName + "...");
                                //Thread.Sleep(2000);
                                p1 = p1.AField.Invocation(card, p1.AField.FindEmpty(1), p1);
                                break;
                            }

                        default:
                            break;

                    }
                }

                else
                {
                    if (card.CardTypeNum == 1)
                    {
                        System.Console.WriteLine("Invoking " + card.CardName + "...");
                        //Thread.Sleep(2000);

                        p1 = p1.AField.Invocation(card, p1.AField.FindEmpty(1), p1);
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                    }

                    else
                    {
                        System.Console.WriteLine("You are trying to invoke a magic card...");
                        //Thread.Sleep(2000);

                        if (ia.AField.IsEmpty() && p1.AField.IsEmpty())
                        {
                            System.Console.WriteLine("But there isn't a single card in both fields!");
                        }

                        System.Console.WriteLine("\nIf this card isn't a card that stays in field and in the actual state is effect is useless will be a waste...");
                        //Thread.Sleep(2000);
                        System.Console.WriteLine("\nYou sure you want to invoke it now? \n(0)Yes    (1)No!");
                        System.Console.WriteLine();
                        ConsoleKey ky = Console.ReadKey(true).Key;

                        switch (ky)
                        {
                            case ConsoleKey.D0:
                                {
                                    System.Console.WriteLine("Invoking " + card.CardName + "...");
                                    //Thread.Sleep(2000);
                                    Console.Clear();
                                    PrintAStateOfGame();
                                    MagicInvocation(card);
                                    break;
                                }

                            case ConsoleKey.D1:
                                {
                                    Console.Clear();
                                    PrintAStateOfGame();
                                    TurnInvoke();
                                    break;
                                }
                        }


                    }

                }

            }

            else
            {
                System.Console.WriteLine("You can't pay for that card!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnInvoke();
            }


        }

        static void MagicInvocation(ACard card)
        {
            System.Console.WriteLine();

            if (DoingEffect.IsAMagicTowerCard(card.EffectTxt1.name))
            {
                System.Console.WriteLine("You invoked a Magical Tower. Point gain per turn will growth!");
                p1.AField.InvocationSpecial(card, p1.AField.FindEmpty(2), p1);
                card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, card, 3, p1);
                //Thread.Sleep(3000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

            else
            {
                System.Console.WriteLine("You invoked the magic card " + card.CardName + ". \nYou will use it in the IA or in yourself? \n (0)Yourself.    (1)IA.");
            }


            YourselfOrIAChoose(card, 0);

        }

        public static void TurnActions()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Actions: (1)Invoke to field, (2)Attack, (3)Use Effect, (4)See Cards In Cementery, (5)EndTurn, (6)Surrender");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.Enter:
                    {
                        TurnInvoke();
                        break;
                    }

                case ConsoleKey.D2:
                    {
                        TurnAttack();
                        break;
                    }

                case ConsoleKey.D3:
                    {
                        TurnEffect();
                        break;
                    }

                case ConsoleKey.D4:
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("What cementery you want to see? \n(0)IA's    (2)Your's");

                        ConsoleKey ku = Console.ReadKey(true).Key;

                        switch (ku)
                        {
                            case ConsoleKey.B:
                                {
                                    TurnActions();
                                    break;
                                }

                            case ConsoleKey.D0:
                                {
                                    PrintIACementery();
                                    break;
                                }

                            case ConsoleKey.D1:
                                {
                                    PrintYourCementery();
                                    break;
                                }
                        }

                        break;
                    }

                case ConsoleKey.D5:
                    {
                        ia.IsHisTurn = true;
                        p1.IsHisTurn = false;
                        TurnChange();
                        break;
                    }

                case ConsoleKey.D6:
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("Are you sure you want to surrender? \n(0)Yes    (1)No!");
                        ConsoleKey check = Console.ReadKey(true).Key;

                        switch (check)
                        {
                            case ConsoleKey.D0:
                                {
                                    ResetValues();
                                    Console.Clear();
                                    PrintMenu();
                                    break;
                                }

                            case ConsoleKey.D1:
                                {
                                    Console.Clear();
                                    PrintAStateOfGame();
                                    TurnActions();
                                    break;
                                }

                            default:
                                break;
                        }

                        break;
                    }

                default:
                    break;
            }
        }

        public static void TurnAttack()
        {
            CanAttack();

            int avalibleoptions = ia.AField.AmountOfNaturalCardInvoked();
            List<ACard> enemies = ia.AField.GetNaturalCardsInField();
            List<ACard> cards = p1.AField.GetNaturalCardsInField();
            int cardsinfield = p1.AField.AmountOfNaturalCardInvoked();


            System.Console.WriteLine();
            System.Console.WriteLine("Choose what card will attack: ");

            GetCardThatDoSomething(cardsinfield, cards);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                        break;
                    }

                case ConsoleKey.D0:
                case ConsoleKey.Enter:
                    {
                        if (cardsinfield >= 1)
                        {
                            DoAttack(avalibleoptions, cardsinfield, enemies, cards[0], cards);
                        }

                        break;
                    }

                case ConsoleKey.D1:
                    {
                        if (cardsinfield >= 2)
                        {
                            DoAttack(avalibleoptions, cardsinfield, enemies, cards[1], cards);
                        }

                        break;
                    }

                case ConsoleKey.D2:
                    {
                        if (cardsinfield >= 3)
                        {
                            DoAttack(avalibleoptions, cardsinfield, enemies, cards[2], cards);
                        }

                        break;
                    }

                case ConsoleKey.D3:
                    {
                        if (cardsinfield >= 4)
                        {
                            DoAttack(avalibleoptions, cardsinfield, enemies, cards[3], cards);
                        }

                        break;
                    }


                case ConsoleKey.D4:
                    {
                        if (cardsinfield >= 5)
                        {
                            DoAttack(avalibleoptions, cardsinfield, enemies, cards[4], cards);
                        }

                        break;
                    }


                default:
                    break;
            }


        }

        public static void CanAttack()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Checking cards in IA's field...");
            //Thread.Sleep(2000);

            if (ia.AField.IsEmpty())
            {
                System.Console.WriteLine("There aren't enemies in IA's field!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Checking cards in your field...");
            //Thread.Sleep(2000);

            if (p1.AField.IsEmpty())
            {
                System.Console.WriteLine("There aren't allies in your field!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }
        }

        public static void DoAttack(int avalibleoptions, int cardsinfield, List<ACard> enemies, ACard card, List<ACard> cards)
        {

            System.Console.WriteLine();
            System.Console.WriteLine("You choosed {0} that as a base ATK of {1}.", card.CardName, card.ATK);
            System.Console.WriteLine("Checking card state...");
            //Thread.Sleep(2000);

            if (card.ActivateEffectOrAttackOnce)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("This card as already attacked or used an effect in this turn!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnAttack();
            }

            if (!card.CanAttack)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("This card as attack restricted for {0} more turns!", cards[0].RestrictAttackTurns);
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnAttack();
            }

            if (!card.CanSelectTarget)
            {
                System.Console.WriteLine("This card can't select target, is confused!");
                System.Console.WriteLine();
                System.Console.WriteLine("Choosing random target...");
                //Thread.Sleep(3000);

                Random r = new();

                if (r.Next(0, 100) > 50)
                {

                    if (cardsinfield >= 2)
                    {
                        ACard tgt = cards[r.Next(0, cards.Count)];
                        System.Console.WriteLine("Due to confusion, attack ally " + tgt.CardName + ". \nDealed " + cards[0].ATK + " of damage!");
                        p1 = card.EffectsObject.NormalAttack.Invoke(p1, card.ListOfCostFKCM, card, p1, new ACard[] { tgt })[0];
                        //Thread.Sleep(2000);
                        card.ActivateEffectOrAttackOnce = true;
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnAttack();
                    }

                    else
                    {
                        System.Console.WriteLine("Due to confusion, " + card.CardName + " hurts herself. \nDealed " + cards[0].ATK + " of damage!");
                        p1 = card.EffectsObject.NormalAttack.Invoke(p1, card.ListOfCostFKCM, card, p1, new ACard[] { card })[0];
                        card.ActivateEffectOrAttackOnce = true;
                        //Thread.Sleep(2000);
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnAttack();
                    }


                }

                else
                {

                    ACard tgt = enemies[r.Next(0, enemies.Count)];
                    System.Console.WriteLine("Even in a confusion state, managed to attack enemy " + tgt.CardName + ". \nDealed " + cards[0].ATK + " of damage!");
                    card.EffectsObject.NormalAttack.Invoke(p1, card.ListOfCostFKCM, card, ia, new ACard[] { tgt });
                    card.ActivateEffectOrAttackOnce = true;
                    //Thread.Sleep(2000);
                    Console.Clear();
                    PrintAStateOfGame();
                    TurnAttack();


                }


            }

            System.Console.WriteLine("Card is good and ready so far to attack. Choose target: ");
            for (int j = 0; j < enemies.Count; j++)
            {
                System.Console.WriteLine("({0}){1}", j, enemies[j].CardName);
            }

            System.Console.WriteLine("(B)ack");

            ConsoleKey key0 = Console.ReadKey(true).Key;

            switch (key0)
            {
                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                        break;
                    }

                case ConsoleKey.D0:
                    {
                        if (avalibleoptions >= 1)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + enemies[0].CardName + ".");
                            System.Console.WriteLine(card.CardName + " attacks " + enemies[0].CardName +
                            ". Dealed " + card.ATK + "of base damage plus the elemental damage variation!");
                            card.EffectsObject.NormalAttack.Invoke(p1, (new double[] { 0, 0, 0, 0 }).ToList(), card, ia, new ACard[] { enemies[0] });
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        break;
                    }


                case ConsoleKey.D1:
                    {
                        if (avalibleoptions >= 2)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + enemies[1].CardName + ".");
                            System.Console.WriteLine(card.CardName + " attacks " + enemies[1].CardName +
                            ". Dealed " + card.ATK + "of base damage plus the elemental damage variation!");
                            card.EffectsObject.NormalAttack.Invoke(p1, (new double[] { 0, 0, 0, 0 }).ToList(), card, ia, new ACard[] { enemies[1] });
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        break;
                    }

                case ConsoleKey.D2:
                    {
                        if (avalibleoptions >= 3)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + enemies[2].CardName + ".");
                            System.Console.WriteLine(card.CardName + " attacks " + enemies[2].CardName +
                            ". Dealed " + card.ATK + "of base damage plus the elemental damage variation!");
                            card.EffectsObject.NormalAttack.Invoke(p1, (new double[] { 0, 0, 0, 0 }).ToList(), card, ia, new ACard[] { enemies[2] });
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        break;
                    }

                case ConsoleKey.D3:
                    {
                        if (avalibleoptions >= 4)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + enemies[3].CardName + ".");
                            System.Console.WriteLine(card.CardName + " attacks " + enemies[3].CardName +
                            ". Dealed " + card.ATK + "of base damage plus the elemental damage variation!");
                            card.EffectsObject.NormalAttack.Invoke(p1, (new double[] { 0, 0, 0, 0 }).ToList(), card, ia, new ACard[] { enemies[3] });
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        break;
                    }


                case ConsoleKey.D4:
                    {
                        if (avalibleoptions >= 5)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + enemies[4].CardName + ".");
                            System.Console.WriteLine(card.CardName + " attacks " + enemies[4].CardName +
                            ". Dealed " + card.ATK + "of base damage plus the elemental damage variation!");
                            card.EffectsObject.NormalAttack.Invoke(p1, (new double[] { 0, 0, 0, 0 }).ToList(), card, ia, new ACard[] { enemies[4] });
                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                        }

                        break;
                    }

                default:
                    break;
            }


        }

        public static void TurnEffect()
        {
            CanUseEffect();

            int avalibleoptions = ia.AField.AmountOfNaturalCardInvoked();
            List<ACard> enemies = ia.AField.GetNaturalCardsInField();
            List<ACard> cards = p1.AField.GetNaturalCardsInField();
            int cardsinfield = p1.AField.AmountOfNaturalCardInvoked();

            System.Console.WriteLine();
            System.Console.WriteLine("Choose what card will use effect: ");

            GetCardThatDoSomething(cardsinfield, cards);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnActions();
                        break;
                    }

                case ConsoleKey.D0:
                case ConsoleKey.Enter:
                    {
                        if (cardsinfield >= 1)
                        {
                            DoEffect(avalibleoptions, cardsinfield, enemies, cards[0], cards);
                        }

                        break;
                    }

                case ConsoleKey.D1:
                    {
                        if (cardsinfield >= 2)
                        {
                            DoEffect(avalibleoptions, cardsinfield, enemies, cards[1], cards);
                        }

                        break;
                    }

                case ConsoleKey.D2:
                    {
                        if (cardsinfield >= 3)
                        {
                            DoEffect(avalibleoptions, cardsinfield, enemies, cards[2], cards);
                        }

                        break;
                    }

                case ConsoleKey.D3:
                    {
                        if (cardsinfield >= 4)
                        {
                            DoEffect(avalibleoptions, cardsinfield, enemies, cards[3], cards);
                        }

                        break;
                    }


                case ConsoleKey.D4:
                    {
                        if (cardsinfield >= 5)
                        {
                            DoEffect(avalibleoptions, cardsinfield, enemies, cards[4], cards);
                        }

                        break;
                    }


                default:
                    break;
            }
        }

        public static void CanUseEffect()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Checking cards in both fields...");
            //Thread.Sleep(2000);

            bool natural = ia.AField.AreNaturalCardInvoked();

            if (!natural && p1.AField.IsEmpty())
            {
                System.Console.WriteLine("There aren't enemies or allies in both fields!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

        }

        public static void GetCardThatDoSomething(int cardsinfield, List<ACard> cards)
        {
            for (int b = 0; b < cardsinfield; b++)
            {
                System.Console.WriteLine("({0}){1}", b, cards[b].CardName);
            }

            System.Console.WriteLine("(B)ack");
        }

        public static void DoingTargetEffect(ACard card, List<ACard> cardsinfield, int efctnum, Player p10, bool player)
        {
            for (int j = 0; j < cardsinfield.Count; j++)
            {
                System.Console.WriteLine("({0}){1}", j, cardsinfield[j].CardName);
            }

            System.Console.WriteLine("(T)arget is in cementery.");
            System.Console.WriteLine("(D)on't need a target");
            System.Console.WriteLine("(B)ack");

            ConsoleKey ko = Console.ReadKey(true).Key;

            if (card.CardTypeNum == 2)
            {
                switch (ko)
                {
                    case ConsoleKey.B:
                        {
                            YourselfOrIAChoose(card, efctnum);
                            break;
                        }

                    case ConsoleKey.T:
                        {
                            if (player)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("Checking cards in your cementery...");
                                System.Console.WriteLine();
                                //Thread.Sleep(2000);



                                TargetInCementery(card, cardsinfield, efctnum, p1, player);
                            }

                            else
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("Checking cards in IA's cementery...");
                                System.Console.WriteLine();
                                //Thread.Sleep(2000);

                                TargetInCementery(card, cardsinfield, efctnum, ia, player);
                            }

                            break;
                        }

                    case ConsoleKey.D:
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You don't choose a target. Let's hope it work. \nActivating magic effect...");

                            //si el efecto es una torre
                            if (DoingEffect.IsAMagicTowerCard(card.EffectTxt1.name))
                            {
                                //entonces le hago una invocación especial en el campo y aumento el índice
                                Player lonly = p1.AField.InvocationSpecial(card, p1.AField.FindEmpty(2), p1);
                                p1 = lonly;
                            }

                            else
                            {
                                Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, card, 3, p10);

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            //Thread.Sleep(2000);
                            System.Console.WriteLine("Done!");
                            //Thread.Sleep(3000);
                            System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                            //Thread.Sleep(3000);



                            card.ActivateEffectOrAttackOnce = true;

                            if (p1.AField.IsInField(card))
                            {
                                p1.AField.SetCard(card, p1.AField.Find(card));
                            }

                            if (p1.CardsInHand.ContainCard(card))
                            {
                                p1.CardsInHand.RemoveFromHand(card);
                                p1.Cementery.ToCementery(card);
                            }

                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                            break;
                        }

                    case ConsoleKey.D0:
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + cardsinfield[0].CardName + ". \nActivating magic effect on it...");
                            Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[0], 3, p10);
                            //Thread.Sleep(2000);
                            System.Console.WriteLine("Done!");

                            if (p1.CheckingRestrictedStates(cardsinfield[0]) == "")
                            {
                                System.Console.WriteLine(cardsinfield[0].CardName + " don't have any restriction!");
                            }

                            else
                            {
                                System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[0]));
                            }

                            //Thread.Sleep(3000);
                            System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                            //Thread.Sleep(3000);

                            if (player)
                            {
                                p1 = both[0];
                            }

                            else
                            {
                                p1 = both[0];
                                ia = (IA)(both[1]);
                            }

                            card.ActivateEffectOrAttackOnce = true;

                            if (p1.AField.IsInField(card))
                            {
                                p1.AField.SetCard(card, p1.AField.Find(card));
                            }

                            if (p1.CardsInHand.ContainCard(card))
                            {
                                p1.CardsInHand.RemoveFromHand(card);
                                p1.Cementery.ToCementery(card);
                            }


                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                            break;
                        }

                    case ConsoleKey.D1:
                        {
                            if (cardsinfield.Count >= 2)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[1].CardName + ". \nActivating magic effect on it...");
                                Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[1], 3, p10);
                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");

                                if (p1.CheckingRestrictedStates(cardsinfield[1]) == "")
                                {
                                    System.Console.WriteLine(cardsinfield[1].CardName + " don't have any restriction!");
                                }

                                else
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[1]));
                                }

                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                if (p1.CardsInHand.ContainCard(card))
                                {
                                    p1.CardsInHand.RemoveFromHand(card);
                                    p1.Cementery.ToCementery(card);
                                }

                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D2:
                        {
                            if (cardsinfield.Count >= 3)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[2].CardName + ". \nActivating magic effect on it...");
                                Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[2], 3, p10);
                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done! ");

                                if (p1.CheckingRestrictedStates(cardsinfield[2]) == "")
                                {
                                    System.Console.WriteLine(cardsinfield[2].CardName + " don't have any restriction!");
                                }

                                else
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[2]));
                                }

                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                if (p1.CardsInHand.ContainCard(card))
                                {
                                    p1.CardsInHand.RemoveFromHand(card);
                                    p1.Cementery.ToCementery(card);
                                }


                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D3:
                        {
                            if (cardsinfield.Count >= 4)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[3].CardName + ". \nActivating magic effect on it...");
                                Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[3], 3, p10);
                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");

                                if (p1.CheckingRestrictedStates(cardsinfield[3]) == "")
                                {
                                    System.Console.WriteLine(cardsinfield[3].CardName + " don't have any restriction!");
                                }

                                else
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[3]));
                                }

                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                if (p1.CardsInHand.ContainCard(card))
                                {
                                    p1.CardsInHand.RemoveFromHand(card);
                                    p1.Cementery.ToCementery(card);
                                }


                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D4:
                        {
                            if (cardsinfield.Count >= 5)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[4].CardName + ". \nActivating magic effect on it...");
                                Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[4], 3, p10);
                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");

                                if (p1.CheckingRestrictedStates(cardsinfield[4]) == "")
                                {
                                    System.Console.WriteLine(cardsinfield[4].CardName + " don't have any restriction!");
                                }

                                else
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[4]));
                                }

                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                if (p1.CardsInHand.ContainCard(card))
                                {
                                    p1.CardsInHand.RemoveFromHand(card);
                                    p1.Cementery.ToCementery(card);
                                }


                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;

                        }
                }

            }

            else
            {

                if (!card.CanSelectTarget)
                {
                    List<ACard> cards = p1.AField.GetNaturalCardsInField();
                    List<ACard> enemies = ia.AField.GetNaturalCardsInField();
                    System.Console.WriteLine();
                    System.Console.WriteLine("This card can't select target, is confused!");
                    System.Console.WriteLine();
                    System.Console.WriteLine("Choosing random target...");
                    //Thread.Sleep(3000);

                    Random r = new();

                    if (r.Next(0, 100) > 50)
                    {
                        if (cardsinfield.Count >= 2)
                        {
                            ACard tgt = cards[r.Next(0, cards.Count)];
                            System.Console.WriteLine("Due to confusion, used effect on ally " + tgt.CardName + ".");

                            if (efctnum == 0)
                            {
                                if (!card.IsACardCreatedByUser)
                                {
                                    Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, tgt, 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else
                                {
                                    Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { tgt }, p10, card.OEObject);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }


                                if (p1.CheckingRestrictedStates(tgt) == "")
                                {
                                    System.Console.WriteLine(tgt.CardName + " don't have any restriction!");
                                }

                                else
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(tgt));
                                }

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }

                            }

                            else if (efctnum == 1)
                            {
                                if (!card.IsACardCreatedByUser)
                                {
                                    Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, tgt, 3, p10);

                                    if (p10.AField.IsInField(tgt))
                                    {
                                        System.Console.WriteLine(tgt.CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(tgt.CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                            }

                            else if (efctnum == 2)
                            {
                                Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { tgt }, 3, p10);

                                if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(tgt) == "")
                                {
                                    System.Console.WriteLine(tgt.CardName + " don't have any restriction!");
                                }

                                else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(tgt) != "")
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(tgt));
                                }

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;
                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 3)
                            {
                                Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { tgt }, 3, p10);

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;
                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnAttack();
                        }

                        else
                        {
                            System.Console.WriteLine("Due to confusion, " + card.CardName + " activate effect on herself.");

                            if (efctnum == 0)
                            {
                                if (!card.IsACardCreatedByUser)
                                {
                                    Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, card, 3, p10);
                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }

                                }

                                else
                                {
                                    Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { card }, p10, card.OEObject);
                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }


                                if (p10.AField.IsInField(card))
                                {
                                    System.Console.WriteLine(card.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(card.CardName + " is still alive!");
                                }


                            }

                            else if (efctnum == 1)
                            {
                                Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, card, 3, p10);

                                if (p10.AField.IsInField(card))
                                {
                                    System.Console.WriteLine(card.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(card.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 2)
                            {
                                Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { card }, 3, p10);

                                if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(card) == "")
                                {
                                    System.Console.WriteLine(card.CardName + " don't have any restriction!");
                                }

                                else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(card) != "")
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(card));
                                }

                                if (p10.AField.IsInField(card))
                                {
                                    System.Console.WriteLine(card.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(card.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 3)
                            {
                                Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { card }, 3, p10);

                                if (p10.AField.IsInField(card))
                                {
                                    System.Console.WriteLine(card.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(card.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }

                            }



                            //Thread.Sleep(2000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnAttack();
                        }

                    }

                    else
                    {
                        if (enemies.Count >= 1)
                        {
                            ACard tgt = enemies[r.Next(0, enemies.Count)];
                            System.Console.WriteLine("Even in a confusion state," + card.CardName + " managed to attack enemy " + tgt.CardName + ". \nDealed " + cards[0].ATK + " of damage!");

                            if (efctnum == 0)
                            {
                                if (!card.IsACardCreatedByUser)
                                {
                                    Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, tgt, 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else
                                {
                                    Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { tgt }, p10, card.OEObject);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }


                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }


                            }

                            else if (efctnum == 1)
                            {
                                Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, tgt, 3, p10);

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 2)
                            {
                                Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { tgt }, 3, p10);

                                if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(tgt) == "")
                                {
                                    System.Console.WriteLine(tgt.CardName + " don't have any restriction!");
                                }

                                else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(tgt) != "")
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(tgt));
                                }

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 3)
                            {
                                Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { tgt }, 3, p10);

                                if (p10.AField.IsInField(tgt))
                                {
                                    System.Console.WriteLine(tgt.CardName + "  is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(tgt.CardName + "  is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                        }

                        else
                        {
                            System.Console.WriteLine("The effect was throw in an empty enemy field... such a waste.");

                            if (efctnum == 0)
                            {
                                p1.Kingdom.Faith -= card.ListCostsFKCMEffect1[0];
                                p1.Kingdom.Knowledge -= card.ListCostsFKCMEffect1[1];
                                p1.Kingdom.Capital -= card.ListCostsFKCMEffect1[2];
                                p1.Kingdom.Militarism -= card.ListCostsFKCMEffect1[3];
                            }

                            else if (efctnum == 1)
                            {
                                p1.Kingdom.Faith -= card.ListCostsFKCMEffect2[0];
                                p1.Kingdom.Knowledge -= card.ListCostsFKCMEffect2[1];
                                p1.Kingdom.Capital -= card.ListCostsFKCMEffect2[2];
                                p1.Kingdom.Militarism -= card.ListCostsFKCMEffect2[3];
                            }

                            else if (efctnum == 2)
                            {
                                p1.Kingdom.Faith -= card.ListCostsFKCMEffect3[0];
                                p1.Kingdom.Knowledge -= card.ListCostsFKCMEffect3[1];
                                p1.Kingdom.Capital -= card.ListCostsFKCMEffect3[2];
                                p1.Kingdom.Militarism -= card.ListCostsFKCMEffect3[3];
                            }

                            else if (efctnum == 3)
                            {
                                p1.Kingdom.Faith -= card.ListCostsFKCMEffect4[0];
                                p1.Kingdom.Knowledge -= card.ListCostsFKCMEffect4[1];
                                p1.Kingdom.Capital -= card.ListCostsFKCMEffect4[2];
                                p1.Kingdom.Militarism -= card.ListCostsFKCMEffect4[3];
                            }

                        }

                        card.ActivateEffectOrAttackOnce = true;

                        if (p1.AField.IsInField(card))
                        {
                            p1.AField.SetCard(card, p1.AField.Find(card));
                        }

                        //Thread.Sleep(2000);
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnAttack();
                    }


                }

                switch (ko)
                {
                    case ConsoleKey.D0:
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You choosed " + cardsinfield[0].CardName + ". \nActivating card effect on it...");


                            if (efctnum == 0)
                            {
                                if (!card.IsACardCreatedByUser)
                                {
                                    Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[0], 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else
                                {
                                    Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { cardsinfield[0] }, p10, card.OEObject);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                if (p10.AField.IsInField(cardsinfield[0]))
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is still alive!");
                                }

                            }

                            else if (efctnum == 1)
                            {
                                Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, cardsinfield[0], 3, p10);

                                if (p10.AField.IsInField(cardsinfield[0]))
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 2)
                            {
                                Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { cardsinfield[0] }, 3, p10);

                                if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[0]) == "")
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " don't have any restriction!");
                                }

                                else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[0]) != "")
                                {
                                    System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[0]));
                                }

                                if (p10.AField.IsInField(cardsinfield[0]))
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is still alive!");
                                }

                                card.ActivateEffectOrAttackOnce = true;

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            else if (efctnum == 3)
                            {
                                Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { cardsinfield[0] }, 3, p10);

                                card.ActivateEffectOrAttackOnce = true;

                                if (p10.AField.IsInField(cardsinfield[0]))
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is dead!");
                                }

                                else
                                {
                                    System.Console.WriteLine(cardsinfield[0].CardName + " is still alive!");
                                }

                                if (player)
                                {
                                    p1 = both[0];
                                }

                                else
                                {
                                    p1 = both[0];
                                    ia = (IA)(both[1]);
                                }
                            }

                            if (p1.AField.IsInField(card))
                            {
                                p1.AField.SetCard(card, p1.AField.Find(card));
                            }

                            //Thread.Sleep(2000);
                            System.Console.WriteLine("Done!");
                            //Thread.Sleep(3000);
                            System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            PrintAStateOfGame();
                            TurnActions();
                            break;
                        }

                    case ConsoleKey.D1:
                        {
                            if (cardsinfield.Count >= 2)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[1].CardName + ". \nActivating card effect on it...");

                                if (efctnum == 0)
                                {
                                    if (!card.IsACardCreatedByUser)
                                    {
                                        Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[1], 3, p10);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    else
                                    {
                                        Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { cardsinfield[1] }, p10, card.OEObject);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    if (p10.AField.IsInField(cardsinfield[1]))
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is still alive!");
                                    }
                                }

                                else if (efctnum == 1)
                                {
                                    Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, cardsinfield[1], 3, p10);

                                    if (p10.AField.IsInField(cardsinfield[1]))
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 2)
                                {
                                    Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { cardsinfield[1] }, 3, p10);

                                    if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[1]) == "")
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " don't have any restriction!");
                                    }

                                    else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[1]) != "")
                                    {
                                        System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[1]));
                                    }

                                    if (p10.AField.IsInField(cardsinfield[1]))
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 3)
                                {
                                    Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { cardsinfield[1] }, 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (p10.AField.IsInField(cardsinfield[1]))
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[1].CardName + " is still alive!");
                                    }

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");
                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D2:
                        {
                            if (cardsinfield.Count >= 3)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[2].CardName + ". \nActivating card effect on it...");

                                if (efctnum == 0)
                                {
                                    if (!card.IsACardCreatedByUser)
                                    {
                                        Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[2], 3, p10);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    else
                                    {
                                        Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { cardsinfield[2] }, p10, card.OEObject);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    if (p10.AField.IsInField(cardsinfield[2]))
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is still alive!");
                                    }
                                }

                                else if (efctnum == 1)
                                {
                                    Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, cardsinfield[2], 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (p10.AField.IsInField(cardsinfield[2]))
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is still alive!");
                                    }

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 2)
                                {
                                    Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { cardsinfield[2] }, 3, p10);

                                    if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[2]) == "")
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " don't have any restriction!");
                                    }

                                    else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[2]) != "")
                                    {
                                        System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[2]));
                                    }

                                    if (p10.AField.IsInField(cardsinfield[2]))
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 3)
                                {
                                    Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { cardsinfield[2] }, 3, p10);

                                    if (p10.AField.IsInField(cardsinfield[2]))
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[2].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");
                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D3:
                        {
                            if (cardsinfield.Count >= 4)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[3].CardName + ". \nActivating card effect on it...");

                                if (efctnum == 0)
                                {
                                    if (!card.IsACardCreatedByUser)
                                    {
                                        Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[3], 3, p10);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    else
                                    {
                                        Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { cardsinfield[3] }, p10, card.OEObject);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    if (p10.AField.IsInField(cardsinfield[3]))
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is still alive!");
                                    }
                                }

                                else if (efctnum == 1)
                                {
                                    Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, cardsinfield[3], 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (p10.AField.IsInField(cardsinfield[3]))
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is still alive!");
                                    }

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 2)
                                {
                                    Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { cardsinfield[3] }, 3, p10);

                                    if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[3]) == "")
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " don't have any restriction!");
                                    }

                                    else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[3]) != "")
                                    {
                                        System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[3]));
                                    }

                                    if (p10.AField.IsInField(cardsinfield[3]))
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 3)
                                {
                                    Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { cardsinfield[3] }, 3, p10);

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (p10.AField.IsInField(cardsinfield[3]))
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[3].CardName + " is still alive!");
                                    }

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");
                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;
                        }

                    case ConsoleKey.D4:
                        {
                            if (cardsinfield.Count >= 5)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("You choosed " + cardsinfield[4].CardName + ". \nActivating card effect on it...");

                                if (efctnum == 0)
                                {
                                    if (!card.IsACardCreatedByUser)
                                    {
                                        Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, cardsinfield[4], 3, p10);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    else
                                    {
                                        Player[] both = LPEffects.UseEffect(p1, card.ListCostsFKCMEffect1.ToList(), card, new ACard[] { cardsinfield[4] }, p10, card.OEObject);

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }
                                    }

                                    if (p10.AField.IsInField(cardsinfield[4]))
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is still alive!");
                                    }
                                }

                                else if (efctnum == 1)
                                {
                                    Player[] both = card.Effect2.Invoke(p1, card.ListCostsFKCMEffect2.ToList(), card, cardsinfield[4], 3, p10);

                                    if (p10.AField.IsInField(cardsinfield[4]))
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 2)
                                {
                                    Player[] both = card.Effect3.Invoke(p1, card.ListCostsFKCMEffect3.ToList(), card, new ACard[] { cardsinfield[4] }, 3, p10);

                                    if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[4]) == "")
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " don't have any restriction!");
                                    }

                                    else if (card.Effect3.GetType() == (new AreaEffect()).GetType() && p1.CheckingRestrictedStates(cardsinfield[4]) != "")
                                    {
                                        System.Console.WriteLine(p1.CheckingRestrictedStates(cardsinfield[4]));
                                    }

                                    if (p10.AField.IsInField(cardsinfield[4]))
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                else if (efctnum == 3)
                                {
                                    Player[] both = card.Effect4.Invoke(p1, card.ListCostsFKCMEffect4.ToList(), card, new ACard[] { cardsinfield[4] }, 3, p10);

                                    if (p10.AField.IsInField(cardsinfield[4]))
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is dead!");
                                    }

                                    else
                                    {
                                        System.Console.WriteLine(cardsinfield[4].CardName + " is still alive!");
                                    }

                                    card.ActivateEffectOrAttackOnce = true;

                                    if (player)
                                    {
                                        p1 = both[0];
                                    }

                                    else
                                    {
                                        p1 = both[0];
                                        ia = (IA)(both[1]);
                                    }
                                }

                                if (p1.AField.IsInField(card))
                                {
                                    p1.AField.SetCard(card, p1.AField.Find(card));
                                }

                                //Thread.Sleep(2000);
                                System.Console.WriteLine("Done!");
                                //Thread.Sleep(3000);
                                System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                //Thread.Sleep(3000);
                                Console.Clear();
                                PrintAStateOfGame();
                                TurnActions();
                            }

                            break;

                        }
                }

            }

        }

        public static void DoEffect(int avalibleoptions, int cardsinfield, List<ACard> enemies, ACard card, List<ACard> cards)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("You choosed {0}.", card.CardName);
            System.Console.WriteLine("Checking card state...");
            //Thread.Sleep(2000);

            if (card.ActivateEffectOrAttackOnce)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("This card as already attacked or used an effect in this turn!");
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnEffect();
            }

            if (!card.CanUseEffect)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("This card as effects restricted for {0} more turns!", cards[0].RestrictAttackTurns);
                //Thread.Sleep(2000);
                Console.Clear();
                PrintAStateOfGame();
                TurnEffect();
            }


            System.Console.WriteLine();
            System.Console.WriteLine("Choose what effect to use wisely: ");
            int amounteffc = 0;


            if (card.Effect1 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect1.ToList()) && !card.IsACardCreatedByUser)
            {

                System.Console.WriteLine("(0){0}", card.EffectTxt1.name);
                amounteffc++;

                if (card.Effect2 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect2.ToList()))
                {
                    System.Console.WriteLine("(1){0}", card.EffectTxt2.name);
                    amounteffc++;

                    if (card.Effect3 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect3.ToList()))
                    {
                        System.Console.WriteLine("(2){0}", card.EffectTxt3.name);
                        amounteffc++;

                        if (card.Effect4 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect4.ToList()))
                        {
                            System.Console.WriteLine("(3){0}", card.EffectTxt4.name);
                            amounteffc++;
                        }
                    }

                }

                else
                {
                    if (card.EvolutionLevel == 1)
                    {
                        System.Console.WriteLine("(0){0}", card.SpecialEffectTxt.name);
                        amounteffc = 1;
                    }

                    else if (card.EvolutionLevel == 2)
                    {
                        System.Console.WriteLine("(0){0}", card.SpecialEffectTxt.name);
                        amounteffc = 1;

                        if (card.Effect2 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect2.ToList()))
                        {
                            System.Console.WriteLine("(1){0}", card.EffectTxt2.name);
                            amounteffc++;

                            if (card.Effect3 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect3.ToList()))
                            {
                                System.Console.WriteLine("(2){0}", card.EffectTxt3.name);
                                amounteffc++;

                                if (card.Effect4 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect4.ToList()))
                                {
                                    System.Console.WriteLine("(3){0}", card.EffectTxt4.name);
                                    amounteffc++;
                                }
                            }
                        }
                    }

                    else
                    {
                        System.Console.WriteLine("(0){0}", card.SpecialEffectTxt.name);
                        amounteffc = 1;

                        if (card.Effect2 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect2.ToList()))
                        {
                            System.Console.WriteLine("(1){0}", card.EffectTxt2.name);
                            amounteffc++;

                            if (card.Effect3 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect3.ToList()))
                            {
                                System.Console.WriteLine("(2){0}", card.EffectTxt3.name);
                                amounteffc++;

                                if (card.Effect4 != null && p1.CanPayEffect(p1, card.ListCostsFKCMEffect4.ToList()))
                                {
                                    System.Console.WriteLine("(3){0}", card.EffectTxt4.name);
                                    amounteffc++;
                                }
                            }
                        }
                    }


                }

            }

            if (amounteffc == 0)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("You can't pay any of this card effects!");
                //Thread.Sleep(2500);
                Console.Clear();
                PrintAStateOfGame();
                TurnEffect();
            }


            ConsoleKey kip = Console.ReadKey(true).Key;

            switch (kip)
            {
                case ConsoleKey.D0:
                    {
                        if (amounteffc > 0)
                        {
                            ActivateEffect(avalibleoptions, cardsinfield, enemies, card, cards, 0);
                        }

                        break;
                    }

                case ConsoleKey.D1:
                    {
                        if (amounteffc >= 2)
                        {
                            ActivateEffect(avalibleoptions, cardsinfield, enemies, card, cards, 1);
                        }

                        break;
                    }

                case ConsoleKey.D2:
                    {
                        if (amounteffc >= 3)
                        {
                            ActivateEffect(avalibleoptions, cardsinfield, enemies, card, cards, 2);
                        }

                        break;
                    }

                case ConsoleKey.D3:
                    {
                        if (amounteffc >= 4)
                        {
                            ActivateEffect(avalibleoptions, cardsinfield, enemies, card, cards, 3);
                        }

                        break;
                    }

            }

        }

        public static void ActivateEffect(int avalibleoptions, int cardsinfield, List<ACard> enemies, ACard card, List<ACard> cards, int efctnum)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Choose wisely in who will use your effect being conscious of what the effect will do:"
            + "\n(0)Yourself    (1)IA   (B)ack.");

            YourselfOrIAChoose(card, efctnum);

        }

        public static void YourselfOrIAChoose(ACard card, int efctnum)
        {
            ConsoleKey ky = Console.ReadKey(true).Key;

            switch (ky)
            {

                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnEffect();
                        break;
                    }

                case ConsoleKey.D0:
                    {
                        System.Console.WriteLine("Checking avalible allies...");
                        //Thread.Sleep(2000);

                        if (p1.AField.IsEmpty())
                        {
                            System.Console.WriteLine("Remember: there aren't ally avalible cards...");
                            //Thread.Sleep(2000);

                            List<ACard> empty = new();
                            DoingTargetEffect(card, empty, efctnum, p1, true);
                        }

                        else
                        {
                            System.Console.WriteLine("Here are all possibles targets. Choose wisely: ");

                            List<ACard> cardsinfield = p1.AField.GetNaturalCardsInField();
                            DoingTargetEffect(card, cardsinfield, efctnum, p1, true);
                        }

                        break;
                    }

                case ConsoleKey.D1:
                    {
                        System.Console.WriteLine("Checking avalible enemies...");
                        //Thread.Sleep(2000);

                        if (ia.AField.IsEmpty())
                        {
                            System.Console.WriteLine("Remember: There aren't enemy avalible cards...");
                            //Thread.Sleep(2000);

                            List<ACard> empty = new();
                            DoingTargetEffect(card, empty, efctnum, p1, true);
                        }

                        else
                        {
                            System.Console.WriteLine("Here are all possibles targets. Choose wisely: ");

                            List<ACard> cardsinfield = ia.AField.GetNaturalCardsInField();
                            DoingTargetEffect(card, cardsinfield, efctnum, ia, false);
                        }

                        break;
                    }
            }


        }

        public static void TargetInCementery(ACard card, List<ACard> cardsinfield, int efctnum, Player p10, bool player)
        {
            List<int> targetnum = new();
            List<string> targetname = new();
            List<ACard> targetss = new();

            for (int j = 0; j < p10.Cementery.CardsInCementery.Count; j++)
            {
                System.Console.WriteLine("({0}){1}", j, p10.Cementery.CardsInCementery[j].CardName);
                targetnum.Add(j);
                targetname.Add(p10.Cementery.CardsInCementery[j].CardName);
                targetss.Add(p10.Cementery.CardsInCementery[j]);
            }

            if (p1.Cementery.CardsInCementery.Count == 0)
            {
                System.Console.WriteLine("Cementery is empty!");
                DoingTargetEffect(card, cardsinfield, efctnum, p10, player);
            }

            else
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Write the number of the card you want to be the target...");
                System.Console.WriteLine();

                while (true)
                {
                    var cardnum = "" + Console.ReadLine();

                    if (!CheckIsNum(cardnum))
                    {
                        System.Console.WriteLine("Invalid Input. Try again!");
                    }

                    else
                    {
                        if (!targetnum.Contains(Int32.Parse(cardnum)))
                        {
                            System.Console.WriteLine("There is no card asociated with that number. Try again!");
                        }

                        else
                        {
                            System.Console.WriteLine("You want " + targetname[Int32.Parse(cardnum)] + " to be the target? \n(0)Yes!    (1)No... ");

                            ConsoleKey kii = Console.ReadKey(true).Key;

                            switch (kii)
                            {
                                case ConsoleKey.D0:
                                    {
                                        ACard tgt0 = targetss[Int32.Parse(cardnum)];

                                        System.Console.WriteLine();
                                        System.Console.WriteLine("You choosed " + tgt0.CardName + ". \nActivating magic effect on it...");

                                        if (p1.CardsInHand.ContainCard(card))
                                        {
                                            p1.CardsInHand.RemoveFromHand(card);
                                        }

                                        Player[] both = card.Effect1.Invoke(p1, card.ListCostsFKCMEffect1.ToList(), card, tgt0, 3, p10);
                                        //Thread.Sleep(2500);
                                        System.Console.WriteLine("Done!");
                                        //Thread.Sleep(3500);
                                        System.Console.WriteLine("If you expected something to happen maybe you had bad luck, if it was an effect that as chance to fail.");
                                        //Thread.Sleep(3500);

                                        if (player)
                                        {
                                            p1 = both[0];
                                        }

                                        else
                                        {
                                            p1 = both[0];
                                            ia = (IA)(both[1]);
                                        }

                                        card.ActivateEffectOrAttackOnce = true;

                                        if (p1.AField.IsInField(card))
                                        {
                                            p1.AField.SetCard(card, p1.AField.Find(card));
                                        }

                                        if (p1.CardsInHand.ContainCard(card))
                                        {
                                            p1.CardsInHand.RemoveFromHand(card);
                                            p1.Cementery.ToCementery(card);
                                        }


                                        Console.Clear();
                                        PrintAStateOfGame();
                                        TurnActions();
                                        break;
                                    }

                                case ConsoleKey.B:
                                    {
                                        Console.Clear();
                                        PrintAStateOfGame();
                                        DoingTargetEffect(card, cardsinfield, efctnum, p10, player);
                                        break;
                                    }

                                default:
                                    {
                                        System.Console.WriteLine("Write the one you want this time...");
                                        //Thread.Sleep(1500);
                                        Console.Clear();
                                        PrintAStateOfGame();
                                        TargetInCementery(card, cardsinfield, efctnum, p10, player);
                                        break;
                                    }
                            }
                        }
                    }

                }

            }

        }

        public static void TurnChange()
        {
            System.Console.WriteLine();

            if (p1.Life <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("You looooose!");
                //Thread.Sleep(3000);
                Console.ForegroundColor = ConsoleColor.Gray;
                System.Console.WriteLine("\nPress any key to continue");
                ConsoleKey kii = Console.ReadKey(true).Key;

                switch (kii)
                {
                    case ConsoleKey.Enter:
                        {
                            return;

                        }


                    default:
                        {
                            return;
                        }

                }



            }

            else if (ia.Life <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("You wiiiiiin!");
                //Thread.Sleep(3000);

                Console.ForegroundColor = ConsoleColor.Gray;
                System.Console.WriteLine("\nPress any key to continue");
                ConsoleKey kii = Console.ReadKey(true).Key;


                switch (kii)
                {
                    case ConsoleKey.Enter:
                        {
                            return;

                        }


                    default:
                        {
                            return;
                        }

                }




            }

            if (!ia.IsHisTurn && p1.IsHisTurn)
            {
                Console.Clear();
                CheckStatusEffect(p1);

                if (!p1.AField.IsEmpty())
                {
                    var field = p1.AField.GetNaturalCardsInField();

                    foreach (ACard card in field)
                    {
                        card.ActivateEffectOrAttackOnce = false;
                    }
                }

                System.Console.WriteLine();
                System.Console.WriteLine("Its your turn.");
                System.Console.WriteLine("\nDrawing " + rl.CardsDrawPerTurn + " card...");
                p1.Kingdom.Capital += p1.Kingdom.RecuperationPerTurnCapital;
                p1.Kingdom.Faith += p1.Kingdom.RecuperationPerTurnFaith;
                p1.Kingdom.Knowledge += p1.Kingdom.RecuperationPerTurnKnowledge;
                p1.Kingdom.Militarism += p1.Kingdom.RecuperationPerTurnMilitarism;
                //Thread.Sleep(2000);
                DrawCards(rl.CardsDrawPerTurn);
                Console.Clear();
                PrintAStateOfGame();
                TurnActions();
            }

            else
            {
                CheckStatusEffect(ia);

                if (!ia.AField.IsEmpty())
                {
                    var field = ia.AField.GetNaturalCardsInField();

                    foreach (ACard card in field)
                    {
                        card.ActivateEffectOrAttackOnce = false;
                    }
                }

                IAMainPhase1();
            }

        }

        public static void CheckStatusEffect(Player wii)
        {
            if (!wii.AField.IsEmpty())
            {
                List<ACard> cards = wii.AField.GetNaturalCardsInField();
                StatusEffect se = new();
                var location = wii.AField.Find(cards[0]);

                for (int j = 0; j < cards.Count; j++)
                {
                    location = wii.AField.Find(cards[j]);

                    if (cards[j].CardState != "Normal")
                    {
                        foreach (string state in cards[j].CardStates)
                        {
                            if (state != "AsInfected" && state != "IsInfected")
                            {
                                cards[j] = se.StatesEffectPerTurn[state].Invoke(cards[j]);
                                System.Console.WriteLine(cards[j].CardName + " is " + state + "! " + se.StatesandDescription[state] + "\nActivating status effect...");
                                wii.AField.SetCard(cards[j], location);
                                //Thread.Sleep(1500);
                                if (cards[j].ListOfAbnormalStateDuration[state] != 0)
                                {
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Done! " + cards[j].CardName + " will be " + state + " for " + cards[j].ListOfAbnormalStateDuration[state] + " turns more!");
                                }

                                else
                                {
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Done! " + cards[j].CardName + " is no longer " + state + "!");
                                }

                                //Thread.Sleep(3500);
                            }

                            else
                            {
                                for (int n = 0; n < cards.Count; n++)
                                {
                                    if (cards[n].CardStates.Contains("IsInfected"))
                                    {
                                        cards[n] = se.StatesEffectPerTurn[state].Invoke(cards[n]);
                                        System.Console.WriteLine();
                                        System.Console.WriteLine(cards[j].CardName + " is Infected! " + se.StatesandDescription[state] + "\nActivating status effect...");
                                        wii.AField.SetCard(cards[j], location);

                                        if (cards[j].ListOfAbnormalStateDuration[state] != 0)
                                        {
                                            System.Console.WriteLine();
                                            System.Console.WriteLine("Done! " + cards[j].CardName + " will be Infected for " + cards[j].ListOfAbnormalStateDuration[state] + " turns more!");
                                        }

                                        else
                                        {
                                            System.Console.WriteLine();
                                            System.Console.WriteLine("Done! " + cards[j].CardName + " is no longer " + state + "!");
                                        }
                                    }
                                }

                                for (int n = 0; n < cards.Count; n++)
                                {
                                    if (cards[n].CardStates.Contains("AsInfected"))
                                    {
                                        cards[n] = se.StatesEffectPerTurn[state].Invoke(cards[n]);
                                        System.Console.WriteLine(cards[j].CardName + " as Infected other card! " + se.StatesandDescription[state] + "\nActivating status effect...");
                                        wii.AField.SetCard(cards[j], location);

                                        if (cards[j].ListOfAbnormalStateDuration[state] != 0)
                                        {
                                            System.Console.WriteLine();
                                            System.Console.WriteLine("Done! The card that " + cards[j].CardName + " infected will be like that for " + cards[j].ListOfAbnormalStateDuration[state] + " turns more!");
                                        }

                                        else
                                        {
                                            System.Console.WriteLine();
                                            System.Console.WriteLine("Done! The card that" + cards[j].CardName + " infected as been recovered!");
                                        }
                                    }
                                }


                            }

                        }
                    }
                }
            }

        }

        public static void IAMainPhase1()
        {
            ia.performedactions = new();
            System.Console.WriteLine();
            System.Console.WriteLine("Now is IA's turn!");
            //Thread.Sleep(1500);
            System.Console.WriteLine("\nEntering Main Phase 1. \n\nDraw Cards and do Invocations.");
            //Thread.Sleep(3500);

            if (ia.DeckOfCards.Deck.Length >= 1)
            {
                ia.CanDrawCard = true;
            }

            if (ia.CanDrawCard)
            {
                if (rl.CardsDrawPerTurn > ia.DeckOfCards.Deck.Length)
                {
                    if (ia.DeckOfCards.Deck.Length == 1)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("IA will draw is last card.");
                    }

                    else
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("IA will draw is lasts " + ia.DeckOfCards.Deck.Length + " cards.");
                    }

                }

                else
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("IA will draw " + rl.CardsDrawPerTurn + " cards.");
                }

            }

            else
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Deck is empty already, so IA won't draw cards anymore.");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("IA is trying to invoke cards in field...");
            //Thread.Sleep(3500);

            if ((p1.AField.GetNaturalCardsInField != null || p1.AField.GetNaturalCardsInField().Count != 0) && (ia.AField.GetNaturalCardsInField() != null || ia.AField.GetNaturalCardsInField().Count != 0))
            {
                ia.TurnActions["MP1"].Invoke(p1, p1.AField.GetNaturalCardsInField().ToArray(), ia.AField.GetNaturalCardsInField().ToArray());
            }


            Console.Clear();
            PrintAStateOfGame();

            System.Console.WriteLine();

            if (ia.performedactions.Count == 0)
            {
                System.Console.WriteLine("For now, there is nothing to do... \nPress any button to continue.");

                ConsoleKey kip0 = Console.ReadKey(true).Key;

                switch (kip0)
                {
                    default:
                        Console.Clear();
                        PrintAStateOfGame();
                        IABattlePhase();
                        break;
                }
            }

            for (int j = 0; j < ia.performedactions.Count; j++)
            {
                System.Console.WriteLine(ia.performedactions[j]);
                System.Console.WriteLine();
                //Thread.Sleep(1500);
            }


            System.Console.WriteLine();
            System.Console.WriteLine("All done! Check results and then press any button to continue!");

            ConsoleKey kip = Console.ReadKey(true).Key;

            switch (kip)
            {
                case ConsoleKey.Enter:
                    {
                        Console.Clear();
                        PrintAStateOfGame();
                        IABattlePhase();
                        break;
                    }

                default:
                    Console.Clear();
                    PrintAStateOfGame();
                    IABattlePhase();
                    break;
            }


        }

        public static void IAMainPhase2()
        {
            ia.performedactions = new();
            System.Console.WriteLine();
            System.Console.WriteLine("Entering Main Phase 2. \n\nInvocations again!");
            //Thread.Sleep(3500);

            System.Console.WriteLine();
            System.Console.WriteLine("IA is trying to invoke cards in field...");
            //Thread.Sleep(3500);

            ia.TurnActions["MP2"].Invoke(p1, p1.AField.GetNaturalCardsInField().ToArray(), ia.AField.GetNaturalCardsInField().ToArray());


            Console.Clear();
            PrintAStateOfGame();

            System.Console.WriteLine();

            if (ia.performedactions.Count == 0)
            {
                System.Console.WriteLine("For now, there is nothing to do... \nPress any button to continue.");

                ConsoleKey kip0 = Console.ReadKey(true).Key;

                switch (kip0)
                {
                    default:
                        ia.IsHisTurn = false;
                        p1.IsHisTurn = true;
                        Console.Clear();
                        PrintAStateOfGame();
                        TurnChange();
                        break;
                }
            }

            for (int j = 0; j < ia.performedactions.Count; j++)
            {
                System.Console.WriteLine(ia.performedactions[j]);
                System.Console.WriteLine();
                //Thread.Sleep(1500);
            }

            System.Console.WriteLine();
            System.Console.WriteLine("All done! Check results and then press any button to continue!");

            ConsoleKey kip = Console.ReadKey(true).Key;

            switch (kip)
            {
                default:
                    ia.IsHisTurn = false;
                    p1.IsHisTurn = true;
                    Console.Clear();
                    PrintAStateOfGame();
                    TurnChange();
                    break;
            }



        }

        public static void IABattlePhase()
        {
            ia.performedactions = new();
            System.Console.WriteLine();
            System.Console.WriteLine("Entering Battle Phase. \n\nAttack and use effect.");
            //Thread.Sleep(3500);

            ia.TurnActions["BP"].Invoke(p1, p1.AField.GetNaturalCardsInField().ToArray(), ia.AField.GetNaturalCardsInField().ToArray());

            Console.Clear();
            PrintAStateOfGame();

            if (ia.performedactions.Count == 0)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("For now, there is nothing to do... \nPress any button to continue.");

                ConsoleKey kip0 = Console.ReadKey(true).Key;

                switch (kip0)
                {
                    default:
                        Console.Clear();
                        PrintAStateOfGame();
                        IAMainPhase2();
                        break;
                }
            }

            System.Console.WriteLine();

            for (int j = 0; j < ia.performedactions.Count; j++)
            {
                System.Console.WriteLine(ia.performedactions[j]);
                System.Console.WriteLine();
                //Thread.Sleep(1500); ;
            }

            System.Console.WriteLine();
            System.Console.WriteLine("All done! Check results and then press any button to continue!");

            ConsoleKey kip = Console.ReadKey(true).Key;

            switch (kip)
            {
                default:
                    Console.Clear();
                    PrintAStateOfGame();
                    IAMainPhase2();
                    break;
            }

        }

        #endregion

        #region Print Methods
        static void PrintIAState()
        {
            if (ianum == 1)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("IA: Blinded Man." + " Disposition: " + ia.Kingdom.Disposition + ". \nLife: " + ia.Life + ". \nFaith: " + ia.Kingdom.Faith + ". Knowledge: " + ia.Kingdom.Knowledge + ". Capital: " + ia.Kingdom.Capital
                + ". Militar: " + ia.Kingdom.Militarism + "\nCardsInHand: " + ia.CardsInHand.Hand.Length + ". CardsInDeck: " + ia.DeckOfCards.Deck.Length + ". CardsInCementery: " + ia.Cementery.CardsInCementery.Count + ".");
            }

            else
            {
                System.Console.WriteLine();
                System.Console.WriteLine("IA: The Berserk. Disposition: " + ia.Kingdom.Disposition + ". \nLife: " + ia.Life + ". \nFaith: " + ia.Kingdom.Faith + ". Knowledge: " + ia.Kingdom.Knowledge + ". Capital: " + ia.Kingdom.Capital
                + ". Militar: " + ia.Kingdom.Militarism + "\nCardsInHand: " + ia.CardsInHand.Hand.Length + ". CardsInDeck: " + ia.DeckOfCards.Deck.Length + ". CardsInCementery: " + ia.Cementery.CardsInCementery.Count + ".");

            }

        }

        static void PrintUserState()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Player: " + playername + ". Disposition: " + p1.Kingdom.Disposition + ". \nLife: " + p1.Life + ". \nFaith: " + p1.Kingdom.Faith + ". Knowledge: " + p1.Kingdom.Knowledge + ". Capital: " + p1.Kingdom.Capital
                           + ". Militar: " + p1.Kingdom.Militarism + "\nCardsInHand: " + p1.CardsInHand.Hand.Length + ". CardsInDeck: " + p1.DeckOfCards.Deck.Length + ". CardsInCementery: " + ia.Cementery.CardsInCementery.Count + ".");

        }

        static void PrintACard(ACard card)
        {
            if (card == null)
            {
                System.Console.WriteLine("Null");
            }

            else if (!card.IsACardCreatedByUser)
            {
                #region Setting card

                ACard clone = card.Clone(card);

                string cloneatk = "" + clone.ATK;
                string clonedef = "" + clone.DEF;
                string clonevit = "" + clone.VIT;

                string cloneaff1name = clone.Affiliations[0].TypeOfKingdomAffiliation;
                string cloneaff2name = clone.Affiliations[1].TypeOfKingdomAffiliation;
                string cloneaff3name = clone.Affiliations[2].TypeOfKingdomAffiliation;
                string cloneaff4name = clone.Affiliations[3].TypeOfKingdomAffiliation;

                string cloneaff1num = "" + clone.Affiliations[0].amount;
                string cloneaff2num = "" + clone.Affiliations[1].amount;
                string cloneaff3num = "" + clone.Affiliations[2].amount;
                string cloneaff4num = "" + clone.Affiliations[3].amount;

                if (clone.CardElementName2 == "None")
                {
                    clone.CardElementName2 = "";
                }

                if (clone.CardElementName3 == "None")
                {
                    clone.CardElementName3 = "";
                }

                if (clone.CardElementName4 == "None")
                {
                    clone.CardElementName4 = "";
                }

                string efcname1 = clone.EffectTxt1.name;
                string efc1 = clone.EffectTxt1.description;
                string efcname2 = "";
                string efc2 = "";
                string efcname3 = "";
                string efc3 = "";
                string efcname4 = "";
                string efc4 = "";

                if (clone.Effect2 != null)
                {
                    efcname2 = clone.EffectTxt2.name;
                    efc2 = clone.EffectTxt2.description;
                }

                if (clone.Effect3 != null)
                {
                    efcname3 = clone.EffectTxt3.name;
                    efc3 = clone.EffectTxt3.description;
                }

                if (clone.Effect4 != null)
                {
                    efcname4 = clone.EffectTxt4.name;
                    efc4 = clone.EffectTxt4.description;
                }

                if (clone.CardTypeNum == 1 && card.EvolutionLevel != 2 && card.EvolutionLevel != 3)
                {
                    card.EvolutionLevel = 1;
                }

                if (clone.CardTypeNum == 2)
                {
                    clone.CardElementName1 = "None.";
                    cloneatk = "None.";
                    clonedef = "None.";
                    clonevit = "None.";
                    clone.RaceName = "None.";
                    cloneaff1name = "None.";
                    cloneaff1num = "";
                    cloneaff2name = "";
                    cloneaff2num = "";
                    cloneaff3name = "";
                    cloneaff3num = "";
                    cloneaff4name = "";
                    cloneaff4num = "";
                    clone.CardLore = "None.";
                    clone.CardState = "None.";
                }

                #endregion


                System.Console.WriteLine(" __________________________________________________________________________________________________________");
                System.Console.WriteLine("|Name: {0}             EvolLv.{1}   " +
                                       "\n|Elements: {2}      {3}             " +
                                       "\n|{4}{5}                             " +
                                       "\n|Race: {6}                          " +
                                       "\n|Disposition:                       " +
                                       "\n|{7} {8}                            " +
                                       "\n|{9} {10}                           " +
                                       "\n|{11} {12}                          " +
                                       "\n|{13}  {14}                         " +
                                       "\n|                                   " +
                                       "\n|Costs:                             " +
                                       "\n|F: {15} K: {16} C:{17} M:{18}      " +
                                       "\n|                                   " +
                                       "\n|Description:                       " +
                                       "\n|{19}                               " +
                                       "\n|                                   " +
                                       "\n|Effects:                           " +
                                       "\n|{20}                               " +
                                       "\n|{21}                               " +
                                       "\n|{22}                               " +
                                       "\n|{23}                               " +
                                       "\n|{24}                               " +
                                       "\n|{25}                               " +
                                       "\n|{26}                               " +
                                       "\n|{27}                               " +
                                       "\n|                                   " +
                                       "\n|State: {28}                        " +
                                       "\n|                                   " +
                                       "\n|ATK:      DEF:           VIT:      " +
                                       "\n|{29}        {30}           {31}    " +
                                       "\n|__________________________________________________________________________________________________________",

                clone.CardName, card.EvolutionLevel, clone.CardElementName1, clone.CardElementName2, clone.CardElementName3,
                clone.CardElementName4, clone.RaceName,
                cloneaff1name, cloneaff1num,
                cloneaff2name, cloneaff2num,
                cloneaff3name, cloneaff3num,
                cloneaff4name, cloneaff4num,
                clone.FaithCost, clone.KnowledgeCost, clone.CapitalCost, clone.MilitarCost, clone.CardLore,
                efcname1, efc1, efcname2, efc2, efcname3, efc3, efcname4, efc4, clone.CardState, cloneatk, clonedef, clonevit);
            }

            else
            {
                ACard clone = card.Clone(card);

                string cloneatk = "" + clone.ATK;
                string clonedef = "" + clone.DEF;
                string clonevit = "" + clone.VIT;

                string cloneaff1name = clone.Affiliations[0].TypeOfKingdomAffiliation;
                string cloneaff2name = clone.Affiliations[1].TypeOfKingdomAffiliation;
                string cloneaff3name = clone.Affiliations[2].TypeOfKingdomAffiliation;
                string cloneaff4name = clone.Affiliations[3].TypeOfKingdomAffiliation;

                string cloneaff1num = "" + clone.Affiliations[0].amount;
                string cloneaff2num = "" + clone.Affiliations[1].amount;
                string cloneaff3num = "" + clone.Affiliations[2].amount;
                string cloneaff4num = "" + clone.Affiliations[3].amount;

                if (clone.CardElementName2 == "None")
                {
                    clone.CardElementName2 = "";
                }

                if (clone.CardElementName3 == "None")
                {
                    clone.CardElementName3 = "";
                }

                if (clone.CardElementName4 == "None")
                {
                    clone.CardElementName4 = "";
                }

                if (clone.CardTypeNum == 1 && card.EvolutionLevel != 2 && card.EvolutionLevel != 3)
                {
                    card.EvolutionLevel = 1;
                }

                string efcname1 = card.OEObject.Tuple.name;
                string efct1 = card.OEObject.Tuple.description;

                System.Console.WriteLine(" __________________________________________________________________________________________________________________________________________");
                System.Console.WriteLine("|Name: {0}             EvolLv.{1}   " +
                                       "\n|Elements: {2}      {3}             " +
                                       "\n|{4}{5}                             " +
                                       "\n|Race: {6}                          " +
                                       "\n|Disposition:                       " +
                                       "\n|{7} {8}                            " +
                                       "\n|{9} {10}                           " +
                                       "\n|{11} {12}                          " +
                                       "\n|{13}  {14}                         " +
                                       "\n|                                   " +
                                       "\n|Costs:                             " +
                                       "\n|F: {15} K: {16} C:{17} M:{18}      " +
                                       "\n|                                   " +
                                       "\n|Description:                       " +
                                       "\n|{19}                               " +
                                       "\n|                                   " +
                                       "\n|Effects:                           " +
                                       "\n|{20}                               " +
                                       "\n|{21}                               " +
                                       "\n|{22}                               " +
                                       "\n|{23}                               " +
                                       "\n|{24}                               " +
                                       "\n|{25}                               " +
                                       "\n|{26}                               " +
                                       "\n|{27}                               " +
                                       "\n|                                   " +
                                       "\n|State: {28}                        " +
                                       "\n|                                   " +
                                       "\n|ATK:      DEF:           VIT:      " +
                                       "\n|{29}        {30}           {31}    " +
                                       "\n|____________________________________________________________________________________________________________________________________________",

                clone.CardName, card.EvolutionLevel, clone.CardElementName1, clone.CardElementName2, clone.CardElementName3,
                clone.CardElementName4, clone.RaceName,
                cloneaff1name, cloneaff1num,
                cloneaff2name, cloneaff2num,
                cloneaff3name, cloneaff3num,
                cloneaff4name, cloneaff4num,
                clone.FaithCost, clone.KnowledgeCost, clone.CapitalCost, clone.MilitarCost, clone.CardLore,
                efcname1, efct1, "", "", "", "", "", "", clone.CardState, cloneatk, clonedef, clonevit);
            }

        }

        static void PrintCards(ACard[] cards)
        {
            for (int k = 0; k < cards.Length; k++)
            {
                PrintACard(cards[k]);
                System.Console.WriteLine();
                //Thread.Sleep(1000);
            }
        }

        static void PrintField(ACard[,] cards)
        {
            System.Console.WriteLine("NaturalSlot 1");
            PrintACard(cards[0, 0]);

            System.Console.WriteLine();
            System.Console.WriteLine("NaturalSlot 2");
            PrintACard(cards[0, 1]);

            System.Console.WriteLine();
            System.Console.WriteLine("NaturalSlot 3");
            PrintACard(cards[0, 2]);

            System.Console.WriteLine();
            System.Console.WriteLine("NaturalSlot 4");
            PrintACard(cards[0, 3]);

            System.Console.WriteLine();
            System.Console.WriteLine("NaturalSlot 5");
            PrintACard(cards[0, 4]);

            System.Console.WriteLine();
            System.Console.WriteLine("MagicSlot 1");
            PrintACard(cards[1, 0]);

            System.Console.WriteLine();
            System.Console.WriteLine("MagicSlot 2");
            PrintACard(cards[1, 1]);

            System.Console.WriteLine();
            System.Console.WriteLine("MagicSlot 3");
            PrintACard(cards[1, 2]);

            System.Console.WriteLine();
            System.Console.WriteLine("MagicSlot 4");
            PrintACard(cards[1, 3]);

            System.Console.WriteLine();
            System.Console.WriteLine("MagicSlot 5");
            PrintACard(cards[1, 4]);

        }

        static void PrintPlayerHand(ACard[] cards)
        {
            for (int k = 0; k < cards.Length; k++)
            {
                PrintACard(cards[k]);
            }
        }

        static void PrintAStateOfGame()
        {
            PrintIAState();
            System.Console.WriteLine();
            System.Console.WriteLine("\nIA Field: \n");
            PrintField(ia.AField.Field);
            System.Console.WriteLine();

            System.Console.WriteLine("\nPlayer Field: \n");
            PrintField(p1.AField.Field);
            System.Console.WriteLine();

            PrintPlayerHand(p1.CardsInHand.Hand);
            PrintUserState();
        }

        static void PrintIACementery()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("IA's Cementery: ");

            for (int j = 0; j < ia.Cementery.CardsInCementery.Count; j++)
            {
                PrintACard(ia.Cementery.CardsInCementery[j]);
                System.Console.WriteLine();
                //Thread.Sleep(1500);
            }

            System.Console.WriteLine();
            System.Console.WriteLine("(B)ack");

            ConsoleKey kio = Console.ReadKey(true).Key;

            switch (kio)
            {
                default:
                    TurnActions();
                    break;
            }
        }

        static void PrintYourCementery()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Your Cementery: ");

            for (int j = 0; j < p1.Cementery.CardsInCementery.Count; j++)
            {
                PrintACard(p1.Cementery.CardsInCementery[j]);
                System.Console.WriteLine();
                //Thread.Sleep(1500);
            }

            System.Console.WriteLine();
            System.Console.WriteLine("(B)ack");

            ConsoleKey kio = Console.ReadKey(true).Key;

            switch (kio)
            {
                default:
                    TurnActions();
                    break;
            }
        }

        #endregion

        #region Options

        static void PrintMenu()
        {

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Welcome new emperor!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Touch a letter to continue...");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[N]ew Game");
            System.Console.WriteLine("[C]ard Creator");
            Console.WriteLine("[S]ettings");
            Console.WriteLine("[E]xit");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        static void Settings()
        {
            Console.Clear();
            PrintOptions0();

        }

        static void PrintOptions0()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("(1)Change Numeric Values. \n(2)Change Your Kingdom Disposition. \n(3)Change Difficult/IA to fight. \n(4)Change Player Name. \n(5)Play with cards created \n(B)ack");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.Enter:
                    PrintOptions1();
                    break;

                case ConsoleKey.D2:
                    PrintOptions2();
                    break;

                case ConsoleKey.D3:
                    PrintOptions3();
                    break;

                case ConsoleKey.D4:
                    Console.Clear();
                    PrintOptions4();
                    break;

                case ConsoleKey.D5:
                    Console.Clear();
                    System.Console.WriteLine("Do you want the cards you made to be part of your starting deck? \nNote: If you made more than 40, only the first 40 will be taken in count, because that's the max size of deck. \n(0)Yes     (1)No");

                    ConsoleKey kip = Console.ReadKey(true).Key;

                    switch (kip)
                    {
                        case ConsoleKey.D0:
                            PlayWithCreatedCards = true;
                            System.Console.WriteLine("Done!");
                            //Thread.Sleep(2000);
                            Console.Clear();
                            Settings();
                            break;

                        default:
                            PlayWithCreatedCards = false;
                            System.Console.WriteLine("Done!");
                            //Thread.Sleep(2000);
                            Console.Clear();
                            Settings();
                            break;
                    }

                    break;

                case ConsoleKey.B:
                    Console.Clear();
                    PrintMenu();
                    return;
                default:
                    break;
            }


        }

        static void PrintOptions1()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("Starting Values: \n(1)Life: {0} \n(2)Faith: {1} \n(3)Militar: {2} \n(4)Capital: {3} \n(5)Knowledge: {4} \n(6)Max Cards In Deck  \n(7)Starting Cards In Hand \n(8)Cards draw per turn \n(9)Back.", Life, F, M, C, K);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.Enter:
                    {
                        Console.Clear();
                        System.Console.WriteLine("Input new value:");
                        var rl = Console.ReadLine();

                        if (rl == null || !CheckIsNum(rl))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            Console.Clear();
                            Life = Double.Parse(rl);
                            System.Console.WriteLine("Good!");
                            //Thread.Sleep(1000);
                            PrintOptions1();
                        }

                        break;
                    }
                case ConsoleKey.D2:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl0 = Console.ReadLine();

                        if (rl0 == null || !CheckIsNum(rl0))
                        {
                            System.Console.WriteLine("Invalid Entry. Min value is 100.");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else if (Double.Parse(rl0) < 100)
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            Console.Clear();
                            F = Double.Parse(rl0);
                            System.Console.WriteLine("Good!");
                            //Thread.Sleep(1000);
                            PrintOptions1();
                        }

                        break;
                    }
                case ConsoleKey.D3:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl1 = Console.ReadLine();

                        if (rl1 == null || !CheckIsNum(rl1))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else if (Double.Parse(rl1) < 100)
                        {
                            System.Console.WriteLine("Invalid Entry. Min value is 100.");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            Console.Clear();
                            M = Double.Parse(rl1);
                            System.Console.WriteLine("Good!");
                            //Thread.Sleep(1000);
                            PrintOptions1();
                        }
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl2 = Console.ReadLine();

                        if (rl2 == null || !CheckIsNum(rl2))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else if (Double.Parse(rl2) < 100)
                        {
                            System.Console.WriteLine("Invalid Entry. Min value is 100.");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            Console.Clear();
                            C = Double.Parse(rl2);
                            System.Console.WriteLine("Good!");
                            //Thread.Sleep(1000);
                            PrintOptions1();
                        }
                        break;
                    }

                case ConsoleKey.D5:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl3 = Console.ReadLine();

                        if (rl3 == null || !CheckIsNum(rl3))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else if (Double.Parse(rl3) < 100)
                        {
                            System.Console.WriteLine("Invalid Entry. Min value is 100.");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            Console.Clear();
                            K = Double.Parse(rl3);
                            System.Console.WriteLine("Good!");
                            //Thread.Sleep(1000);
                            PrintOptions1();
                        }

                        break;
                    }
                case ConsoleKey.D6:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl4 = Console.ReadLine();

                        if (rl4 == null || !CheckIsNum(rl4))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            var res = Int32.Parse(rl4);
                            if (res > 40)
                            {
                                System.Console.WriteLine("A deck can't have more than 40 cards.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else if (res < 15)
                            {
                                System.Console.WriteLine("A deck can't have less than 15 cards.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else
                            {
                                Console.Clear();
                                MaxCD = res;
                                System.Console.WriteLine("Good!");
                                //Thread.Sleep(1000);
                                PrintOptions1();
                            }
                        }
                        break;
                    }
                case ConsoleKey.D7:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl5 = Console.ReadLine();

                        if (rl5 == null || !CheckIsNum(rl5))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            var res = Int32.Parse(rl5);
                            if (res > 6)
                            {
                                System.Console.WriteLine("At start you can only have max 6 cards.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else if (res < 1)
                            {
                                System.Console.WriteLine("At start you need at least 1 card in hand.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else
                            {
                                Console.Clear();
                                MCH = res;
                                System.Console.WriteLine("Good!");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }
                        }
                        break;
                    }
                case ConsoleKey.D8:
                    {
                        Console.Clear();
                        Console.WriteLine("Input new value:");
                        var rl6 = Console.ReadLine();

                        if (rl6 == null || !CheckIsNum(rl6))
                        {
                            System.Console.WriteLine("Invalid Entry");
                            //Thread.Sleep(1000);
                            Console.Clear();
                            PrintOptions1();
                        }

                        else
                        {
                            var res = Int32.Parse(rl6);
                            if (res > 3)
                            {
                                System.Console.WriteLine("You can draw max 3 cards per turn.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else if (res < 1)
                            {
                                System.Console.WriteLine("You need to draw at least 1 card each turn.");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }

                            else
                            {
                                Console.Clear();
                                MDT = res;
                                System.Console.WriteLine("Good!");
                                //Thread.Sleep(1000);
                                Console.Clear();
                                PrintOptions1();
                            }
                        }
                        break;
                    }
                case ConsoleKey.D9:
                    {
                        Console.Clear();
                        PrintOptions0();
                        break;
                    }
                default:
                    break;
            }


        }

        static void PrintOptions2()
        {
            Console.Clear();
            System.Console.WriteLine("Dispositions are predefined as: \n(1)Faith. \n(2)Capital. \n(3)Militar. \n(4)Knowledge. \nChoose one.");
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.Enter:
                    System.Console.WriteLine("Faith then. Good!");
                    disposition = "Faith";
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;

                case ConsoleKey.D2:
                    System.Console.WriteLine("Capital. Nice!");
                    disposition = "Capital";
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;

                case ConsoleKey.D3:
                    System.Console.WriteLine("Knowledge then. Good!");
                    disposition = "Knowledge";
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;

                case ConsoleKey.D4:
                    System.Console.WriteLine("Militar then. Good!");
                    disposition = "Militar";
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;
                default:
                    break;
            }
        }

        static void PrintOptions3()
        {
            Console.Clear();
            System.Console.WriteLine("Enemy IA and difficults are two: \n(1)Random IA - Blinded Man. The easy one. For Default. \n(2)Fighter IA - The Berserk. The though one. \nSelect one.");
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.Enter:
                    System.Console.WriteLine("Blinded Man then. Good!");
                    ianum = 1;
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;

                case ConsoleKey.D2:
                    System.Console.WriteLine("The Berserk. Nice!");
                    ianum = 2;
                    //Thread.Sleep(1000);
                    Console.Clear();
                    PrintOptions0();
                    break;
                default:
                    break;
            }
        }

        static void PrintOptions4()
        {
            Console.Clear();
            System.Console.WriteLine("Player name: " + playername + ". Input new name.");
            playername = Console.ReadLine()!;
            System.Console.WriteLine("Good name!");
            //Thread.Sleep(1000);
            Console.Clear();
            PrintOptions0();
        }

        static bool CheckIsNum(string s)
        {
            for (int k = 0; k < s.Length; k++)
            {
                if (!Char.IsNumber(s[k]))
                {
                    return false;
                }
            }

            return true;

        }

        static void ResetValues()
        {
            dc = MakeRandomDeck();
            dc0 = MakeRandomDeck();
            ch = new(new ACard[] { });
            ch0 = new(new ACard[] { });
            M = mi;
            F = fa;
            C = ca;
            K = kw;
            Life = lif;
        }

        static double CopyValue(double from)
        {
            return from;


        }

        #endregion

        #region Card Creator
        static void CardCreator()
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("You need to go to the carpet DataBase and there in the txt +CardCreator"
            + "write the code to create a new card. Then press (C)reate cards! \nSee:  \n(A)n example random deck. \n(C)reate card(s)  \n(V)iew Created Cards  \n(B)ack");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.Enter:
                    {
                        Console.Clear();

                        foreach (ACard card0 in dc.Deck)
                        {
                            System.Console.WriteLine();
                            PrintACard(card0);
                            System.Console.WriteLine();
                        }

                        System.Console.WriteLine("\nPress any button to go back...");

                        ConsoleKey key0 = Console.ReadKey(true).Key;

                        switch (key0)
                        {
                            default:
                                Console.Clear();
                                CardCreator();
                                break;
                        }


                        break;
                    }

                case ConsoleKey.C:
                    {
                        Console.Clear();

                        List<string> precode = CardDataBase.GetExternalTxt("DataBase", "+CardMaker", new string[] { "{", "}", " ", "=", "(", ")", ";" });

                        string code = "";

                        foreach (String ch in precode)
                        {
                            code += ch;
                        }

                        if (code != "")
                        {
                            string code1 = "";
                            List<ACard> cardscreatednow = new();

                            for (int k = 0; k < code.Length; k++)
                            {
                                if (code[k] == '}')
                                {
                                    code1 += code[k];
                                    ObjectEffect oe0 = new ObjectEffect(code1);
                                    ACard card0 = RandomFactory.CreateRandomCard(oe0);
                                    card0.IsACardCreatedByUser = true;
                                    CreatedCards.Add(card0);
                                    cardscreatednow.Add(card0);

                                    if ((k + 1) < code.Length)
                                    {
                                        code = code.Substring(k + 1);
                                        k = 0;
                                        code1 = "";
                                    }

                                    else
                                    {
                                        break;
                                    }

                                }

                                code1 += code[k];
                            }


                            foreach (ACard card in cardscreatednow)
                            {
                                System.Console.WriteLine();
                                PrintACard(card);
                                System.Console.WriteLine();
                            }

                            System.Console.WriteLine();

                            if (cardscreatednow.Count == 1)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("Do you want to save this card?   \n(0)Yes     (1)No");
                            }

                            else
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("Do you want to save these cards?   \n(0)Yes     (1)No");
                            }


                            ConsoleKey kaa = Console.ReadKey(true).Key;

                            switch (kaa)
                            {
                                case ConsoleKey.D0:
                                    {
                                        foreach (ACard card1 in cardscreatednow)
                                        {
                                            SaveCard(card1, code);
                                        }

                                        if (cardscreatednow.Count == 1)
                                        {
                                            Console.Clear();
                                            System.Console.WriteLine("Card Saved Sucesfully! \nNote: To make new cards with other effect you need to erase the code, because if you don't another card will be created with the same effect that the one now! \nAnd of course, any other amount of cards with the others effects you wrote below!");
                                        }

                                        else
                                        {
                                            Console.Clear();
                                            System.Console.WriteLine("Cards Saved Sucesfully! \nNote: To make new cards with other effect you need to erase the code, because if you don't other cards will be created with the same effect that the ones now! \nAnd of course, any other amount of cards with the others effects you wrote below!");
                                        }
                                        //Thread.Sleep(4300);
                                        Console.Clear();
                                        break;
                                    }

                                default:
                                    {
                                        Console.Clear();
                                        CardCreator();
                                        break;
                                    }

                            }

                        }

                        else
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You didn't wrote any code yet!");
                            //Thread.Sleep(3000);
                            Console.Clear();
                            CardCreator();
                        }

                        break;
                    }

                case ConsoleKey.V:
                    {
                        if (CreatedCards.Count != 0)
                        {
                            foreach (ACard card0 in CreatedCards)
                            {
                                System.Console.WriteLine();
                                PrintACard(card0);
                                System.Console.WriteLine();
                            }

                            System.Console.WriteLine("\nPress any button to go back...");

                            ConsoleKey key1 = Console.ReadKey(true).Key;

                            switch (key1)
                            {
                                default:
                                    Console.Clear();
                                    CardCreator();
                                    break;
                            }

                        }

                        else
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("You dont have created any card yet! \nNote: If you wrote the code (correctly) in the +CardMaker txt then you only need to use the option (C)reate Cards!");
                            //Thread.Sleep(3800);
                            Console.Clear();
                            CardCreator();

                        }

                        break;
                    }

                case ConsoleKey.B:
                    {
                        Console.Clear();
                        PrintMenu();
                        break;
                    }
            }

        }

        static void SaveCard(ACard card, string code)
        {
            List<string> dates = new();

            dates.Add(" ");
            dates.Add("ATK");
            dates.Add("" + card.ATK);
            dates.Add("DEF");
            dates.Add("" + card.DEF);
            dates.Add("VIT");
            dates.Add("" + card.VIT);
            dates.Add("Elements");
            dates.Add("A1");
            dates.Add("" + card.CardElementNum1);


            if (card.CardElementName2 != "None")
            {
                dates.Add("A2");
                dates.Add("" + card.CardElementNum2);
            }

            dates.Add("Race");
            dates.Add("" + card.RaceObject.RacesNum[card.RaceName]);
            dates.Add("Lore");
            dates.Add(card.CardName);
            dates.Add(card.CardLore);
            dates.Add("Cost1");
            dates.Add("" + card.FaithCost);
            dates.Add("" + card.KnowledgeCost);
            dates.Add("" + card.CapitalCost);
            dates.Add("" + card.MilitarCost);
            dates.Add("SpecialEffect");
            dates.Add(code);

            string path = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "Database", "SavedCards.txt");
            System.IO.File.AppendAllLines(path, dates);
        }

        static void LoadSavedCardsToDeck()
        {
            CreatedCards = CardLoader.SpecialCardsLoader("SavedCards");

            ACard[] newdeck = new ACard[40];

            float lim = (MaxCD / 2);
            lim -= 2;
            int res = (int)(Math.Round(lim));

            for (int k = 0; k < res; k++)
            {
                if (k < CreatedCards.Count)
                {
                    newdeck[k] = CreatedCards[k];
                }

                else
                {
                    newdeck[k] = dc.Deck[0];
                }

            }

            int newres = res * 2;

            for (int k = res; k < newres; k++)
            {
                if (k < CreatedCards.Count)
                {
                    newdeck[k] = CreatedCards[k];
                }

                else
                {
                    newdeck[k] = dc.Deck[0];
                }

            }

            newdeck[newdeck.Length - 4] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(19, 2)),
           new Description("Tower of Guns", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Militarism en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            newdeck[newdeck.Length - 3] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(20, 2)),
           new Description("Tower of Salomon", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Knowledge en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            newdeck[newdeck.Length - 2] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(21, 2)),
           new Description("Tower of God", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Faith en 200% y el resto en 100%.",
           new AType(2), new Race(1), 500, 500, 1000, 500)));

            newdeck[newdeck.Length - 1] = (new ACard(new ProjectClasses.Attribute(0, 0, 0, new Element()), new Effects(new SingleEffect(22, 2)),
            new Description("Tower of Money", "Efecto de carta mágica invocada en el campo. Aumenta la recuperación natural de Capital en 200% y el resto en 100%.",
            new AType(2), new Race(1), 500, 500, 1000, 500)));

            if (newdeck.ToList().Count < MaxCD)
            {
                for (int j = newdeck.ToList().Count; j < MaxCD; j++)
                {
                    newdeck[j] = database[50];
                }
            }

            dc.Deck = newdeck;
            dc.Shuffle(dc.Deck);

        }

        #endregion

    }
}





