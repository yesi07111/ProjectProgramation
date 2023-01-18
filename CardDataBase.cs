using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Data;
using System.Runtime;

namespace ProjectClasses
{

    public static class CardDataBase
    {
        public static List<ACard> cardlist = new();


        /// <summary>Creo una base de datos de cartas al azar. Crea 80 cartas naturales y 40 mágicas, para un total de 120 cartas.</summary>
        public static void CreateABaseOfCards(string DataBaseName)
        {
            //creo las listas necesarias para crear la cardlist
            List<double> atks = new();
            List<double> defs = new();
            List<double> vits = new();
            List<Element> elementslist = new();
            List<Race> raceslist = new();
            List<string> magicalcardname = new();
            List<string> magicalcardlore = new();
            List<Effects> magicaleffects = new();
            List<Effects> naturaleffects = new();

            //listas necesarias para crear el cache
            List<string> atacks = new();
            List<string> defenses = new();
            List<string> vitalities = new();
            List<string> elements = new();
            List<string> races = new();
            List<string> titlesnameslores = new();
            List<string> effects = new();
            List<string> costs = new();

            //lista de strings del cache
            List<string> cache = new();


            //creo un random para usos variados
            System.Random r = new System.Random();

            #region Crear los 80 atk, def, vit

            //al inicio de estas listas añado una palabra clave
            atacks.Add("ATK");
            defenses.Add("DEF");
            vitalities.Add("VIT");

            for (int k = 0; k < 80; k++)
            {
                //los creo y guardo númericos para crear luego las cartas, y como string para el cache
                double num1 = r.Next(4000, 8000);
                double num2 = r.Next(2000, 8000);
                double num3 = r.Next(5000, 12000);

                num1 = Math.Round(((num1 / 1000))) * 1000;
                num2 = Math.Round(((num2 / 1000))) * 1000;
                num3 = Math.Round(((num3 / 1000))) * 1000;

                atks.Add(num1);
                defs.Add(num2);
                vits.Add(num3);
                atacks.Add("" + num1);
                defenses.Add("" + num2);
                vitalities.Add("" + num3);
            }

            #endregion

            #region Crear los elementos de 80 cartas

            //al inicio de esta lista añado una palabra clave
            elements.Add("Elements");

            for (int k = 0; k < 80; k++)
            {
                //creo un int para decidir cuantos elementos tendrá la carta y dos índices de elementos random
                int oneortwo = r.Next(0, 101);
                int elem1 = r.Next(1, 13);
                int elem2 = r.Next(1, 13);

                //creo el resultado 
                Element elements0 = new Element();

                //si el random es mayor que 50
                if (oneortwo > 50)
                {
                    //tendrá dos elementos
                    elements0 = new Element(elem1, elem2);

                    //uso A# para indicar la cantidad de elementos que tendrá y en la línea de abajo cuales
                    elements.Add("A2");
                    elements.Add("" + elem1 + " " + elem2);
                }

                //sino 
                else
                {
                    //tendrá uno
                    elements0 = new Element(elem1);

                    elements.Add("A1");
                    elements.Add("" + elem1);
                }

                elementslist.Add(elements0);
            }

            #endregion

            #region Crear las razas, ajustables luego

            //al inicio de esta lista añado una palabra clave
            races.Add("Race");

            for (int k = 0; k < 80; k++)
            {
                //tomo un random, creo la raza con este y la guardo
                int rac1 = r.Next(1, 10);
                Race race0 = new Race(rac1);
                raceslist.Add(race0);

                //guardo el random usado
                races.Add("" + rac1);
            }

            #endregion

            #region Tomar Array de Títulos, Nombres y Lores

            string[] titles = GetStringList(1);

            string[] names = GetStringList(2);

            string[] lores = GetStringList(3);

            #endregion

            #region Crear Efectos

            //al inicio de esta lista añado una palabra clave
            effects.Add("EffectsMagical");

            //primero los mágicos
            for (int k = 0; k < 40; k++)
            {
                //creo el efecto sencillo
                int ind = r.Next(0, 19);
                SingleEffect se = new SingleEffect(ind, 2);
                Effects efc = new Effects(se);

                string[] magicnames = GetStringList(4);

                int ind2 = r.Next(0, magicnames.Length);

                //y lo agrego a la lista, a la lista de nombres su nombre y a la del lore su lore
                magicaleffects.Add(efc);
                magicalcardname.Add(magicnames[ind2] + se.ASingleMagicalEffect.label.name);
                magicalcardlore.Add(se.ASingleMagicalEffect.label.effectdesc);

                effects.Add("se1 " + ind + " name " + ind2);
            }

            //aquí añado otra palabra clave
            effects.Add("EffectsNatural");

            //y ahora los naturales
            for (int k = 0; k < 80; k++)
            {
                //creo el efecto sencillo
                int ind = r.Next(0, 7);
                SingleEffect se = new SingleEffect(ind, 1);

                //creo el elemental

                //creo una lista de los posibles índices de los ataques que son equivalentes a los índices de sus elementos en la lista de todos los elementos

                //ya que los ataques están en una lista guardados en el mismo orden que los elementos que se relacionan con ellos en la lista de todos los elementos

                //ejemplo: elemento Fire es el índice 0 de la lista de todos los elementos y el ataque FireBolt es el índice 0 de la lista de ataques

                List<int> possibleindexofattack0 = new();

                //reviso el elemento 1 y 2 de la lista de elementos
                string element1 = elementslist[k].Element1.elementname;
                string element2 = elementslist[k].Element2.elementname;

                //si el elemento 1 es distinto de None
                if (element1 != "None")
                {
                    //agrego su índice en la lista de todos los elementos a los índice de los posibles ataques
                    possibleindexofattack0.Add(elementslist[k].AllElements.elements.IndexOf(element1));
                }

                //y si el elemento 2 también es distinto de None
                if (element2 != "None")
                {
                    //agrego su índice en la lista de todos los elementos a los índice de los posibles ataques
                    possibleindexofattack0.Add(elementslist[k].AllElements.elements.IndexOf(element2));
                }

                //creo el efecto de daño elemental sencillo random que coincida con algún elemento de la carta
                int ind2 = possibleindexofattack0[r.Next(0, possibleindexofattack0.Count)];
                ElementalDamage ed = new ElementalDamage(ind2 - 1, 1);

                effects.Add("se2 " + ind + " ed " + ind2);

                //añado el efecto sencillo y el elemental
                Effects efc = new Effects(se, ed);

                naturaleffects.Add(efc);
            }

            #endregion

            titlesnameslores.Add("TNL");

            //Crear las 80 cartas naturales
            for (int k = 0; k < 80; k++)
            {
                //tomo un efecto
                Effects efc = naturaleffects[k];

                //creo su atributo
                ProjectClasses.Attribute atr = new ProjectClasses.Attribute(atks[k], defs[k], vits[k], elementslist[k]);

                //su raza
                Race race = CheckRaceWithElements(elementslist[k], raceslist[k]);

                //su tipo
                AType type = new AType(1);

                //tomo un random para el título y el nombre
                int tit = r.Next(0, titles.Length);
                int nm = r.Next(0, names.Length);

                //para el lore tomo 4 diferentes
                int lr1 = r.Next(0, lores.Length);
                int lr2 = r.Next(0, lores.Length);
                int lr3 = r.Next(0, lores.Length);
                int lr4 = r.Next(0, lores.Length);


                titlesnameslores.Add("T " + tit + " N " + nm + " L " + lr1 + " " + lr2 + " " + lr3 + " " + lr4);

                //aquí creo los costos
                double cost1 = r.Next(0, 9) * 100;
                double cost2 = r.Next(0, 9) * 100;
                double cost3 = r.Next(0, 9) * 100;
                double cost4 = r.Next(0, 9) * 100;

                //y para darles variedad

                //si se cumple esta condición entonces el costo 1 aumenta en 50
                if (r.Next(1, 101) > 50)
                {
                    cost1 += 50;
                }

                //si se cumple esta condición entonces el costo 2 aumenta en 50
                if (r.Next(1, 101) > 50)
                {
                    cost2 += 50;
                }

                //si se cumple esta condición entonces el costo 3 aumenta en 50
                if (r.Next(1, 101) > 50)
                {
                    cost3 += 50;
                }

                //si se cumple esta condición entonces el costo 4 aumenta en 50
                if (r.Next(1, 101) > 50)
                {
                    cost4 += 50;
                }

                costs.Add("Costs");
                costs.Add("c1 " + cost1 + " c2 " + cost2 + " c3 " + cost3 + " c4 " + cost4);


                //creo la descripción
                ProjectClasses.Description desc = new Description(titles[tit] + names[nm], lores[lr1] + " " + lores[lr2] + " " +
                lores[lr3] + " " + lores[lr4], type, race, cost1, cost2, cost3, cost4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardlist.Add(card);
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
                double cost1 = 0;
                double cost2 = 0;
                double cost3 = 0;
                double cost4 = 0;


                //creo la descripción
                ProjectClasses.Description desc = new Description(magicalcardname[j], magicalcardlore[j], type, race,
                cost1, cost2, cost3, cost4);

                //y finalmente la carta
                ProjectClasses.ACard card = new ACard(atr, efc, desc);

                //para luego agregarla
                cardlist.Add(card);

            }

            //Creo el cache 

            //path del cache
            string path = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "DataBase");

            cache = FillCache(atacks, defenses, vitalities, elements, races, titlesnameslores, effects, costs);

            //Cache en forma de txt
            System.IO.File.WriteAllLines(path + @"\" + DataBaseName + ".txt", cache);

        }

        /// <summary>Devuelve el array por defecto de (1) títulos, (2) nombres, (3) lores y (4)títulos para cartas mágicas. Otro número devuelve títulos para cartas mágicas.</summary>
        public static string[] GetStringList(int titlenameloremagicnames)
        {
            List<string> res = new();

            //si es uno devuelve los títulos
            if (titlenameloremagicnames == 1)
            {
                res = GetExternalTxt(@"DataBase\StringList", "Titles", new string[] { "'", "-", ":" });
            }

            //si es 2 devuelve los nombres
            if (titlenameloremagicnames == 2)
            {
                res = GetExternalTxt(@"DataBase\StringList", "Names", new string[] { "'", "-", ":" });
            }

            //si es 3 devuelve los lores
            if (titlenameloremagicnames == 3)
            {
                res = GetExternalTxt(@"DataBase\StringList", "Lores", new string[] { "'", "-", ":", " ", "." }, '\"');
            }

            //si es 4 u otro devuelve los títulos de cartas mágicas
            if (titlenameloremagicnames >= 4 || titlenameloremagicnames < 1)
            {
                //en este caso hace falta agregarle a cada título un espacio en blanco al final
                List<string> preres = GetExternalTxt(@"DataBase\StringList", "MagicNames", new string[] { "'", "-", ":" });

                for (int k = 0; k < preres.Count; k++)
                {
                    res.Add(preres[k] + " ");
                }

            }

            return res.ToArray();
        }

        /// <summary>Lee un documento .txt externo y lo convierte a lista de palabras separadas de solo letras y números, el resto se ignora.</summary>
        public static List<string> GetExternalTxt(string carpetname, string txtname)
        {
            //creo una variable = a la dirección completa de la carpeta encima de la carpeta actual + la carpeta DataBase
            string actualdir = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, carpetname);

            //creo un array que tomará las direcciones de cada documento existente en la dirección asignada a la var actual
            string[] files = Directory.GetFiles(actualdir);

            //creo array de títulos
            string[] titles = new string[files.Length];

            //recorro por los archivos y le igualo el path sin la extensión que sería el título
            for (int i = 0; i < files.Length; i++)
            {
                titles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }


            //encuentro el txt que quiero
            int indtxt = -1;

            for (int u = 0; u < titles.Length; u++)
            {
                if (titles[u] == txtname)
                {
                    indtxt = u;
                }
            }

            //creo un streamreader para que lea el documento que quiero con el enconding en UTF8
            StreamReader rd = new StreamReader(files[indtxt], System.Text.Encoding.UTF8);

            //creo una lista para guardar las palabras del archivo
            List<string> file = new();

            //y un string para ir guardando la palabra
            string pre_word = "";

            //este ciclo sigue mientras el streamreader no alcance el final del archivo
            while (!rd.EndOfStream)
            {
                //igualo una variable a lo que va leyendo el streamreader que fuerzo a que sea de char en char
                var caract = (char)rd.Read();

                //Luego si la variable es un caracter o número
                if (Char.IsLetterOrDigit(caract))
                {
                    //ya tenemos un caracter válido y se lo sumamos al string vacío
                    pre_word += caract;
                }

                //Si no es un caracter o número entonces
                else
                {
                    //si la variable continúa vacía es que no hay palabra válida, asi que continúa a la siguiente iteración
                    if (pre_word == "")
                    {
                        continue;
                    }

                    //si la variable no es vacía entonces
                    else
                    {
                        //se la añado a la lista actual de la lista de listas
                        file.Add(pre_word);

                        //para guardar la próxima palabra me aseguro de tener el string vacío 
                        pre_word = "";

                    }
                }
            }

            //esto es por si faltó la última palabra
            if (pre_word != "")
            {
                //se la añado a la lista actual de la lista de listas
                file.Add(pre_word);

                //para guardar la proxima palabra me aseguro de tener el string vacio 
                pre_word = "";

            }

            return file;
        }

