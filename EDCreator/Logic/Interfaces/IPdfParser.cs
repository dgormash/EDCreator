namespace FDCreator.Logic.Interfaces
{
    public interface IPdfParser
    {
        string GetStringValueFromRegion(string file, iTextSharp.text.Rectangle rectangle);
    }
}