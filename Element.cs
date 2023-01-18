using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IElement
    {
        //Propiedades y métodos necesarios para toda clase que implemente la interfaz.
        public (string elementname, int elementnum) Element1 { get; set; }
        public (string elementname, int elementnum) Element2 { get; set; }
        public (string elementname, int elementnum) Element3 { get; set; }
        public (string elementname, int elementnum) Element4 { get; set; }
        public (List<string> elements, List<int> num) AllElements { get; set; }
        public List<(string elem1, string elem2)> StrongTo { get; set; }
        public List<(string elem1, string elem2)> ResistentTo { get; set; }
        public List<(string elem1, string elem2)> InmunityTo { get; set; }

        public void NewElements(List<string> leist);
        public void NewElement(string elem1);


    }

    public class Element : IElement
    {

        //Propiedades accesibles: los 4 elementos que puede tener cada carta por defecto, la lista de todos ellos, 
        // y las relaciones de fuerte-débil.
        public (string elementname, int elementnum) Element1 { get; set; }
        public (string elementname, int elementnum) Element2 { get; set; }
        public (string elementname, int elementnum) Element3 { get; set; }
        public (string elementname, int elementnum) Element4 { get; set; }
        public (List<string> elements, List<int> num) AllElements { get; set; }
        public List<(string elem1, string elem2)> StrongTo { get; set; }
        public List<(string elem1, string elem2)> ResistentTo { get; set; }
        public List<(string elem1, string elem2)> InmunityTo { get; set; }


        //Tiene varias sobrecargas el constructor:

        /// <summary>Si no se le pasa nada se asume que tiene un único elemento por defecto y es Normal.</summary>
        public Element()
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Le asigno como element1 Normal y el resto none.
            Element1 = (AllElements.elements![0], AllElements.num![0]);
            Element2 = ("None", 0);
            Element3 = ("None", 0);
            Element4 = ("None", 0);

        }

        /// <summary>Por defecto hay 12 elementos. Si se quiere que tenga un solo elemento de los que hay por defecto se pone: (1)Normal, (2)Fire, (3)Aqua, (4)Earth, (5)Wind, (6)Lightning, (7)Ice, (8)Cristal, (9)Plant, (10)Shadow, (11)Light y (12)Dark. Cualquier otro número da por defecto Normal. Las relaciones vienen por defecto.</summary>
        public Element(int elem1)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si el número no es del 2 al 12 se asume que por defecto el único elemento que tiene es Normal.
            if (elem1 > 12 || elem1 <= 1)
            {
                Element1 = (AllElements.elements![0], AllElements.num![0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element1 = (AllElements.elements![elem1 - 1], AllElements.num![elem1 - 1]);
            }

            //El resto son none:
            Element2 = ("None", 0); ;
            Element3 = ("None", 0); ;
            Element4 = ("None", 0);

        }

        /// <summary>Pasarle el número de los dos elementos por defecto que va a tener. Se pone: (1)Normal, (2)Fire, (3)Aqua, (4)Earth, (5)Wind, (6)Lightning, (7)Ice, (8)Cristal, (9)Plant, (10)Shadow, (11)Light y (12)Dark. Cualquier otro número da por defecto Normal. Las relaciones vienen por defecto.</summary>>
        public Element(int elem1, int elem2)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si el número del primer elemento no es del 2 al 12 se asume que por defecto elemento 1 es Normal.
            if (elem1 > 12 || elem1 <= 1)
            {
                Element1 = (AllElements.elements![0], AllElements.num![0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element1 = (AllElements.elements![elem1 - 1], AllElements.num![elem1 - 1]);
            }

            //Si el número del segundo elemento no es del 2 al 12 se asume que por defecto elemento 2 es Normal.
            if (elem2 > 12 || elem2 <= 1)
            {
                Element2 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element2 = (AllElements.elements[elem2 - 1], AllElements.num[elem2 - 1]);
            }

            //Compruebo si el elemento 1 y 2 son el mismo y cambio a None el último en caso de que sí.
            CheckAndOrg(Element1.elementname, Element2.elementname);

            //Pongo los que faltan en None y listo.
            Element3 = ("None", 0); ;
            Element4 = ("None", 0); ;

        }

        /// <summary>Pasarle el número de los tres elementos por defecto que va a tener. Se pone: (1)Normal, (2)Fire, (3)Aqua, (4)Earth, (5)Wind, (6)Lightning, (7)Ice, (8)Machine, (9)Plant, (10)Shadow, (11)Light y (12)Dark. Cualquier otro número da por defecto Normal. Las relaciones vienen por defecto.</summary>>
        public Element(int elem1, int elem2, int elem3)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si el número del primer elemento no es del 2 al 12 se asume que por defecto elemento 1 es Normal.
            if (elem1 > 12 || elem1 <= 1)
            {
                Element1 = (AllElements.elements![0], AllElements.num![0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element1 = (AllElements.elements![elem1 - 1], AllElements.num![elem1 - 1]);
            }

            //Si el número del segundo elemento no es del 2 al 12 se asume que por defecto elemento 2 es Normal.
            if (elem2 > 12 || elem2 <= 1)
            {
                Element2 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element2 = (AllElements.elements[elem2 - 1], AllElements.num[elem2 - 1]);
            }

            //Si el número del tercer elemento no es del 2 al 12 se asume que por defecto elemento 3 es Normal.
            if (elem3 > 12 || elem3 <= 1)
            {
                Element3 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element3 = (AllElements.elements[elem3 - 1], AllElements.num[elem3 - 1]);
            }


            //Compruebo si algún elemento está repetido y lo cambio a None en caso de que sí.
            CheckAndOrg(Element1.elementname, Element2.elementname, Element3.elementname);

            //Lleno el que falta y listo
            Element4 = ("None", 0); ;
        }

        /// <summary>En caso de que se quieran cuatro elementos por defecto. Se pone: (1)Normal, (2)Fire, (3)Aqua, (4)Earth, (5)Wind, (6)Lightning, (7)Ice, (8)Machine, (9)Plant, (10)Shadow, (11)Light y (12)Dark. Cualquier otro número da por defecto Normal. Las relaciones vienen por defecto.</summary>
        public Element(int elem1, int elem2, int elem3, int elem4)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si el número del primer elemento no es del 2 al 12 se asume que por defecto elemento 1 es Normal.
            if (elem1 > 12 || elem1 <= 1)
            {
                Element1 = (AllElements.elements![0], AllElements.num![0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element1 = (AllElements.elements![elem1 - 1], AllElements.num![elem1 - 1]);
            }

            //Si el número del segundo elemento no es del 2 al 12 se asume que por defecto elemento 2 es Normal.
            if (elem1 > 12 || elem1 <= 1)
            {
                Element2 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element2 = (AllElements.elements[elem2 - 1], AllElements.num[elem2 - 1]);
            }

            //Si el número del tercer elemento no es del 2 al 12 se asume que por defecto elemento 3 es Normal.
            if (elem3 > 12 || elem3 <= 1)
            {
                Element3 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element3 = (AllElements.elements[elem3 - 1], AllElements.num[elem3 - 1]);
            }

            //Finalmente si el número del tercer elemento no es del 2 al 12 se asume que por defecto el elemento 4 es Normal.
            if (elem4 > 12 || elem4 <= 1)
            {
                Element4 = (AllElements.elements[0], AllElements.num[0]);
            }

            //Sino se le asigna el elemento que le corresponde al número.
            else
            {
                Element4 = (AllElements.elements[elem4 - 1], AllElements.num[elem4 - 1]);
            }

            //Compruebo si algún elemento está repetido y lo cambio a None en caso de que sí.
            CheckAndOrg(Element1.elementname, Element2.elementname, Element3.elementname, Element4.elementname);

        }

        /// <summary>Si se desea establecer una cantidad x de elementos que dada su raza pueda tener, se le pasa la lista de los elementos que puede tener y de ahí se toman los elementos de índice z de la lista de índices. Si la cantidad x a poner es mayor que el 4 (máximo número de elementos que cada carta puede tener) se asume que se pedía colocar 4.</summary>
        public Element(List<string> elementstheycanhaveduetorace, List<int> indexes, int amountofelementstoset)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si es negativo, cero, mayor o igual a cuatro entonces se colocan los 4 primeros.
            if (amountofelementstoset >= 4 || amountofelementstoset < 1)
            {
                amountofelementstoset = 4;

                //Listas de los 4 elementos y sus números
                List<string> elementstoset = new();
                List<int> elementsnum = new();

                //Verifico que las listas tienen la cantidad mínima de elementos a poner
                if (elementstheycanhaveduetorace.Count < 4 || indexes.Count < 4)
                {
                    throw new Exception("Se quieren colocar 4 elementos pero la lista de elementos o índices tiene menos que 4 elementos.");
                }

                elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[2]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[3]]);

                //Aquí busco los números de todos los elementos a poner
                elementsnum = NumSearch(elementstoset);

                //Luego con los 4 elementos a poner y sus números ya no se necesita nada más. Los añado:
                Element1 = (elementstoset[0], elementsnum[0]);
                Element2 = (elementstoset[1], elementsnum[1]);
                Element3 = (elementstoset[2], elementsnum[2]);
                Element4 = (elementstoset[3], elementsnum[3]);

            }

            if (amountofelementstoset == 3)
            {
                //Listas de los 3 elementos y sus números
                List<string> elementstoset = new();
                List<int> elementsnum = new();

                //Verifico que las listas tienen la cantidad mínima de elementos a poner
                if (elementstheycanhaveduetorace.Count < 3 || indexes.Count < 3)
                {
                    throw new Exception("Se quieren colocar 3 elementos pero la lista de elementos o índices tiene menos que 3 elementos.");
                }

                elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[2]]);

                //Aquí busco los números de todos los elementos a poner
                elementsnum = NumSearch(elementstoset);

                //Luego con los 3 elementos a poner y sus números ya no se necesita nada más. Los añado y pongo el que falta en None, 0:
                Element1 = (elementstoset[0], elementsnum[0]);
                Element2 = (elementstoset[1], elementsnum[1]);
                Element3 = (elementstoset[2], elementsnum[2]);
                Element4 = ("None", 0);


            }

            if (amountofelementstoset == 2)
            {
                //Listas de los 2 elementos y sus números
                List<string> elementstoset = new();
                List<int> elementsnum = new();

                //Verifico que las listas tienen la cantidad mínima de elementos a poner
                if (elementstheycanhaveduetorace.Count < 2 || indexes.Count < 2)
                {
                    throw new Exception("Se quieren colocar 2 elementos pero la lista de elementos o índices tiene menos que 2 elementos.");
                }

                elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);

                //Aquí busco los números de todos los elementos a poner
                elementsnum = NumSearch(elementstoset);

                //Luego con los 2 elementos a poner y sus números ya no se necesita nada más. Los añado y pongo los que faltan en None, 0:
                Element1 = (elementstoset[0], elementsnum[0]);
                Element2 = (elementstoset[1], elementsnum[1]);
                Element3 = ("None", 0);
                Element4 = ("None", 0);

            }

            if (amountofelementstoset == 1)
            {
                if (elementstheycanhaveduetorace.Count < 1 || indexes.Count < 1)
                {
                    throw new Exception("Se quieren colocar 1 elementos pero la lista de elementos o índices tiene menos que 1 elementos.");
                }

                //Obtengo el elemento a poner 
                string elem1 = elementstheycanhaveduetorace[indexes[0]];

                //Guardo el elemento en una lista para buscarle su número
                List<string> listof1 = new();
                listof1.Add(elem1);

                //Aquí busco su número 
                var elem1num = NumSearch(listof1);

                //Pongo el Elemento1 y el resto en None, 0 y listo
                Element1 = (elem1, elem1num[0]);
                Element2 = ("None", 0);
                Element3 = ("None", 0);
                Element4 = ("None", 0);
            }

        }

        /// <summary>Si se desea establecer una cantidad x de elementos que dada su raza pueda tener y mantener j elementos por defecto, se le pasa la lista de los elementos que puede tener y de ahí se toman los elementos de índice z de la lista de índices. Si la cantidad x a poner es mayor que f (máximo número de elementos que cada carta puede tener - elementos por defecto a mantener) se asume que se pedía colocar f.</summary>
        public Element(List<string> elementstheycanhaveduetorace, List<int> indexes, int amountofelementstoset, List<(string elem1, int elem1num)> listofdefaultelements, int defaultelemnetstomantain)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Si por defecto se mantiene 1 entonces se pueden asignar 1, 2 o 3.
            if (defaultelemnetstomantain == 1)
            {
                if (amountofelementstoset == 1)
                {
                    //Verifico que las listas tengan el mínimo que se quiere poner
                    if (elementstheycanhaveduetorace.Count < 1 || indexes.Count < 1 || listofdefaultelements.Count < 1)
                    {
                        throw new Exception("Se quieren colocar 1 elementos pero la lista de elementos, la lista por defecto a colocar o índices tiene menos que 1 elementos.");
                    }

                    //Obtengo el elemento a poner 
                    string elem1 = elementstheycanhaveduetorace[indexes[0]];

                    //Guardo el elemento en una lista para buscarle su número
                    List<string> listof1 = new();
                    listof1.Add(elem1);

                    //Aquí busco su número 
                    var elem1num = NumSearch(listof1);

                    //Pongo el Elemento1 por defecto, el Elemento2 buscado, el 3 y 4 en (None, 0) y listo
                    Element1 = listofdefaultelements[0];
                    Element2 = (elem1, elem1num[0]);
                    Element3 = ("None", 0);
                    Element4 = ("None", 0);

                }

                if (amountofelementstoset == 2)
                {
                    //Listas de los 2 elementos y sus números
                    List<string> elementstoset = new();
                    List<int> elementsnum = new();

                    //Verifico que las listas tengan el mínimo que se quiere poner
                    if (elementstheycanhaveduetorace.Count < 2 || indexes.Count < 2 || listofdefaultelements.Count < 1)
                    {
                        throw new Exception("Se quieren colocar 2 elementos pero la lista de elementos o índices tiene menos que 2 elementos o la lista por defecto a colocar tiene menos que 1 elemento.");
                    }

                    elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                    elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);

                    //Aquí busco los números de todos los elementos a poner
                    elementsnum = NumSearch(elementstoset);

                    //Luego con los 2 elementos a poner y sus números ya no se necesita nada más. Pongo el por defecto, luego estos:
                    Element1 = listofdefaultelements[0];
                    Element2 = (elementstoset[0], elementsnum[0]);
                    Element3 = (elementstoset[1], elementsnum[1]);
                    Element4 = ("None", 0);
                }

                if (amountofelementstoset >= 3 || amountofelementstoset < 0)
                {
                    amountofelementstoset = 3;

                    //Listas de los 3 elementos y sus números
                    List<string> elementstoset = new();
                    List<int> elementsnum = new();

                    //Verifico que las listas tengan el mínimo que se quiere poner
                    if (elementstheycanhaveduetorace.Count < 3 || indexes.Count < 3 || listofdefaultelements.Count < 1)
                    {
                        throw new Exception("Se quieren colocar 3 elementos pero la lista de elementos o índices tiene menos que 3 elementos o la lista por defecto a colocar tiene menos que 1 elemento.");
                    }

                    elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                    elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);
                    elementstoset.Add(elementstheycanhaveduetorace[indexes[2]]);

                    //Aquí busco los números de todos los elementos a poner
                    elementsnum = NumSearch(elementstoset);

                    //Luego con los 3 elementos a poner y sus números ya no se necesita nada más. Pongo el por defecto y estos:
                    Element1 = listofdefaultelements[0];
                    Element2 = (elementstoset[0], elementsnum[0]);
                    Element3 = (elementstoset[1], elementsnum[1]);
                    Element4 = (elementstoset[2], elementsnum[2]);
                }

            }

            //Si por defecto mantienen 2 entonces se pueden asignar 1 o 2.
            if (defaultelemnetstomantain == 2)
            {
                if (amountofelementstoset == 1)
                {
                    //Verifico que las listas tengan el mínimo que se quiere poner
                    if (elementstheycanhaveduetorace.Count < 1 || indexes.Count < 1 || listofdefaultelements.Count < 2)
                    {
                        throw new Exception("Se quieren colocar 1 elementos pero la lista de elementos o índices tiene menos que 1 elementos o la lista por defecto a colocar tiene menos que 2.");
                    }


                    //Obtengo el elemento a poner 
                    string elem1 = elementstheycanhaveduetorace[indexes[0]];

                    //Guardo el elemento en una lista para buscarle su número
                    List<string> listof1 = new();
                    listof1.Add(elem1);

                    //Aquí busco su número 
                    var elem1num = NumSearch(listof1);

                    //Pongo el Elemento1 y Elemento2 por defecto, el Elemento3 buscado, el 4 en None, 0 y listo
                    Element1 = listofdefaultelements[0];
                    Element2 = listofdefaultelements[1];
                    Element3 = (elem1, elem1num[0]);
                    Element4 = ("None", 0);

                }

                if (amountofelementstoset >= 2 || amountofelementstoset < 0)
                {
                    //Listas de los 2 elementos y sus números
                    List<string> elementstoset = new();
                    List<int> elementsnum = new();

                    //Verifico que las listas tengan el mínimo que se quiere poner
                    if (elementstheycanhaveduetorace.Count < 2 || indexes.Count < 2 || listofdefaultelements.Count < 2)
                    {
                        throw new Exception("Se quieren colocar 2 elementos pero la lista de elementos, la lista por defecto a colocar o índices tiene menos que 2 elementos.");
                    }

                    elementstoset.Add(elementstheycanhaveduetorace[indexes[0]]);
                    elementstoset.Add(elementstheycanhaveduetorace[indexes[1]]);

                    //Aquí busco los números de todos los elementos a poner
                    elementsnum = NumSearch(elementstoset);

                    //Luego con los 2 elementos a poner y sus números ya no se necesita nada más. Pongo los por defecto y luego estos:
                    Element1 = listofdefaultelements[0];
                    Element2 = listofdefaultelements[1];
                    Element3 = (elementstoset[0], elementsnum[0]);
                    Element4 = (elementstoset[1], elementsnum[1]);

                }
            }

            //Si por defecto se mantienen 3, entonces solo 1 se puede colocar
            if (defaultelemnetstomantain >= 3 || defaultelemnetstomantain < 0)
            {
                defaultelemnetstomantain = 3;
                amountofelementstoset = 1;
                Element1 = listofdefaultelements[0];
                Element2 = listofdefaultelements[1];
                Element3 = listofdefaultelements[2];

                //Pongo el elemento a ubicar, dado su índice y la lista de los que pueden ser, en una lista.
                List<string> listof1 = new();
                listof1.Add(elementstheycanhaveduetorace[indexes[0]]);

                //Luego lo guardo, el nombre es el primer y único elemento de listof1, y su número es la primera posición de la lista que
                //devuelve el método para buscarle el número a una lista de elementos
                Element4 = (listof1[0], NumSearch(listof1)[0]);
            }
        }

        /// <summary>Se usa cuando se quiere que el Elemento1 sea uno nuevo. El resto será None. Se le puede pasar cuantas relaciones se quiera, o ninguna.</summary>
        public Element(string newelem, List<string> strong, List<string> resist, List<string> inmunity)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Aquí le agrego el nuevo elemento y lo pongo como el Elemento 1, como está recién agregado su número coincide con
            //el count de la lista de todos los elementos. El resto es (None, 0).
            NewElement(newelem);
            Element1 = (newelem, AllElements.num!.Count);
            Element2 = ("None", 0);
            Element3 = ("None", 0);
            Element4 = ("None", 0);

            //Compruebo que relación se le pasaron y las agrego
            if (strong.Count != 0)
            {
                for (int k = 0; k < strong.Count; k++)
                {
                    StrongTo.Add((newelem, strong[k]));
                }
            }

            if (resist.Count != 0)
            {
                for (int k = 0; k < resist.Count; k++)
                {
                    ResistentTo.Add((newelem, resist[k]));
                }
            }

            if (inmunity.Count != 0)
            {
                for (int k = 0; k < inmunity.Count; k++)
                {
                    InmunityTo.Add((newelem, inmunity[k]));
                }
            }

        }

        /// <summary>Se usa cuando se quiere agregar más de un elemento nuevo. Le pasas una lista con los nuevos elementos y la cantidad de esos elementos nuevos que desean en las propiedades de Elemento del 1 al 4, si es menor que 4 el resto se pone None, y se toman por defecto los primeros de la lista en la cantidad indicada. Las relaciones pueden tener o no, pero en caso de que no se pasa una lista vacía para saber que es que X elemento no tiene Y relación. </summary>
        public Element(List<string> newelements, int amounttoset, List<List<string>> strong,
                        List<List<string>> resist, List<List<string>> inmunity)
        {
            AllElements = new();
            StrongTo = new();
            ResistentTo = new();
            InmunityTo = new();

            CreatorOfElementsPerDefect();

            //Aquí le agrego los nuevos elementos.
            NewElements(newelements);

            //Compruebo si hay elementos duplicados para eliminarlos.
            CheckAndOrg(AllElements.elements!);

            //Si la cantidad a colocar no es del 1 al 3 entonces se ponen los 4 primeros elementos nuevos. Y se eliman los repetidos.
            if (amounttoset < 0 || amounttoset >= 4)
            {
                //Aquí busco sus números en la lista de todos, ya que pudieron agregarse x cantidad y de esos x los 4 primeros no se 
                //sabe que número son
                List<int> nums = new();

                for (int k = 0; k < AllElements.elements!.Count; k++)
                {
                    //Una vez lo encuentras el resto son los 3 k a continuación.
                    if (AllElements.elements[k] == newelements[0])
                    {
                        nums.Add(AllElements.num![k]);
                        nums.Add(AllElements.num[k + 1]);
                        nums.Add(AllElements.num[k + 2]);
                        nums.Add(AllElements.num[k + 3]);
                        break;
                    }
                }

                Element1 = (newelements[0], nums[0]);
                Element2 = (newelements[1], nums[1]);
                Element3 = (newelements[2], nums[2]);
                Element4 = (newelements[3], nums[3]);

                CheckAndOrg(Element1.elementname, Element2.elementname, Element3.elementname, Element4.elementname);

            }

            //Sino dependiendo de cuantos se quieren poner se les da el valor del nuevo elemento y el resto None. Y se eliman los repetidos.
            if (amounttoset == 1)
            {

                //Aquí busco sus números en la lista de todos, ya que pudieron agregarse x cantidad y de esos x los 4 primeros no se 
                //sabe que número son
                List<int> nums = new();

                for (int k = 0; k < AllElements.elements!.Count; k++)
                {
                    //Una vez lo encuentras el resto son los 3 k a continuación.
                    if (AllElements.elements[k] == newelements[0])
                    {
                        nums.Add(AllElements.num![k]);
                        break;
                    }
                }


                Element1 = (newelements[0], nums[0]);
                Element2 = ("None", 0);
                Element3 = ("None", 0);
                Element4 = ("None", 0);
            }

            if (amounttoset == 2)
            {
                //Aquí busco sus números en la lista de todos, ya que pudieron agregarse x cantidad y de esos x los 4 primeros no se 
                //sabe que número son
                List<int> nums = new();

                for (int k = 0; k < AllElements.elements!.Count; k++)
                {
                    //Una vez lo encuentras el resto son los 3 k a continuación.
                    if (AllElements.elements[k] == newelements[0])
                    {
                        nums.Add(AllElements.num![k]);
                        nums.Add(AllElements.num[k + 1]);
                        break;
                    }
                }

                Element1 = (newelements[0], nums[0]);
                Element2 = (newelements[1], nums[1]);
                Element3 = ("None", 0);
                Element4 = ("None", 0);

                CheckAndOrg(Element1.elementname, Element2.elementname);
            }

            if (amounttoset == 3)
            {
                //Aquí busco sus números en la lista de todos, ya que pudieron agregarse x cantidad y de esos x los 4 primeros no se 
                //sabe que número son
                List<int> nums = new();

                for (int k = 0; k < AllElements.elements!.Count; k++)
                {
                    //Una vez lo encuentras el resto son los 3 k a continuación.
                    if (AllElements.elements[k] == newelements[0])
                    {
                        nums.Add(AllElements.num![k]);
                        break;
                    }
                }

                Element1 = (newelements[0], nums[0]);
                Element2 = (newelements[1], nums[1]);
                Element3 = (newelements[2], nums[2]);
                Element4 = ("None", 0);

                CheckAndOrg(Element1.elementname, Element2.elementname, Element3.elementname);
            }

            //Y a partir de aquí le agrego las relaciones en caso de tener.

            //Recorro por la lista de elementos
            for (int k = 0; k < newelements.Count; k++)
            {
                //Compruebo si la lista de elementos a los que es fuerte el newelement[k] no es vacía
                if (strong[k].Count != 0)
                {
                    //En caso de existir elementos con los que el newelement[k] sea fuerte, recorro por los mismos y los agrego todos.
                    for (int j = 0; j < strong[k].Count; j++)
                    {
                        StrongTo.Add((newelements[k], strong[k][j]));
                    }

                }

                //Compruebo si la lista de elementos a los que es resistente el newelement[k] no es vacía
                if (resist[k].Count != 0)
                {
                    //En caso de existir elementos a los que el newelement[k] sea resistente, recorro por los mismos y los agrego todos.
                    for (int j = 0; j < strong[k].Count; j++)
                    {
                        ResistentTo.Add((newelements[k], resist[k][j]));
                    }
                }

                //Compruebo si la lista de elementos a los que es inmune el newelement[k] no es vacía
                if (inmunity[k].Count != 0)
                {
                    //En caso de existir elementos a los que el newelement[k] es inmune, recorro por los mismos y los agrego todos.
                    for (int j = 0; j < inmunity[k].Count; j++)
                    {
                        InmunityTo.Add((newelements[k], inmunity[k][j]));
                    }
                }
            }


        }


        public List<int> NumSearch(List<string> elements)
        {
            List<int> num = new();
            int j = 0; //contador para pasar por los elementos a poner

            //Busco los números de j elementos.
            for (int k = 0; k < AllElements.elements.Count; k++)
            {
                //Si en la lista de todos los elementos el elemento actual es igual al elemento j entonces tomo su número
                if (AllElements.elements[k] == elements[j])
                {
                    num.Add(AllElements.num[k]);

                    //Si j+1 no se pasa de los elementos a poner, busco el próximo elemento y reseteo el for
                    if (j + 1 < elements.Count)
                    {
                        j++;
                        k = 0;
                    }

                    //sino ya encontré todos y rompo el ciclo
                    else
                    {
                        break;
                    }

                }
            }

            return num;
        }

        /// <summary>Método de instancia para agregar nuevos elementos a la lista de todos los elementos.</summary>
        public void NewElements(List<string> leist)
        {
            for (int k = 0; k < leist.Count; k++)
            {
                AllElements.num.Add(AllElements.num.Count + 1);
                AllElements.elements.Add(leist[k]);
            }
        }

        /// <summary>Método de instancia para agregar un nuevo elemento a la lista de todos los elementos.</summary>
        public void NewElement(string elem1)
        {
            AllElements.num.Add(AllElements.num.Count + 1);
            AllElements.elements.Add(elem1);
        }

        /// <summary>Revisa que no tenga elementos duplicados. Le pasas los elementos 1 y 2 y en caso de ser iguales, el último se pone None.</summary>
        void CheckAndOrg(string elem1, string elem2)
        {
            if (elem1 == elem2)
            {
                Element2 = ("None", 0);
            }
        }

        /// <summary>Revisa que no tenga elementos duplicados. Le pasas los elementos 1, 2 y 3. En caso de ser iguales, el último se pone None.</summary>
        void CheckAndOrg(string elem1, string elem2, string elem3)
        {
            if (elem1 == elem2)
            {
                Element2 = ("None", 0);
            }

            if (elem1 == elem3)
            {
                Element3 = ("None", 0);
            }

            else if (Element2 == ("None", 0))
            {
                var change = Element3;
                Element2 = change;
                Element3 = ("None", 0);
            }

            if (elem2 == elem3)
            {
                Element3 = ("None", 0);
            }
        }

        /// <summary>Revisa que no tenga elementos duplicados. Le pasas los elementos 1, 2, 3 y 4. En caso de ser iguales, el/los último/s se pone/n None.</summary>
        void CheckAndOrg(string elem1, string elem2, string elem3, string elem4)
        {
            if (elem1 == elem2)
            {
                Element2 = ("None", 0);
            }

            if (elem1 == elem3)
            {
                Element3 = ("None", 0);
            }

            else if (Element2 == ("None", 0))
            {
                var change = Element3;
                Element2 = change;
                Element3 = ("None", 0);
            }

            if (elem1 == elem4)
            {
                Element4 = ("None", 0);
            }

            else if (Element3 == ("None", 0))
            {
                var change = Element4;
                Element3 = change;
                Element4 = ("None", 0);
            }

            if (elem2 == elem3)
            {
                Element3 = ("None", 0);
            }

            if (elem2 == elem4)
            {
                Element4 = ("None", 0);
            }

            else if (Element3 == ("None", 0))
            {
                var change = Element4;
                Element3 = change;
                Element4 = ("None", 0);
            }

            if (elem3 == elem4)
            {
                Element4 = ("None", 0);
            }
        }

        /// <summary>Reivsa que no tenga elementos duplicados en el array de todos los elementos y los modifica acorde.</summary>
        void CheckAndOrg(List<string> elems)
        {

            List<string> lis = elems;
            List<int> num = new();

            for (int k = elems.Count - 1; k >= 0; k--)
            {
                for (int j = k - 1; j >= 0; j--)
                {
                    if (elems[k] == elems[j])
                    {
                        lis.Remove(elems[j]);
                        k = elems.Count - 1;
                        j = k - 1;
                    }
                }
            }

            for (int k = 0; k < elems.Count; k++)
            {
                num.Add(k + 1);
            }

            AllElements = (lis, num);
        }

        /// <summary>Método de instancia que devuelve una lista de todos los elementos que guarden una relación con el elemento seleccionado. Se debe especificar el tipo de relación: (1)StrongTo, (2)ResistentTo, (3)InmunityTo. Cualquier otro número se le asigna por defecto a la relación (1)StrongTo.</summary>
        List<string> OneElementRelation(string element, int typerelation)
        {
            List<string> relationto = new();

            if (typerelation == 2)
            {
                //Recorro por la lista de ResistentTo
                for (int k = 0; k < ResistentTo.Count; k++)
                {
                    //Si el primer elemento en la posición k es el elemento dado, entonces le agrego el elemento 2 que mantiene la
                    //relación ResistentTo con él. Recordar que elem1 es resistente a elem2.

                    if (ResistentTo[k].elem1 == element)
                    {
                        relationto.Add(ResistentTo[k].elem2);
                    }
                }

                //Al salir ya tengo todos los elementos a los que es resistente el elemento dado.
            }

            else if (typerelation == 3)
            {
                //Recorro por la lista de InmunityTo
                for (int k = 0; k < InmunityTo.Count; k++)
                {
                    //Si el primer elemento en la posición k es el elemento dado, entonces le agrego el elemento 2 que mantiene la
                    //relación InmunityTo con él. Recordar que elem1 es inmune a elem2.

                    if (InmunityTo[k].elem1 == element)
                    {
                        relationto.Add(InmunityTo[k].elem2);
                    }
                }

                //Al salir ya tengo todos los elementos a los que es inmune el elemento dado.
            }

            else
            {
                //Recorro por la lista de StrongTo
                for (int k = 0; k < StrongTo.Count; k++)
                {
                    //Si el primer elemento en la posición k es el elemento dado, entonces le agrego el elemento 2 que mantiene la
                    //relación StrongTo con él. Recordar que elem1 es fuerte contra elem2.

                    if (StrongTo[k].elem1 == element)
                    {
                        relationto.Add(StrongTo[k].elem2);
                    }
                }

                //Al salir ya tengo todos los elementos a los que es fuerte el elemento dado.
            }

            return relationto;

        }

        /// <summary>Método de instancia que devuelve una lista de 4 listas de todos los elementos que guarden una relación con los 4 elementos de la carta (los cuales pueden ser None, osea, inválidos en cuyo caso su lista de relaciones estaría vacía). Se debe especificar el tipo de relación: (1)StrongTo, (2)ResistentTo, (3)InmunityTo. Cualquier otro número se le asigna por defecto a la relación (1)StrongTo.</summary>
        public List<List<string>> AllElementsRelationOfACard(List<string> elements, int typerelation)
        {
            List<List<string>> all = new();

            //Luego recorro la lista de los elementos.
            for (int k = 0; k < elements.Count; k++)
            {
                //En caso que sea un elemento válido busco los otros elementos que se relacionan con él según el tipo especificado
                //de relación.
                if (elements[k] != "None")
                {
                    all.Add(OneElementRelation(elements[k], typerelation));
                }

                //Sino le paso 4 listas vacía.
                else
                {
                    List<string> empty = new();
                    all.Add(empty);
                }

            }

            //regreso una lista de 4 listas que tiene las relaciones de los 4 elementos de la carta
            return all;
        }

        /// <summary>Creador de los elementos por defecto.</summary>
        void CreatorOfElementsPerDefect()
        {

            #region Elementos por defecto

            //Aquí creo la lista de todos los elementos (la cual es extensible).
            List<string> allelements = new();
            allelements.Add("Normal");
            allelements.Add("Fire");
            allelements.Add("Aqua");
            allelements.Add("Earth");
            allelements.Add("Wind");
            allelements.Add("Lightning");
            allelements.Add("Ice");
            allelements.Add("Cristal");
            allelements.Add("Plant");
            allelements.Add("Shadow");
            allelements.Add("Light");
            allelements.Add("Dark");

            //Y aquí creo las relaciones fuerte-débil, resistente-poco efectivo e inmunidad por defecto
            List<(string elem1, string elem2)> strongto = new();
            strongto.Add(("Fire", "Ice"));
            strongto.Add(("Fire", "Dark"));
            strongto.Add(("Aqua", "Fire"));
            strongto.Add(("Aqua", "Light"));
            strongto.Add(("Aqua", "Cristal"));
            strongto.Add(("Earth", "Fire"));
            strongto.Add(("Earth", "Lightining"));
            strongto.Add(("Earth", "Cristal"));
            strongto.Add(("Wind", "Earth"));
            strongto.Add(("Wind", "Plant"));
            strongto.Add(("Lightning", "Aqua"));
            strongto.Add(("Lightning", "Wind"));
            strongto.Add(("Ice", "Aqua"));
            strongto.Add(("Ice", "Cristal"));
            strongto.Add(("Ice", "Normal"));
            strongto.Add(("Cristal", "Plant"));
            strongto.Add(("Cristal", "Light"));
            strongto.Add(("Plant", "Aqua"));
            strongto.Add(("Plant", "Light"));
            strongto.Add(("Plant", "Earth"));
            strongto.Add(("Shadow", "Lightning"));
            strongto.Add(("Shadow", "Earth"));
            strongto.Add(("Light", "Dark"));
            strongto.Add(("Light", "Shadow"));
            strongto.Add(("Dark", "Light"));
            strongto.Add(("Dark", "Ice"));

            List<(string elem1, string elem2)> resistto = new();
            resistto.Add(("Fire", "Ice"));
            resistto.Add(("Fire", "Light"));
            resistto.Add(("Aqua", "Earth"));
            resistto.Add(("Aqua", "Cristal"));
            resistto.Add(("Earth", "Lightning"));
            resistto.Add(("Earth", "Fire"));
            resistto.Add(("Wind", "Earth"));
            resistto.Add(("Wind", "Plant"));
            resistto.Add(("Lightning", "Light"));
            resistto.Add(("Lightning", "Dark"));
            resistto.Add(("Lightning", "Fire"));
            resistto.Add(("Ice", "Light"));
            resistto.Add(("Ice", "Lightning"));
            resistto.Add(("Cristal", "Plant"));
            resistto.Add(("Cristal", "Wind"));
            resistto.Add(("Plant", "Aqua"));
            resistto.Add(("Plant", "Light"));
            resistto.Add(("Shadow", "Cristal"));
            resistto.Add(("Shadow", "Ice"));
            resistto.Add(("Light", "Shadow"));
            resistto.Add(("Light", "Lightning"));
            resistto.Add(("Light", "Fire"));
            resistto.Add(("Dark", "Wind"));
            resistto.Add(("Dark", "Earth"));


            List<(string elem1, string elem2)> inmunityto = new();
            inmunityto.Add(("Shadow", "Dark"));
            inmunityto.Add(("Dark", "Shadow"));
            inmunityto.Add(("Fire", "Wind"));
            inmunityto.Add(("Normal", "Cristal"));
            inmunityto.Add(("Cristal", "Light"));
            inmunityto.Add(("Ice", "Aqua"));
            inmunityto.Add(("Lightning", "Normal"));

            //Finalmente creo la lista de los números que representan cada elemento.
            List<int> num = new();

            for (int k = 0; k < allelements.Count; k++)
            {
                num.Add(k + 1);
            }

            //Y a la propiedad de todos los elementos le paso la lista de todos los elementos y la lista de su número en una tupla.
            AllElements = (allelements, num);
            StrongTo = strongto;
            ResistentTo = resistto;
            InmunityTo = inmunityto;

            #endregion

        }

    }
}
