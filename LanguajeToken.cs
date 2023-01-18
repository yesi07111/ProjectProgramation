namespace ProjectClasses
{
    public class LanguajeToken
    {
        public LenguajeTokenType Type { get; set; }
    }

    public enum LenguajeTokenType
    {

        SubjectWord, //Ally and Enemy
        ActionWord, //Heal, Atk

    }
}