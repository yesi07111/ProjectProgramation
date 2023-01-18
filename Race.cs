using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IRace
    {
        //Propiedades de necesario acceso:
        public string RaceName { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public (string evolname1, string evolname2) EvolvedNames { get; set; }
        public Dictionary<string, int> RacesNum { get; set; }
    }

    /// <summary>Clase para las razas. Cualquier string funciona como nombre de raza. Cada raza tiene un determinado nivel de afiliación a la disposición de los reinos y tiene restricciones en los elementos que no puede usar y las clases que no pueden ser.</summary>
    public class Race : IRace
    {
        //Propiedades accesibles:
        public string RaceName { get; set; }
        public List<(string TypeOfKingdomAffiliation, int amount)> Affiliations { get; set; }
        public (string evolname1, string evolname2) EvolvedNames { get; set; }
        public List<string> ElementsTheyCanHave { get; set; }
        public List<string> AllRacesName { get; set; }
        public List<(string evolname1, string evolname2)> AllRacesEvolvedNames { get; set; }
        public List<List<(string TypeOfKingdomAffiliation, int amount)>> AllRacesAffiliations { get; set; }
        public List<List<string>> AllRacesElementsTheyCanHave { get; set; }
        public Dictionary<string, int> RacesNum { get; set; }

        /// <summary>Usa una raza por defecto pasandole su número identificador. Se pone: (1)Human, (2)Elf, (3)Dwarf, (4)Dragon, (5)Giant, (6)Demon, (7)Angel, (8)Machine y (9)Beast. Cualquier otro número da por defecto raza humana.</summary>
        public Race(int racenum)

        {
            AllRacesName = new();
            AllRacesEvolvedNames = new();
            AllRacesAffiliations = new();
            AllRacesElementsTheyCanHave = new();
            RacesNum = new();
            ElementsTheyCanHave = new();

            CreatorOfRacesPerDefect();

            //Ahora si el número dado es mayor que 9 o menor/igual que 1 entonces por defecto la raza es Humano
            if (racenum > 9 || racenum <= 1)
            {
                RaceName = AllRacesName[0];
                Affiliations = AllRacesAffiliations[0];
                EvolvedNames = AllRacesEvolvedNames[0];
                ElementsTheyCanHave = AllRacesElementsTheyCanHave[0];
            }

            //sino se asignan valores según el número de raza: (1)Human, (2)Elf, (3)Dwarf, (4)Dragon, (5)Giant, (6)Demon, (7)Angel, (8)Machine y (9)Beast
            else
            {
                RaceName = AllRacesName[racenum - 1];
                Affiliations = AllRacesAffiliations[racenum - 1];
                EvolvedNames = AllRacesEvolvedNames[racenum - 1];
                ElementsTheyCanHave = AllRacesElementsTheyCanHave[racenum - 1];
            }


        }

        /// <summary>Una raza nueva se crea con un nombre, su nivel de afiliación [1-100] en faith, knowledge, militarism and capital, los nombres de (su primera evolución, su segunda evolución) y los elementos que podría tener.</summary>
        public Race(string name, List<(string KingdomDisposition, int amount)> affiliations, (string evolname1, string evolname2) evolvednames, List<string> elementstheycanhave)
        {
            AllRacesName = new();
            AllRacesEvolvedNames = new();
            AllRacesAffiliations = new();
            AllRacesElementsTheyCanHave = new();
            RacesNum = new();
            ElementsTheyCanHave = new();

            CreatorOfRacesPerDefect();

            //Se establecen los valores de la nueva raza y se agregan a las listas por defecto.
            RaceName = name;
            Affiliations = affiliations;
            EvolvedNames = evolvednames;
            ElementsTheyCanHave = elementstheycanhave;

            AllRacesName.Add(RaceName);
            AllRacesAffiliations.Add(Affiliations);
            AllRacesEvolvedNames.Add(EvolvedNames);
            AllRacesElementsTheyCanHave.Add(ElementsTheyCanHave);
        }

        /// <summary>Creador de razas por defecto.</summary>
        void CreatorOfRacesPerDefect()
        {

            #region Razas por defecto
            //Asigno 9 razas por defecto: (1)Human, (2)Elf, (3)Dwarf, (4)Dragon, (5)Giant, (6)Demon, (7)Angel, (8)Machine y (9)Beast
            List<string> allracesnames = new();
            allracesnames.Add("Human");
            allracesnames.Add("Elf");
            allracesnames.Add("Dwarf");
            allracesnames.Add("Dragon");
            allracesnames.Add("Giant");
            allracesnames.Add("Demon");
            allracesnames.Add("Angel");
            allracesnames.Add("Machine");
            allracesnames.Add("Beast");

            //Creo la lista de los nombres evolucionados de las 9 razas.
            List<(string evolname1, string evolname2)> allracesevolvednames = new();
            allracesevolvednames.Add(("Super Human", "Over Human"));
            allracesevolvednames.Add(("Great Elf", "High Elf"));
            allracesevolvednames.Add(("Smith Dwarf", "Master Dwarf"));
            allracesevolvednames.Add(("High Dragon", "ArchDragon"));
            allracesevolvednames.Add(("Inmense", "Massive"));
            allracesevolvednames.Add(("ArchDemon", "Demon Lord"));
            allracesevolvednames.Add(("High Angel", "ArchAngel"));
            allracesevolvednames.Add(("Ultron", "Terminator"));
            allracesevolvednames.Add(("Peace Beast", "War Beast"));


            //Creo la lista de las listas de afiliaciones
            List<List<(string TypeOfKingdomAffiliation, int amount)>> allracesaffiliations = new();

            //Creo una lista de la afiliacion de cada raza y se las voy añadiendo a la lista de listas:

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation1 = new(); //human 
            affiliation1.Add(("Faith", 50));
            affiliation1.Add(("Militarism", 60));
            affiliation1.Add(("Knowledge", 40));
            affiliation1.Add(("Capital", 70));
            allracesaffiliations.Add(affiliation1);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation2 = new(); //Elf
            affiliation2.Add(("Faith", 10));
            affiliation2.Add(("Militarism", 20));
            affiliation2.Add(("Knowledge", 100));
            affiliation2.Add(("Capital", 70));
            allracesaffiliations.Add(affiliation2);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation3 = new(); //Dwarf
            affiliation3.Add(("Faith", 5));
            affiliation3.Add(("Militarism", 80));
            affiliation3.Add(("Knowledge", 75));
            affiliation3.Add(("Capital", 30));
            allracesaffiliations.Add(affiliation3);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation4 = new(); //Dragon
            affiliation4.Add(("Faith", 0));
            affiliation4.Add(("Militarism", 100));
            affiliation4.Add(("Knowledge", 75));
            affiliation4.Add(("Capital", 20));
            allracesaffiliations.Add(affiliation4);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation5 = new(); //Giant
            affiliation5.Add(("Faith", 75));
            affiliation5.Add(("Militarism", 80));
            affiliation5.Add(("Knowledge", 20));
            affiliation5.Add(("Capital", 10));
            allracesaffiliations.Add(affiliation5);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation6 = new(); //Demon
            affiliation6.Add(("Faith", 0));
            affiliation6.Add(("Militarism", 100));
            affiliation6.Add(("Knowledge", 85));
            affiliation6.Add(("Capital", 10));
            allracesaffiliations.Add(affiliation6);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation7 = new(); //Angel
            affiliation7.Add(("Faith", 100));
            affiliation7.Add(("Militarism", 0));
            affiliation7.Add(("Knowledge", 100));
            affiliation7.Add(("Capital", 0));
            allracesaffiliations.Add(affiliation7);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation8 = new(); //Machine
            affiliation8.Add(("Faith", 0));
            affiliation8.Add(("Militarism", 100));
            affiliation8.Add(("Knowledge", 95));
            affiliation8.Add(("Capital", 0));
            allracesaffiliations.Add(affiliation8);

            List<(string TypeOfKingdomAffiliation, int amount)> affiliation9 = new(); //Beast
            affiliation9.Add(("Faith", 75));
            affiliation9.Add(("Militarism", 80));
            affiliation9.Add(("Knowledge", 30));
            affiliation9.Add(("Capital", 40));
            allracesaffiliations.Add(affiliation9);


            //Creo la lista de listas de los elementos que una raza puede tener:
            List<List<string>> allraceselementstheycanhave = new();

            //Voy creando una lista por raza y se la voy añadiendo a la lista de listas:

            List<string> e1 = new(); //human
            e1.Add("None");
            e1.Add("Normal");
            e1.Add("Fire");
            e1.Add("Aqua");
            e1.Add("Earth");
            e1.Add("Wind");
            e1.Add("Lightning");
            e1.Add("Ice");
            e1.Add("Cristal");
            e1.Add("Plant");
            e1.Add("Shadow");
            e1.Add("Light");
            e1.Add("Dark");
            allraceselementstheycanhave.Add(e1);


            List<string> e2 = new(); //Elf
            e2.Add("None");
            e2.Add("Normal");
            e2.Add("Fire");
            e2.Add("Aqua");
            e2.Add("Earth");
            e2.Add("Wind");
            e2.Add("Plant");
            allraceselementstheycanhave.Add(e2);

            List<string> e3 = new(); //Dwarf
            e3.Add("None");
            e3.Add("Normal");
            e3.Add("Fire");
            e3.Add("Earth");
            e3.Add("Ice");
            e3.Add("Cristal");
            allraceselementstheycanhave.Add(e3);

            List<string> e4 = new(); //Dragon
            e4.Add("None");
            e4.Add("Fire");
            e4.Add("Aqua");
            e4.Add("Earth");
            e4.Add("Wind");
            e4.Add("Lightning");
            e4.Add("Ice");
            e4.Add("Light");
            e4.Add("Dark");
            allraceselementstheycanhave.Add(e4);


            List<string> e5 = new(); //Giant
            e5.Add("None");
            e5.Add("Normal");
            e5.Add("Earth");
            e5.Add("Ice");
            allraceselementstheycanhave.Add(e5);

            List<string> e6 = new(); //Demon
            e6.Add("None");
            e6.Add("Fire");
            e6.Add("Aqua");
            e6.Add("Earth");
            e6.Add("Wind");
            e6.Add("Lightning");
            e6.Add("Ice");
            e6.Add("Cristal");
            e6.Add("Shadow");
            e6.Add("Dark");
            allraceselementstheycanhave.Add(e6);

            List<string> e7 = new(); //Angel
            e7.Add("None"); ;
            e7.Add("Fire");
            e7.Add("Aqua");
            e7.Add("Earth");
            e7.Add("Wind");
            e7.Add("Lightning");
            e7.Add("Ice");
            e7.Add("Plant");
            e7.Add("Cristal");
            e7.Add("Light");
            allraceselementstheycanhave.Add(e7);

            List<string> e8 = new(); //Machine
            e8.Add("None");
            e8.Add("Fire");
            e8.Add("Earth");
            e8.Add("Cristal");
            e8.Add("Light");
            allraceselementstheycanhave.Add(e8);

            List<string> e9 = new(); //Beast
            e9.Add("Normal");
            e9.Add("None");
            e9.Add("Fire");
            e9.Add("Aqua");
            e9.Add("Earth");
            e9.Add("Wind");
            e9.Add("Lightning");
            e9.Add("Ice");
            e9.Add("Shadow");
            e9.Add("Light");
            e9.Add("Dark");
            allraceselementstheycanhave.Add(e9);

            //Finalmente igualo las propiedades correspondientes a las listas creadas:
            AllRacesName = allracesnames;
            AllRacesEvolvedNames = allracesevolvednames;
            AllRacesAffiliations = allracesaffiliations;
            AllRacesElementsTheyCanHave = allraceselementstheycanhave;

            RacesNum.Add("Human", 1);
            RacesNum.Add("Elf", 2);
            RacesNum.Add("Dwarf", 3);
            RacesNum.Add("Dragon", 4);
            RacesNum.Add("Giant", 5);
            RacesNum.Add("Demon", 6);
            RacesNum.Add("Angel", 7);
            RacesNum.Add("Machine", 8);
            RacesNum.Add("Beast", 9);

            #endregion

        }


    }
}