        /// <summary>Sobrecarga para no ignorar los signos que se le pasen.</summary>
        public static List<string> GetExternalTxt(string carpetname, string txtname, string[] symbols)
        {
            //creo una variable = a la dirección completa de la carpeta encima de la carpeta actual + la carpeta DataBase
            string actualdir = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, carpetname);

            //creo un array que tomará las direcciones de cada documento existente en la dirección asignada a la var actual
            string[] files = Directory.GetFiles(actualdir);

            //creo array de títulos
            string[] titles = new string[files.Length];

            //recorro por los archivos y le igualo el path sin la extensión que sería el título
            for (int i = 0; i < files.Length; i++)
            {
                titles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }

            //encuentro el txt que quiero
            int indtxt = -1;

            for (int u = 0; u < titles.Length; u++)
            {
                if (titles[u] == txtname)
                {
                    indtxt = u;
                    break;
                }
            }

            //creo un streamreader para que lea el documento que quiero con el enconding en UTF8
            StreamReader rd = new StreamReader(files[indtxt], System.Text.Encoding.UTF8);

            //creo una lista para guardar las palabras del archivo
            List<string> file = new();

            //y un string para ir guardando la palabra
            string pre_word = "";

            //este ciclo sigue mientras el streamreader no alcance el final del archivo
            while (!rd.EndOfStream)
            {
                //igualo una variable a lo que va leyendo el streamreader que fuerzo a que sea de char en char
                var caract = (char)rd.Read();

                //Luego si la variable es un caracter o número
                if (Char.IsLetterOrDigit(caract))
                {
                    //ya tenemos un caracter válido y se lo sumamos al string vacío
                    pre_word += caract;
                }

                //si no lo es
                else if (!Char.IsLetterOrDigit(caract))
                {
                    //pero es uno de los símbolos a no ignorar
                    for (int k = 0; k < symbols.Length; k++)
                    {
                        if (caract == symbols[k][0])
                        {
                            //ya tenemos un caracter válido y se lo sumamos al string vacío
                            pre_word += caract;
                        }
                    }
                }



                //Si no es un caracter, número o símbolo válido entonces
                if (caract == ' ')
                {
                    //si la variable continúa vacía es que no hay palabra válida, asi que continúa a la siguiente iteración
                    if (pre_word == "")
                    {
                        continue;
                    }

                    //si la variable no es vacía entonces
                    else
                    {
                        //se la añado a la lista actual de la lista de listas
                        file.Add(pre_word);

                        //para guardar la próxima palabra me aseguro de tener el string vacío 
                        pre_word = "";

                    }
                }
            }

