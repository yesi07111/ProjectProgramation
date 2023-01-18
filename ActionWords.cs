namespace ProjectClasses
{
/*
Clase preventiva que crea objetos Action Word (Palabras del Lenguaje que representan acciones)
*/

public class ActionWord
{
    public string Value { get; set; }
    public int Cuantify { get; set; }

    public ActionWord(string Value, int Cuantify)
    {
        this.Value = Value;
        this.Cuantify = Cuantify;
    }

    public ActionWord()
    {
        
    }
}
}