using FDCreator.Logic.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace FDCreator.Logic.Implementations
{
    public class PdfParser:IPdfParser
    {
        public string GetStringValueFromRegion(string file, iTextSharp.text.Rectangle rectangle)
        {
            var reader = new PdfReader(file);
            var renderFilter = new RenderFilter[1];
            renderFilter[0] = new RegionTextRenderFilter(rectangle);
            ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            return PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);
        }
    }
}