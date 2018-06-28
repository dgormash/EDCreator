using System.Collections.Generic;
using System.Data;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace EDCreator.Logic
{
    public class TestPdfParser:AbstractPdfParser
    {
        public override StringBuilder ParseFile(string file)
        {
            string currentText = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<string> linestringlist = new List<string>();
            PdfReader reader = new PdfReader(file);
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(105,592, 119, 614);
            RenderFilter[] renderFilter = new RenderFilter[1];
            renderFilter[0] = new RegionTextRenderFilter(rect);
            ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            string text = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);
            return null;
        }
    }
}