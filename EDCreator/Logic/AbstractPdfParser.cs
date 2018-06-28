using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDCreator.Misc;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace EDCreator.Logic
{
    public abstract class AbstractPdfParser
    {
        public virtual StringBuilder ParseFile(string file)
        {
            var parsedData = new ParsedData();
            var reader = new PdfReader(file);
            var extractorStrategy = new LocationTextExtractionStrategy();

            var sb = new StringBuilder();
            for (int i = 1; i<= reader.NumberOfPages; i++)
            {
               sb.Append( PdfTextExtractor.GetTextFromPage(reader, i, extractorStrategy));
            }

            return sb;
        }
    }
}
