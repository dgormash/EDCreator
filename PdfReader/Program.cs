using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfReader
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://stackoverflow.com/a/23915452/9051104
            //Create our test file, nothing special
            var testFile = $"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\in\\Stabilizer Inspection.pdf";
            //using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    using (var doc = new Document())
            //    {
            //        using (var writer = PdfWriter.GetInstance(doc, fs))
            //        {
            //            doc.Open();

            //            doc.Add(new Paragraph("This is my sample file"));

            //            doc.Close();
            //        }
            //    }
            //}

            //Create an instance of our strategy
            var t = new RectTextExtractionStrategy();

            //Parse page 1 of the document above
            using (var r = new iTextSharp.text.pdf.PdfReader(testFile))
            {
                var ex = PdfTextExtractor.GetTextFromPage(r, 1, t);
            }

            //Loop through each chunk found
            foreach (var p in t.Collection)
            {
                Console.WriteLine("Found text {0} at lx: {1}; ly: {2}; rx: {3}; ry: {4}", p.Text, p.Rect.Left, p.Rect.Bottom, p.Rect.Right,p.Rect.Top);
            }
        }
    }
}
