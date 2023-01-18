namespace ProjectClasses
{
    public static class RandomFactory
    {
        //Crea una carta aleatoriamente
        public static ProjectClasses.ACard CreateRandomCard()
        {
            Random r = new Random();


            ProjectClasses.Attribute attribute = new ProjectClasses.Attribute(Math.Round(r.NextDouble() * 10000),
            Math.Round(r.NextDouble() * 10000), Math.Round(r.NextDouble() * 10000), new ProjectClasses.Element(r.Next(1, 12)));

            int typeOfCard = r.Next(1, 2);
            int cantEffects = 82;
            if (typeOfCard == 1)
            {
                cantEffects = 6;
            }
            else
            {
                cantEffects = 21;
            }
            int singleormidarea = r.Next(1, 2);

            ProjectClasses.Effects eff = new ProjectClasses.Effects(new ProjectClasses.SingleEffect(cantEffects, typeOfCard),
            new ProjectClasses.ElementalDamage(singleormidarea, r.Next(0, 11)));

            ProjectClasses.Description descr = new ProjectClasses.Description
            (ReservadeWords.CardNames[r.Next(0, ReservadeWords.CardNames.Length - 1)],
            ReservadeWords.CardLore[r.Next(0, ReservadeWords.CardLore.Length - 1)],
            new ProjectClasses.AType(typeOfCard), new ProjectClasses.Race(r.Next(1, 9)),
            Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000));

            ProjectClasses.ACard ac = new ProjectClasses.ACard(attribute, eff, descr);
            return ac;
        }

        //Crea un efecto aleatorio
        public static ObjectEffect CreateRandomObjectEffet()
        {
            Random r = new Random();

            ActionWord aw = new ActionWord(ReservadeWords.ActionWords[r.Next(0, ReservadeWords.ActionWords.Length - 1)]
            , r.Next(1, (int)(r.NextDouble() * 10000)));

            SubjectWord sw = new SubjectWord(r.Next(1, 5),
            ReservadeWords.SubjectWords[r.Next(0, ReservadeWords.SubjectWords.Length - 1)], aw);

            ObjectEffect e = new ObjectEffect(sw, (int)(r.NextDouble() * 10000), (int)(r.NextDouble() * 10000),
            (int)(r.NextDouble() * 10000), (int)(r.NextDouble() * 10000));

            return e;
        }

        public static ProjectClasses.ACard CreateRandomCard(ObjectEffect oe)
        {
            Random r = new Random();


            ProjectClasses.Attribute attribute = new ProjectClasses.Attribute(Math.Round(r.NextDouble() * 10000),
            Math.Round(r.NextDouble() * 10000), Math.Round(r.NextDouble() * 10000), new ProjectClasses.Element(r.Next(1, 12)));

            int typeOfCard = r.Next(1, 2);
            int cantEffects = 82;
            if (typeOfCard == 1)
            {
                cantEffects = 6;
            }
            else
            {
                cantEffects = 21;
            }
            int singleormidarea = r.Next(1, 2);

            ProjectClasses.Effects eff = new ProjectClasses.Effects();

            ProjectClasses.Description descr = new ProjectClasses.Description
            (ReservadeWords.CardNames[r.Next(0, ReservadeWords.CardNames.Length - 1)],
            ReservadeWords.CardLore[r.Next(0, ReservadeWords.CardLore.Length - 1)],
            new ProjectClasses.AType(typeOfCard), new ProjectClasses.Race(r.Next(1, 9)),
            Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000), Math.Round(r.NextDouble() * 1000));

            ProjectClasses.ACard ac = new ProjectClasses.ACard(attribute, oe, descr, eff);
            return ac;
        }
    }
}