            //esto es por si faltó la última palabra
            if (pre_word != "")
            {
                //se la añado a la lista actual de la lista de listas
                file.Add(pre_word);

                //para guardar la proxima palabra me aseguro de tener el string vacio 
                pre_word = "";

            }

            return file;
        }

        /// <summary>Sobrecarga para no ignorar los signos que se le pasen y tomar en vez de palabra a palabra frases entre un símbolo.</summary>
        public static List<string> GetExternalTxt(string carpetname, string txtname, string[] symbols, char betw)
        {
            //creo una variable = a la dirección completa de la carpeta encima de la carpeta actual + la carpeta DataBase
            string actualdir = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, carpetname);

            //creo un array que tomará las direcciones de cada documento existente en la dirección asignada a la var actual
            string[] files = Directory.GetFiles(actualdir);

            //creo array de títulos
            string[] titles = new string[files.Length];

            //recorro por los archivos y le igualo el path sin la extensión que seria el tótulo
            for (int i = 0; i < files.Length; i++)
            {
                titles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }


            //encuentro el txt que quiero
            int indtxt = -1;

            for (int u = 0; u < titles.Length; u++)
            {
                if (titles[u] == txtname)
                {
                    indtxt = u;
                    break;
                }
            }

            //creo un streamreader para que lea el documento que quiero con el enconding en UTF8
            StreamReader rd = new StreamReader(files[indtxt], System.Text.Encoding.UTF8);

