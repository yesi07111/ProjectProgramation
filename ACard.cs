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
    public interface ICard
    {
        public string CardName { get; set; }
        public string CardLore { get; set; }
        public string CardTypeName { get; set; }
        public int CardTypeNum { get; set; }
        public string RaceName { get; set; }
        public (string evolname1, string evolname2) RaceEvolvedNames { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public (string evolname1, string evolname2) EvolvedNames { get; set; }
        public List<string> ElementsTheyCanHave { get; set; }
        public double FaithCost { get; set; }
        public double MilitarCost { get; set; }
        public double CapitalCost { get; set; }
        public double KnowledgeCost { get; set; }
        public List<double> ListOfCostFKCM { get; set; }

        public double ATK { get; set; }
        public double DEF { get; set; }
        public double VIT { get; set; }
        public string CardElementName1 { get; set; }
        public string CardElementName2 { get; set; }
        public string CardElementName3 { get; set; }
        public string CardElementName4 { get; set; }
        public int CardElementNum1 { get; set; }
        public int CardElementNum2 { get; set; }
        public int CardElementNum3 { get; set; }
        public int CardElementNum4 { get; set; }
        public List<string> ActualFourElements { get; set; }

        public Attribute AttributesObject { get; set; }
        public Effects EffectsObject { get; set; }
        public ObjectEffect OEObject { get; set; }
        public Description DescriptionObject { get; set; }
        public AType ATypeObject { get; set; }
        public Element ElementsObject { get; set; }
        public Race RaceObject { get; set; }


        public (int f, int c) LocationInField { get; set; }
        public int EvolutionLevel { get; set; }
        public int StartingEvolutionLevel { get; set; }
        public bool IsInHand { get; set; }
        public bool IsInCementery { get; set; }
        public bool IsInvoked { get; set; }
        public bool ActivateEffectOrAttackOnce { get; set; }
        public bool ChangedPositionInField { get; set; }
        public bool AsAnAbnormalState { get; set; }
        public bool CanAttack { get; set; }
        public bool CanUseEffect { get; set; }
        public bool CanSelectTarget { get; set; }
        public int RestrictAttackTurns { get; set; }
        public int RestrictEffectTurns { get; set; }
        public int AbnormalStateDuration { get; set; }
        public Dictionary<string, int> ListOfAbnormalStateDuration { get; set; }
        public bool AsPhysicalInmunity { get; set; }
        public bool AsEffectInmunity { get; set; }
        public bool AsInmunityChance { get; set; }
        public int InmunityPhysicalDuration { get; set; }
        public int InmunityEffectDuration { get; set; }
        public int InmunityChanceDuration { get; set; }
        public string CardState { get; set; }
        public bool InfectedOther { get; set; }
        public bool IsInfected { get; set; }
        public bool IsACardCreatedByUser { get; set; }
        public void Reset();
        public ACard Clone(ACard card);

        public Func<Player, List<double>, ACard, ACard, double, Player, Player[]> Effect1 { get; set; }
        public Func<Player, List<double>, ACard, ACard, double, Player, Player[]> Effect2 { get; set; }
        public Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> Effect3 { get; set; }
        public Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> Effect4 { get; set; }



    }

    /// <summary>Clase de las cartas. Cada carta tiene atributos, efectos y descripción, quienes a su vez son objetos de instancia. Los atributos consisten en ATK, DEF, VIT y Elemento(s). La descripción consiste de un nombre, un lore, el tipo de carta, raza, clase y costo de invocación. Los efectos varían.</summary>
    public class ACard : ICard
    {
        #region Propiedades de las cartas:

        //Derivadas de la descripción.
        public string CardName { get; set; }
        public string CardLore { get; set; }
        public string CardTypeName { get; set; }
        public int CardTypeNum { get; set; }
        public string RaceName { get; set; }
        public (string evolname1, string evolname2) RaceEvolvedNames { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public (string evolname1, string evolname2) EvolvedNames { get; set; }
        public List<string> ElementsTheyCanHave { get; set; }
        public double FaithCost { get; set; }
        public double MilitarCost { get; set; }
        public double CapitalCost { get; set; }
        public double KnowledgeCost { get; set; }
        public List<double> ListOfCostFKCM { get; set; }

        //Derivadas de los atributos.
        public double ATK { get; set; }
        public double DEF { get; set; }
        public double VIT { get; set; }
        public double ATKOriginal { get; set; }
        public double DEFOriginal { get; set; }
        public double VITOriginal { get; set; }
        public string CardElementName1 { get; set; }
        public string CardElementName2 { get; set; }
        public string CardElementName3 { get; set; }
        public string CardElementName4 { get; set; }
        public int CardElementNum1 { get; set; }
        public int CardElementNum2 { get; set; }
        public int CardElementNum3 { get; set; }
        public int CardElementNum4 { get; set; }
        public List<string> ActualFourElements { get; set; }


        //Propiedades para acceder a los objetos de instancia atributo, efecto, descripción, tipo, elemento, raza y clase.
        public Attribute AttributesObject { get; set; }
        public Effects EffectsObject { get; set; }
        public ObjectEffect OEObject { get; set; }
        public Description DescriptionObject { get; set; }
        public AType ATypeObject { get; set; }
        public Element ElementsObject { get; set; }
        public Race RaceObject { get; set; }

        //Propiedades a usar cuando la carta esté en el campo.
        public (int f, int c) LocationInField { get; set; }
        public int EvolutionLevel { get; set; }
        public int StartingEvolutionLevel { get; set; }

        //Propiedades para verificar cambios de estado
        public bool IsInHand { get; set; }
        public bool IsInCementery { get; set; }
        public bool IsInvoked { get; set; }
        public bool ActivateEffectOrAttackOnce { get; set; }
        public bool ChangedPositionInField { get; set; }
        public bool AsAnAbnormalState { get; set; }
        public bool AsManyAbnormalState { get; set; }
        public bool CanAttack { get; set; }
        public bool CanUseEffect { get; set; }
        public bool CanSelectTarget { get; set; }
        public int RestrictAttackTurns { get; set; }
        public int RestrictEffectTurns { get; set; }
        public int AbnormalStateDuration { get; set; }
        public Dictionary<string, int> ListOfAbnormalStateDuration { get; set; }
        public bool AsPhysicalInmunity { get; set; }
        public bool AsEffectInmunity { get; set; }
        public bool AsInmunityChance { get; set; }
        public int InmunityPhysicalDuration { get; set; }
        public int InmunityEffectDuration { get; set; }
        public int InmunityChanceDuration { get; set; }
        public string CardState { get; set; }
        public List<string> CardStates { get; set; }
        public bool InfectedOther { get; set; }
        public bool IsInfected { get; set; }

        //Propiedad para identificar carta
        public int CardID { get; set; }
        public bool IsACardCreatedByUser { get; set; }

        //Propiedades de los efectos: 1 y 2 Efectos sencillos de un único objetivo. 3 y 4 de múltiples o uno.
        public Func<Player, List<double>, ACard, ACard, double, Player, Player[]> Effect1 { get; set; }
        public Func<Player, List<double>, ACard, ACard, double, Player, Player[]> Effect2 { get; set; }
        public Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> Effect3 { get; set; }
        public Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> Effect4 { get; set; }
        public (string name, string description) EffectTxt1 { get; set; }
        public (string name, string description) EffectTxt2 { get; set; }
        public (string name, string description) EffectTxt3 { get; set; }
        public (string name, string description) EffectTxt4 { get; set; }
        public int SpecialEffectID { get; set; }
        public (string name, string description) SpecialEffectTxt { get; set; }
        public double[] ListCostsFKCMEffect1 { get; set; }
        public double[] ListCostsFKCMEffect2 { get; set; }
        public double[] ListCostsFKCMEffect3 { get; set; }
        public double[] ListCostsFKCMEffect4 { get; set; }
        public double[] ListCostsFKCMSpecialEffect { get; set; }

        #endregion

        //Contructor de la carta. Se le pasan como parámetros los objetos de instancia Atributo, Efecto y Descripción.
        public ACard(Attribute atributes, Effects efects, Description description)
        {
            //eliminando advertencias
            ListOfAbnormalStateDuration = new();
            Effect1 = null!;
            Effect2 = null!;
            Effect3 = null!;
            Effect4 = null!;
            ListCostsFKCMEffect1 = null!;
            ListCostsFKCMEffect2 = null!;
            ListCostsFKCMEffect3 = null!;
            ListCostsFKCMEffect4 = null!;
            OEObject = null!;
            CardStates = new();
            ListCostsFKCMSpecialEffect = new double[4];


            //Propiedades para acceder a los objetos de instancia atributo, efecto, descripción, tipo, elemento, raza y clase.
            AttributesObject = atributes;               //objeto atributo
            EffectsObject = efects;                     //objeto efecto
            DescriptionObject = description;            //objeto descripción
            ATypeObject = description.ATypeObject;      //objeto tipo
            ElementsObject = atributes.ElementsObject;  //objeto elemento
            RaceObject = description.RaceObject;        //objeto raza

            //Propiedades de la descripción:  
            CardName = description.CardName;                            //nombre
            CardLore = description.CardLore;                            //lore
            CardTypeName = description.CardTypeName;                    //nombre del tipo
            CardTypeNum = description.CardTypeNum;                      //número del tipo
            RaceName = description.RaceName;                            //nombre de raza
            Affiliations = description.Affiliations;                    //afiliaciones
            EvolvedNames = description.CardEvolvedNames;                    //nombres cuando evolucionan
            RaceEvolvedNames = description.RaceEvolvedNames;
            ElementsTheyCanHave = description.ElementsTheyCanHave;      //elementos que puede tener
            FaithCost = description.InvocationCostFaith;                //costo de fé 
            MilitarCost = description.InvocationCostMilitarism;         //costo de fuerza militar
            CapitalCost = description.InvocationCostCapital;            //costo de capital
            KnowledgeCost = description.InvocationCostKnowledge;        //costo de conocimiento
            ListOfCostFKCM = new();                                     //instancio la lista de costos en orden Fe, Conocimiento, Militar y Capital
            ListOfCostFKCM.Add(FaithCost);                              //añado costo de fé
            ListOfCostFKCM.Add(KnowledgeCost);                          //añado costo de conocimiento
            ListOfCostFKCM.Add(CapitalCost);                            //añado costo de capital
            ListOfCostFKCM.Add(MilitarCost);                            //añado costo militar


            //Propiedades de los atributos:
            ATK = atributes.ATK;                                                    //Ataque
            DEF = atributes.DEF;                                                    //Defensa
            VIT = atributes.VIT;                                                    //Vitalidad
            ATKOriginal = atributes.ATK;                                            //Ataque Original
            DEFOriginal = atributes.DEF;                                            //Defensa Original
            VITOriginal = atributes.VIT;                                            //Vitalidad Original
            CardElementName1 = atributes.ElementsObject.Element1.elementname;       //Elemento 1 nombre
            CardElementName2 = atributes.ElementsObject.Element2.elementname;       //Elemento 2 nombre
            CardElementName3 = atributes.ElementsObject.Element3.elementname;       //Elemento 3 nombre  
            CardElementName4 = atributes.ElementsObject.Element4.elementname;       //Elemento 4 nombre
            CardElementNum1 = atributes.ElementsObject.Element1.elementnum;         //Elemento 1 número
            CardElementNum2 = atributes.ElementsObject.Element2.elementnum;         //Elemento 2 número   
            CardElementNum3 = atributes.ElementsObject.Element3.elementnum;         //Elemento 3 número                                                                                                                                        
            CardElementNum4 = atributes.ElementsObject.Element4.elementnum;         //Elemento 4 número
            ActualFourElements = new();                                             //Inicializo la lista de los 4 elementos de la carta
            ActualFourElements.Add(CardElementName1);                               //Y guardo el elemento 1
            ActualFourElements.Add(CardElementName2);                               //El elemento 2
            ActualFourElements.Add(CardElementName3);                               //El elemento 3
            ActualFourElements.Add(CardElementName4);                               //Y el elemento 4


            //Valores por defecto localización y nivel de evolución. Se deben modificar cuando la carta sea invocada o evolucione.
            LocationInField = (-1, -1);
            StartingEvolutionLevel = 1;

            if (CardTypeNum == 2)
            {
                EvolutionLevel = 0;
                StartingEvolutionLevel = 0;
            }

            //Estados por defecto verdaderos:
            CanAttack = true;
            CanUseEffect = true;
            CanSelectTarget = true;
            CardState = "Normal";

            //Propiedades de los efectos

            //si es carta mágica tiene un solo efecto, y su costo de efecto pasa a ser su costo de invocación
            if (CardTypeNum == 2)
            {
                if (efects.ObjectEffect1 != null)
                {
                    Effect1 = efects.ObjectEffect1.ASingleMagicalEffect.action;                 //delegado que realiza el efecto al ser invocado
                    EffectTxt1 = efects.ObjectEffect1.ASingleMagicalEffect.label;               //Nombre y Descripción del efecto

                    ListCostsFKCMEffect1 = efects.ObjectEffect1.ASingleMagicalEffect.costs;     //lista de costos de su efecto

                    //Arreglo los costos de invocación
                    FaithCost = ListCostsFKCMEffect1[0];                                        //costo de fé 
                    KnowledgeCost = ListCostsFKCMEffect1[1];                                    //costo de conocimiento
                    CapitalCost = ListCostsFKCMEffect1[2];                                      //costo de capital
                    MilitarCost = ListCostsFKCMEffect1[3];                                      //costo de fuerza militar

                    ListOfCostFKCM = new();                                                     // vuelvo a instanciar la lista de costos de invocación
                    ListOfCostFKCM.Add(FaithCost);                                              //añado el nuevo costo de fé
                    ListOfCostFKCM.Add(KnowledgeCost);                                          //añado el nuevo costo de conocimiento
                    ListOfCostFKCM.Add(CapitalCost);                                            //añado el nuevo costo de capital
                    ListOfCostFKCM.Add(MilitarCost);                                            //añado el nuevo costo militar
                }

            }

            //si no es mágica
            else
            {
                if (efects.ObjectEffect1 != null)
                {
                    Effect1 = efects.ObjectEffect1.ASingleNaturalEffect.action;
                    EffectTxt1 = efects.ObjectEffect1.ASingleNaturalEffect.label;
                    ListCostsFKCMEffect1 = efects.ObjectEffect1.ASingleNaturalEffect.costs;
                }

                if (efects.ObjectEffect2 != null)
                {
                    Effect2 = efects.ObjectEffect2.ASingleElementalEffect.action;
                    EffectTxt2 = efects.ObjectEffect2.ASingleElementalEffect.label;
                    ListCostsFKCMEffect2 = efects.ObjectEffect2.ASingleElementalEffect.costs;
                }

                if (efects.ObjectEffect3Area != null)
                {
                    Effect3 = efects.ObjectEffect3Area.AMidAreaEffect.action;
                    EffectTxt3 = efects.ObjectEffect3Area.AMidAreaEffect.label;
                    ListCostsFKCMEffect3 = efects.ObjectEffect3Area.AMidAreaEffect.costs;
                }

                if (efects.ObjectEffect3Elemental != null)
                {
                    Effect3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.action;
                    EffectTxt3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.label;
                    ListCostsFKCMEffect3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.costs;
                }

                if (efects.ObjectEffect4 != null)
                {
                    Effect4 = efects.ObjectEffect4.AHighAreaEffect.action;
                    EffectTxt4 = efects.ObjectEffect4.AHighAreaEffect.label;
                    ListCostsFKCMEffect4 = efects.ObjectEffect4.AHighAreaEffect.costs;
                }


            }

        }

        public ACard(Attribute atributes, ObjectEffect oe, Description description, Effects efects)
        {
            //eliminando advertencias
            ListOfAbnormalStateDuration = new();
            Effect1 = null!;
            Effect2 = null!;
            Effect3 = null!;
            Effect4 = null!;
            ListCostsFKCMEffect1 = null!;
            ListCostsFKCMEffect2 = null!;
            ListCostsFKCMEffect3 = null!;
            ListCostsFKCMEffect4 = null!;
            EffectsObject = null!;
            CardStates = new();

            //Propiedades para acceder a los objetos de instancia atributo, efecto, descripción, tipo, elemento, raza y clase.
            AttributesObject = atributes;               //objeto atributo
            DescriptionObject = description;            //objeto descripción
            ATypeObject = description.ATypeObject;      //objeto tipo
            ElementsObject = atributes.ElementsObject;  //objeto elemento
            RaceObject = description.RaceObject;        //objeto raza

            //Propiedades de la descripción:  
            CardName = description.CardName;                            //nombre
            CardLore = description.CardLore;                            //lore
            CardTypeName = description.CardTypeName;                    //nombre del tipo
            CardTypeNum = description.CardTypeNum;                      //número del tipo
            RaceName = description.RaceName;                            //nombre de raza
            Affiliations = description.Affiliations;                    //afiliaciones
            EvolvedNames = description.CardEvolvedNames;                    //nombres cuando evolucionan
            RaceEvolvedNames = description.RaceEvolvedNames;
            ElementsTheyCanHave = description.ElementsTheyCanHave;      //elementos que puede tener
            FaithCost = description.InvocationCostFaith;                //costo de fé 
            MilitarCost = description.InvocationCostMilitarism;         //costo de fuerza militar
            CapitalCost = description.InvocationCostCapital;            //costo de capital
            KnowledgeCost = description.InvocationCostKnowledge;        //costo de conocimiento
            ListOfCostFKCM = new();                                     //instancio la lista de costos en orden Fe, Conocimiento, Militar y Capital
            ListOfCostFKCM.Add(FaithCost);                              //añado costo de fé
            ListOfCostFKCM.Add(KnowledgeCost);                          //añado costo de conocimiento
            ListOfCostFKCM.Add(CapitalCost);                            //añado costo de capital
            ListOfCostFKCM.Add(MilitarCost);                            //añado costo militar


            //Propiedades de los atributos:
            ATK = atributes.ATK;                                                    //Ataque
            DEF = atributes.DEF;                                                    //Defensa
            VIT = atributes.VIT;                                                    //Vitalidad
            ATKOriginal = atributes.ATK;                                            //Ataque Original
            DEFOriginal = atributes.DEF;                                            //Defensa Original
            VITOriginal = atributes.VIT;                                            //Vitalidad Original
            CardElementName1 = atributes.ElementsObject.Element1.elementname;       //Elemento 1 nombre
            CardElementName2 = atributes.ElementsObject.Element2.elementname;       //Elemento 2 nombre
            CardElementName3 = atributes.ElementsObject.Element3.elementname;       //Elemento 3 nombre  
            CardElementName4 = atributes.ElementsObject.Element4.elementname;       //Elemento 4 nombre
            CardElementNum1 = atributes.ElementsObject.Element1.elementnum;         //Elemento 1 número
            CardElementNum2 = atributes.ElementsObject.Element2.elementnum;         //Elemento 2 número   
            CardElementNum3 = atributes.ElementsObject.Element3.elementnum;         //Elemento 3 número                                                                                                                                        
            CardElementNum4 = atributes.ElementsObject.Element4.elementnum;         //Elemento 4 número
            ActualFourElements = new();                                             //Inicializo la lista de los 4 elementos de la carta
            ActualFourElements.Add(CardElementName1);                               //Y guardo el elemento 1
            ActualFourElements.Add(CardElementName2);                               //El elemento 2
            ActualFourElements.Add(CardElementName3);                               //El elemento 3
            ActualFourElements.Add(CardElementName4);                               //Y el elemento 4


            //Valores por defecto localización y nivel de evolución. Se deben modificar cuando la carta sea invocada o evolucione.
            LocationInField = (-1, -1);
            StartingEvolutionLevel = 1;

            if (CardTypeNum == 2)
            {
                EvolutionLevel = 0;
                StartingEvolutionLevel = 0;
            }

            //Estados por defecto verdaderos:
            CanAttack = true;
            CanUseEffect = true;
            CanSelectTarget = true;
            CardState = "Normal";
            IsACardCreatedByUser = true;
            SpecialEffectID = oe.ID;
            SpecialEffectTxt = oe.Tuple;
            OEObject = oe;
            ListCostsFKCMSpecialEffect = new double[4];
            ListCostsFKCMSpecialEffect[0] = oe.FaithCost;
            ListCostsFKCMSpecialEffect[1] = oe.KnowledgeCost;
            ListCostsFKCMSpecialEffect[2] = CapitalCost;
            ListCostsFKCMSpecialEffect[3] = MilitarCost;
            EffectsObject = efects;

            if (efects.ObjectEffect2 != null)
            {
                Effect2 = efects.ObjectEffect2.ASingleElementalEffect.action;
                EffectTxt2 = efects.ObjectEffect2.ASingleElementalEffect.label;
                ListCostsFKCMEffect2 = efects.ObjectEffect2.ASingleElementalEffect.costs;
            }

            if (efects.ObjectEffect3Area != null)
            {
                Effect3 = efects.ObjectEffect3Area.AMidAreaEffect.action;
                EffectTxt3 = efects.ObjectEffect3Area.AMidAreaEffect.label;
                ListCostsFKCMEffect3 = efects.ObjectEffect3Area.AMidAreaEffect.costs;
            }

            if (efects.ObjectEffect3Elemental != null)
            {
                Effect3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.action;
                EffectTxt3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.label;
                ListCostsFKCMEffect3 = efects.ObjectEffect3Elemental.AMidAreaElementalEffects.costs;
            }

            if (efects.ObjectEffect4 != null)
            {
                Effect4 = efects.ObjectEffect4.AHighAreaEffect.action;
                EffectTxt4 = efects.ObjectEffect4.AHighAreaEffect.label;
                ListCostsFKCMEffect4 = efects.ObjectEffect4.AHighAreaEffect.costs;
            }

        }

        /// <summary>Comprueba si la carta tiene un elemento.</summary>
        public bool ContainElement(string elem)
        {
            return ActualFourElements.Contains(elem);
        }

        /// <summary>Resetea la carta a sus valores originales (por defecto).</summary>
        public void Reset()
        {
            ATK = ATKOriginal;
            DEF = DEFOriginal;
            VIT = VITOriginal;
            LocationInField = (-1, -1);
            IsInHand = false;
            IsInCementery = false;
            IsInvoked = false;

            ActivateEffectOrAttackOnce = false;
            ChangedPositionInField = false;
            AsAnAbnormalState = false;
            AsManyAbnormalState = false;
            CanAttack = true;
            CanUseEffect = true;
            CanSelectTarget = true;
            RestrictAttackTurns = 0;
            RestrictEffectTurns = 0;
            AbnormalStateDuration = 0;
            ListOfAbnormalStateDuration = new();
            AsPhysicalInmunity = false;
            AsEffectInmunity = false;
            AsInmunityChance = false;
            InmunityPhysicalDuration = 0;
            InmunityEffectDuration = 0;
            InmunityChanceDuration = 0;
            InfectedOther = false;
            IsInfected = false;
            CardState = "Normal";
            ListOfAbnormalStateDuration = new();
        }

        /// <summary>Resetea el ATK a su valor original en el campo.</summary>
        public void ResetATK(Player play1)
        {
            ATK = ATKOriginal;
            ACard card = this;

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
        }

        /// <summary>Resetea la DEF a su valor original en el campo.</summary>
        public void ResetDEF(Player play1)
        {
            DEF = DEFOriginal;
            ACard card = this;

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

        }

        /// <summary>Resetea la VIT a su valor original en el campo.</summary>
        public void ResetVIT(Player play1)
        {
            VIT = VITOriginal;
            ACard card = this;

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
        }

        /// <summary>Resetea la VIT a su valor original.</summary>
        public void ResetATKToOrigen()
        {
            VIT = VITOriginal;
        }

        /// <summary>Resetea la DEF a su valor original.</summary>
        public void ResetDEFToOrigen()
        {
            DEF = DEFOriginal;
        }

        /// <summary>Resetea la VIT a su valor original.</summary>
        public void ResetVITToOrigen()
        {
            VIT = VITOriginal;
        }

        /// <summary>Método para clonar una carta.</summary>
        public ACard Clone(ACard card)
        {
            ACard card0 = card;
            return card0;
        }

    }


}