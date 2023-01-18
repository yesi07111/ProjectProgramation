using Microsoft.Win32.SafeHandles;
using System.Data.Common;
using System.Collections;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using ProjectClasses;

namespace ProjectClasses
{

    public class CardLoader
    {
        /// <summary>Carga una base de datos cualquiera ya existente en la carpeta de DataBase y devuelve la lista de cartas.</summary>
        public static List<ACard> LoadDataBase(string DataBaseName)
        {
            List<string> file = CardDataBase.GetExternalTxt("DataBase", DataBaseName);

            //creo las listas necesarias para crear las cartas, con el tamaño predeterminado para hacer 80 naturales y 40 mágicas
            List<double> atks = new();
            List<double> defs = new();
            List<double> vits = new();
            List<Element> elementslist = new();
            List<Race> raceslist = new();
            List<int> titlesorder = new();
            List<int> namesorder = new();
            List<int> loreorder = new();
            List<string> magicalcardname = new();
            List<string> magicalcardlore = new();
            List<Effects> magicaleffects = new();
            List<Effects> naturaleffects = new();
            List<double> cost1 = new();
            List<double> cost2 = new();
            List<double> cost3 = new();
            List<double> cost4 = new();

            //recorro por el documento
            for (int j = 0; j < file.Count; j++)
            {
                //si se encuentra la palabra clave ATK
                if (file[j] == "ATK")
                {
                    //relleno los 80 ataques que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        atks.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave DEF
                if (file[j] == "DEF")
                {
                    //relleno los 80 defensas que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        defs.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave VIT
                if (file[j] == "VIT")
                {
                    //relleno los 80 vidas que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        vits.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave Elements
                if (file[j] == "Elements")
                {
                    //creo los 80 elementos según la info de ellos que está justo debajo de la palabra clave
                    for (int k = j + 1; ; k++)
                    {
                        //si se encuentra la palabra clave a1 es que el elemento tiene 1 elemento y debajo viene su número
                        if (file[k] == "A1")
                        {
                            //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                            Element elm = new(Int32.Parse(file[k + 1]));
                            elementslist.Add(elm);

                        }

                        //si se encuentra la palabra clave a2 es que el elemento tiene 2 elemento y debajo vienen sus números
                        if (file[k] == "A2")
                        {
                            //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                            Element elm = new(Int32.Parse(file[k + 1]), Int32.Parse(file[k + 1]));
                            elementslist.Add(elm);

                        }

                        if (elementslist.Count == 80)
                        {
                            break;
                        }

                    }

                    j += 160;
                }

                //si se encuentra la palabra clave Race
                if (file[j] == "Race")
                {
                    //creo las 80 razas según la info que está justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        Race race = new(Int32.Parse(file[k]));
                        raceslist.Add(race);
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave TNL (Title, Name, Lore)
                if (file[j] == "TNL")
                {
                    for (int k = j + 1; ; k++)
                    {
                        if (file[k] == "T")
                        {
                            titlesorder.Add(Int32.Parse(file[k + 1]));
                        }

                        if (file[k] == "N")
                        {
                            namesorder.Add(Int32.Parse(file[k + 1]));
                        }

                        if (file[k] == "L")
                        {
                            loreorder.Add(Int32.Parse(file[k + 1]));
                            loreorder.Add(Int32.Parse(file[k + 2]));
                            loreorder.Add(Int32.Parse(file[k + 3]));
                            loreorder.Add(Int32.Parse(file[k + 4]));
                        }

                        if (titlesorder.Count == 80 && namesorder.Count == 80 && loreorder.Count == 320)
                        {
                            break;
                        }

                    }

                }

                //si se encuentra la palabra clave EffectsMagical
                if (file[j] == "EffectsMagical")
                {
                    for (int k = j + 1; ; k++)
                    {

                        if (file[k] == "se1")
                        {

                            SingleEffect se = new(Int32.Parse(file[k + 1]), 2);
                            Effects efc = new Effects(se);
                            magicaleffects.Add(efc);

                            magicalcardlore.Add(se.ASingleMagicalEffect.label.effectdesc);

                            if (file[k + 2] == "name")
                            {
                                string[] magicnames = CardDataBase.GetStringList(4);
                                magicalcardname.Add(magicnames[Int32.Parse(file[k + 3])] + se.ASingleMagicalEffect.label.name);
                            }

                            k = k + 3;
                        }

                        if (magicaleffects.Count == 40 && magicalcardlore.Count == 40 && magicalcardname.Count == 40)
                        {
                            break;
                        }
                    }
                }

                //si se encuentra la palabra clave EffectsNatural
                if (file[j] == "EffectsNatural")
                {
                    for (int k = j + 1; ; k++)
                    {
                        SingleEffect se = new(0, 0);
                        ElementalDamage ed = new(0, 0);

                        if (file[k] == "se2")
                        {
                            se = new(Int32.Parse(file[k + 1]), 1);

                            if (file[k + 2] == "ed")
                            {
                                ed = new(Int32.Parse(file[k + 3]), 1);
                            }

                            Effects efc = new(se, ed);
                            naturaleffects.Add(efc);
                        }

                        if (naturaleffects.Count == 80)
                        {
                            break;
                        }

                    }
                }

                //si se encuentra la palabra clave Costs
                if (file[j] == "Costs")
                {
                    for (int k = 0; ; k++)
                    {
                        if (file[k] == "c1")
                        {
                            cost1.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c2")
                        {
                            cost2.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c3")
                        {
                            cost3.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c4")
                        {
                            cost4.Add(Double.Parse(file[k + 1]));
                        }

                        if (cost1.Count == 80 && cost2.Count == 80 && cost3.Count == 80 && cost4.Count == 80)
                        {
                            break;
                        }
                    }
                }

            }

            //finalmente creo las cartas y las agrego a una lista final que es el resultado
            List<ACard> cardres = new();

            //Crear las 80 cartas naturales
            for (int k = 0; k < 80; k++)
            {
                //tomo un efecto
                Effects efc = naturaleffects[k];

                //creo su atributo
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(atks[k], defs[k], vits[k], elementslist[k]);

                //su raza
                Race race = CardDataBase.CheckRaceWithElements(elementslist[k], raceslist[k]);

                //su tipo
                AType type = new AType(1);

                //tomo las listas del título, el nombre y el lore
                string[] titles = CardDataBase.GetStringList(1);
                string[] names = CardDataBase.GetStringList(2);
                string[] lores = CardDataBase.GetStringList(3);

                //aquí tomo los costos
                double cosst1 = cost1[k];
                double cosst2 = cost2[k];
                double cosst3 = cost3[k];
                double cosst4 = cost4[k];

                //creo la descripción
                ProjectClasses.Description desc = new Description(titles[titlesorder[k]] + names[namesorder[k]], lores[loreorder[k]] + " " + lores[loreorder[k + 1]] + " " + lores[loreorder[k + 2]] + " " + lores[loreorder[k + 3]], type, race,
                cosst1, cosst2, cosst3, cosst4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardres.Add(card);
            }

            //Crear las 40 cartas mágicas
            for (int j = 0; j < 40; j++)
            {
                //tomo un efecto
                Effects efc = magicaleffects[j];

                //creo su atributo 
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(0, 0, 0, new Element());

                //su raza
                Race race = new Race(0);

                //su tipo
                AType type = new AType(2);

                //aquí creo los costos de invocación que después se actualizan con los costos del efecto
                double cosst1 = 0;
                double cosst2 = 0;
                double cosst3 = 0;
                double cosst4 = 0;


                //creo la descripción
                Description desc = new Description(magicalcardname[j], magicalcardlore[j], type, race,
                cosst1, cosst2, cosst3, cosst4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardres.Add(card);

            }

            return cardres;

        }

        public static List<ACard> SpecialCardsLoader(string txtname)
        {
            List<string> file = CardDataBase.GetExternalTxt("DataBase", txtname);
            string code = CardDataBase.GetBruteExternalTxt("DataBAse", "+CardMaker");

            //creo las listas necesarias para crear las cartas, con el tamaño predeterminado para hacer 80 naturales y 40 mágicas
            List<double> atks = new();
            List<double> defs = new();
            List<double> vits = new();
            List<Element> elementslist = new();
            List<Race> raceslist = new();
            List<string> lores = new();
            List<string> names = new();
            List<double> cost1 = new();
            List<double> cost2 = new();
            List<double> cost3 = new();
            List<double> cost4 = new();
            List<string> SpecialCode = new();

            //recorro por el documento
            for (int j = 0; j < file.Count; j++)
            {
                //si se encuentra la palabra clave ATK
                if (file[j] == "ATK")
                {
                    atks.Add(Double.Parse(file[j + 1]));
                }

                //si se encuentra la palabra clave DEF
                if (file[j] == "DEF")
                {
                    defs.Add(Double.Parse(file[j + 1]));
                }

                //si se encuentra la palabra clave VIT
                if (file[j] == "VIT")
                {
                    vits.Add(Double.Parse(file[j + 1]));
                }

                //si se encuentra la palabra clave Elements
                if (file[j] == "Elements")
                {
                    //si se encuentra la palabra clave a1 es que el elemento tiene 1 elemento y debajo viene su número
                    if (file[j + 1] == "A1")
                    {
                        //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                        Element elm = new(Int32.Parse(file[j + 2]));
                        elementslist.Add(elm);
                    }

                    //si se encuentra la palabra clave a2 es que el elemento tiene 2 elemento y debajo vienen sus números
                    if (file[j + 3] == "A2")
                    {
                        //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                        Element elm = new(Int32.Parse(file[j + 4]), Int32.Parse(file[j + 4]));
                        elementslist.Add(elm);
                    }

                }

                //si se encuentra la palabra clave Race
                if (file[j] == "Race")
                {
                    Race race = new(Int32.Parse(file[j + 1]));
                    raceslist.Add(race);
                }

                //si se encuentra la palabra clave TNL (Title, Name, Lore)
                if (file[j] == "Lore")
                {
                    names.Add(file[j + 1]);
                    string lor = "";
                    for (int n = j + 2; ; n++)
                    {
                        if (file[n] == "Cost1")
                        {
                            break;
                        }

                        if (lor == "")
                        {
                            lor += file[n];
                        }

                        else
                        {
                            lor += " " + file[n];
                        }

                    }

                    lores.Add(lor);
                }

                //si se encuentra la palabra clave Costs
                if (file[j] == "Cost1")
                {
                    cost1.Add(Double.Parse(file[j + 1]));
                    cost2.Add(Double.Parse(file[j + 2]));
                    cost3.Add(Double.Parse(file[j + 3]));
                    cost4.Add(Double.Parse(file[j + 4]));
                }

                if (file[j] == "SpecialEffect")
                {
                    string code1 = "";

                    for (int n = 0; ; n++)
                    {
                        if (code[n] == '}')
                        {
                            code1 += code[n];
                            break;
                        }
                        code1 += code[n];
                    }

                    SpecialCode.Add(code1);
                }

            }

            //finalmente creo las cartas y las agrego a una lista final que es el resultado
            List<ACard> cardres = new();

            //Crear las 80 cartas naturales
            for (int k = 0; k < atks.Count; k++)
            {
                //tomo el efecto especial
                ProjectClasses.ObjectEffect oe = new(SpecialCode[k]);

                //creo su atributo
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(atks[k], defs[k], vits[k], elementslist[k]);

                //su raza
                Race race = CardDataBase.CheckRaceWithElements(elementslist[k], raceslist[k]);

                //su tipo
                AType type = new AType(1);

                //aquí tomo los costos
                double cosst1 = cost1[k];
                double cosst2 = cost2[k];
                double cosst3 = cost3[k];
                double cosst4 = cost4[k];

                //creo la descripción
                ProjectClasses.Description desc = new Description(names[k], lores[k], type, race,
                cosst1, cosst2, cosst3, cosst4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, oe, desc, new());

                //para luego agregarla
                cardres.Add(card);
            }

            return cardres;

        }

        /// <summary>Carga la primera base de datos que ya existe por defecto en la carpeta de DataBase y devuelve la lista de cartas.</summary>
        public static List<ACard> LoadFirstDataBase()
        {
            List<string> file = CardDataBase.GetExternalTxt("DataBase", "FirstDataBase");

            //creo las listas necesarias para crear las cartas, con el tamaño predeterminado para hacer 80 naturales y 40 mágicas
            List<double> atks = new();
            List<double> defs = new();
            List<double> vits = new();
            List<Element> elementslist = new();
            List<Race> raceslist = new();
            List<int> titlesorder = new();
            List<int> namesorder = new();
            List<int> loreorder = new();
            List<string> magicalcardname = new();
            List<string> magicalcardlore = new();
            List<Effects> magicaleffects = new();
            List<Effects> naturaleffects = new();
            List<double> cost1 = new();
            List<double> cost2 = new();
            List<double> cost3 = new();
            List<double> cost4 = new();

            //recorro por el documento
            for (int j = 0; j < file.Count; j++)
            {
                //si se encuentra la palabra clave ATK
                if (file[j] == "ATK")
                {
                    //relleno los 80 ataques que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        atks.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave DEF
                if (file[j] == "DEF")
                {
                    //relleno los 80 defensas que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        defs.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave VIT
                if (file[j] == "VIT")
                {
                    //relleno los 80 vidas que están justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        vits.Add(Double.Parse(file[k]));
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave Elements
                if (file[j] == "Elements")
                {
                    //creo los 80 elementos según la info de ellos que está justo debajo de la palabra clave
                    for (int k = j + 1; ; k++)
                    {
                        //si se encuentra la palabra clave a1 es que el elemento tiene 1 elemento y debajo viene su número
                        if (file[k] == "A1")
                        {
                            //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                            Element elm = new(Int32.Parse(file[k + 1]));
                            elementslist.Add(elm);

                        }

                        //si se encuentra la palabra clave a2 es que el elemento tiene 2 elemento y debajo vienen sus números
                        if (file[k] == "A2")
                        {
                            //creo el nuevo elemento, lo añado y cambio el índice del for acorde
                            Element elm = new(Int32.Parse(file[k + 1]), Int32.Parse(file[k + 1]));
                            elementslist.Add(elm);

                        }

                        if (elementslist.Count == 80)
                        {
                            break;
                        }

                    }

                    j += 160;
                }

                //si se encuentra la palabra clave Race
                if (file[j] == "Race")
                {
                    //creo las 80 razas según la info que está justo debajo de la palabra clave
                    for (int k = j + 1; k < 80 + j + 1; k++)
                    {
                        Race race = new(Int32.Parse(file[k]));
                        raceslist.Add(race);
                    }

                    j += 80;
                }

                //si se encuentra la palabra clave TNL (Title, Name, Lore)
                if (file[j] == "TNL")
                {
                    for (int k = j + 1; ; k++)
                    {
                        if (file[k] == "T")
                        {
                            titlesorder.Add(Int32.Parse(file[k + 1]));
                        }

                        if (file[k] == "N")
                        {
                            namesorder.Add(Int32.Parse(file[k + 1]));
                        }

                        if (file[k] == "L")
                        {
                            loreorder.Add(Int32.Parse(file[k + 1]));
                            loreorder.Add(Int32.Parse(file[k + 2]));
                            loreorder.Add(Int32.Parse(file[k + 3]));
                            loreorder.Add(Int32.Parse(file[k + 4]));
                        }

                        if (titlesorder.Count == 80 && namesorder.Count == 80 && loreorder.Count == 320)
                        {
                            break;
                        }

                    }

                }

                //si se encuentra la palabra clave EffectsMagical
                if (file[j] == "EffectsMagical")
                {
                    for (int k = j + 1; ; k++)
                    {

                        if (file[k] == "se1")
                        {

                            SingleEffect se = new(Int32.Parse(file[k + 1]), 2);
                            Effects efc = new Effects(se);
                            magicaleffects.Add(efc);

                            magicalcardlore.Add(se.ASingleMagicalEffect.label.effectdesc);

                            if (file[k + 2] == "name")
                            {
                                string[] magicnames = CardDataBase.GetStringList(4);
                                magicalcardname.Add(magicnames[Int32.Parse(file[k + 3])] + se.ASingleMagicalEffect.label.name);
                            }

                            k = k + 3;
                        }

                        if (magicaleffects.Count == 40 && magicalcardlore.Count == 40 && magicalcardname.Count == 40)
                        {
                            break;
                        }
                    }
                }

                //si se encuentra la palabra clave EffectsNatural
                if (file[j] == "EffectsNatural")
                {
                    for (int k = j + 1; ; k++)
                    {
                        SingleEffect se = new(0, 0);
                        ElementalDamage ed = new(0, 0);

                        if (file[k] == "se2")
                        {
                            se = new(Int32.Parse(file[k + 1]), 1);

                            if (file[k + 2] == "ed")
                            {
                                ed = new(Int32.Parse(file[k + 3]), 1);
                            }

                            Effects efc = new(se, ed);
                            naturaleffects.Add(efc);
                        }

                        if (naturaleffects.Count == 80)
                        {
                            break;
                        }

                    }
                }

                //si se encuentra la palabra clave Costs
                if (file[j] == "Costs")
                {
                    for (int k = 0; ; k++)
                    {
                        if (file[k] == "c1")
                        {
                            cost1.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c2")
                        {
                            cost2.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c3")
                        {
                            cost3.Add(Double.Parse(file[k + 1]));
                        }

                        if (file[k] == "c4")
                        {
                            cost4.Add(Double.Parse(file[k + 1]));
                        }

                        if (cost1.Count == 80 && cost2.Count == 80 && cost3.Count == 80 && cost4.Count == 80)
                        {
                            break;
                        }
                    }
                }
            }

            //finalmente creo las cartas y las agrego a una lista final que es el resultado
            List<ACard> cardres = new();

            //Crear las 80 cartas naturales
            for (int k = 0; k < 80; k++)
            {
                //tomo un efecto
                Effects efc = naturaleffects[k];

                //creo su atributo
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(atks[k], defs[k], vits[k], elementslist[k]);

                //su raza
                Race race = CardDataBase.CheckRaceWithElements(elementslist[k], raceslist[k]);

                //su tipo
                AType type = new AType(1);

                //tomo las listas del título, el nombre y el lore
                string[] titles = CardDataBase.GetStringList(1);
                string[] names = CardDataBase.GetStringList(2);
                string[] lores = CardDataBase.GetStringList(3);

                //aquí tomo los costos
                double cosst1 = cost1[k];
                double cosst2 = cost2[k];
                double cosst3 = cost3[k];
                double cosst4 = cost4[k];

                //creo la descripción
                ProjectClasses.Description desc = new Description(titles[titlesorder[k]] + " " + names[namesorder[k]], lores[loreorder[k]] + " " + lores[loreorder[k + 1]] + " " + lores[loreorder[k + 2]] + " " + lores[loreorder[k + 3]], type, race,
                cosst1, cosst2, cosst3, cosst4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardres.Add(card);
            }

            //Crear las 40 cartas mágicas
            for (int j = 0; j < 40; j++)
            {
                //tomo un efecto
                Effects efc = magicaleffects[j];

                //creo su atributo 
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(0, 0, 0, new Element());

                //su raza
                Race race = new Race(0);

                //su tipo
                AType type = new AType(2);

                //aquí creo los costos de invocación que después se actualizan con los costos del efecto
                double cosst1 = 0;
                double cosst2 = 0;
                double cosst3 = 0;
                double cosst4 = 0;


                //creo la descripción
                ProjectClasses.Description desc = new Description(magicalcardname[j], magicalcardlore[j], type, race,
                cosst1, cosst2, cosst3, cosst4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardres.Add(card);

            }

            return cardres;

        }


    }


}
