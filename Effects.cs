using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Data.Common;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    /// <summary>Cada objeto tipo efecto debe tener: un método de ataque básico o ataque por defecto, ya que toda carta incluso si no tuviera efectos debe poder atacar; además debe tener la lista de efectos de estados y la relación de estos con los elementos.</summary>
    public interface IEffect
    {
        //Un delegado que tiene como parámetros un usuario que es un tipo Player, 
        //lista de los costos de ataque (0 en este caso), el enemigo que es tipo Player, lista de las cartas objetivos
        //y el tipo de retorno que es un array con los player usuario y enemigo
        public Func<Player, List<double>, ACard, Player, ACard[], Player[]> NormalAttack { get; set; }

        //lista de estados
        public List<string> states { get; set; }

        //Diccionario de estados y sus elementos asociados como values
        public Dictionary<string, string> statesandelements { get; set; }

    }

    /// <summary>Clase general de efectos, contiene el ataque básico a otra carta o ataque directo.</summary>
    public class Effects : IEffect
    {

        #region Propiedades desagradables a la vista, se recomienda leer despacio

        //***Nota: Cada propiedad que representa un efecto es un delegado, con una tupla que contiene nombre y descripción del
        //*** efecto y una colección de sus costos.

        //*** El Delegado es Func y sus parámetros son: 
        //*** >Player: jugador/IA que activa el efecto (user),  
        //*** >List<double>: lista de costos de efeto en orden FKCM (Primero costo de Faith, Knowledge, Capital, Militarism) (costs)
        //*** >ACard: carta que activa el efecto   (card)
        //*** >Player: jugador/IA enemigo (enemy)
        //*** >ACard/ACard[]: carta o cartas objetivo (targets/tgt)
        //*** Tipo de retorno: Player[]. Contiene al {user, enemy}. En algunos casos no afecta a enemy el efecto y devuelve {user, user} 
        //*** >La Tupla tiene la forma (string name, string txt)
        //*** >La colección de los costos es double[], y los costos vienen en orden FKCM

        //Propiedad del ataque normal, que es un delegado.
        public Func<Player, List<double>, ACard, Player, ACard[], Player[]> NormalAttack { get; set; }

        //Propiedad que es la lista de estados, el diccionario que relaciona un estado con el elemento que lo provoca
        //y el diccionario que relaciona el nombre del ataque con su estado
        public List<string> states { get; set; }
        public Dictionary<string, string> statesandelements { get; set; }
        public Dictionary<string, string> attackandelements { get; set; }


        //Lista de delegados que son los efectos sencillos o de un solo objetivo. Se les pasa el jugador que la activa, la carta que lo hace,
        //la carta objetivo, la cantidad del efecto, el jugador que tiene la carta objetivo y lista de precios en orden de Fe, Conocimiento, Capital y Militar.
        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,
        (string name, string effectdesc) label, double[] costs)> EBSingleMagicalEffects           //Efectos de cartas mágicas
        { get; set; }


        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,
       (string name, string effectdesc) label, double[] costs)> EBSingleNaturalEffects            //Efectos de cartas naturales
        { get; set; }


        //Lista de efectos de rango medio (afectan 3 enemigos)
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,   //Efectos de cartas naturales 
         (string name, string effect) label, double[] costs)> EBMidAreaEffects                       //de rango medio
        { get; set; }


        //Lista de efectos de rango total (afectan todos enemigos o son overpowered)
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,    //Efectos de cartas naturales
        (string name, string effect) label, double[] costs)> EBHighAreaEffects                        //de rango alto
        { get; set; }



        //lista de los efectos elementales que afectan un solo objetivo
        public List<(Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action, //Efectos Elementales sencillos
      (string name, string effectdesc) label, double[] costs)> EBSingleElementalEffect            //de cartas naturales
        { get; set; }


        //lista de los efectos elementales que afectan varios objetivos             
        public List<(Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action, //Efectos Elementales Múltiples
        (string name, string effect) label, double[] costs)> EBMidAreaElementalEffects               //de cartas naturales
        { get; set; }


        //Diccionario que relaciona el nombre del efecto y su método
        public Dictionary<string, Func<ACard, ACard>> EBStatesEffectPerTurn { get; set; }


        //Propiedades de los efectos en cuestión al crear una carta

        public (Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,  //Un Efecto
        (string name, string effectdesc) label, double[] costs) ASingleMagicalEffect        //De carta
        { get; set; }                                                                       //Mágica


        public (Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action, //Un Efecto
       (string name, string effectdesc) label, double[] costs) ASingleNaturalEffect         //De carta
        { get; set; }                                                                       //Natural;


        public (Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,   //Un Efecto de cartas naturales 
                 (string name, string effect) label, double[] costs) AMidAreaEffect            //de rango medio
        { get; set; }

        public (Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,    //Un Efecto de cartas naturales
                (string name, string effect) label, double[] costs) AHighAreaEffect              //de rango alto
        { get; set; }


        public (Func<Player, List<double>, ACard, ACard, double, Player, Player[]> action,          //Un Efecto Elemental sencillo
              (string name, string effectdesc) label, double[] costs) ASingleElementalEffect         //de cartas naturales
        { get; set; }


        public (Func<Player, List<double>, ACard, ACard[], double, Player, Player[]> action,        //Un Efecto Elemental Múltiple
        (string name, string effect) label, double[] costs) AMidAreaElementalEffects                //de cartas naturales
        { get; set; }

        //Listas de nombres de efectos naturales
        public List<string> NaturalEffectToDealDamageSingle { get; set; }
        public List<string> NaturalEffectToDealDamageMultiple { get; set; }
        public List<string> NaturalEffectToDealElementalDamage { get; set; }
        public List<string> NaturalEffectToChangeAllyStats { get; set; }
        public List<string> NaturalEffectToChangeEnemyStats { get; set; }
        public List<string> NaturalEffectToDealInmunity { get; set; }
        public List<string> NaturalEffectToPutRestrictions { get; set; }
        public List<string> NaturalEffectsToCureEffectsStatus { get; set; }
        public List<string> NaturalEffectsToRemoveRestrictions { get; set; }
        public List<string> NaturalEffectsToReviveCards { get; set; }
        public List<string> NaturalEffectsOthers { get; set; }

        //Listas de nombres de efectos mágicos
        public List<string> MagicalEffectToDealDamage { get; set; }
        public List<string> MagicalEffectToChangeAllyStats { get; set; }
        public List<string> MagicalEffectToChangeEnemyStats { get; set; }
        public List<string> MagicalEffectToPutRestrictions { get; set; }
        public List<string> MagicalEffectsToCureEffectsStatus { get; set; }
        public List<string> MagicalEffectsToRemoveRestrictions { get; set; }
        public List<string> MagicalEffectsToReviveCards { get; set; }
        public List<string> MagicalEffectsOthers { get; set; }

        //Objetos que representan los efectos
        public SingleEffect ObjectEffect1 { get; set; }
        public ElementalDamage ObjectEffect2 { get; set; }
        public AreaEffect ObjectEffect3Area { get; set; }
        public ElementalDamage ObjectEffect3Elemental { get; set; }
        public AreaEffect ObjectEffect4 { get; set; }


        #endregion

        //Tiene muchos constructores para cada caso a la hora de evolucionar los efectos

        /// <summary>En el constructor vacío se asigan únicamente las propiedades que no tienen que ver con efectos específicos.</summary>
        public Effects()
        {
            //Asigno el valor del ataque normal
            NormalAttack = DoingEffect.DamageDealer;

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
        }

        /// <summary>Constructor de 1 efecto, para cartas mágicas</summary>
        public Effects(SingleEffect se)
        {

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();

            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ASingleMagicalEffect = se.ASingleMagicalEffect;

        }

        /// <summary>Constructor con efecto 1 y efecto 2 de la carta.</summary>
        public Effects(SingleEffect se, ElementalDamage ed)
        {
            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();

            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ObjectEffect2 = ed;
            ASingleNaturalEffect = se.ASingleNaturalEffect;
            ASingleElementalEffect = ed.ASingleElementalEffect;

        }

        /// <summary>Constructor para el efecto 1, 2 y 3 de la carta en caso que el tercero sea de área.</summary>
        public Effects(SingleEffect se, ElementalDamage ed, AreaEffect ae)
        {

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();

            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ObjectEffect2 = ed;
            ObjectEffect3Area = ae;
            ASingleNaturalEffect = se.ASingleNaturalEffect;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaEffect = ae.AMidAreaEffect;

        }

        /// <summary>Constructor para el efecto 1, 2 y 3 de la carta en caso que el tercero sea de daño elemental.</summary>
        public Effects(SingleEffect se, ElementalDamage ed, ElementalDamage ed2)
        {

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();

            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ObjectEffect2 = ed;
            ObjectEffect3Elemental = ed2;
            ASingleNaturalEffect = se.ASingleNaturalEffect;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaElementalEffects = ed2.AMidAreaElementalEffects;

        }

        /// <summary>Constructor para el efecto 1, 2, 3 y 4 de la carta en caso que el tercero sea de área.</summary>
        public Effects(SingleEffect se, ElementalDamage ed, AreaEffect ae, AreaEffect ae2)
        {

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();

            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ObjectEffect2 = ed;
            ObjectEffect3Area = ae;
            ObjectEffect4 = ae2;
            ASingleNaturalEffect = se.ASingleNaturalEffect;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaEffect = ae.AMidAreaEffect;
            AHighAreaEffect = ae2.AHighAreaEffect;
        }

        /// <summary>Constructor para el efecto 1, 2, 3 y 4 de la carta en caso que el tercero sea de daño elemental.</summary>
        public Effects(SingleEffect se, ElementalDamage ed, ElementalDamage ed2, AreaEffect ae2)
        {

            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect1 = se;
            ObjectEffect2 = ed;
            ObjectEffect3Elemental = ed2;
            ObjectEffect4 = ae2;
            ASingleNaturalEffect = se.ASingleNaturalEffect;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaElementalEffects = ed2.AMidAreaElementalEffects;
            AHighAreaEffect = ae2.AHighAreaEffect;

        }

        /// <summary>Constructor para el caso que el efecto 1 sea uno especial creado por el usuario. Crea 2 y 3, en caso que el tercero sea de daño elemental.</summary>
        public Effects(ElementalDamage ed, ElementalDamage ed2)
        {
            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect2 = ed;
            ObjectEffect3Elemental = ed2;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaElementalEffects = ed2.AMidAreaElementalEffects;

        }

        /// <summary>Constructor para el caso que el efecto 1 sea uno especial creado por el usuario. Crea 2 y 3, en caso que el tercero sea de área.</summary>
        public Effects(ElementalDamage ed, AreaEffect ae)
        {
            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect2 = ed;
            ObjectEffect3Area = ae;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaEffect = ae.AMidAreaEffect;

        }

        /// <summary>Constructor para el caso que el efecto 1 sea uno especial creado por el usuario. Crea 2, 3 y 4, en caso que el tercero sea de daño elemental.</summary>
        public Effects(ElementalDamage ed, ElementalDamage ed2, AreaEffect ae2)
        {
            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect2 = ed;
            ObjectEffect3Elemental = ed2;
            ObjectEffect4 = ae2;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaElementalEffects = ed2.AMidAreaElementalEffects;
            AHighAreaEffect = ae2.AHighAreaEffect;

        }

        /// <summary>Constructor para el caso que el efecto 1 sea uno especial creado por el usuario. Crea 2, 3 y 4, en caso que el tercero sea de área.</summary>
        public Effects(ElementalDamage ed, AreaEffect ae, AreaEffect ae2)
        {
            #region Eliminar advertancias de nullabilidad e inicializar listas

            EBSingleElementalEffect = null!;
            EBSingleMagicalEffects = null!;
            EBSingleNaturalEffects = null!;
            EBMidAreaEffects = null!;
            EBMidAreaElementalEffects = null!;
            EBHighAreaEffects = null!;
            EBStatesEffectPerTurn = null!;
            ObjectEffect1 = null!;
            ObjectEffect2 = null!;
            ObjectEffect3Area = null!;
            ObjectEffect3Elemental = null!;
            ObjectEffect4 = null!;


            states = new();
            statesandelements = new();
            attackandelements = new();
            NaturalEffectsOthers = new();
            NaturalEffectsToCureEffectsStatus = new();
            NaturalEffectsToReviveCards = new();
            NaturalEffectToChangeAllyStats = new();
            NaturalEffectToChangeEnemyStats = new();
            NaturalEffectToDealDamageMultiple = new();
            NaturalEffectToDealDamageSingle = new();
            NaturalEffectToDealElementalDamage = new();
            NaturalEffectToDealInmunity = new();
            NaturalEffectToPutRestrictions = new();
            NaturalEffectsToRemoveRestrictions = new();
            MagicalEffectsOthers = new();
            MagicalEffectsToCureEffectsStatus = new();
            MagicalEffectsToRemoveRestrictions = new();
            MagicalEffectsToReviveCards = new();
            MagicalEffectToChangeAllyStats = new();
            MagicalEffectToChangeEnemyStats = new();
            MagicalEffectToDealDamage = new();
            MagicalEffectToPutRestrictions = new();

            #endregion

            CreatingEffectPerDefect();
            //Asigno el valor del ataque normal y las propiedades de los objetos
            NormalAttack = DoingEffect.DamageDealer;
            ObjectEffect2 = ed;
            ObjectEffect3Area = ae;
            ObjectEffect4 = ae2;
            ASingleElementalEffect = ed.ASingleElementalEffect;
            AMidAreaEffect = ae.AMidAreaEffect;
            AHighAreaEffect = ae2.AHighAreaEffect;

        }

        /// <summary>Creador de listas con delegados que hacen referencia a efectos por defectos. Se llama en cada constructor de efectos.</summary>
        void CreatingEffectPerDefect()
        {
            #region Estados de las cartas por defecto
            //elementos que pueden infligirlo:
            states.Add("Normal");          //None
            states.Add("Burned");          //Fire
            states.Add("Poisoned");        //Aqua
            states.Add("Petrified");       //Earth
            states.Add("Confused");        //Wind
            states.Add("Paralyzed");       //Lightining
            states.Add("Freezed");         //Ice
            states.Add("Bleeding");        //Cristal
            states.Add("IsInfected");       //Plant
            states.Add("Tied");            //Shadow
            states.Add("Blinded");         //Light
            states.Add("Cursed");          //Dark
            states.Add("DefenseBroken");   //Lightning
            states.Add("AsInfected");      //Ninguno, si es Plant puede tener este estado al usar un efecto


            Element elm = new Element();
            statesandelements = new();

            //guardo la lista de estados con su elemento asociado
            for (int k = 0; k < elm.AllElements.elements.Count; k++)
            {
                statesandelements.Add(states[k], elm.AllElements.elements[k]);
            }

            //y los que faltan
            statesandelements.Add("DefenseBroken", "Lightning");
            statesandelements.Add("AsInfected", "Plant");

            //agrego el diccionario de ataques y el estado que pueden provocar
            attackandelements = new();
            attackandelements.Add("FireStorm", "Burned");
            attackandelements.Add("Maelstrom", "Poisoned");
            attackandelements.Add("Earthquake", "Petrified");
            attackandelements.Add("Typhonn", "Confused");
            attackandelements.Add("ThunderStorm", "Paralyzed");
            attackandelements.Add("Absolute Zero", "Freezed");
            attackandelements.Add("Fragmentation", "Bleeding");
            attackandelements.Add("Natural Disaster ", "IsInfected");
            attackandelements.Add("Shadow Jail", "Tied");
            attackandelements.Add("DivineBlow", "Blinded");
            attackandelements.Add("AbsoluteDarkness", "Cursed");
            attackandelements.Add("Ultra M. Shocker", "DefenseBroken");
            attackandelements.Add("FireBolt", "Burned");
            attackandelements.Add("Poison Shot", "Poisoned");
            attackandelements.Add("Meduza's Gaze", "Petrified");
            attackandelements.Add("Whirlind", "Confused");
            attackandelements.Add("ThunderBolt", "Paralyzed");
            attackandelements.Add("Freeze", "Freezed");
            attackandelements.Add("Fractals", "Bleeding");
            attackandelements.Add("Seed", "IsInfected");
            attackandelements.Add("Shadow Chain", "Tied");
            attackandelements.Add("Holy Light", "Blinded");
            attackandelements.Add("Darkness", "Cursed");
            attackandelements.Add("Mechanic Shocker", "DefenseBroken");

            #endregion

            #region Llenando las listas de nombres de efectos de cartas naturales

            NaturalEffectToDealElementalDamage.Add("FireStorm");
            NaturalEffectToDealElementalDamage.Add("Maelstrom");
            NaturalEffectToDealElementalDamage.Add("Earthquake");
            NaturalEffectToDealElementalDamage.Add("Typhonn");
            NaturalEffectToDealElementalDamage.Add("ThunderStorm");
            NaturalEffectToDealElementalDamage.Add("Absolute Zero");
            NaturalEffectToDealElementalDamage.Add("Fragmentation");
            NaturalEffectToDealElementalDamage.Add("Natural Disaster ");
            NaturalEffectToDealElementalDamage.Add("Shadow Jail");
            NaturalEffectToDealElementalDamage.Add("DivineBlow");
            NaturalEffectToDealElementalDamage.Add("AbsoluteDarkness");
            NaturalEffectToDealElementalDamage.Add("Ultra M. Shocker");
            NaturalEffectToDealElementalDamage.Add("FireBolt");
            NaturalEffectToDealElementalDamage.Add("Poison Shot");
            NaturalEffectToDealElementalDamage.Add("Meduza's Gaze");
            NaturalEffectToDealElementalDamage.Add("Whirlind");
            NaturalEffectToDealElementalDamage.Add("ThunderBolt");
            NaturalEffectToDealElementalDamage.Add("Freeze");
            NaturalEffectToDealElementalDamage.Add("Fractals");
            NaturalEffectToDealElementalDamage.Add("Seed");
            NaturalEffectToDealElementalDamage.Add("Shadow Chain");
            NaturalEffectToDealElementalDamage.Add("Holy Light");
            NaturalEffectToDealElementalDamage.Add("Darkness");
            NaturalEffectToDealElementalDamage.Add("Mechanic Shocker");

            NaturalEffectToDealDamageMultiple.Add("Three Attack At Once");
            NaturalEffectToDealDamageSingle.Add("Turn Undead");
            NaturalEffectToDealDamageMultiple.Add("Chain Lightining");
            NaturalEffectToDealDamageMultiple.Add("Gears's Command");
            NaturalEffectToDealDamageMultiple.Add("Messiah's Sacrifice");

            NaturalEffectToPutRestrictions.Add("Silence");
            NaturalEffectToPutRestrictions.Add("A Sword of Reveling Light");
            NaturalEffectToPutRestrictions.Add("Three Chains");
            NaturalEffectToPutRestrictions.Add("Three Silence");

            NaturalEffectToChangeAllyStats.Add("Cure for Three");
            NaturalEffectToChangeAllyStats.Add("Berserk");
            NaturalEffectToChangeAllyStats.Add("Three Attack Up");
            NaturalEffectToChangeAllyStats.Add("Three Defense Up");

            NaturalEffectToChangeEnemyStats.Add("Three Defense Down");
            NaturalEffectToChangeEnemyStats.Add("Three Attack Down");
            NaturalEffectToChangeEnemyStats.Add("Three Life Down");

            NaturalEffectToDealInmunity.Add("Imagine Breaker");
            NaturalEffectToDealInmunity.Add("Saint Blessing");
            NaturalEffectToDealInmunity.Add("Divine Shield");

            NaturalEffectsToRemoveRestrictions.Add("Cure");
            NaturalEffectsToRemoveRestrictions.Add("Cure All");
            NaturalEffectsToRemoveRestrictions.Add("Remove Restrictions");
            NaturalEffectsToRemoveRestrictions.Add("Three Remove Restrictions");

            NaturalEffectsToCureEffectsStatus.Add("Three Remove Debuff");
            NaturalEffectsToCureEffectsStatus.Add("Three Remove All Debuff");

            NaturalEffectsToReviveCards.Add("Saint's Sacrifice");

            NaturalEffectsOthers.Add("Erwin's Sacrifice");

            #endregion

            #region Llenando las listas de nombres de efectos de cartas mágicas

            MagicalEffectToDealDamage.Add("Turn Undead");

            MagicalEffectToPutRestrictions.Add("Silence");
            MagicalEffectToPutRestrictions.Add("A Sword of Reveling Light");

            MagicalEffectToChangeAllyStats.Add("Berserk");
            MagicalEffectToChangeAllyStats.Add("Healing Light");
            MagicalEffectToChangeAllyStats.Add("Attack Up");
            MagicalEffectToChangeAllyStats.Add("Defense Up");
            MagicalEffectToChangeAllyStats.Add("Full Power Up");

            MagicalEffectToChangeEnemyStats.Add("Life Down");
            MagicalEffectToChangeEnemyStats.Add("Defense Down");
            MagicalEffectToChangeEnemyStats.Add("Attack Down");
            MagicalEffectToChangeEnemyStats.Add("All Power Down");

            MagicalEffectsToCureEffectsStatus.Add("Cure");
            MagicalEffectsToCureEffectsStatus.Add("Cure All");

            MagicalEffectsToRemoveRestrictions.Add("Remove Restrictions");

            MagicalEffectsToReviveCards.Add("Back to Life");

            MagicalEffectsOthers.Add("Pot of Greedy");
            MagicalEffectsOthers.Add("Life's Secure");
            MagicalEffectsOthers.Add("Mimic");

            #endregion

        }

    }


}

