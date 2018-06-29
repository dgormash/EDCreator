namespace EDCreator.Logic
{
    public interface IPdfParser
    {
        string GetStringValueFromRegion(string file, iTextSharp.text.Rectangle rectangle);
    }
}