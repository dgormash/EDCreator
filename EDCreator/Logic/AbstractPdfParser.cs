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
        //private Dictionary<string, int[]> _coordinats;

        //protected AbstractPdfParser()
        //{
        //    _coordinats = new Dictionary<string, int[]>
        //    {
        //        {"Name", new[] {296, 722, 323, 728}},
        //        {"SerialNumber", new[] {437, 722, 460, 728}},
        //        {"Length", new[] {148, 640, 165, 647}},
        //        {"OD1", new[] {109, 564, 130, 570}},
        //        {"ID1", null},
        //        {"OD2", new[] {347, 564, 368, 570}},
        //        {"ID2", new[] {437, 722, 460, 728}},
        //        {"BOX", new[] {103, 600, 118, 606}},
        //        {"Postifx1", new[] {119, 600, 126, 606}},
        //        {"(1", new[] {103, 592, 105, 598}},
        //        {"val)1", new[] {105, 592, 125, 598}},
        //        {"PIN", new[] {339, 600, 355, 606}},
        //        {"Postifx2", new[] {355, 600, 362, 606}},
        //        {"(2", new[] {339, 592, 341, 598}},
        //        {"val)2", new[] {342, 592, 362, 598}}
        //    };

        //} 
        public virtual ParsedData ParseFile(string file)
        {
            var transferingData = new ParsedData();
            var reader = new PdfReader(file);
            var renderFilter = new RenderFilter[1];
            ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            //Вытаскиваем данные по очереди
            //Name
            var rect = new iTextSharp.text.Rectangle(296, 722, 323, 728);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            transferingData.Name = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);
            
            //SerialNumber
            rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            transferingData.SerialNumber = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            transferingData.Length = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

            //Connection 1 OD
            rect = new iTextSharp.text.Rectangle(109, 564, 130, 570);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            var connection = new Connection();
            connection.Od = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

            //Connection 1 BOX Tread Size
            rect = new iTextSharp.text.Rectangle(103, 600, 118, 606);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            var sb = new StringBuilder();
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(119, 600, 126, 606);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(103, 592, 105, 598);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(105, 592, 125, 598);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));

            connection.Box = sb.ToString();
            transferingData.ConnectionOne = connection;

            //Connection 2 OD
            rect = new iTextSharp.text.Rectangle(347, 564, 368, 570);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            connection = new Connection();
            connection.Od = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

            //Connection 2 ID
            rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            connection.Id = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

            //Connection 2 BOX Tread Size
            rect = new iTextSharp.text.Rectangle(339, 600, 355, 606);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb = new StringBuilder();
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(355, 600, 362, 606);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(339, 592, 341, 598);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));
            rect = new iTextSharp.text.Rectangle(342, 592, 362, 598);
            renderFilter[0] = new RegionTextRenderFilter(rect);
            textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            sb.Append(PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy));

            connection.Box = sb.ToString();
            transferingData.ConnectionTwo = connection;

            return transferingData;
        }
    }
}