            //creo una lista para guardar las palabras del archivo
            List<string> file = new();

            //y un string para ir guardando la palabra
            string pre_word = "";

            //este ciclo sigue mientras el streamreader no alcance el final del archivo
            while (!rd.EndOfStream)
            {
                //igualo una variable a lo que va leyendo el streamreader que fuerzo a que sea de char en char
                var caract = (char)rd.Read();

                //Luego si la variable es un caracter o número
                if (Char.IsLetterOrDigit(caract))
                {
                    //ya tenemos un caracter válido y se lo sumamos al string vacío
                    pre_word += caract;
                }

                //si no lo es
                else if (!Char.IsLetterOrDigit(caract))
                {
                    //pero es uno de los símbolos a no ignorar
                    for (int k = 0; k < symbols.Length; k++)
                    {
                        if (caract == symbols[k][0])
                        {
                            //y si no es vacía la palabra ya tenemos un caracter válido y se lo sumamos al string 
                            if (pre_word != "")
                            {
                                pre_word += caract;
                            }

                        }
                    }
                }


                //Si no es un caracter, número o símbolo válido entonces
                if (caract == betw)
                {
                    //si la variable continúa vacía es que no hay frase válida, asi que continúa a la siguiente iteración
                    if (pre_word == "" || pre_word == " " || pre_word == "        ")
                    {
                        continue;
                    }

                    //si la variable no es vacía entonces
                    else
                    {
                        //se la añado a la lista actual de la lista de listas
                        file.Add(pre_word);

                        //para guardar la próxima palabra me aseguro de tener el string vacío 
                        pre_word = "";

                    }
                }
            }

