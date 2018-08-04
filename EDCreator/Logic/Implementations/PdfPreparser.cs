using FDCreator.Logic.Interfaces;
using iTextSharp.text;

namespace FDCreator.Logic.Implementations
{
    public class PdfPreparser
    {
        private readonly IPdfParser _parser = new PdfParser();
        
        public string GetParsedDataFromPdf(string file)
        {
            var rect = new Rectangle(279, 727, 309, 732);
            return _parser.GetStringValueFromRegion(file, rect);
        }
    }
}