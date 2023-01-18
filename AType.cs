using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IType
    {
        //Propiedades y métodos que son necesarios:
        public (string typename, int typenum) Type { get; set; }
        public List<(string typename, int typenum)> AllTypes { get; set; }
        public void NewTypes(List<string> leist, int[] relatedto);
        public void NewType(string type1, int relatedto);
    }

    /// <summary>Clase para los tipos. Son 4 por defecto: 1-Natural, 2-Magic, 3-Tower, 4-Commander.</summary>
    public class AType : IType
    {

        /// <summary>Asignar un tipo por defecto. Le pasas 1 para Natural y 2 para Magic. Cualquier otro número devuelve tipo Natural.</summary>
        public AType(int typenum)
        {
            List<(string, int)> types = new();
            types.Add(("Natural", 1));
            types.Add(("Magic", 2));

            AllTypes = types;

            //Si no es un número del 2 al 4 entonces por defecto su tipo es 1 - Natural.
            if (typenum == 2)
            {
                Type = ("Magic", 2);
            }

            else
            {
                Type = ("Natural", 1);
            }
        }

        /// <summary>Asignar un nuevo tipo. Le pasas el nombre del tipo nuevo y el tipo al que se relaciona: (1)Natural, (2)Magic. Otro número devuelve por defecto Natural. Relaciona en este caso quiere decir que actua como este. Luego solo se crea un nuevo tipo que se comporta como uno ya existente, pero con diferente nombre.</summary>
        public AType(string typename, int relatedto)
        {
            List<(string, int)> types = new();
            types.Add(("Natural", 1));
            types.Add(("Magic", 2));

            AllTypes = types;

            //Si no se relaciona al tipo (2)Magic, se relaciona al tipo (1)Natural
            if (relatedto != 2)
            {
                relatedto = 1;
            }

            //Asigno la propiedad al nuevo tipo y se lo agrego a la lista de los tipos.
            Type = (typename, relatedto);
            AllTypes.Add(Type);
        }

        //Propiedad tipo que toda carta tiene.
        public (string typename, int typenum) Type { get; set; }

        //Lista de todos los tipos por defecto y los que se agreguen.
        public List<(string typename, int typenum)> AllTypes { get; set; }

        /// <summary>Método de instancia para agregar nuevos tipos a la lista de todos los tipos. Dichos tipos nuevos solo serán de nombre, a nivel de código serán tratados como el tipo al que estén relacionados: 1 para Natual, 2 para Magic, 3 para Tower y 4 para Commander. Cualquier otro número será tratado por defecto como relacionado al tipo Natural.</summary>
        public void NewTypes(List<string> leist, int[] relatedto)
        {
            for (int k = 0; k < leist.Count; k++)
            {
                if (relatedto[k] > 4 || relatedto[k] < 1)
                {
                    relatedto[k] = 1;
                }

                AllTypes.Add((leist[k], relatedto[k]));
            }
        }

        /// <summary>Método de instancia para agregar un nuevo tipo a la lista de todos los tipos y a quien este se relaciona.</summary>
        public void NewType(string type1, int relatedto)
        {
            if (relatedto > 4 || relatedto < 1)
            {
                relatedto = 1;
            }

            AllTypes.Add((type1, relatedto));
        }

    }

}