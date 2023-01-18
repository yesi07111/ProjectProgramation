using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;


namespace ProjectClasses
{
    public interface IDescription
    {
        public string CardName { get; set; }
        public string CardLore { get; set; }
        public string CardTypeName { get; set; }
        public int CardTypeNum { get; set; }
        public double InvocationCostCapital { get; set; }
        public double InvocationCostFaith { get; set; }
        public double InvocationCostMilitarism { get; set; }
        public double InvocationCostKnowledge { get; set; }
        public string RaceName { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public List<string> ElementsTheyCanHave { get; set; }
        public (string evolname1, string evolname2) CardEvolvedNames { get; set; }
        public (string evolname1, string evolname2) RaceEvolvedNames { get; set; }
        public AType ATypeObject { get; set; }
        public Race RaceObject { get; set; }


    }

    /// <summary>Clase que contiene la descripción de las cartas: nombre, lore, tipo, raza, clase y costo de invocación.</summary>
    public class Description : IDescription
    {
        //Constructor con varias sobrecargas: 

        /// <summary>Puedes pasarle el nombre, el lore de la carta, un tipo, la raza, la clase y los costos de invocación.</summary>
        public Description(string name, string lore, AType type, Race race, double CostCapital, double CostFaith, double CostMilitarism, double CostKnowledge)
        {
            CardName = name;
            CardLore = lore;

            CardTypeName = type.Type.typename;
            CardTypeNum = type.Type.typenum;

            InvocationCostCapital = CostCapital;
            InvocationCostFaith = CostFaith;
            InvocationCostMilitarism = CostMilitarism;
            InvocationCostKnowledge = CostKnowledge;

            RaceName = race.RaceName;
            Affiliations = race.Affiliations;
            CardEvolvedNames = ("Super", "Ultra");
            RaceEvolvedNames = race.EvolvedNames;
            ElementsTheyCanHave = race.ElementsTheyCanHave;

            ATypeObject = type;
            RaceObject = race;

        }


        //Propiedades accesibles por defecto:
        public string CardName { get; set; }
        public string CardLore { get; set; }
        public (string evolname1, string evolname2) CardEvolvedNames { get; set; }

        //Propiedades de tipo:
        public string CardTypeName { get; set; }
        public int CardTypeNum { get; set; }

        //Propiedades del costo de invocación:
        public double InvocationCostCapital { get; set; }
        public double InvocationCostFaith { get; set; }
        public double InvocationCostMilitarism { get; set; }
        public double InvocationCostKnowledge { get; set; }

        //Propiedades de la raza:
        public string RaceName { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public (string evolname1, string evolname2) RaceEvolvedNames { get; set; }
        public List<string> ElementsTheyCanHave { get; set; }


        //Propiedades para acceder a los objetos: tipo, raza y clase:
        public AType ATypeObject { get; set; }
        public Race RaceObject { get; set; }


    }
}

