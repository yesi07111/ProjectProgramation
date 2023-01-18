using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IAttribute
    {
        //Conjunto de propiedades que necesariamente debe tener un objeto que implemente la interfaz.
        public double ATK { get; set; }
        public double DEF { get; set; }
        public double VIT { get; set; }
        public Element ElementsObject { get; set; }
        public (string elementname, int elementnum) Element1 { get; set; }
        public (string elementname, int elementnum) Element2 { get; set; }
        public (string elementname, int elementnum) Element3 { get; set; }
        public (string elementname, int elementnum) Element4 { get; set; }
        public List<(string elem1, string elem2)> StrongTo { get; set; }
        public List<(string elem1, string elem2)> ResistentTo { get; set; }
        public List<(string elem1, string elem2)> InmunityTo { get; set; }

    }

    /// <summary>Clase que contiene los atributos de las cartas: ATK, DEF, VIT y Elemento(s).</summary>
    public class Attribute : IAttribute
    {
        //Contructor con todas los atributos que debe tener una carta.
        public Attribute(double atk, double def, double vit, Element elements)
        {
            ATK = atk;
            DEF = def;
            VIT = vit;
            ElementsObject = elements;
            Element1 = elements.Element1;
            Element2 = elements.Element2;
            Element3 = elements.Element3;
            Element4 = elements.Element4;
            StrongTo = elements.StrongTo;
            ResistentTo = elements.ResistentTo;
            InmunityTo = elements.InmunityTo;

        }

        //Propiedades por defecto:
        public double ATK { get; set; }
        public double DEF { get; set; }
        public double VIT { get; set; }

        //Propiedades de los elementos:
        public Element ElementsObject { get; set; }
        public (string elementname, int elementnum) Element1 { get; set; }
        public (string elementname, int elementnum) Element2 { get; set; }
        public (string elementname, int elementnum) Element3 { get; set; }
        public (string elementname, int elementnum) Element4 { get; set; }
        public List<(string elem1, string elem2)> StrongTo { get; set; }
        public List<(string elem1, string elem2)> ResistentTo { get; set; }
        public List<(string elem1, string elem2)> InmunityTo { get; set; }
    }

}
