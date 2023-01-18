using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProjectClasses
{
    public interface IKingdom
    {
        public string Disposition { get; set; }
        public double Faith { get; set; }
        public double Militarism { get; set; }
        public double Knowledge { get; set; }
        public double Capital { get; set; }
        public double RecuperationPerTurnFaith { get; set; }
        public double RecuperationPerTurnMilitarism { get; set; }
        public double RecuperationPerTurnKnowledge { get; set; }
        public double RecuperationPerTurnCapital { get; set; }
        public double OriginalRecuperationKnowledge { get; set; }
        public double OriginalRecuperationCapital { get; set; }
        public double OriginalRecuperationFaith { get; set; }
        public double OriginaRecuperationMilitar { get; set; }
    }

    /// <summary>Esta clase define el concepto de reino</summary> 
    public class Kingdoms : IKingdom
    {
        public string Disposition { get; set; }
        public double Faith { get; set; }
        public double Militarism { get; set; }
        public double Knowledge { get; set; }
        public double Capital { get; set; }
        public double RecuperationPerTurnFaith { get; set; }
        public double RecuperationPerTurnMilitarism { get; set; }
        public double RecuperationPerTurnKnowledge { get; set; }
        public double RecuperationPerTurnCapital { get; set; }
        public double OriginalRecuperationKnowledge { get; set; }
        public double OriginalRecuperationCapital { get; set; }
        public double OriginalRecuperationFaith { get; set; }
        public double OriginaRecuperationMilitar { get; set; }

        public Kingdoms(string disposition, double faith,
        double militarism, double knowledge, double capital)
        {

            this.Disposition = disposition;
            this.Faith = faith;
            this.Knowledge = knowledge;
            this.Militarism = militarism;
            this.Capital = capital;

            OriginalRecuperationFaith = 1000;
            OriginaRecuperationMilitar = 1000;
            OriginalRecuperationKnowledge = 1000;
            OriginalRecuperationCapital = 1000;

            RecuperationPerTurnFaith = 1000;
            RecuperationPerTurnMilitarism = 1000;
            RecuperationPerTurnKnowledge = 1000;
            RecuperationPerTurnCapital = 1000;

            if (disposition == "Capital")
            {
                OriginalRecuperationCapital += 1000;
                RecuperationPerTurnCapital += 1000;
            }

            if (disposition == "Faith")
            {
                OriginalRecuperationFaith += 1000;
                RecuperationPerTurnFaith += 1000;
            }

            if (disposition == "Knowledge")
            {
                OriginalRecuperationKnowledge += 1000;
                RecuperationPerTurnKnowledge += 1000;
            }

            if (disposition == "Militar")
            {
                OriginaRecuperationMilitar += 1000;
                RecuperationPerTurnMilitarism += 1000;
            }

        }

    }

}