            //esto es por si faltó la última palabra
            if (pre_word != "")
            {
                //se la añado a la lista actual de la lista de listas
                file.Add(pre_word);

                //para guardar la proxima palabra me aseguro de tener el string vacio 
                pre_word = "";

            }

            return file;
        }

        public static string GetBruteExternalTxt(string carpetname, string txtname)
        {
            //creo una variable = a la dirección completa de la carpeta encima de la carpeta actual + la carpeta 
            string actualdir = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, carpetname);

            //creo un array que tomará las direcciones de cada documento existente en la dirección asignada a la var actual
            string[] files = Directory.GetFiles(actualdir);

            //creo array de títulos
            string[] titles = new string[files.Length];

            //recorro por los archivos y le igualo el path sin la extensión que sería el título
            for (int i = 0; i < files.Length; i++)
            {
                titles[i] = Path.GetFileNameWithoutExtension(files[i]);
            }


            //encuentro el txt que quiero
            int indtxt = -1;

            for (int u = 0; u < titles.Length; u++)
            {
                if (titles[u] == txtname)
                {
                    indtxt = u;
                }
            }

            //creo un streamreader para que lea el documento que quiero con el enconding en UTF8
            StreamReader rd = new StreamReader(files[indtxt], System.Text.Encoding.UTF8);

            //y un string para el resultado
            string txt = rd.ReadToEnd();


            return txt;
        }

        /// <summary>Comprueba que la raza sea válida, o sea pueda tener los elementos dados.</summary>
        public static Race CheckRaceWithElements(Element el, Race race0)
        {
            bool needtochange1 = true;
            bool needtochange2 = true;

            //si el elemento 1 está entre los que dicha raza puede tener no hay que cambiar la raza
            foreach (string elem in race0.ElementsTheyCanHave)
            {
                if (el.Element1.elementname == elem)
                {
                    needtochange1 = false;
                }
            }

            //si el elemento 2 está entre los que dicha raza puede tener o es None no hay que cambiar la raza
            foreach (string elem in race0.ElementsTheyCanHave)
            {
                if (el.Element2.elementname == elem || el.Element2.elementname == "None")
                {
                    needtochange2 = false;
                }
            }

            if (needtochange1 || needtochange2)
            {
                Race ra1 = new Race(1);
                return ra1;
            }

            return race0;

        }

        /// <summary>Método auxiliar para rellenar el cache.</summary>
        static List<string> FillCache(List<string> atacks, List<string> defenses, List<string> vitalities, List<string> elements, List<string> races,
                                List<string> titlesnameslores, List<string> effects, List<string> costs)
        {

            List<string> cache = new();

            //lleno la lista del cache con las otras listas
            for (int k = 0; k < atacks.Count; k++)
            {
                cache.Add(atacks[k]);
            }

            for (int k = 0; k < defenses.Count; k++)
            {
                cache.Add(defenses[k]);
            }

            for (int k = 0; k < vitalities.Count; k++)
            {
                cache.Add(vitalities[k]);
            }

            for (int k = 0; k < elements.Count; k++)
            {
                cache.Add(elements[k]);
            }

            for (int k = 0; k < races.Count; k++)
            {
                cache.Add(races[k]);
            }

            for (int k = 0; k < titlesnameslores.Count; k++)
            {
                cache.Add(titlesnameslores[k]);
            }

            for (int k = 0; k < effects.Count; k++)
            {
                cache.Add(effects[k]);
            }

            for (int k = 0; k < costs.Count; k++)
            {
                cache.Add(costs[k]);
            }

            return cache;
        }

    }

}