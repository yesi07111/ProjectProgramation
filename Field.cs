using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IHaveAField
    {
        //Por defecto hay 5 posiciones para colocar criaturas
        public ACard monsterposition1 { get; set; }
        public ACard monsterposition2 { get; set; }
        public ACard monsterposition3 { get; set; }
        public ACard monsterposition4 { get; set; }
        public ACard monsterposition5 { get; set; }

        //Y por defecto hay también 5 posiciones para colocar cartas mágicas
        public ACard magicposition1 { get; set; }
        public ACard magicposition2 { get; set; }
        public ACard magicposition3 { get; set; }
        public ACard magicposition4 { get; set; }
        public ACard magicposition5 { get; set; }

        //Propiedad campo que contiene un array bidimensional de 2 filas de 5 columnas, la primera fila representa
        //las criaturas y la segunda fila las cartas mágicas.
        public ACard[,] Field { get; set; }
    }

    /// <summary>Clase para el campo donde se ubican los cartas.</summary>
    public class AField : IHaveAField
    {

        //Por defecto hay 5 posiciones (x, y) para colocar criaturas
        public ACard monsterposition1 { get; set; }
        public ACard monsterposition2 { get; set; }
        public ACard monsterposition3 { get; set; }
        public ACard monsterposition4 { get; set; }
        public ACard monsterposition5 { get; set; }

        //Y por defecto hay también 5 posiciones (x, y) para colocar cartas mágicas
        public ACard magicposition1 { get; set; }
        public ACard magicposition2 { get; set; }
        public ACard magicposition3 { get; set; }
        public ACard magicposition4 { get; set; }
        public ACard magicposition5 { get; set; }

        //Propiedad campo que contiene un array bidimensional de 2 filas de 5 columnas, la primera fila representa
        //las criaturas y la segunda fila las cartas mágicas. 
        public ACard[,] Field { get; set; }

        public AField()
        {
            ACard[,] field = new ACard[2, 5];
            Field = field;


            #region Eliminar advertancias de nullabilidad

            monsterposition1 = null!;
            monsterposition2 = null!;
            monsterposition3 = null!;
            monsterposition4 = null!;
            monsterposition5 = null!;
            magicposition1 = null!;
            magicposition2 = null!;
            magicposition3 = null!;
            magicposition4 = null!;
            magicposition5 = null!;

            #endregion

        }

        /// <summary>Método de instancia para invocar una carta natural en una localización del tablero y en una de dos posiciones: (1)Ataque y (2)Defensa Boca Abajo. Además se le pasa el jugador pues según la afiliación de su reino, ya sea religiosa, militar, conocimiento o capital y los niveles de afiliación de la carta se decide en cuanto aumentan sus atributos atk, def y vit. Finalmente si el lugar a invocarla contiene una carta igual, entonces evoluciona.</summary>
        public Player Invocation(ACard card, (int f, int c) location, Player play1)
        {
            //Cobro el coste de invocación de la carta al reino
            play1.Kingdom.Faith -= card.FaithCost;
            play1.Kingdom.Capital -= card.CapitalCost;
            play1.Kingdom.Militarism -= card.MilitarCost;
            play1.Kingdom.Knowledge -= card.KnowledgeCost;

            //En caso de que venga del cementerio o la mano hace falta modificar el permiso de ataque y de usar efecto
            //Y actualizar su ubicación
            card.CanAttack = true;
            card.CanUseEffect = true;
            card.IsInCementery = false;
            card.IsInHand = false;
            card.IsInvoked = true;


            //Compruebo si en la posicion [f, c] de la propiedad Field se encuentra un espacio en blanco o la misma carta.
            if (location.c >= 0 && location.f >= 0 && Field[location.f, location.c] != null && card.CardTypeNum == 1 &&
            Field[location.f, location.c].CardLore == card.CardLore &&
            Field[location.f, location.c].CardName == card.CardName &&
            Field[location.f, location.c].EvolvedNames == card.EvolvedNames &&
            Field[location.f, location.c].RaceName == card.RaceName &&
            Field[location.f, location.c].RaceEvolvedNames == card.RaceEvolvedNames &&
            Field[location.f, location.c].ATKOriginal == card.ATKOriginal &&
            Field[location.f, location.c].DEFOriginal == card.DEFOriginal &&
            Field[location.f, location.c].VITOriginal == card.VITOriginal)
            {
                Field[location.f, location.c] = Evolution(card, play1);
            }

            //sino
            else if (location.c >= 0 && location.f >= 0)
            {
                //Le aplico el efecto de pertencer a un reino, busco la afiliación de la carta que coincide con la del reino
                for (int k = 0; k < card.Affiliations.Count; k++)
                {
                    //Cuando la encuentre aumento los atributos numéricos en dicha cantidad de afiliación por 10.
                    if (play1.Kingdom.Disposition == card.Affiliations[k].TypeOfKingdomAffiliation)
                    {
                        card.ATK += (card.Affiliations[k].amount * 10);
                        card.DEF += (card.Affiliations[k].amount * 10);
                        card.VIT += (card.Affiliations[k].amount * 10);
                    }
                }


                //y la pongo en el campo
                Field[location.f, location.c] = card;
            }


            //Y asigno desde aquí las posiciones de las criaturas y cartas mágicas, así al modificar la propiedad Field
            //estas también se modifiquen.
            monsterposition1 = Field[0, 0];
            monsterposition2 = Field[0, 1];
            monsterposition3 = Field[0, 2];
            monsterposition4 = Field[0, 3];
            monsterposition5 = Field[0, 4];

            magicposition1 = Field[1, 0];
            magicposition2 = Field[1, 1];
            magicposition3 = Field[1, 2];
            magicposition4 = Field[1, 3];
            magicposition5 = Field[1, 4];


            //Finalmente la elimino de la mano en caso que venga de la aquí
            foreach (ACard card0 in play1.CardsInHand.Hand)
            {
                if (card0 != null && card0.CardTypeNum == 1 &&
                        card0.CardElementName1 == card.CardElementName1 &&
                        card0.ATKOriginal == card.ATKOriginal &&
                        card0.DEFOriginal == card.DEFOriginal &&
                        card0.VITOriginal == card.VITOriginal)
                {
                    play1.CardsInHand.RemoveFromHand(card0);
                    break;
                }
            }

            return play1;
        }

        /// <summary>Método de instancia para invocar cartas mágicas que permanecen en el campo.</summary>
        public Player InvocationSpecial(ACard card, (int f, int c) location, Player user)
        {
            //Cobro el coste de invocación de la carta al reino
            user.Kingdom.Faith -= card.FaithCost;
            user.Kingdom.Capital -= card.CapitalCost;
            user.Kingdom.Militarism -= card.MilitarCost;
            user.Kingdom.Knowledge -= card.KnowledgeCost;

            user.CardsInHand.RemoveFromHand(card);

            //Y la coloco en el campo
            user.AField.SetCard(card, location);

            return user;
        }

        /// <summary>Método de instancia para devolver una carta al campo, en ataque y en algún lugar disponile. </summary>
        public void ReturnToField(ACard card, Player play1)
        {

            //Cobro el doble del coste de invocación de la carta al reino
            play1.Kingdom.Faith -= 2 * card.FaithCost;
            play1.Kingdom.Capital -= 2 * card.CapitalCost;
            play1.Kingdom.Militarism -= 2 * card.MilitarCost;
            play1.Kingdom.Knowledge -= 2 * card.KnowledgeCost;

            //Ahora para realizar el efecto de pertencer a un reino, busco la afiliación de la carta que coincide con la del reino
            for (int k = 0; k < card.Affiliations.Count; k++)
            {
                //Cuando la encuentre aumento los atributos numéricos en dicha cantidad de afiliación por 10.
                if (play1.Kingdom.Disposition == card.Affiliations[k].TypeOfKingdomAffiliation)
                {
                    card.ATK += (card.Affiliations[k].amount * 10);
                    card.DEF += (card.Affiliations[k].amount * 10);
                    card.VIT += (card.Affiliations[k].amount * 10);
                }
            }

            //Colocarla en algún lugar disponible del campo
            for (int k = 0; k < play1.AField.Field.GetLength(1); k++)
            {
                if (play1.AField.Field[0, k] == null)
                {
                    play1.AField.Field[0, k] = card;
                }
            }

            //En caso de que venga del cementerio o la mano hace falta modificar el permiso de ataque y de usar efecto
            //Y actualizar su ubicación
            card.CanAttack = true;
            card.CanUseEffect = true;
            card.IsInCementery = false;
            card.IsInHand = false;
            card.IsInvoked = true;

            //Y asigno desde aquí las posiciones de las criaturas y cartas mágicas, así al modificar la propiedad Field
            //estas también se modifiquen.
            monsterposition1 = Field[0, 0];
            monsterposition2 = Field[0, 1];
            monsterposition3 = Field[0, 2];
            monsterposition4 = Field[0, 3];
            monsterposition5 = Field[0, 4];

            magicposition1 = Field[1, 0];
            magicposition2 = Field[1, 1];
            magicposition3 = Field[1, 2];
            magicposition4 = Field[1, 3];
            magicposition5 = Field[1, 4];

        }

        /// <summary>Método auxiliar para evolucionar una carta por defecto.</summary>
        ACard Evolution(ACard card, Player play)
        {
            //Sus atributos numéricos aumentan en 1/4 del valor original y se evolucionan los elementos. Esto es independiente del número de evol que sea.
            Attribute evolvedatribute = new Attribute(card.ATK + Math.Round(card.ATK / 4), card.DEF + Math.Round(card.DEF / 4), card.VIT + Math.Round(card.VIT / 4), ElementEvolution(card));

            //los efectos evolucionados por ahora los igualo a los actuales
            Effects evolvedeffects = card.EffectsObject;

            //y la descripción igual la igualo a la actual
            Description evolveddescription = card.DescriptionObject;


            //si es su primera evolución
            if (card.EvolutionLevel == 1)
            {
                //cambio su nivel de evolución
                card.EvolutionLevel = 2;

                //en la descripción le cambio su nombre, se le agrega una línea al lore, se le evoluciona la raza y el precio de invocación se duplica.
                evolveddescription = new Description(card.EvolvedNames.evolname1 + " " + card.CardName, card.CardLore + " This card as evolved once.",
                card.ATypeObject, evolveddescription.RaceObject, card.CapitalCost * 2, card.FaithCost * 2, card.MilitarCost * 2, card.KnowledgeCost * 2);

                //dependiendo de si fue una carta creada
                if (card.IsACardCreatedByUser)
                {
                    //se evolucionan los efectos teniendo en cuenta que es una carta creada con un único efecto especial
                    evolvedeffects = EffectsEvolution(card.EffectsObject, 1, card, true);
                }

                //si no fue creada por un humano
                else
                {
                    //Tomo los efectos evolucionados de forma usual
                    evolvedeffects = EffectsEvolution(card.EffectsObject, 1, card);

                }
            }

            //En caso que sea su segunda evolución 
            else
            {
                //cambio su nivel de evolución
                card.EvolutionLevel = 3;

                //en la descripción cambia la nueva línea del lore y el efecto, el resto igual que arriba.
                evolveddescription = new Description(card.EvolvedNames.evolname2, card.CardLore +
                " This card as reached the peak of evolution.", card.ATypeObject, evolveddescription.RaceObject,
                 card.CapitalCost * 2, card.FaithCost * 2, card.MilitarCost * 2, card.KnowledgeCost * 2);

                //dependiendo de si fue una carta creada
                if (card.IsACardCreatedByUser)
                {
                    //se evolucionan los efectos teniendo en cuenta que es una carta creada con un único efecto especial
                    evolvedeffects = EffectsEvolution(card.EffectsObject, 2, card, true);
                }

                //sino fue creada por un humano
                else
                {
                    //Tomo los efectos evolucionados de forma usual
                    evolvedeffects = EffectsEvolution(card.EffectsObject, 2, card);
                }
            }

            //Se crea la nueva carta evolucionada 
            ACard evolvedcard = new ACard(evolvedatribute, evolvedeffects, evolveddescription);

            //y si fue creada pues con su respectivo constructor
            if (card.IsACardCreatedByUser)
            {
                evolvedcard = new(evolvedatribute, card.OEObject, evolveddescription, evolvedeffects);
                evolvedcard.IsACardCreatedByUser = true;
            }

            evolvedcard.EvolutionLevel = card.EvolutionLevel;

            //se le arreglan las propiedades faltantes

            //si evolucionó hasta el nivel 2 entonces falta agregar las propiedaded del efecto 3
            if (evolvedcard.EvolutionLevel == 2)
            {
                //si fue creada primero le falat el efecto 2
                if (evolvedcard.IsACardCreatedByUser && evolvedcard.EffectsObject.ObjectEffect2 != null)
                {
                    evolvedcard.Effect2 = evolvedcard.EffectsObject.ObjectEffect2.ASingleElementalEffect.action;
                    evolvedcard.EffectTxt2 = evolvedcard.EffectsObject.ObjectEffect2.ASingleElementalEffect.label;
                    evolvedcard.ListCostsFKCMEffect2 = evolvedcard.EffectsObject.ObjectEffect2.ASingleElementalEffect.costs;
                }

                //el efecto 3 puede ser de área o de daño elemental múltiple

                //si la propiedad de efecto de área no es null entonces es esa la que se guarda
                if (evolvedcard.EffectsObject.ObjectEffect3Area != null)
                {
                    evolvedcard.Effect3 = evolvedcard.EffectsObject.ObjectEffect3Area.AMidAreaEffect.action;
                    evolvedcard.EffectTxt3 = evolvedcard.EffectsObject.ObjectEffect3Area.AMidAreaEffect.label;
                    evolvedcard.ListCostsFKCMEffect3 = evolvedcard.EffectsObject.ObjectEffect3Area.AMidAreaEffect.costs;
                }

                //sino es de daño elemental múltiple
                else
                {
                    evolvedcard.Effect3 = evolvedcard.EffectsObject.ObjectEffect3Elemental.AMidAreaElementalEffects.action;
                    evolvedcard.EffectTxt3 = evolvedcard.EffectsObject.ObjectEffect3Elemental.AMidAreaElementalEffects.label;
                    evolvedcard.ListCostsFKCMEffect3 = evolvedcard.EffectsObject.ObjectEffect3Elemental.AMidAreaElementalEffects.costs;
                }
            }

            //sino evolucionó hasta el nivel 3 entonces falta agregar las propiedaded del efecto 4
            else if (evolvedcard.EvolutionLevel == 3)
            {
                evolvedcard.Effect4 = evolvedcard.EffectsObject.ObjectEffect4.AHighAreaEffect.action;
                evolvedcard.EffectTxt4 = evolvedcard.EffectsObject.ObjectEffect4.AHighAreaEffect.label;
                evolvedcard.ListCostsFKCMEffect4 = evolvedcard.EffectsObject.ObjectEffect4.AHighAreaEffect.costs;
            }

            if (evolvedcard.EvolutionLevel == 3)
            {
                evolvedcard.RaceName = evolvedcard.RaceEvolvedNames.evolname2;
            }

            else
            {
                evolvedcard.RaceName = evolvedcard.RaceEvolvedNames.evolname1;
            }

            play.DeckOfCards.Add(evolvedcard);
            play.DeckOfCards.Shuffle(play.DeckOfCards.Deck);

            return evolvedcard;

        }


        /// <summary>Método auxiliar para la evolución del/de los elemento/s.</summary>
        Element ElementEvolution(ACard card)
        {
            //Creo un Element igual al de carta
            Element evolved = card.ElementsObject;

            //Creo un random para asignar elementos y un int para saber cuantos tiene la carta antes de evolucionar.
            Random r = new Random();
            int cardoriginalelement = 0;

            //Creo una lista igual a la de los elementos que podría tener. A esta luego le quitare los elementos por defecto.
            List<string> elementcanhavewithout = new();
            elementcanhavewithout = card.ElementsTheyCanHave;

            //Lista para guardar el número del elemento nuevo 
            List<int> indofelement = new();

            //Pongo los elementos por defecto en una lista:
            List<(string elem1, int elem1num)> defaultelem = new();
            defaultelem.Add((card.CardElementName1, card.CardElementNum1));
            defaultelem.Add((card.CardElementName2, card.CardElementNum2));
            defaultelem.Add((card.CardElementName3, card.CardElementNum3));
            defaultelem.Add((card.CardElementName4, card.CardElementNum4));

            //En este caso la carta solo tiene un Elemento. Por evolución se le suma un elemento.
            if (card.CardElementName2 == "None")
            {
                //Le asigno valor 1 como cantidad original de elementos, este int se usa para que no entre a otros If
                cardoriginalelement = 1;

                //A la lista de todos los elementos que podría tener le quito los que tiene:
                elementcanhavewithout.Remove(card.CardElementName1);

                //Este es el valor de la cantidad de elementos en la lista de elementos que la carta según su raza puede usar.
                int cantmaxelements = elementcanhavewithout.Count;


                //Tomo el número del elemento existente y busco uno nuevo random del 1 a la cantidad de elementos que tenga la lista de todos
                //los que podría tener.
                int elem1num = card.CardElementNum1;
                int elem2num = ((int)r.Next(1, cantmaxelements));

                //Luego lo pongo en la lista que cree para eso:
                indofelement.Add(elem2num);

                //Aquí creo el nuevo elemento
                Element ev = new Element(elementcanhavewithout, indofelement, 1, defaultelem, 1);
                evolved = ev;

            }

            //En este caso la carta tiene 2 elementos originales
            if (card.CardElementName3 == "None" && cardoriginalelement == 0)
            {
                //Le asigno valor 2 como cantidad original de elementos, este int se usa para que no entre a otros If
                cardoriginalelement = 2;

                //A la lista de todos los elementos que podría tener le quito los que tiene:
                elementcanhavewithout = card.ElementsTheyCanHave;
                elementcanhavewithout.Remove(card.CardElementName1);
                elementcanhavewithout.Remove(card.CardElementName2);

                //Este es el valor de la cantidad de elementos en la lista de elementos que la carta según su raza puede usar.
                int cantmaxelements = elementcanhavewithout.Count;


                //Tomo el número de los 2 elementos existentes y busco uno nuevo random del 1 a la cantidad de elementos que tenga la lista de todos
                //los que podría tener.
                int elem1num = card.CardElementNum1;
                int elem2num = card.CardElementNum2;
                int elem3num = ((int)r.Next(1, cantmaxelements));

                //Luego lo pongo en la lista:
                indofelement.Add(elem3num);

                //Aquí creo el nuevo elemento
                Element ev = new Element(elementcanhavewithout, indofelement, 1, defaultelem, 2);
                evolved = ev;
            }

            //En este caso la carta tiene tres por defecto
            if (card.CardElementName4 == "None" && cardoriginalelement == 0)
            {
                //Le asigno valor 3 como cantidad original de elementos, este int se usa para que no entre a otros If
                cardoriginalelement = 3;

                //A la lista de todos los elementos que podría tener le quito los que tiene:
                elementcanhavewithout = card.ElementsTheyCanHave;
                elementcanhavewithout.Remove(card.CardElementName1);
                elementcanhavewithout.Remove(card.CardElementName2);
                elementcanhavewithout.Remove(card.CardElementName3);

                //Este es el valor de la cantidad de elementos en la lista de elementos que la carta según su raza puede usar.
                int cantmaxelements = elementcanhavewithout.Count;


                //Tomo el número de los 3 elementos existentes y busco uno nuevo random del 1 a la cantidad de elementos que tenga la lista de todos
                //los que podría tener.
                int elem1num = card.CardElementNum1;
                int elem2num = card.CardElementNum2;
                int elem3num = card.CardElementNum3;
                int elem4num = ((int)r.Next(1, cantmaxelements));

                //Luego lo pongo en la lista:
                indofelement.Add(elem4num);

                //Aquí creo el nuevo elemento
                Element ev = new Element(elementcanhavewithout, indofelement, 1, defaultelem, 3);
                evolved = ev;
            }

            //Intento crear el elemento asumiendo que algún if anterior se cumplió
            try
            {
                //Aquí obtengo los elementos de su evolución.
                Element evolvedelements = new Element(elementcanhavewithout, indofelement, 1, defaultelem, 1);
            }

            //sino entonces hay 4 elementos por defecto y no se puede agregar nada
            catch
            {
                //Tiene cuatro elementos, así que solo se mantienen
                Element evolvedelements = new Element();
                evolvedelements = card.ElementsObject;
            }

            return evolved;

        }

        /// <summary>Método auxiliar para la evolución de los efectos.</summary>
        Effects EffectsEvolution(Effects effects, int numofevol, ACard card)
        {
            //por ahora igualo los efectos evolucionados al efecto actual
            Effects evolvedeffects = effects;

            //creo un random
            System.Random r = new System.Random();

            //si es la primera evolución se le agrega el 3er efecto
            if (numofevol == 1)
            {
                //si el random entre 1 y 100 es mayor que 50
                if (r.Next(1, 101) > 50)
                {
                    //entonces el nuevo efecto es de área
                    AreaEffect ae = new AreaEffect(r.Next(0, 12), 1);

                    //el efecto evolucionado es los efectos 1 y 2 que ya tenía más el efecto nuevo de área
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect1, evolvedeffects.ObjectEffect2, ae);
                }

                //sino
                else
                {
                    //entonces el nuevo efecto es de daño elemental

                    //creo una lista de los posibles índices de los ataques que son equivalentes a los índices de sus elementos en la lista de todos los elementos

                    //ya que los ataques están en una lista guardados en el mismo orden que los elementos que se relacionan con ellos en la lista de todos los elementos

                    //ejemplo: elemento Fire es el índice 0 de la lista de todos los elementos y el ataque FireBolt es el índice 0 de la lista de ataques

                    List<int> possibleindexofattack0 = new();

                    //recorro por la lista de los 4 elementos de la carta
                    for (int k = 0; k < card.ActualFourElements.Count; k++)
                    {
                        //tomo el elemento actual
                        string elm = card.ActualFourElements[k];

                        //si el elemento es distinto de None
                        if (elm != "None")
                        {
                            //agrego su índice en la lista de todos los elementos a los índice de los posibles ataques - 1
                            //ya que el primer elemento es normal y no hay efecto para ese elemento
                            possibleindexofattack0.Add(card.ElementsObject.AllElements.elements.IndexOf(elm) - 1);
                        }
                    }

                    //creo el efecto de daño elemental random que coincida con algún elemento de la carta
                    ElementalDamage ed = new ElementalDamage(possibleindexofattack0[r.Next(0, possibleindexofattack0.Count)], 2);

                    //el efecto evolucionado es los efectos 1 y 2 que ya tenía más el efecto nuevo de daño elemental
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect1, evolvedeffects.ObjectEffect2, ed);
                }
            }

            //sino es su segunda evolución y se le agrega el 4rto efecto
            else
            {
                //creo un efecto de rango alto al azar entre los que existen
                AreaEffect ae = new AreaEffect(r.Next(0, 8), 2);

                //el efecto evolucionado es los efectos 1, 2 y 3 que ya tenía más el efecto nuevo de área

                //Compruebo si el 3er efecto es elemental o de área
                if (evolvedeffects.ObjectEffect3Area != null)
                {
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect1, evolvedeffects.ObjectEffect2, evolvedeffects.ObjectEffect3Area, ae);
                }

                else
                {
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect1, evolvedeffects.ObjectEffect2, evolvedeffects.ObjectEffect3Elemental, ae);

                }

            }

            return evolvedeffects;


        }

        /// <summary>Método auxiliar para la evolución de los efectos de cartas con un primer efecto especial.</summary>
        Effects EffectsEvolution(Effects effects, int numofevol, ACard card, bool IsACardCreatedByUser)
        {
            //por ahora igualo los efectos evolucionados al efecto actual
            Effects evolvedeffects = effects;

            //creo un random
            System.Random r = new System.Random();

            //si es la primera evolución se le agrega el 2do y 3er efecto
            if (numofevol == 1)
            {
                //creo una lista de los posibles índices de los ataques que son equivalentes a los índices de sus elementos en la lista de todos los elementos
                //ya que los ataques están en una lista guardados en el mismo orden que los elementos que se relacionan con ellos en la lista de todos los elementos
                //ejemplo: elemento Fire es el índice 0 de la lista y el ataque FireBolt es el índice 0 de su lista igual
                List<int> possibleindexofattack = new();

                //recorro por la lista de los 4 elementos de la carta
                for (int k = 0; k < card.ActualFourElements.Count; k++)
                {
                    //tomo el elemento actual
                    string elm = card.ActualFourElements[k];

                    //si el elemento es distinto de None
                    if (elm != "None")
                    {
                        //agrego su índice en la lista de todos los elementos a los índice de los posibles ataques
                        possibleindexofattack.Add(card.ElementsObject.AllElements.elements.IndexOf(elm));
                    }
                }

                //creo el efecto de daño elemental sencillo random que coincida con algún elemento de la carta
                ElementalDamage ed = new ElementalDamage(possibleindexofattack[r.Next(0, possibleindexofattack.Count)] - 1, 1);


                //si el random entre 1 y 100 es mayor que 50
                if (r.Next(1, 101) > 50)
                {
                    //entonces el 3er efecto es de área
                    AreaEffect ae = new AreaEffect(r.Next(0, 12), 1);

                    //el efecto evolucionado es los efectos 2 y 3 recién creados
                    evolvedeffects = new Effects(ed, ae);
                }

                //sino
                else
                {
                    //entonces el nuevo efecto es de daño elemental

                    //creo una lista de los posibles índices de los ataques que son equivalentes a los índices de sus elementos en la lista de todos los elementos
                    //ya que los ataques están en una lista guardados en el mismo orden que los elementos que se relacionan con ellos en la lista de todos los elementos
                    //ejemplo: elemento Fire es el índice 0 de la lista y el ataque FireBolt es el índice 0 de su lista igual
                    List<int> possibleindexofattack0 = new();

                    //recorro por la lista de los 4 elementos de la carta
                    for (int k = 0; k < card.ActualFourElements.Count; k++)
                    {
                        //tomo el elemento actual
                        string elm = card.ActualFourElements[k];

                        //si el elemento es distinto de None
                        if (elm != "None")
                        {
                            //agrego su índice en la lista de todos los elementos a los índice de los posibles ataques
                            possibleindexofattack0.Add(card.ElementsObject.AllElements.elements.IndexOf(elm));
                        }
                    }

                    //creo el efecto de daño elemental sencillo random que coincida con algún elemento de la carta
                    ElementalDamage ed2 = new ElementalDamage(possibleindexofattack0[r.Next(0, possibleindexofattack0.Count)], 2);

                    //el efecto evolucionado es los efectos 1 y 2 que ya tenía más el efecto nuevo de daño elemental
                    evolvedeffects = new Effects(ed, ed2);
                }
            }

            //sino es su segunda evolución y se le agrega el 4rto efecto
            else
            {
                //creo un efecto de rango alto al azar entre los que existen
                AreaEffect ae = new AreaEffect(r.Next(0, 8), 2);

                //el efecto evolucionado es los efectos 2 y 3 que ya tenía más el efecto nuevo de área

                //Compruebo si el 3er efecto es elemental o de área
                if (evolvedeffects.ObjectEffect3Area != null)
                {
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect2, evolvedeffects.ObjectEffect3Area, ae);
                }

                else
                {
                    evolvedeffects = new Effects(evolvedeffects.ObjectEffect2, evolvedeffects.ObjectEffect3Elemental, ae);

                }

            }

            return evolvedeffects;


        }

        /// <summary>Método de instancia para quitar cartas usadas o destruidas del campo.</summary>
        public void UsedOrDestroyed(ACard card)
        {
            //Recorro por el campo buscando la carta
            for (int k = 0; k < Field.GetLength(0); k++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    //Cuando aparezca su posición la pongo en null.
                    if (Field[k, j] != null && Field[k, j].CardName == card.CardName &&
                    Field[k, j].ATKOriginal == card.ATKOriginal &&
                    Field[k, j].DEFOriginal == card.DEFOriginal &&
                    Field[k, j].VITOriginal == card.VITOriginal &&
                    Field[k, j].CardElementName1 == card.CardElementName1)
                    {
                        Field[k, j] = null!;
                        break;
                    }
                }
            }

            //Y asigno desde aquí las posiciones de las criaturas y cartas mágicas, así al modificar la propiedad Field
            //estas también se modifiquen.
            monsterposition1 = Field[0, 0];
            monsterposition2 = Field[0, 1];
            monsterposition3 = Field[0, 2];
            monsterposition4 = Field[0, 3];
            monsterposition5 = Field[0, 4];

            magicposition1 = Field[1, 0];
            magicposition2 = Field[1, 1];
            magicposition3 = Field[1, 2];
            magicposition4 = Field[1, 3];
            magicposition5 = Field[1, 4];
        }

        /// <summary>Método de instancia para obtener las coordenadas de una carta en el campo.</summary>
        public (int x, int y) Find(ACard card)
        {
            for (int k = 0; k < Field.GetLength(0); k++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if (Field[k, j] != null && Field[k, j].CardName == card.CardName
                    && Field[k, j].CardLore == card.CardLore &&
                      Field[k, j].ATKOriginal == card.ATKOriginal &&
                       Field[k, j].DEFOriginal == card.DEFOriginal &&
                        Field[k, j].VITOriginal == card.VITOriginal)
                    {
                        return (k, j);
                    }

                }
            }

            return (-1, -1);
        }

        /// <summary>Método de instancia para obtener las coordenadas de un espacio vacío en el campo. (1) Para espacios de cartas naturales y (2) Para espacios de cartas mágicas. Cualquier otro int devuelve (1) por defecto.</summary>
        public (int x, int y) FindEmpty(int type)
        {
            if (type == 2)
            {
                for (int k = 0; k < Field.GetLength(1); k++)
                {
                    if (Field[1, k] == null)
                    {
                        return (1, k);
                    }
                }

                return (-1, -1);
            }

            else
            {
                for (int k = 0; k < Field.GetLength(1); k++)
                {
                    if (Field[0, k] == null)
                    {
                        return (0, k);
                    }
                }

                return (-1, -1);
            }
        }

        /// <summary>Método de instancia para poner una carta en el campo, sin contar costos o efectos.</summary>
        public void SetCard(ACard card, (int x, int y) location)
        {
            if (card != null && location.x >= 0 && location.y >= 0)
            {
                Field[location.x, location.y] = card;

                //Y asigno desde aquí las posiciones de las criaturas y cartas mágicas, así al modificar la propiedad Field
                //estas también se modifiquen.
                monsterposition1 = Field[0, 0];
                monsterposition2 = Field[0, 1];
                monsterposition3 = Field[0, 2];
                monsterposition4 = Field[0, 3];
                monsterposition5 = Field[0, 4];

                magicposition1 = Field[1, 0];
                magicposition2 = Field[1, 1];
                magicposition3 = Field[1, 2];
                magicposition4 = Field[1, 3];
                magicposition5 = Field[1, 4];
            }

        }

        /// <summary>Método de instancia para saber si una carta está en el campo.</summary>
        public bool IsInField(ACard card)
        {
            for (int k = 0; k < Field.GetLength(0); k++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if (Field[k, j] != null && Field[k, j].CardName == card.CardName
                   && Field[k, j].CardLore == card.CardLore &&
                     Field[k, j].ATKOriginal == card.ATKOriginal &&
                      Field[k, j].DEFOriginal == card.DEFOriginal &&
                       Field[k, j].VITOriginal == card.VITOriginal)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Método de instancia para comprobar si hay espacio en el campo para invocar una carta monstruo.</summary>
        public bool AreNaturalCardsInvokable()
        {
            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[0, k] == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método de instancia para comprobar si hay espacio en el campo para invocar una carta mágica.</summary>
        public bool AreMagicalCardsInvokable()
        {
            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[1, k] == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método de instancia para comprobar cuanto espacio hay en el campo para invocar una carta monstruo.</summary>
        public int AmountOfInvokableNaturalCards()
        {
            int res = 0;

            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[0, k] == null)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia para comprobar cuanto espacio hay en el campo para invocar una carta mágica.</summary>
        public int AmountOfInvokableMagicalCards()
        {
            int res = 0;

            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[1, k] == null)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia para comprobar si hay al menos una carta natural en el campo.</summary>
        public bool AreNaturalCardInvoked()
        {
            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[0, k] != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método de instancia para comprobar si hay al menos una carta mágica en el campo.</summary>
        public bool AreMagicalCardInvoked()
        {
            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[1, k] != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Método de instancia para contar cuantas cartas naturales hay en el campo.</summary>
        public int AmountOfNaturalCardInvoked()
        {
            int res = 0;

            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[0, k] != null)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia para contar cuantas cartas mágicas hay en el campo.</summary>
        public int AmountOfMagicalCardInvoked()
        {
            int res = 0;

            for (int k = 0; k < Field.GetLength(1); k++)
            {
                if (Field[1, k] == null)
                {
                    res++;
                }
            }

            return res;
        }

        /// <summary>Método de instancia que devuelve las cartas mágicas en el campo.</summary>
        public List<ACard> GetNaturalCardsInField()
        {
            List<ACard> cards = new();

            foreach (ACard card in Field)
            {
                if (card != null && card.CardTypeNum == 1)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        /// <summary>Método de instancia que devuelve las cartas mágicas en el campo.</summary>
        public List<ACard> GetMagicalCardsInField()
        {
            List<ACard> cards = new();

            foreach (ACard card in Field)
            {
                if (card != null && card.CardTypeNum == 2)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        /// <summary>Método de instancia que determina si el campo (parte de las cartas naturales) está vacío o no.</summary>
        public bool IsEmpty()
        {
            //recorro por el campo
            for (int k = 0; k < Field.GetLength(1); k++)
            {
                //si una posición no es null entonces no está vacío
                if (Field[0, k] != null)
                {
                    return false;
                }
            }

            return true;
        }

    }

}
