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
            StringBuilder text = new StringBuilder();
            PdfReader pdfReader = new PdfReader(file);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.UTF8.GetBytes(currentText)));
                text.Append(currentText);
                pdfReader.Close();
            }

            string[] data = currentText.Split('\n');

            //Creating DataTable
            DataTable dt = new DataTable("PdfTable");

            string[] headers = data[0].Split(' ');

            //Adding the Columns
            for (int j = 0; j < headers.Length; j++)
            {
                if (!string.IsNullOrEmpty(headers[j]))
                {
                    dt.Columns.Add(headers[j], typeof(string));
                }

            }
            for (int i = 1; i < data.Length; i++)
            {
                string[] content = data[i].Split(' ');
                dt.Rows.Add();
                for (int k = 0; k < content.Length; k++)
                {
                    if (!string.IsNullOrEmpty(content[k]))
                    {
                        dt.Rows[dt.Rows.Count - 1][k] = content[k];
                    }
                }
            }
            return null;
        }
    }
}